using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Estatistica
{
    public class EstatisticaManager : MonoBehaviour
    {
        public TelaSelecao telaSelecao;
        public TelaAluno telaAluno;
        public TelaFase telaFase;

        public bool isProf;

        public void Start()
        {
            string is_prof = PlayerPrefs.GetString("rememberIsProf");
            if (is_prof == "1")
            {
                isProf = true;
                telaSelecao.gameObject.SetActive(true);
                telaSelecao.RecalculateList();
            } else
            {
                isProf = false;
                telaAluno.gameObject.SetActive(true);
                string idString = PlayerPrefs.GetString("rememberId");
                telaAluno.CarregaDadosAluno(int.Parse(idString));
            }
        }

        #region LOADING_SCREEN
        public RectTransform loadObject;
        public Image background;
        public Image icon;
        private Coroutine loadingRoutine;
        public bool isLoading;


        public void StartLoadingAnimation()
        {
            isLoading = true;
            icon.fillAmount = 0;
            icon.fillClockwise = true;
            loadObject.gameObject.SetActive(true);
            loadingRoutine = StartCoroutine(LoadingAnimation());
        }

        public void StopLoadingAnimation()
        {
            StopCoroutine(loadingRoutine);
            loadObject.gameObject.SetActive(false);
            isLoading = false;
        }

        IEnumerator LoadingAnimation()
        {
            while (true)
            {
                while (icon.fillAmount < 1)
                {
                    icon.fillAmount += Time.deltaTime;
                    yield return null;
                }

                icon.fillClockwise = false;
                yield return new WaitForSeconds(0.2f);

                while (icon.fillAmount > 0)
                {
                    icon.fillAmount -= Time.deltaTime;
                    yield return null;
                }

                icon.fillClockwise = true;
                yield return new WaitForSeconds(0.2f);
            }
        }
        #endregion

        public void ReturnToMainScreen()
        {
            SceneManager.LoadScene("login");
        }
    }
}
