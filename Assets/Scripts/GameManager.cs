/*
 * Esse script deve ser atrelado à um GameObject.
 * O intuito dele é controlar o jogo em todos os seus aspectos.
 * Ele é responsável por permitir que modificações em células tenham
 * impactos em componentes da Unity, por exemplo, nas cores das primitivas.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Lista de todas as células selecionadas até o momento
    // (Útil para o mecanismo de rollback)
    public  List<Cell> SelectedCells;

    // Referência para o outros Managers
    private UIManager m_uiManager;
    private GridManager m_gridManager;

    // Célula selecionada atualmente
    private Cell m_currentSelectedCell;

    // Variável utilizada na animação da célula selecionada
    private float m_lerpValue;

    // Variável para controlar a gameplay
    private bool m_isPlayable;

    void Start () {
        m_lerpValue = 0f;
        m_isPlayable = true;
        m_currentSelectedCell = null;
        SelectedCells = new List<Cell>();
        
        m_uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        m_gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
    }

    void Update () {
        if (m_isPlayable) {
            // Verifica se o usuário apertou o botão de rollback
            if (Input.GetKeyDown(KeyCode.Backspace) && !m_uiManager.IsFocusingUI) {
                Rollback();
            }
            // Anima a célula selecionada atualmente
            if (m_currentSelectedCell != null) {
                AnimateCell(m_currentSelectedCell);
            }
        }
    }

    // Método auxiliar usado para controlar quando o usuário pode ou não jogar
    public void SetPlayable (bool status) {
        m_isPlayable = status;
    }

    // Método responsável por tratar o clique do usuário em uma célula
    public void SelectedCell (Vector3 cellCoordinates) {
        if (m_isPlayable) {
            int cellX = (int)cellCoordinates.x;
            int cellY = (int)cellCoordinates.y;

            // Verifica se a célula selecionada não foi selecionada anteriormente
            if (!m_gridManager.GridMatrix[cellX, cellY].GetStatus()) {
                // Verifica se a célula selecionada é a primeira do tabuleiro
                if (m_currentSelectedCell == null) {
                    // Atualiza a variável com a nova célula selecionada
                    m_currentSelectedCell = m_gridManager.GridMatrix[cellX, cellY];
                    m_currentSelectedCell.SetStatus(true);
                    // Desenha os caminhos possíveis
                    DrawPossiblePaths(m_currentSelectedCell);
                    // Adiciona à lista para manter rastreio de todas as células já posicionadas
                    SelectedCells.Add(m_currentSelectedCell);
                    // Carrega as informações da célula
                    m_uiManager.LoadInfoToUI(m_currentSelectedCell);
                } else {
                    // Se já existem células posicionadas no tabuleiro, e considerando que o usuário
                    // pode ficar "passeando" entre as células já selecionadas, ele só pode realizar movimentos
                    // a partir da última célula selecionada
                    if (m_currentSelectedCell == SelectedCells[SelectedCells.Count - 1]) {
                        // Se não é a primeira célula do tabuleiro, é preciso checar
                        // se é uma célula viável
                        int diffX, oldCellX;
                        int diffY, oldCellY;

                        oldCellX = (int)m_currentSelectedCell.GetPosition().x;
                        oldCellY = (int)m_currentSelectedCell.GetPosition().y;

                        diffX = Mathf.Abs(oldCellX - cellX);
                        diffY = Mathf.Abs(oldCellY - cellY);

                        // Portanto, verifica se a distância entre a célula antiga e a nova célula é igual à 1
                        // e se os arredores da nova célula não irá criar ambiguidades no tabuleiro
                        if (((diffX == 1 && diffY == 0) || (diffX == 0 && diffY == 1)) && CheckSurroundings(cellX, cellY)) {
                            Cell tmp = m_currentSelectedCell;
                            // Atualiza a variável com a nova célula selecionada
                            m_currentSelectedCell = m_gridManager.GridMatrix[cellX, cellY];
                            m_currentSelectedCell.SetStatus(true);
                            // Limpa os caminhos possíveis antigos e os substitui com os novos caminhos
                            ClearPossiblePaths();
                            DrawPossiblePaths(m_currentSelectedCell);
                            // Deixa a célula antiga com a coloração vermelha
                            tmp.SetColor(Cell.MiddleCellColor);
                            // Adiciona à lista para manter rastreio de todas as células já posicionadas
                            SelectedCells.Add(m_currentSelectedCell);
                            // Salva as informações da célula antiga e reseta a UI
                            m_uiManager.SaveInfoToCell(tmp);
                            m_uiManager.ResetUIInfo();
                            // Carrega novas informações da célula selecionada atualmente
                            m_uiManager.LoadInfoToUI(m_currentSelectedCell);
                        } else {
                            Debug.Log("The selected cell is too far away.");
                        }
                    } else {
                        Debug.Log("Tried to perform a movement without being the last cell inside the list.");
                    }
                }
            } else {
                Cell tmp = m_currentSelectedCell;
                // Atualiza a variável com a nova célula selecionada
                m_currentSelectedCell = m_gridManager.GridMatrix[cellX, cellY];
                // Restaura a cor da célula selecionada anteriormente
                tmp.SetColor(Cell.MiddleCellColor);
                // Salva as informações da UI para a célula selecionada anteriormente
                // e reseta a UI
                m_uiManager.SaveInfoToCell(tmp);
                m_uiManager.ResetUIInfo();
                // Carrega para a UI as novas informações da célula selecionada atualmente
                m_uiManager.LoadInfoToUI(m_currentSelectedCell);
            }
        }
    }

    // Método responsável por animar uma célula
    void AnimateCell (Cell cell) {
        cell.SetColor(Color.Lerp(Cell.MiddleCellColor, Color.magenta, Mathf.Abs(Mathf.Cos(m_lerpValue))));
        m_lerpValue = (m_lerpValue + Time.deltaTime * 2f) % Mathf.PI;
    }

    // Método responsável por desenhar os caminhos possíveis a partir da célula selecionada atualmente
    public void DrawPossiblePaths (Cell cell) {
        int cellX = (int)cell.GetPosition().x;
        int cellY = (int)cell.GetPosition().y;

        // Checa a disponibilidade da célula imediatamente a esquerda da atual selecionada
        if (cellX > 0 && !m_gridManager.GridMatrix[cellX - 1, cellY].GetStatus() && CheckSurroundings(cellX - 1, cellY)) {
            m_gridManager.GridMatrix[cellX - 1, cellY].SetColor(Cell.SurroundingCellColor);
        }

        // Checa a disponibilidade da célula imediatamente abaixo da atual selecionada
        if (cellY > 0 && !m_gridManager.GridMatrix[cellX, cellY - 1].GetStatus() && CheckSurroundings(cellX, cellY - 1)) {
            m_gridManager.GridMatrix[cellX, cellY - 1].SetColor(Cell.SurroundingCellColor);
        }

        // Checa a disponibilidade da célula imediatamente a direita da atual selecionada
        if (cellX < m_gridManager.GridLines - 1 && !m_gridManager.GridMatrix[cellX + 1, cellY].GetStatus() && CheckSurroundings(cellX + 1, cellY)) {
            m_gridManager.GridMatrix[cellX + 1, cellY].SetColor(Cell.SurroundingCellColor);
        }

        // Checa a disponibilidade da célula imediatamente acima da atual selecionada
        if (cellY < m_gridManager.GridColumns - 1 && !m_gridManager.GridMatrix[cellX, cellY + 1].GetStatus() && CheckSurroundings(cellX, cellY + 1)) {
            m_gridManager.GridMatrix[cellX, cellY + 1].SetColor(Cell.SurroundingCellColor);
        }

    }

    // Método auxiliar para checar a disponibilidade da célula de coordenada (cellX, cellY)
    bool CheckSurroundings (int cellX, int cellY) {
        int selectedCells = 0;

        for (int i = cellX - 1; i <= cellX + 1; ++i) {
            for (int j = cellY - 1; j <= cellY + 1; ++j) {
                if (((i >= 0 && i < m_gridManager.GridLines) && (j >= 0 && j < m_gridManager.GridColumns)) && m_gridManager.GridMatrix[i, j].GetStatus()) {
                    ++selectedCells;
                }

                if (selectedCells == 3) {
                    return false;
                }
            }
        }
        return true;
    }

    // Método auxiliar para apagar todas os caminhos possíveis a partir da célula selecionada atualmente
    void ClearPossiblePaths () {
        for (int i = 0; i < m_gridManager.GridLines; ++i) {
            for (int j = 0; j < m_gridManager.GridColumns; ++j) {
                if (m_gridManager.GridMatrix[i, j].GetColor() == Cell.SurroundingCellColor) {
                    m_gridManager.GridMatrix[i, j].SetColor(Cell.DefaultColor);
                }
            }
        }
    }

    // Método responsável por limpar o tabuleiro de todas as células posicionadas
    // Utiliza o mecanismo de rollback
    public void ClearAllCells () {
        while (SelectedCells.Count != 0) {
            Rollback();
        }
    }

    // Método auxiliar responsável por desfazer a última seleção
    void Rollback () {
        // Verifica se existem itens na lista,
        // caso contrário não é preciso fazer rollback
        if (SelectedCells.Count != 0) {
            // Se existe um único elemento
            if (SelectedCells.Count == 1) {
                // Remove o elemento da lista
                SelectedCells.Remove(m_currentSelectedCell);

                // Reseta as informações da célula
                Cell tmp = m_currentSelectedCell;

                m_currentSelectedCell.ResetCell();
                m_currentSelectedCell = null;

                tmp.SetColor(Cell.DefaultColor);

                ClearPossiblePaths();

                // Reseta as informações da UI
                m_uiManager.ResetUIInfo();
            } else {
                // Armazena o último e o penúltimo elemento da lista
                Cell tmp1 = SelectedCells[SelectedCells.Count - 1];
                Cell tmp2 = SelectedCells[SelectedCells.Count - 2];
                
                // Remove o último elemento da lista
                SelectedCells.Remove(tmp1);

                // Reseta as informações do último elemento selecionado e
                // atualiza o elemento que foi selecionado
                tmp1.ResetCell();
                m_currentSelectedCell = tmp2;

                // Restaura a cor branca para a célula que sofreu rollback
                tmp1.SetColor(Cell.DefaultColor);

                ClearPossiblePaths();
                DrawPossiblePaths(m_currentSelectedCell);

                // Carrega as informações da célula selecionada atualmente para a UI
                m_uiManager.LoadInfoToUI(m_currentSelectedCell);
            }
        }
    }


    // Método auxiliar usado por OUTROS scripts, para poderem adicionar
    // novas células à lista de selecionados, sem precisar tornar a variável
    // m_currentSelectedCell pública
    // Até o momento o único script que referencia esse método é o UIManager.LoadLevel()
    // na hora de reconstruir o tabuleiro do banco de dados
    public void AddSelectedCellToList (Cell cell) {
        SelectedCells.Add(cell);
        m_currentSelectedCell = cell;
    }
}
