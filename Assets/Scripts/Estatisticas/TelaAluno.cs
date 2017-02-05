using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Estatistica
{
    public class TelaAluno : MonoBehaviour
    {

        //private string url = "https://mathrun.000webhostapp.com/selectPerguntasDoUsuario.php";
        private string urlFicha = "https://mathrun.000webhostapp.com/selectDadosAluno_Aluno.php";
        private string urlPontos = "https://mathrun.000webhostapp.com/selectDadosAluno_Pontos.php";
        public enum Escopo { Ultimos30Dias, Total };
        Escopo escopo;

        public Dropdown dropdown;
        [Space]
        public Text statusText;
        public Text nomeAluno;
        public Text DOB;
        public Text eMail;
        public Text total;
        public Text soma;
        public Text subtracao;
        public Text multiplicacao;
        public Text divisao;
        public Text misto;

        private int mIdAluno;

        private void Start()
        {
            escopo = Escopo.Ultimos30Dias;
        }

        private void ClearContent()
        {
            nomeAluno.text = "";
            DOB.text = "";
            eMail.text = "";
            total.text = "";
            soma.text = "";
            subtracao.text = "";
            multiplicacao.text = "";
            divisao.text = "";
            misto.text = "";
        }

        public void RecarregaAluno()
        {
            escopo = (dropdown.value == 0 ? Escopo.Ultimos30Dias : Escopo.Total);
            CarregaDadosAluno(mIdAluno);
        }

        public void CarregaDadosAluno(int idAluno)
        {
            mIdAluno = idAluno;
            StartCoroutine(SelectPerguntaUsuario());
        }

        IEnumerator SelectPerguntaUsuario()
        {
            ClearContent();
            FindObjectOfType<EstatisticaManager>().StartLoadingAnimation();

            bool sucessoAluno = false;
            WWW www = new WWW(urlFicha + "?id_usuario=" + mIdAluno);

            yield return www;
            if (www.error == null)
            {
                int numDadosLinha;
                int numLinhas;

                ArrayList dados = MenuManager.GetDadosWWW(www, out sucessoAluno, out numDadosLinha, out numLinhas);
                if (sucessoAluno)
                {
                    MenuManager.FeedBackOk(statusText, "Status: Consulta concluída!");
                    ExibirDadosAluno(dados, numDadosLinha, numLinhas);
                }
                else
                {
                    MenuManager.FeedBackError(statusText, "Status: Consulta concluída (nenhum dado deste aluno encontrado).");
                }
            }
            else
            {
                MenuManager.FeedBackError(statusText, "Status: Erro ao conectar ao servidor.");
            }

            if (sucessoAluno)
            {
                bool sucesso;
                www = new WWW(urlPontos + "?id_usuario=" + mIdAluno);
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
                        MenuManager.FeedBackError(statusText, "Status: Consulta concluída (este aluno ainda não jogou).");
                    }
                }
                else
                {
                    MenuManager.FeedBackError(statusText, "Status: Erro ao conectar ao servidor.");
                }

            }

            FindObjectOfType<EstatisticaManager>().StopLoadingAnimation();
        }

        void ExibirDadosAluno(ArrayList dados, int largura, int altura)
        {
            nomeAluno.text = dados[0 + 1].ToString();
            DOB.text = EstatisticaManager.CorrigeFormatoData(dados[0 + 2].ToString());
            eMail.text = dados[0 + 3].ToString();
        }

        void ExibirDadosPontos(ArrayList dados, int largura, int altura)
        {
            int totalGeral = altura;
            int acertoGeral = 0;
            int totalSoma = 0;
            int acertoSoma = 0;
            int totalSubt = 0;
            int acertoSubt = 0;
            int totalDiv = 0;
            int acertoDiv = 0;
            int totalMult = 0;
            int acertoMult = 0;
            int totalMisto = 0;
            int acertoMisto = 0;

            for (int i = 0, k = 0; i < (largura * altura) && k < altura; i += largura, k++)
            {
                if (escopo == Escopo.Ultimos30Dias)
                {
                    string[] data = dados[i + 2].ToString().Split('-');
                    System.DateTime hoje = System.DateTime.Now.Date;
                    System.DateTime dia = new System.DateTime(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));
                    if ((hoje - dia).TotalDays > 30)
                        continue;
                }
                switch (int.Parse(dados[i+3].ToString()))
                {
                    case 0: //SOMA
                        totalSoma++;
                        if (int.Parse(dados[i + 1].ToString()) == 1)
                        {
                            acertoSoma++;
                            acertoGeral++;
                        }
                        break;
                    case 1: // SUBT
                        totalSubt++;
                        if (int.Parse(dados[i + 1].ToString()) == 1)
                        {
                            acertoSubt++;
                            acertoGeral++;
                        }
                        break;
                    case 2: //MULT
                        totalMult++;
                        if (int.Parse(dados[i + 1].ToString()) == 1)
                        {
                            acertoMult++;
                            acertoGeral++;
                        }
                        break;
                    case 3: //DIV
                        totalDiv++;
                        if (int.Parse(dados[i + 1].ToString()) == 1)
                        {
                            acertoDiv++;
                            acertoGeral++;
                        }
                        break;
                    case 4: //MISTO
                        totalMisto++;
                        if (int.Parse(dados[i + 1].ToString()) == 1)
                        {
                            acertoMisto++;
                            acertoGeral++;
                        }
                        break;
                    default:
                        break;
                }
            }

            WriteAndPaintScores(total, acertoGeral, totalGeral);
            WriteAndPaintScores(soma, acertoSoma, totalSoma);
            WriteAndPaintScores(subtracao, acertoSubt, totalSubt);
            WriteAndPaintScores(multiplicacao, acertoMult, totalMult);
            WriteAndPaintScores(divisao, acertoDiv, totalDiv);
            WriteAndPaintScores(misto, acertoMisto, totalMisto);
        }

        private void WriteAndPaintScores(Text t, int scored, int total)
        {
            if (scored > total / 2f)
            {
                t.color = new Color(201 / 255f, 1, 184 / 255f);
            } else
            {
                t.color = new Color(1, 171 / 255f, 171 / 255f);
            }

            t.text = scored + "/" + total;
        }
    }
}
