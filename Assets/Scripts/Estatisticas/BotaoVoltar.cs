using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoVoltar : MonoBehaviour {

    public void VoltarSelecao()
    {
        Estatistica.EstatisticaManager mgr = FindObjectOfType<Estatistica.EstatisticaManager>();
        if (mgr.isProf)
        {
            mgr.telaSelecao.gameObject.SetActive(true);
            mgr.telaSelecao.RecalculateList();
            mgr.telaAluno.gameObject.SetActive(false);
            mgr.telaFase.gameObject.SetActive(false);
        } else
        {
            mgr.ReturnToMainScreen();
        }
    }
}
