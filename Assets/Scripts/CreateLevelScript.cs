using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelScript : MonoBehaviour {
	private string m_levelUrl;
	private string m_questionUrl;
	private string m_cellUrl;
	private string m_lastLevelUrl;
	private string m_lastQuestionUrl;

	private InputField m_levelName;
	private GridScript m_gridScript;

	void Start () {
		m_levelUrl = "https://mathrun.000webhostapp.com/cadastraFase.php";
		m_questionUrl = "https://mathrun.000webhostapp.com/cadastraPergunta.php";
		m_cellUrl = "https://mathrun.000webhostapp.com/cadastraCasas.php";
		m_lastLevelUrl = "https://mathrun.000webhostapp.com/selectLastFase.php";
		m_lastQuestionUrl = "https://mathrun.000webhostapp.com/selectLastPergunta.php";

		m_gridScript = GameObject.Find("Grid").GetComponent<GridScript>();
		m_levelName = GameObject.Find("LevelNameInput").GetComponent<InputField>();
	}
	public void SaveLevel () {
		if (m_levelName.text == "") {
			Debug.Log("Erro: tentou salvar um nível sem nome.");
			return;
		}

		string sDate = DateTime.Now.ToString();
		DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

		string dy = datevalue.Day.ToString();
		string mn = datevalue.Month.ToString();
		string yy = datevalue.Year.ToString();

		int id = int.Parse(PlayerPrefs.GetString("rememberId"));
		WWW www = new WWW(m_levelUrl + "?nome=" + WWW.EscapeURL(m_levelName.text) + "&autor=" + id + "&data=" + (dy + "-" + mn + "-" + yy));
		StartCoroutine(ISaveData(www));
		//www = new WWW(m_lastLevelUrl);
		//StartCoroutine(IGetId(www));

		for (int j = 0; j < m_gridScript.gridLines; ++j) {
			for (int i = 0; i < m_gridScript.gridColumns; ++i) {
				if (m_gridScript.gridMatrix[i, j].GetCellStatus()) {
					string question = m_gridScript.gridMatrix[i, j].GetCellQuestion();
					float answer = m_gridScript.gridMatrix[i, j].GetCellAnswer();
					int difficulty = m_gridScript.gridMatrix[i, j].GetCellDifficulty();
					int type = (int) m_gridScript.gridMatrix[i, j].GetCellType();

					www = new WWW(m_questionUrl + "?questao=" + WWW.EscapeURL(question) + "&resposta=" + answer + "&dificuldade=" + difficulty + "&tipo=" + type + "&autor=" + id);
					StartCoroutine(ISaveData(www));
					//www = new WWW(m_lastQuestionUrl);
					//StartCoroutine(IGetId(www));
					www = new WWW(m_cellUrl + "?x=" + i + "&y=" + j);
					StartCoroutine(ISaveData(www));
				}
			}
		}
	}

	/*
	IEnumerator IGetId(WWW www, int tipo = 0) {
		yield return www;

		bool resultado;
		int dadosPorLinha;
		int numLinhas;

		if (www.error == null)
        {
            ArrayList dados = MenuManager.GetDadosWWW(www, out resultado, out dadosPorLinha, out numLinhas);
            if (tipo == 0) {
				// Fase
				lastSavedLevelId = int.Parse(dados[0].ToString());
			} else if (tipo == 1) {
				// Pergunta
				lastSavedQuestionId = int.Parse(dados[0].ToString());
			}
		}
	}
	*/

	IEnumerator ISaveData(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            if (www.text == "1")
            {
                Debug.Log("Salvou os dados com sucesso!");
            }
            else
            {
                Debug.Log("Erro ao salvar.");
            }
        }
        else
        {
            Debug.Log("Erro de conexão com o servidor.");
        }
    }
}
