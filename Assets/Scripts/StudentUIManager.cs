using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StudentUIManager : MonoBehaviour {
    // Usado para decidir o que fazer depois que o estudante
    // ganhou o jogo, baseado no retorno dos botões de Sim ou Não
    // no Dialog de vencedor.
    public int WinnerDialogResult;

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
    private StudentGameManager m_gameManager;
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
    private Text m_cellLabel;
    [SerializeField]
    private InputField m_questionInputField;
    [SerializeField]
    private InputField m_answerInputField;
    [SerializeField]
    private Text m_scoreLabel;
    [SerializeField]
    private Text m_errorLabel;

    // UI Panels
    [SerializeField]
    private GameObject m_loadLevelPanel;
    [SerializeField]
    private GameObject m_winnerDialogPanel;

    // URLs para acesso de informações no BD
    private const string m_selectLevelsUrl = "https://mathrun.000webhostapp.com/selectFases.php";
    private const string m_selectCellsUrl = "https://mathrun.000webhostapp.com/selectFase_Casa_Pergunta.php";
    private const string m_selectQuestionsUrl = "https://mathrun.000webhostapp.com/selectPerguntas.php";
    private const string m_historyUrl = "https://mathrun.000webhostapp.com/cadastraHistorico.php";

    private string m_loadedLevel;

    void Start() {
        IsFocusingUI = false;
        WinnerDialogResult = 0;
        m_gameManager = GameObject.Find("Game Manager").GetComponent<StudentGameManager>();
        m_gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();

        // Primeira coisa que faz é mostrar a tela de 
        // seleção de fases para o estudante
        RetrieveLevels();
    }


    // Método usado pelos botões para controlar o
    // que fazer após a fase ser completada
    // (Deseja jogar novamente?)
    // 1 - Sim
    // 2 - Não
    public void SetDialogResult (string button) {
        if (button == "Yes") {
            WinnerDialogResult = 1;
        } else {
            WinnerDialogResult = 2;
        }
    }
    
    // Método responsável por avaliar a resposta do usuário, e promover o avanço caso esteja correta.
    public void SubmitAnswer () {
        if (string.IsNullOrEmpty(m_answerInputField.text)) {
            Debug.Log("Tried to submit without an answer.");
            MenuManager.FeedBackError(m_dialogLabel, "Insira a resposta.");
            return;
        }

        // Pega id do usuário
        int userId = int.Parse(PlayerPrefs.GetString("rememberId"));
        // Pega o id da pergunta (Através do nome do gameObject. Ex: Célula (2,4):984
        string[] str = m_gameManager.GetCurrentCell().GetPrimitive().transform.name.Split(':');
        // Pega a data do sistema
        string sDate = DateTime.Now.ToString();
        DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

        string dy = datevalue.Day.ToString();
        string mn = datevalue.Month.ToString();
        string yy = datevalue.Year.ToString();

        // Para salvar a resposta
        WWW www;

        // Se acertou...
        if (m_answerInputField.text == m_gameManager.GetCurrentCell().GetAnswer().ToString()) {
            // Pontuação = 10 * (Dificuldade da célula resolvida)
            m_gameManager.SubmitionScore = 10 * m_gameManager.GetCurrentCell().GetDifficulty();
            m_scoreLabel.text = m_gameManager.SubmitionScore.ToString();

            // Adiciona para WWWList
            www = new WWW(m_historyUrl + "?usuario=" + userId + "&fase=" + WWW.EscapeURL(m_loadedLevel) + "&pergunta=" + str[1] + "&resposta=" + m_answerInputField.text + "&acertou=" + 1 + "&data=" + (dy + "-" + mn + "-" + yy));
            StartCoroutine(ISaveAnswer(www));
            // Avança de casa. Se tiver o mecanismo de pular casas conforme a dificuldade
            // é aqui que isso deve ser tratado
            int index = m_gameManager.PositionedCells.IndexOf(m_gameManager.GetCurrentCell());
            // Verifica se não terminou o jogo
            if (index < m_gameManager.PositionedCells.Count - 1) {
                Cell tmp = m_gameManager.GetCurrentCell();
                // Avança a casa
                m_gameManager.SetCurrentCell(m_gameManager.PositionedCells[++index]);
                // Reseta a cor da célula antiga
                tmp.SetColor(Cell.MiddleCellColor);
                // Atualiza a UI com informação nova
                ResetUIInfo();
                LoadInfoToUI(m_gameManager.GetCurrentCell());
            } else {
                // Ganhou o jogo
                // Nova pontuação = score da pontuação - (2 * número de erros de submissão)
                m_gameManager.SubmitionScore -= (2 * m_gameManager.SubmitionErrors);
                StartCoroutine(WaitForUserResponse());
            }
        } else {
            m_gameManager.SubmitionErrors++;
            m_errorLabel.text = m_gameManager.SubmitionErrors.ToString();

            // Adiciona para WWWList
            www = new WWW(m_historyUrl + "?usuario=" + userId + "&fase=" + WWW.EscapeURL(m_loadedLevel) + "&pergunta=" + str[1] + "&resposta=" + m_answerInputField.text + "&acertou=" + 0 + "&data=" + (dy + "-" + mn + "-" + yy));
            StartCoroutine(ISaveAnswer(www));
        }
    }

    IEnumerator ISaveAnswer(WWW www) {
        yield return www;

        if (www.error == null) {
            if (www.text == "1") {
                Debug.Log("Salvou os dados com sucesso! " + www.url);
            } else {
                Debug.Log("Erro ao salvar. " + www.url);
            }
        } else {
            Debug.Log("Erro de conexão com o servidor. " + www.url);
        }
    }

    // Mostra a tela final, perguntando se o usuário quer
    // ou não jogar novamente.
    IEnumerator WaitForUserResponse () {
        m_winnerDialogPanel.SetActive(true);

        while (WinnerDialogResult == 0) {
            yield return null;
        }

        m_gameManager.SubmitionScore = 0;
        m_gameManager.SubmitionErrors = 0;
        m_scoreLabel.text = "0";
        m_errorLabel.text = "0";

        ResetUIInfo();
        m_gameManager.ClearAllCells();

        // Se quer jogar novamente
        if (WinnerDialogResult == 1) {
            RetrieveLevels();
        } else if (WinnerDialogResult == 2) {
            ReturnToMenu();
        }
        WinnerDialogResult = 0;
    }
  

    // Método auxiliar responsável por checar se o usuário está focalizando a UI
    // Utilizado pela propriedade IsFocusingUI
    bool CheckIfFocusingUI() {
        return (m_questionInputField.isFocused || m_answerInputField.isFocused);
    }

    // Método responsável por resetar informações da UI
    public void ResetUIInfo() {
        m_dialogLabel.text = "";
        m_cellLabel.text = "Célula (0,0)";
        m_questionInputField.text = "";
        m_answerInputField.text = "";
    }

    // Método responsável por carregar informações da célula para a UI
    public void LoadInfoToUI(Cell cell) {
        m_cellLabel.text = cell.GetPrimitive().transform.name;
        // Verifica se a célula possui outras informações preenchidas por
        // meio do campo de Perguntas
        m_cellLabel.text = "Célula (" + ((int)cell.GetPosition().x).ToString() + "," + ((int)cell.GetPosition().y).ToString() + ")";
        m_questionInputField.text = cell.GetQuestion();
        //m_answerInputField.text = (string.IsNullOrEmpty(m_questionInputField.text)) ? "" : cell.GetAnswer().ToString();
    }

    // Método responsável por retornar ao menu
    public void ReturnToMenu() {
        // O ideal seria verificar se o nível foi salvo
        // e mostrar um diálogo para deixar o usuário decidir
        // se deve ser salvo ou não, mas como estamos sem tempo...
        SceneManager.LoadScene("login", LoadSceneMode.Single);
    }

    // Método responsável por carregar o nível selecionado no painel de níveis disponíveis
    void LoadLevel(string id) {
        m_loadLevelPanel.SetActive(false);

        m_gameManager.ClearAllCells();

        m_loadedLevel = id;
        WWW www = new WWW(m_selectCellsUrl + "?fase=" + int.Parse(id));
        StartCoroutine(ILoadLevel(www));
    }

    // Corotina auxiliar para reconstruir o nível selecionado
    IEnumerator ILoadLevel(WWW www) {
        yield return www;

        bool result;
        int numOfLines, dataPerLine;

        if (www.error == null) {
            // Recebe os dados do SELECT referente à fase, casa e pergunta
            ArrayList data = MenuManager.GetDadosWWW(www, out result, out dataPerLine, out numOfLines);
            if (result == true)
            {
                for (int i = 0; i < (dataPerLine * numOfLines); i += 9)
                {
                    int x = int.Parse(data[i + 1].ToString());
                    int y = int.Parse(data[i + 2].ToString());

                    m_gridManager.GridMatrix[x, y].SetColor(Cell.MiddleCellColor);
                    m_gridManager.GridMatrix[x, y].SetCell(data[i + 4].ToString(), float.Parse(data[i + 5].ToString()), int.Parse(data[i + 6].ToString()), (Cell.OperationType)int.Parse(data[i + 7].ToString()));
                    m_gridManager.GridMatrix[x, y].SetStatus(true);
                    m_gridManager.GridMatrix[x, y].GetPrimitive().transform.name = (":" + data[i + 3]);

                    m_gameManager.AddSelectedCellToList(m_gridManager.GridMatrix[x, y]);

                    if (i == 0)
                    {
                        m_gameManager.SetCurrentCell(m_gridManager.GridMatrix[x, y]);
                        LoadInfoToUI(m_gridManager.GridMatrix[x, y]);
                    }
                }

                //m_gameManager.DrawPossiblePaths(m_gameManager.SelectedCells[m_gameManager.SelectedCells.Count - 1]);

                MenuManager.FeedBackOk(m_dialogLabel, "Carregou nível com sucesso.");
            }
            else MenuManager.FeedBackError(m_dialogLabel, "Erro ao carregar nível.");
        }

        m_loadLevelPanel.SetActive(false);
        m_gameManager.SetPlayable(true);
    }

    // Método responsável por mostrar o painel de níveis disponíveis
    public void RetrieveLevels() {
        m_loadLevelPanel.SetActive(true);
        m_gameManager.SetPlayable(false);

        // Remove possíveis resultados anteriores
        foreach (Transform child in GameObject.Find("Load Level Grid Layout").transform) {
            Destroy(child.gameObject);
        }

        WWW www = new WWW(m_selectLevelsUrl);
        StartCoroutine(IRetrieveLevels(www));
    }

    // Corotina auxiliar para mostrar todos os níveis disponíveis no BD
    IEnumerator IRetrieveLevels(WWW www) {
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
}
