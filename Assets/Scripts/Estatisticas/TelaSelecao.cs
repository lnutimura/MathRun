using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Estatistica
{
    public class TelaSelecao : MonoBehaviour
    {

        private string urlFases = "https://mathrun.000webhostapp.com/selectFases.php";
        private string urlAlunos = "https://mathrun.000webhostapp.com/selectAlunos.php";

        public RectTransform Content;
        public RectTransform ButtonPrefab;
        public Dropdown dropdown;
        public Text statusText;

        public int NoOfButtons;
        public float ySpacing;

        public void RecalculateList()
        {
            StartCoroutine(SelectTuplas(dropdown.value));
        }

        void ClearContent()
        {
            int count = Content.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Transform t = Content.GetChild(i);
                Destroy(t.gameObject);
            }
        }

        IEnumerator SelectTuplas(int value)
        {
            ClearContent();
            FindObjectOfType<EstatisticaManager>().StartLoadingAnimation();

            WWW www;
            if (value == 0)
            {
                www = new WWW(urlFases);
            }
            else if (value == 1)
            {
                www = new WWW(urlAlunos);
            }
            else
            {
                print("no value");
                throw new System.Exception();
            }

            yield return www;
            if (www.error == null)
            {
                bool sucesso;
                int numDadosLinha;
                int numLinhas;

                ArrayList dados = MenuManager.GetDadosWWW(www, out sucesso, out numDadosLinha, out numLinhas);
                if (sucesso)
                {
                    MenuManager.FeedBackOk(statusText, "Status: Consulta concluída!");
                    ExibirDados(dados, numDadosLinha, numLinhas, value);
                }
                else
                {
                    MenuManager.FeedBackError(statusText, "Status: Consulta concluída (nenhum dado de " + (value == 1 ? "alunos" : "fases") + " encontrado).");
                }
            }
            else
            {
                MenuManager.FeedBackError(statusText, "Status: Erro ao conectar ao servidor.");
            }

            FindObjectOfType<EstatisticaManager>().StopLoadingAnimation();
        }

        private void ExibirDados(ArrayList dados, int largura, int altura, int fasesOuAlunos)
        {
            NoOfButtons = altura;
            GameObject[] allButtons = SpawnButtons();

            for (int i = 0, k = 0; i < (largura * altura) && k < altura; i += largura, k++)
            {
                allButtons[k].GetComponentInChildren<Text>().text = dados[i + 1].ToString();
            }
        }


        /*SpawnButtons():
         * instancia um botão para cada fase, e os atribui a um vetor para facilitar acesso
         */
        GameObject[] SpawnButtons()
        {
            GameObject[] allButtons = new GameObject[NoOfButtons];
            float buttonHeight = ButtonPrefab.sizeDelta.y;

            Vector2 sizeDelta = Content.sizeDelta;
            Content.sizeDelta = new Vector2(sizeDelta.x, buttonHeight * NoOfButtons + Mathf.Max(0, (NoOfButtons - 1)) * ySpacing);

            for (int i = 0; i < NoOfButtons; i++)
            {
                allButtons[i] = Instantiate(ButtonPrefab.gameObject) as GameObject;
                allButtons[i].transform.SetParent(Content);
                allButtons[i].name = (dropdown.value == 0 ? "F" : "A") + i;
                allButtons[i].transform.localPosition = new Vector3(928, (buttonHeight / 2f + buttonHeight * i + ySpacing * i) * -1, 0);
                allButtons[i].transform.localScale = Vector3.one;
                allButtons[i].GetComponent<Button>().onClick.AddListener(delegate
                {
                    SelecaoParaPrincipal(allButtons[i].name);
                });
            }

            return allButtons;
        }

        public void SelecaoParaPrincipal(string buttonName)
        {
            switch (buttonName[0])
            {
                case 'F':
                    FindObjectOfType<TelaFase>().gameObject.SetActive(true);
                    break;
                case 'A':
                    FindObjectOfType<TelaAluno>().gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}
