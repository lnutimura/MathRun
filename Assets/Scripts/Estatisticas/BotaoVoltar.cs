using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoVoltar : MonoBehaviour {

    public void VoltarSelecao()
    {
        if (FindObjectOfType<Estatistica.EstatisticaManager>().isProf)
        {
            FindObjectOfType<Estatistica.TelaSelecao>().gameObject.SetActive(true);
            FindObjectOfType<Estatistica.TelaSelecao>().RecalculateList();
            FindObjectOfType<Estatistica.TelaAluno>().gameObject.SetActive(false);
        } else
        {
            FindObjectOfType<Estatistica.EstatisticaManager>().ReturnToMainScreen();
        }
    }
}
