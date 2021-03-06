﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

    private static string id = null;
    private string is_prof = null;

    private string url = "https://mathrun.000webhostapp.com/login.php";

    [SerializeField]
    private InputField userField = null;
    [SerializeField]
    private InputField senhaField = null;
    [SerializeField]
    private Toggle rememberData = null;
    [SerializeField]
    private Text messageText = null;
    [SerializeField]
    private Button loginButton = null;

    //telas
    [SerializeField]
    private GameObject telaLogin = null;
    [SerializeField]
    private GameObject telaProfessor = null;
    [SerializeField]
    private GameObject telaAluno = null;

    // Use this for initialization
    void Start () {
        //carrega dados salvos
	    if (PlayerPrefs.HasKey("remember") && PlayerPrefs.GetInt("remember") == 1)
        {
            userField.text = PlayerPrefs.GetString("rememberLogin");
            senhaField.text = PlayerPrefs.GetString("rememberSenha");
        }

        //focus no input user
        userField.Select();
    }
    
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (userField.isFocused) senhaField.Select();
            else if (senhaField.isFocused) loginButton.Select();
            else if (loginButton.isActiveAndEnabled) userField.Select();
        }
	}

    public void FazerLogin()
    {
        string login = userField.text;
        string senha = senhaField.text;

        //verifica dados
        if (login == "" || senha == "")
        {
            MenuManager.FeedBackError(messageText, "Digite todos os dados.");
        }
        else
        {
            //busca no banco de dados
            WWW www = new WWW(url + "?login=" + WWW.EscapeURL(login) + "&senha=" + WWW.EscapeURL(senha));
            StartCoroutine(ValidaLogin(www, login, senha));
        }
    }

    IEnumerator ValidaLogin(WWW www, string login, string senha)
    {
        yield return www;
        bool resultado = false;
        int dadosPorLinha = 0;
        int numLinhas = 0;

        if (www.error == null)
        {
            ArrayList dados = MenuManager.GetDadosWWW(www, out resultado, out dadosPorLinha, out numLinhas);
            //sucesso
            if (resultado == true)
            {
                id = dados[0].ToString();
                is_prof = dados[1].ToString();
            
            
                //salva dados do player
                if (rememberData.isOn) SalvaPlayerPrefs(1, id, login, senha, is_prof);
                else SalvaPlayerPrefs(0, id, "", "", "");

                //carrega o jogo
                MenuManager.FeedBackOk(messageText, "Login realizado com sucesso! \n Carregando...");
                StartCoroutine(CarregaScene());
            }
            else MenuManager.FeedBackError(messageText, "Usuario e/ou senha incorretos.");
        }
        else MenuManager.FeedBackError(messageText, "Erro ao conectar ao servidor.");
    }

    void SalvaPlayerPrefs (int remember, string id, string login, string senha, string is_prof)
    {
        PlayerPrefs.SetInt("remember", remember);
        PlayerPrefs.SetString("rememberId", id);
        PlayerPrefs.SetString("rememberLogin", login);
        PlayerPrefs.SetString("rememberSenha", senha);
        PlayerPrefs.SetString("rememberIsProf", is_prof);
    }

    IEnumerator CarregaScene ()
    {
        yield return new WaitForSeconds(2);

        if (is_prof == "1") ShowProfessor();
        else ShowAluno();
    }

    public void ShowProfessor()
    {
        messageText.text = "";
        telaLogin.SetActive(false);
        telaProfessor.SetActive(true);
    }

    public void ShowAluno()
    {
        messageText.text = "";
        telaLogin.SetActive(false);
        telaAluno.SetActive(true);
    }

    public static string GetIdUser ()
    {
        return id;
    }
}
