using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Estatistica
{
    public class EstatisticaManager : MonoBehaviour
    {
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
    }
}
