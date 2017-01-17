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
	
	// Update is called once per frame
	void Update () {
		
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
}
