using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Estatistica
{
    public class TelaAluno : MonoBehaviour
    {

        private string url = "https://mathrun.000webhostapp.com/selectPerguntasDoUsuario.php";

        public enum Escopo { Ultimos30Dias, Total };
        Escopo escopo;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !FindObjectOfType<EstatisticaManager>().isLoading)
            {
                CarregaDadosAluno(0);
            }
        }

        public void CarregaDadosAluno(int idAluno)
        {
            WWW www = new WWW(url + "?id_usuario=" + idAluno);
            StartCoroutine(SelectPerguntaUsuario(www));
        }

        IEnumerator SelectPerguntaUsuario(WWW www)
        {
            ClearContent();
            FindObjectOfType<EstatisticaManager>().StartLoadingAnimation();

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
                    ExibirDados(dados, numDadosLinha, numLinhas);
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

            FindObjectOfType<EstatisticaManager>().StopLoadingAnimation();
        }

        void ExibirDados(ArrayList dados, int largura, int altura)
        {
            nomeAluno.text = dados[0 + 6].ToString();
            DOB.text = dados[0 + 7].ToString();
            eMail.text = dados[0 + 8].ToString();

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
                print(dados[i + 4].ToString());
                switch ((int)dados[i+2])
                {
                    case 0: //SOMA
                        totalSoma++;
                        if ((int)dados[i + 3] == 1)
                        {
                            acertoSoma++;
                            acertoGeral++;
                        }
                        break;
                    case 1: // SUBT
                        totalSubt++;
                        if ((int)dados[i + 3] == 1)
                        {
                            acertoSubt++;
                            acertoGeral++;
                        }
                        break;
                    case 2: //MULT
                        totalMult++;
                        if ((int)dados[i + 3] == 1)
                        {
                            acertoMult++;
                            acertoGeral++;
                        }
                        break;
                    case 3: //DIV
                        totalDiv++;
                        if ((int)dados[i + 3] == 1)
                        {
                            acertoDiv++;
                            acertoGeral++;
                        }
                        break;
                    case 4: //MISTO
                        totalMisto++;
                        if ((int)dados[i + 3] == 1)
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
                t.color = Color.green;
            } else
            {
                t.color = Color.red;
            }

            t.text = scored + "/" + total;
        }
    }
}
