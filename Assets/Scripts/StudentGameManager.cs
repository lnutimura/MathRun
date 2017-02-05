/*
 * Esse script deve ser atrelado à um GameObject.
 * O intuito dele é controlar o jogo do estudante em todos os seus aspectos.
 * O ideal seria colocar tudo isso no próprio GameManager para não ter certa
 * redundância, mas não temos tempo, kek
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentGameManager : MonoBehaviour {
    // Lista das células carregadas,
    // na ordem em que o professor posicionou elas
    public List<Cell> PositionedCells;


    // Informações sobre o jogo do estudante
    public int SubmitionScore;
    public int SubmitionErrors;

    // Referência para o outros Managers
    private StudentUIManager m_uiManager;
    private GridManager m_gridManager;

    // Célula selecionada atualmente
    private Cell m_currentSelectedCell;

    // Variável utilizada na animação da célula selecionada
    private float m_lerpValue;

    // Variável para controlar a gameplay
    private bool m_isPlayable;

    void Start () {
        m_lerpValue = 0f;
        SubmitionScore = 0;
        m_isPlayable = true;
        SubmitionErrors = 0;
        m_currentSelectedCell = null;
        PositionedCells = new List<Cell>();

        m_uiManager = GameObject.Find("UI Manager").GetComponent<StudentUIManager>();
        m_gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
    }

    void Update () {
        if (m_isPlayable) {
            // Anima a célula selecionada atualmente
            if (m_currentSelectedCell != null) {
                AnimateCell(m_currentSelectedCell);
            }
        }
    }

    // Método auxiliar usado por outras classes
    public void SetCurrentCell (Cell cell) {
        m_currentSelectedCell = cell;
    }

    // Método auxiliar usado por outras classes
    public Cell GetCurrentCell () {
        return m_currentSelectedCell;
    }

    // Método auxiliar usado para controlar quando o usuário pode ou não jogar
    public void SetPlayable (bool status) {
        m_isPlayable = status;
    }

    // Método responsável por animar uma célula
    void AnimateCell(Cell cell) {
        cell.SetColor(Color.Lerp(Cell.MiddleCellColor, Color.magenta, Mathf.Abs(Mathf.Cos(m_lerpValue))));
        m_lerpValue = (m_lerpValue + Time.deltaTime * 2f) % Mathf.PI;
    }

    // Método responsável por limpar o tabuleiro de todas as células posicionadas
    // Utiliza o mecanismo de rollback
    public void ClearAllCells() {
        while (PositionedCells.Count != 0) {
            PositionedCells[PositionedCells.Count - 1].GetPrimitive().transform.name = PositionedCells[PositionedCells.Count - 1].GetPrimitive().transform.name.Split(':')[0];
            Rollback();
        }
    }

    // Método auxiliar responsável por desfazer a última seleção
    void Rollback() {
        // Verifica se existem itens na lista,
        // caso contrário não é preciso fazer rollback
        if (PositionedCells.Count != 0) {
            // Se existe um único elemento
            if (PositionedCells.Count == 1) {
                // Remove o elemento da lista
                PositionedCells.Remove(m_currentSelectedCell);

                // Reseta as informações da célula
                Cell tmp = m_currentSelectedCell;

                m_currentSelectedCell.ResetCell();
                m_currentSelectedCell = null;

                tmp.SetColor(Cell.DefaultColor);

                // Reseta as informações da UI
                //m_uiManager.ResetUIInfo();
            } else {
                // Armazena o último e o penúltimo elemento da lista
                Cell tmp1 = PositionedCells[PositionedCells.Count - 1];
                Cell tmp2 = PositionedCells[PositionedCells.Count - 2];

                // Remove o último elemento da lista
                PositionedCells.Remove(tmp1);

                // Reseta as informações do último elemento selecionado e
                // atualiza o elemento que foi selecionado
                tmp1.ResetCell();
                m_currentSelectedCell = tmp2;

                // Restaura a cor branca para a célula que sofreu rollback
                tmp1.SetColor(Cell.DefaultColor);

                // Carrega as informações da célula selecionada atualmente para a UI
                //m_uiManager.LoadInfoToUI(m_currentSelectedCell);
            }
        }
    }

    // Método auxiliar usado por OUTROS scripts, para poderem adicionar
    // novas células à lista de selecionados, sem precisar tornar a variável
    // m_currentSelectedCell pública
    // Até o momento o único script que referencia esse método é o UIManager.LoadLevel()
    // na hora de reconstruir o tabuleiro do banco de dados
    public void AddSelectedCellToList(Cell cell) {
        PositionedCells.Add(cell);
    }
}
