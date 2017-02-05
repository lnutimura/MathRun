/*
 * Esse script deve ser atrelado à um GameObject.
 * O intuito dele é contornar uma espécie de bug da UnityEngine
 * quanto ao instanciamento de componentes em tempo real e adição
 * de listeners que passam parâmetros inteiros.
 * Portanto tudo que ele faz é repassar o evento de OnClick para o
 * GameManager tratar.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadScript : MonoBehaviour {

	public Vector3 CellCoordinates;
	private GameManager m_gameManager;
	void Start () {
		m_gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	public void OnClick () {
        if (SceneManager.GetActiveScene().name != "StudentModule") {
            m_gameManager.SelectedCell(CellCoordinates);
        }
	}
}
