using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CadastroManager : MonoBehaviour {

    private string url = "https://mathrun.000webhostapp.com/cadastraUser.php";

    [SerializeField]
    private InputField nomeField = null;
    [SerializeField]
    private InputField emailField = null;
    [SerializeField]
    private InputField userField = null;
    [SerializeField]
    private InputField senhaField = null;
    [SerializeField]
    private InputField diaField = null;
    [SerializeField]
    private InputField mesField = null;
    [SerializeField]
    private InputField anoField = null;
    [SerializeField]
    private InputField profField = null;
    [SerializeField]
    private Toggle is_prof = null;
    [SerializeField]
    private Text messageText = null;
    [SerializeField]
    private Button cadastraButton = null;

    //telas
    [SerializeField]
    private GameObject telaCadastro = null;
    [SerializeField]
    private GameObject telaProfessor = null;
    [SerializeField]
    private GameObject telaAluno = null;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (nomeField.isFocused) emailField.Select();
            else if (emailField.isFocused) userField.Select();
            else if (userField.isFocused) senhaField.Select();
            else if (senhaField.isFocused) diaField.Select();
            else if (diaField.isFocused) mesField.Select();
            else if (mesField.isFocused) anoField.Select();
            else if (anoField.isFocused) profField.Select();
            else if (profField.isFocused) cadastraButton.Select();
            else if (cadastraButton.isActiveAndEnabled) nomeField.Select();
        }
    }

    public void FazerCadastro()
    {
        string nome = nomeField.text;
        string email = emailField.text;
        string login = userField.text;
        string senha = senhaField.text;
        string dia = diaField.text;
        string mes = mesField.text;
        string ano = anoField.text;
        string prof = profField.text;
        string isprof = "0";

        //verifica dados
        if (nome == "" || email == "" || login == "" || senha == "" || dia == "" || mes == "" || ano == "")
        {
            MenuManager.FeedBackError(messageText, "Digite todos os dados.");
        }
        else
        {
            int d = Convert.ToInt32(dia);
            int m = Convert.ToInt32(mes);
            int a = Convert.ToInt32(ano);

            if (!(ValidaData(d,m,a)))
            {
                MenuManager.FeedBackError(messageText, "Data Inválida.");
            }
            else {
                if (prof == "") prof = "1";
                if (is_prof.isOn)
                {
                    isprof = "1";
                    prof = "1";
                }
                else isprof = "0";

                //envia para o banco de dados
                WWW www = new WWW(url + "?nome=" + WWW.EscapeURL(nome) + "&email=" + WWW.EscapeURL(email) + "&login=" + WWW.EscapeURL(login) + "&senha=" + WWW.EscapeURL(senha) + "&data=" + dia + "-" + mes + "-" + ano + "&prof=" + prof + "&isprof=" + isprof);
                StartCoroutine(CadastraUser(www));
            }
        }
    }

    bool ValidaData (int dia, int mes, int ano)
    {
        if (dia > 31 || dia < 1 || mes > 12 || mes < 1 || ano < 1900 || ano > 2100)
        {
            //conferencia grosseira
            return false;
        }
        else if (mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12)
        {
            //confere meses com 31 dias
            return true;
        }
        else if (mes == 4 || mes == 6 || mes == 9 || mes == 11)
        {
            //confere meses com 30 dias
            if (dia <= 30) return true;
            else return false;
        }
        else if (mes == 2)
        {
            //verifica se ano é bissexto
            if (ano % 4 == 0 && !(ano % 100 == 0))
            {
                if (dia <= 29) return true;
                else return false;
            }
            else
            {
                if (dia <= 28) return true;
                else return false;
            }
        }
        else return false;
    }

    IEnumerator CadastraUser(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            //sucesso
            if (www.text == "1")
            {
                //carrega o jogo
                MenuManager.FeedBackOk(messageText, "Cadastro realizado com sucesso! \n Carregando...");
                StartCoroutine(CarregaScene());
            }
            else
            {
                MenuManager.FeedBackError(messageText, "Dados já existentes ou inválidos.");
            }
        }
        else
        {
            MenuManager.FeedBackError(messageText, "Erro ao conectar ao servidor.");
        }
    }

    IEnumerator CarregaScene()
    {
        yield return new WaitForSeconds(2);

        if (is_prof.isOn) ShowProfessor();
        else ShowAluno();
    }

    public void ShowProfessor()
    {
        messageText.text = "";
        telaCadastro.SetActive(false);
        telaProfessor.SetActive(true);
    }

    public void ShowAluno()
    {
        messageText.text = "";
        telaCadastro.SetActive(false);
        telaAluno.SetActive(true);
    }
}
