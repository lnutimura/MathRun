using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLevelScript : MonoBehaviour {
    public GameObject LoadLevelPanel;

    // Base das URLs para inserir e consultar dados no BD
	private string m_levelUrl;
	private string m_cellUrl;
    private string m_selectLevelsUrl;
    private string m_selectCellsUrl;
    // Lista de URLs completas para inserir dados no BD
    private List<WWW> wwwList;
	private InputField m_levelName;
	private GridScript m_gridScript;
    private SelectCell m_selectCellScript;


	void Start () {
        m_levelUrl = "https://mathrun.000webhostapp.com/cadastraFase.php";
		m_cellUrl = "https://mathrun.000webhostapp.com/cadastraPergunta_e_Casa.php";
        m_selectLevelsUrl = "https://mathrun.000webhostapp.com/selectFases.php";
        m_selectCellsUrl = "https://mathrun.000webhostapp.com/selectFase_Casa_Pergunta.php";

        m_selectCellScript = GetComponent<SelectCell>();
        m_gridScript = GameObject.Find("Grid").GetComponent<GridScript>();
		m_levelName = GameObject.Find("LevelNameInput").GetComponent<InputField>();
	}

	public void SaveLevel () {
		if (m_levelName.text == "") {
			Debug.Log("Erro: tentou salvar um nível sem nome.");
			return;
		}

        m_selectCellScript.SaveCellInfo(m_selectCellScript.currentSelectedCell);
        wwwList = new List<WWW>();

		string sDate = DateTime.Now.ToString();
		DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

		string dy = datevalue.Day.ToString();
		string mn = datevalue.Month.ToString();
		string yy = datevalue.Year.ToString();

		int id = int.Parse(PlayerPrefs.GetString("rememberId"));
        wwwList.Add(new WWW(m_levelUrl + "?nome=" + WWW.EscapeURL(m_levelName.text) + "&autor=" + id + "&data=" + (dy + "-" + mn + "-" + yy)));

        for (int i = 0; i < m_gridScript.numOfSelectedCells; ++i) {
            int x = (int) m_gridScript.selectedCells[i].GetCellPosition().x;
            int y = (int) m_gridScript.selectedCells[i].GetCellPosition().y;

            string question = m_gridScript.selectedCells[i].GetCellQuestion();
			float answer = m_gridScript.selectedCells[i].GetCellAnswer();
			int difficulty = m_gridScript.selectedCells[i].GetCellDifficulty();
			int type = (int) m_gridScript.selectedCells[i].GetCellType();

            wwwList.Add(new WWW(m_cellUrl + "?questao=" + WWW.EscapeURL(question) + "&resposta=" + answer + "&dificuldade=" + difficulty + "&tipo=" + type + "&autor=" + id + "&x=" + x + "&y=" + y + "&fase=" + WWW.EscapeURL(m_levelName.text)));
        }

        StartCoroutine(ISaveWWWList(wwwList));
	}

    public void LoadLevel (string id) {
        LoadLevelPanel.SetActive(false);

        m_gridScript.ResetAllCells();

        WWW www = new WWW(m_selectCellsUrl + "?fase=" + int.Parse(id));
        StartCoroutine(ILoadLevel(www));
    }

    IEnumerator ILoadLevel (WWW www) {
        yield return www;

        bool result;
        int numOfLines = 0;
        int dataPerLine = 0;

        if (www.error == null) {
            ArrayList data = MenuManager.GetDadosWWW(www, out result, out dataPerLine, out numOfLines);

            for (int i = 0; i < (dataPerLine * numOfLines); i += 9) {
                int x = int.Parse(data[i + 1].ToString());
                int y = int.Parse(data[i + 2].ToString());

                m_gridScript.gridMatrix[x, y].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.red;
                m_gridScript.gridMatrix[x, y].SetCell((Cell.OperationType)int.Parse(data[i + 7].ToString()), int.Parse(data[i + 6].ToString()), data[i + 4].ToString(), float.Parse(data[i + 5].ToString()));
                m_gridScript.gridMatrix[x, y].SetCellStatus(true);

                m_gridScript.selectedCells[m_gridScript.numOfSelectedCells] = m_gridScript.gridMatrix[x, y];
                ++m_gridScript.numOfSelectedCells;
                m_gridScript.lastSelectedCell = m_gridScript.gridMatrix[x, y];

                m_selectCellScript.currentSelectedCell = m_gridScript.gridMatrix[x, y];
            }
        }

        LoadLevelPanel.SetActive(false);
        m_gridScript.SetPlayable(true);
    }

    public void RetrieveLevels () {
        LoadLevelPanel.SetActive(true);
        m_gridScript.SetPlayable(false);

        foreach (Transform child in GameObject.Find("Level Grid Layout").transform) {
            Destroy(child.gameObject);
        }

        WWW www = new WWW(m_selectLevelsUrl);
        StartCoroutine(IRetrieveLevels(www));
    }

    IEnumerator IRetrieveLevels (WWW www) {
        yield return www;

        bool result;
        int numOfLines = 0;
        int dataPerLine = 0;

        if (www.error == null) {
            ArrayList data = MenuManager.GetDadosWWW(www, out result, out dataPerLine, out numOfLines);
            
            for (int i = 0; i < (dataPerLine * numOfLines); i += 4) {
                GameObject go = (GameObject)Instantiate(Resources.Load("DownloadedLevel"));

                go.transform.SetParent(GameObject.Find("Level Grid Layout").transform);
                go.transform.localScale = new Vector3(1f, 1f, 1f);
                go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
                go.GetComponent<Button>().onClick.AddListener(delegate () { LoadLevel(go.transform.GetChild(0).GetComponent<Text>().text); });

                go.name = data[i] + ":" + data[i + 1].ToString() + ":" + data[i + 3].ToString();
                go.transform.GetChild(0).GetComponent<Text>().text = data[i].ToString();
                go.transform.GetChild(1).GetComponent<Text>().text = data[i + 1].ToString();
                go.transform.GetChild(2).GetComponent<Text>().text = data[i + 3].ToString();
            }
        }
    }

    IEnumerator ISaveWWWList(List<WWW> wwwList) {
        List<WWW> wwwList2 = new List<WWW>();
        for (int i = 0; i < wwwList.Count; i++) {

            yield return wwwList[i];

            if (wwwList[i].error == null)
            {
                if (wwwList[i].text == "1")
                {
                    Debug.Log("Salvou os dados com sucesso! " + wwwList[i].url);
                }
                else
                {
                    Debug.Log("Erro ao salvar. " + wwwList[i].url);
                }
            }
            else
            {
                Debug.Log("Erro de conexão com o servidor. " + wwwList[i].url);
                wwwList2.Add(new WWW(wwwList[i].url));
            }

            yield return new WaitForSeconds(2f);
        }

        if (wwwList2.Count > 0) StartCoroutine(ISaveWWWList(wwwList2));
        else Debug.Log("Fase salva com sucesso!");
    }
}
