using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CriaPergunta : MonoBehaviour
{

    private string url = "https://mathrun.000webhostapp.com/cadastraPergunta.php";

    [SerializeField]
    private InputField perguntaField = null;
    [SerializeField]
    private InputField respostaField = null;
    [SerializeField]
    private InputField dificuldadeField = null;
    [SerializeField]
    private Dropdown tipoDropdown = null;
    [SerializeField]
    private Text messageText = null;
    [SerializeField]
    private Button salvarButton = null;

    //telas
    [SerializeField]
    private GameObject telaProfessor = null;
    [SerializeField]
    private GameObject telaPergunta = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (perguntaField.isFocused) respostaField.Select();
            else if (respostaField.isFocused) dificuldadeField.Select();
            else if (dificuldadeField.isFocused) tipoDropdown.Select();
            else if (tipoDropdown.isActiveAndEnabled) salvarButton.Select();
            else if (salvarButton.isActiveAndEnabled) perguntaField.Select();
        }
    }

    public void SalvarPergunta()
    {
        string pergunta = perguntaField.text;
        string resposta = respostaField.text;
        string dificuldade = dificuldadeField.text;
        int tipo = tipoDropdown.value;
        string autor = LoginManager.GetIdUser();

        //verifica dados
        if (pergunta == "" || resposta == "" || dificuldade == "" )
        {
            MenuManager.FeedBackError(messageText, "Digite todos os dados.");
        }
        else
        {
            int dificult = Convert.ToInt32(dificuldade);

            if (dificult < 1 || dificult > 10) MenuManager.FeedBackError(messageText, "Dificuldade Inválida.");
            else
            {   
                //envia para o banco de dados
                WWW www = new WWW(url + "?questao=" + WWW.EscapeURL(pergunta) + "&resposta=" + resposta + "&dificuldade=" + dificuldade + "&tipo=" + tipo + "&autor=" + autor);
                StartCoroutine(CadastraPergunta(www));
            }
        }
    }

    IEnumerator CadastraPergunta(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //sucesso
            if (www.text == "1")
            {
                MenuManager.FeedBackOk(messageText, "Pergunta criada com sucesso!");
            }
            else
            {
                MenuManager.FeedBackError(messageText, "Dados inválidos.");
            }
        }
        else
        {
            MenuManager.FeedBackError(messageText, "Erro ao conectar ao servidor.");
        }
    }

    public void VoltaProfessor()
    {
        messageText.text = "";
        telaPergunta.SetActive(false);
        telaProfessor.SetActive(true);
    }

    public void ShowCriaPergunta()
    {
        messageText.text = "";
        telaProfessor.SetActive(false);
        telaPergunta.SetActive(true);
    }
}
