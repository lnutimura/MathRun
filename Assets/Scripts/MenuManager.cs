using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    
    //telas
    [SerializeField]
    private GameObject telaLogin = null;
    [SerializeField]
    private GameObject telaCadastro = null;
    [SerializeField]
    private GameObject telaProfessor = null;
    [SerializeField]
    private GameObject telaAluno = null;
    [SerializeField]
    private GameObject telaPergunta = null;

    // Use this for initialization
    void Start () {
        telaCadastro.SetActive(false);
        telaProfessor.SetActive(false);
        telaAluno.SetActive(false);
        telaPergunta.SetActive(false);
        telaLogin.SetActive(true);
    }

    public void Jogar()
    {
        //StartCoroutine(CarregaScene("Jogo"));
    }

    public void CriarFase()
    {
        StartCoroutine(CarregaScene("Main"));
    }

    public void Estatisticas()
    {
        //StartCoroutine(CarregaScene("Estatisticas"));
    }

    IEnumerator CarregaScene(string nome)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nome, LoadSceneMode.Single);
    }

    public void ShowCadastro()
    {
        telaLogin.SetActive(false);
        telaCadastro.SetActive(true);
    }

    public void ShowLogin()
    {
        telaCadastro.SetActive(false);
        telaLogin.SetActive(true);
    }

    public void Sair()
    {
        telaProfessor.SetActive(false);
        telaAluno.SetActive(false);
        telaLogin.SetActive(true);
    }

    public static void FeedBackOk(Text messageText, string mensagem)
    {
        messageText.CrossFadeAlpha(100f, 0f, false);
        messageText.color = Color.yellow;
        messageText.text = mensagem;
        messageText.CrossFadeAlpha(0f, 2f, false);
    }

    public static void FeedBackError(Text messageText, string mensagem)
    {
        messageText.CrossFadeAlpha(100f, 0f, false);
        messageText.color = new Color(1, 0.63F, 0); //orange
        messageText.text = mensagem;
        messageText.CrossFadeAlpha(0f, 2f, false);
    }

    public static ArrayList GetDadosWWW(WWW www, out bool sucesso, out int numDadosLinha, out int numLinhas)
    {
        ArrayList dados = new ArrayList();
        string resultado = "";
        int cont = 0;

        //tratamento da string HTML recebida
        string[] linhas = www.text.Split('&'); //resultado & dados
        foreach (string linha in linhas)
        {
            if (linha.StartsWith("id")) //a segunda parte (apos o &) traz os dados do usuario
            {
                int inicioNum = linha.IndexOf("=");
                string resto_linha = linha.Substring(inicioNum + 1);

                string[] dado = resto_linha.Split('#'); //cada dado esta separado por um #
                cont = 0;
                foreach (string palavra in dado)
                {
                    dados.Add(palavra);
                    cont++;
                }
            }
            else resultado = linha; //a primeira parte indica sucesso (1) ou fracasso (0)
        }

        if (resultado == "1") sucesso = true;
        else sucesso = false;
        numDadosLinha = cont;

        try {
            numLinhas = dados.Count / numDadosLinha;
        } catch {
            numLinhas = 0;
            Debug.Log("Divisão por zero no cálculo de linhas. Assumindo que nenhum dado foi retornado.");
        }
        return dados;
    }
}
