using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SelectCell : MonoBehaviour {
	private float t;
	private GridScript gridScript;
	private Cell currentSelectedCell;

	void Start () {
		t = 0f;
		currentSelectedCell = null;
		gridScript = GameObject.Find("Grid").GetComponent<GridScript>();
	}

	void LateUpdate () {
		// Verifica a célula que o usuário deseja selecionar
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)) {
				// Se for uma célula válida
				if (gridScript.gridMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y].GetCellStatus()) {
					// A célula selecionada anteriormente (se existir), volta a ser vermelha
					if (currentSelectedCell != null) {
						// Atribui à célula antiga todas as informações contidas na UI
						SaveCellInfo(currentSelectedCell);
						currentSelectedCell.GetCellPrimitive().GetComponent<Renderer>().material.color = Color.red;
					}
					// A célula selecionada é atualizada e será animada ao fim do LateUpdate()
					currentSelectedCell = gridScript.gridMatrix[(int)hit.transform.position.x, (int)hit.transform.position.y];
					// Carrega para a UI todas as informações contidas na célula
					LoadCellInfo(currentSelectedCell);
				} else {
					Debug.Log("Failed.");
				}
			}
		}

		// Responsável por fazer a animação da célula selecionada
		if (currentSelectedCell != null) {
			currentSelectedCell.GetCellPrimitive().GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.magenta,
				Mathf.Abs(Mathf.Cos(t * 2f)));

			t += Time.deltaTime;
		}
	}

	void SaveCellInfo (Cell cell) {
		// Componentes de UI com informações de uma célula
		InputField question = GameObject.Find("QuestionInput").GetComponent<InputField>();
		InputField answer = GameObject.Find("AnswerInput").GetComponent<InputField>();
		InputField difficulty = GameObject.Find("DifficultyInput").GetComponent<InputField>();
		Dropdown operation = GameObject.Find("Dropdown").GetComponent<Dropdown>();

		float result;
		if (!float.TryParse(answer.text, out result)) {
			answer.text = "";
			result = 0;
		}

		int dif;
		if (!int.TryParse(difficulty.text, out dif)) {
			difficulty.text = "";
			dif = 0;
		}

		switch (operation.value) {
			case 0:
				cell.SetCell(Cell.OperationType.Soma, dif, question.text, result);
				break;
			case 1:
				cell.SetCell(Cell.OperationType.Subtracao, dif, question.text, result);
				break;
			case 2:
				cell.SetCell(Cell.OperationType.Multiplicacao, dif, question.text, result);
				break;
			case 3:
				cell.SetCell(Cell.OperationType.Divisao, dif, question.text, result);
				break;
			case 4:
				cell.SetCell(Cell.OperationType.Mista, dif, question.text, result);
				break;
		}
	}

	void LoadCellInfo (Cell cell) {
		// Componentes de UI com informações de uma célula
		Text selectedCell = GameObject.Find("SelectedCellText").GetComponent<Text>();
		InputField question = GameObject.Find("QuestionInput").GetComponent<InputField>();
		InputField answer = GameObject.Find("AnswerInput").GetComponent<InputField>();
		InputField difficulty = GameObject.Find("DifficultyInput").GetComponent<InputField>();
		Dropdown operation = GameObject.Find("Dropdown").GetComponent<Dropdown>();

		selectedCell.text = "Célula (" + (int)cell.GetCellPosition().x + "," + (int)cell.GetCellPosition().y + ")";
		question.text = cell.GetCellQuestion();
		answer.text = string.IsNullOrEmpty(question.text) ? "" : cell.GetCellAnswer().ToString();
		difficulty.text = string.IsNullOrEmpty(question.text) ? "" : cell.GetCellDifficulty().ToString();

		operation.value = (int)cell.GetCellType();
	}
}
