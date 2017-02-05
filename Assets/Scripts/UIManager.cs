/*
 * Esse script deve ser atrelado à um GameObject.
 * O intuito dele é controlar tudo relacionado à UI do jogo,
 * ou seja:
 * 	- Passar informações da célula para UI;
 *  - Passar inforamções da UI para a célula;
 *  - Disponibilizar uma "API" para sabermos quando o usuário está focando um
 *    componente de UI;
 *  - Salvar e carregar níveis;
 *  - Carregar perguntas;
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour {
	// Variável que pode ser acessada por outros
	// scripts para controle do usuário, por exemplo:
	// evitar o rollback enquanto apaga um texto.
	private bool isFocusingUI;
	public bool IsFocusingUI {
		get { return CheckIfFocusingUI(); }
		set { isFocusingUI = value; }
	}

	// Referência ao GameManager, apenas para acesso das informações
	// de todas as células posicionadas no tabuleiro para salvar corretamente
	private GameManager m_gameManager;
    // Também é preciso referência ao GridManager, para posicionar os objetos
    // corretamente quando carregados
    private GridManager m_gridManager;

    // Componentes da UI
    [SerializeField]
    private Text m_dialogLabel;
    [SerializeField]
    private Color m_successDialogColor;
    [SerializeField]
    private Color m_failureDialogColor;

	[SerializeField]
	private InputField m_levelInputField;
	[SerializeField]
	private Text m_cellLabel;
	[SerializeField]
	private InputField m_questionInputField;
	[SerializeField]
	private InputField m_answerInputField;
	[SerializeField]
	private Dropdown m_operationDropdown;
	[SerializeField]
	private InputField m_difficultyInputField;

	// UI Panels
	[SerializeField]
	private GameObject m_loadLevelPanel;
    [SerializeField]
    private GameObject m_loadQuestionPanel;
    [SerializeField]
    private GameObject m_genericDialogPanel;

	// URLs para acesso de informações no BD
	private const string m_selectLevelsUrl = "https://mathrun.000webhostapp.com/selectFases.php";
    private const string m_selectCellsUrl = "https://mathrun.000webhostapp.com/selectFase_Casa_Pergunta.php";
    private const string m_selectQuestionsUrl = "https://mathrun.000webhostapp.com/selectPerguntas.php";

    void Start () {
		IsFocusingUI = false;
		m_gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        m_gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
	}

	// Método auxiliar responsável por checar se o usuário está focalizando a UI
	// Utilizado pela propriedade IsFocusingUI
	bool CheckIfFocusingUI () {
		return (m_levelInputField.isFocused || m_questionInputField.isFocused ||
			    m_answerInputField.isFocused || m_difficultyInputField.isFocused);
	}

	// Método responsável por resetar informações da UI
	public void ResetUIInfo () {
        //m_levelInputField.text = "";
        m_dialogLabel.text = "";
		m_cellLabel.text = "Célula (0,0)";
		m_questionInputField.text = "";
		m_answerInputField.text = "";
		m_operationDropdown.value = 0;
		m_difficultyInputField.text = "";
	}

	// Método responsável por carregar informações da célula para a UI
	public void LoadInfoToUI (Cell cell) {
		m_cellLabel.text = cell.GetPrimitive().transform.name;
        // Verifica se a célula possui outras informações preenchidas por
        // meio do campo de Perguntas
        m_cellLabel.text = "Célula (" + ((int)cell.GetPosition().x).ToString() + "," + ((int)cell.GetPosition().y).ToString() + ")";
		m_questionInputField.text = cell.GetQuestion();
		m_answerInputField.text = (string.IsNullOrEmpty(m_questionInputField.text)) ? "" : cell.GetAnswer().ToString();
		m_operationDropdown.value = (int) cell.GetOperationType();
		m_difficultyInputField.text = (string.IsNullOrEmpty(m_questionInputField.text)) ? "" : cell.GetDifficulty().ToString();
	}

	// Método responsável por carregar infomações da UI para a célula
	public void SaveInfoToCell (Cell cell) {
        // Verifica se tentou salvar uma célula com a resposta em branco,
        // neste caso, assume-se que a célula possui resposta 0f
        float parsedAnswer;
        if (!float.TryParse(m_answerInputField.text, out parsedAnswer)) {
            parsedAnswer = 0f;
        }

        // Verifica se tentou salvar uma célula com a dificuldade em branco,
        // neste caso, assume-se que a célula possui dificuldade 1
        int parsedDifficulty;
        if (!int.TryParse(m_difficultyInputField.text, out parsedDifficulty)) {
            parsedDifficulty = 1;
        }

        // Seta as configurações para a célula
		cell.SetCell (
			m_questionInputField.text,
			parsedAnswer,
            Mathf.Clamp(parsedDifficulty, 1, 10),
			(Cell.OperationType) m_operationDropdown.value
		);

		Debug.Log("Saved info to cell " + cell.GetPosition());
	}

    // Método auxiliar utilizado para mostrar um novo diálogo ao usuário
    // (Não é utilizado atualmente)
    public void DisplayDialog (string title, string text) {
        m_genericDialogPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = title;
        m_genericDialogPanel.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = text;

        m_genericDialogPanel.SetActive(true);
        m_gameManager.SetPlayable(false);
    }

    // Método responsável por retornar ao menu
    public void ReturnToMenu () {
        // O ideal seria verificar se o nível foi salvo
        // e mostrar um diálogo para deixar o usuário decidir
        // se deve ser salvo ou não, mas como estamos sem tempo...
        SceneManager.LoadScene("login", LoadSceneMode.Single);
    }

    // Método responsável por salvar o nível atual para o BD
    public void SaveLevel () {
		// Verifica se o usuário preencheu o nome do novo nível
		if (string.IsNullOrEmpty(m_levelInputField.text)) {
			Debug.Log("Tried to save a level without a name.");
            m_dialogLabel.text = "LUL".ToString();
            m_dialogLabel.color = m_failureDialogColor;
            return;
		}

        //Salva as informações da UI para a última célula (garantia)
        SaveInfoToCell(m_gameManager.SelectedCells[m_gameManager.SelectedCells.Count - 1]);
            
		// Pega a data do sistema
		string sDate = DateTime.Now.ToString();
		DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

		string dy = datevalue.Day.ToString();
		string mn = datevalue.Month.ToString();
		string yy = datevalue.Year.ToString();
		// Pega o id do autor
		int authorId = int.Parse(PlayerPrefs.GetString("rememberId"));
		// Criar um form para enviar as informações via POST
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("LevelName", m_levelInputField.text);
		wwwForm.AddField("Author", authorId.ToString());
		wwwForm.AddField("Date", dy + "-" + mn + "-" + yy);
		// Itera sobre todas as células posicionadas no tabuleiro
		for (int i = 0; i < m_gameManager.SelectedCells.Count; ++i) {
			wwwForm.AddField("Question[]", m_gameManager.SelectedCells[i].GetQuestion());
			wwwForm.AddField("Answer[]", m_gameManager.SelectedCells[i].GetAnswer().ToString());
			wwwForm.AddField("Type[]", ((int)m_gameManager.SelectedCells[i].GetOperationType()).ToString());
			wwwForm.AddField("Difficulty[]", m_gameManager.SelectedCells[i].GetDifficulty().ToString());
			wwwForm.AddField("x[]", ((int)m_gameManager.SelectedCells[i].GetPosition().x).ToString());
			wwwForm.AddField("y[]", ((int)m_gameManager.SelectedCells[i].GetPosition().y).ToString());
		}
		// Chama a corotina responsável por salvar os dados efetivamente
		StartCoroutine(ISaveWWWForm(wwwForm));
    }

    // Corotina auxiliar para enviar o formulário WWW e efetivar o salvamento
    IEnumerator ISaveWWWForm (WWWForm wwwForm) {
		WWW www = new WWW("https://mathrun.000webhostapp.com/SaveLevel.php", wwwForm);
		yield return www;

		if (www.error == null) {
			if (www.text == "1") {
				Debug.Log("Salvou!");
                m_dialogLabel.text = "Salvou nível com sucesso!";
                m_dialogLabel.color = m_successDialogColor;
			} else {
				Debug.Log("Não salvou: " + www.text);
                m_dialogLabel.text = "Não foi possível salvar o nível";
                m_dialogLabel.color = m_failureDialogColor;
            }
		} else {
			Debug.Log("Erro de conexão: " + www.error);
            m_dialogLabel.text = "Erro de conexão.";
            m_dialogLabel.color = m_failureDialogColor;
        }
    }

    // Método responsável por carregar o nível selecionado no painel de níveis disponíveis
    void LoadLevel (string id) {
        m_loadLevelPanel.SetActive(false);

        m_gameManager.ClearAllCells();

        WWW www = new WWW(m_selectCellsUrl + "?fase=" + int.Parse(id));
        StartCoroutine(ILoadLevel(www));
    }

    // Corotina auxiliar para reconstruir o nível selecionado
    IEnumerator ILoadLevel (WWW www) {
        yield return www;

        bool result;
        int numOfLines, dataPerLine;

        if (www.error == null) {
            // Recebe os dados do SELECT referente à fase, casa e pergunta
            ArrayList data = MenuManager.GetDadosWWW(www, out result, out dataPerLine, out numOfLines);

            for (int i = 0; i < (dataPerLine * numOfLines); i += 9) {
                int x = int.Parse(data[i + 1].ToString());
                int y = int.Parse(data[i + 2].ToString());

                m_gridManager.GridMatrix[x, y].SetColor(Cell.MiddleCellColor);
                m_gridManager.GridMatrix[x, y].SetCell(data[i + 4].ToString(), float.Parse(data[i + 5].ToString()), int.Parse(data[i + 6].ToString()), (Cell.OperationType) int.Parse(data[i + 7].ToString()));
                m_gridManager.GridMatrix[x, y].SetStatus(true);

                m_gameManager.AddSelectedCellToList(m_gridManager.GridMatrix[x, y]);
                LoadInfoToUI(m_gridManager.GridMatrix[x, y]);
            }

            m_gameManager.DrawPossiblePaths(m_gameManager.SelectedCells[m_gameManager.SelectedCells.Count - 1]);

            m_dialogLabel.text = "Carregou novo nível com sucesso.";
            m_dialogLabel.color = m_successDialogColor;
        }

        m_loadLevelPanel.SetActive(false);
        m_gameManager.SetPlayable(true);
    }

    // Método responsável por mostrar o painel de níveis disponíveis
	public void RetrieveLevels () {
		m_loadLevelPanel.SetActive(true);
        m_gameManager.SetPlayable(false);

        // Remove possíveis resultados anteriores
        foreach (Transform child in GameObject.Find("Load Level Grid Layout").transform) {
			Destroy(child.gameObject);
		}

		WWW www = new WWW (m_selectLevelsUrl);
        StartCoroutine(IRetrieveLevels(www));
    }

    // Corotina auxiliar para mostrar todos os níveis disponíveis no BD
	IEnumerator IRetrieveLevels (WWW www) {
        yield return www;

        bool result;
        int dataPerLine, numOfLines;

        if (www.error == null) {
            // Recebe os dados do SELECT de fases existentes
            ArrayList data = MenuManager.GetDadosWWW(www, out result, out dataPerLine, out numOfLines);
            if (result) {
                // Itera sobre os dados, para mostrar no painel corretamente
                for (int i = 0; i < (dataPerLine * numOfLines); i += dataPerLine) {
                    GameObject go = (GameObject)Instantiate(Resources.Load("Load Level Item"));
                    // Ajusta as dimensões
                    go.transform.SetParent(GameObject.Find("Load Level Grid Layout").transform);
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
                    go.GetComponent<Button>().onClick.AddListener(delegate () { LoadLevel(go.transform.GetChild(0).GetComponent<Text>().text); });
                    // Aplica as informações ao painel
                    go.name = data[i] + ":" + data[i + 1].ToString() + ":" + data[i + 3].ToString();
                    go.transform.GetChild(0).GetComponent<Text>().text = data[i].ToString();
                    go.transform.GetChild(1).GetComponent<Text>().text = data[i + 1].ToString();
                    go.transform.GetChild(2).GetComponent<Text>().text = data[i + 3].ToString();
                }
            }
        }
    }

    // Método responsável por mostrar o painel de perguntas disponíveis
    public void RetrieveQuestions () {
        m_loadQuestionPanel.SetActive(true);
        m_gameManager.SetPlayable(false);

        // Remove possíveis resultados anteriores
        foreach (Transform child in GameObject.Find("Load Question Grid Layout").transform) {
            Destroy(child.gameObject);
        }

        WWW www = new WWW(m_selectQuestionsUrl);
        StartCoroutine(IRetrieveQuestions(www));
    }

    // Corotina auxiliar para mostrar todos as perguntas disponíveis no BD
    IEnumerator IRetrieveQuestions (WWW www) {
        yield return www;

        bool result;
        int dataPerLine, numOfLines;

        if (www.error == null) {
            // Recebe os dados do SELECT de perguntas existentes
            ArrayList data = MenuManager.GetDadosWWW(www, out result, out dataPerLine, out numOfLines);
            if (result == true) {
                // Itera sobre os dados, para mostrar no painel corretamente
                for (int i = 0; i < (dataPerLine * numOfLines); i += dataPerLine) {
                    GameObject go = (GameObject)Instantiate(Resources.Load("Load Question Item"));
                    // Ajusta as dimensões
                    go.transform.SetParent(GameObject.Find("Load Question Grid Layout").transform);
                    go.transform.localScale = new Vector3(1f, 1f, 1f);
                    go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
                    go.GetComponent<Button>().onClick.AddListener(delegate () {
                        string[] str = go.name.Split(':');

                        m_questionInputField.text = str[1].ToString();
                        m_answerInputField.text = str[2].ToString();
                        m_operationDropdown.value = int.Parse(str[4].ToString());
                        m_difficultyInputField.text = str[3].ToString();

                        m_loadQuestionPanel.SetActive(false);
                        m_gameManager.SetPlayable(true);
                    });
                    // Aplica as informações ao painel
                    go.name = data[i] + ":" + data[i + 1].ToString() + ":" + data[i + 2].ToString() + ":" + data[i + 3].ToString() + ":" + data[i + 4].ToString() + ":" + data[i + 5].ToString();
                    go.transform.GetChild(0).GetComponent<Text>().text = data[i].ToString();
                    go.transform.GetChild(1).GetComponent<Text>().text = data[i + 1].ToString();
                    go.transform.GetChild(2).GetComponent<Text>().text = data[i + 2].ToString();
                }
            }
        }
    }
}