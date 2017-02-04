using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Estatistica
{
    public class TelaFase : MonoBehaviour
    {

        private string urlAutor = "https://mathrun.000webhostapp.com/selectDadosFase_Autor.php";
        private string urlPontos = "https://mathrun.000webhostapp.com/selectDadosFase_Pontos.php";

        public Dropdown dropdown;
        [Space]
        public Text statusText;
        public Text nomeAutor;
        public Text DOB;
        public Text eMail;
        [Space]
        public Text nomeFase;
        public Text vezesJogada;
        public Text melhorAluno;
        public Text piorAluno;

        private int mIdFase;

        public enum Escopo { Ultimos30Dias, Total };
        Escopo escopo;

        void Start()
        {
            escopo = Escopo.Ultimos30Dias;
        }

        public void RecarregaFase()
        {
            escopo = (dropdown.value == 0 ? Escopo.Ultimos30Dias : Escopo.Total);
            CarregaDadosFase(mIdFase);
        }
        public void CarregaDadosFase(int idFase)
        {
            mIdFase = idFase;
            StartCoroutine(SelectDadosFase(idFase));
        }

        IEnumerator SelectDadosFase(int idFase)
        {
            ClearContent();
            FindObjectOfType<EstatisticaManager>().StartLoadingAnimation();

            bool sucessoAutor = false;

            WWW www = new WWW(urlAutor + "?id_fase=" + idFase);

            yield return www;
            if (www.error == null)
            {
                int numDadosLinha;
                int numLinhas;

                ArrayList dados = MenuManager.GetDadosWWW(www, out sucessoAutor, out numDadosLinha, out numLinhas);
                if (sucessoAutor)
                {
                    MenuManager.FeedBackOk(statusText, "Status: Consulta concluída!");
                    ExibirDadosAutor(dados, numDadosLinha, numLinhas);
                }
                else
                {
                    MenuManager.FeedBackError(statusText, "Status: Consulta concluída (nenhum dado desta fase encontrado).");
                }
            }
            else
            {
                MenuManager.FeedBackError(statusText, "Status: Erro ao conectar ao servidor.");
            }

            if (sucessoAutor)
            {
                bool sucesso;
                www = new WWW(urlPontos + "?id_fase=" + idFase);

                yield return www;
                if (www.error == null)
                {
                    int numDadosLinha;
                    int numLinhas;

                    ArrayList dados = MenuManager.GetDadosWWW(www, out sucesso, out numDadosLinha, out numLinhas);
                    if (sucesso)
                    {
                        MenuManager.FeedBackOk(statusText, "Status: Consulta concluída!");
                        ExibirDadosPontos(dados, numDadosLinha, numLinhas);
                    }
                    else
                    {
                        MenuManager.FeedBackError(statusText, "Status: Consulta concluída (esta fase ainda não foi jogada).");
                        //ExibirDadosPontosVazio();
                    }
                }
                else
                {
                    MenuManager.FeedBackError(statusText, "Status: Erro ao conectar ao servidor.");
                }
            }

            FindObjectOfType<EstatisticaManager>().StopLoadingAnimation();
        }

        private void ClearContent()
        {
            nomeFase.text = "";
            nomeAutor.text = "";
            DOB.text = "";
            eMail.text = "";
            vezesJogada.text = "";
            melhorAluno.text = "";
            piorAluno.text = "";
        }

        /*exibe dados do autor + o nome da fase*/
        private void ExibirDadosAutor(ArrayList dados, int largura, int altura)
        {
            nomeFase.text = dados[0 + 1].ToString();
            nomeAutor.text = dados[0 + 2].ToString();
            DOB.text = EstatisticaManager.CorrigeFormatoData(dados[0 + 3].ToString());
            eMail.text = dados[0 + 4].ToString();
        }

        private void ExibirDadosPontos(ArrayList dados, int largura, int altura)
        {
            Dictionary<string, int> listaPontos = new Dictionary<string, int>();

            for (int i = 0, k = 0; i < (largura * altura) && k < altura; i += largura, k++)
            {
                if (escopo == Escopo.Ultimos30Dias)
                {
                    string[] data = dados[i + 3].ToString().Split('-');
                    System.DateTime hoje = System.DateTime.Now.Date;
                    System.DateTime dia = new System.DateTime(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));
                    if ((hoje - dia).TotalDays > 30)
                        continue;
                }
                if (!listaPontos.ContainsKey(dados[i + 1].ToString()))
                {
                    listaPontos.Add(dados[i + 1].ToString(), 0);
                }

                if (int.Parse(dados[i + 2].ToString()) == 1)
                {
                    listaPontos[dados[i + 1].ToString()]++;
                }
            }

            string pior = "", melhor = "";
            int piorScore = int.MaxValue, melhorScore = int.MinValue;
            string[] keyArray = listaPontos.Keys.ToArray();
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (listaPontos[keyArray[i]] > melhorScore)
                {
                    melhorScore = listaPontos[keyArray[i]];
                    melhor = keyArray[i];
                }
                if (listaPontos[keyArray[i]] < piorScore)
                {
                    piorScore = listaPontos[keyArray[i]];
                    pior = keyArray[i];
                }
            }
            piorAluno.text = pior + " (" + piorScore + ")";
            melhorAluno.text = melhor + " (" + melhorScore + ")";
            vezesJogada.text = keyArray.Length.ToString();
        }

        //private void ExibirDadosPontosVazio()
        //{

        //}
    }    
}
