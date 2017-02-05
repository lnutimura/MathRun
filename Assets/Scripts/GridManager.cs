/*
 * Esse script deve ser atrelado à um GameObject.
 * O intuito dele é criar o tabuleiro, e representá-lo
 * por meio de uma matriz de células. A matriz é pública e
 * deve ser utilizada como referência ao tabuleiro por outros scripts.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (GameObject))]
public class GridManager : MonoBehaviour {
    // Dados informacionais do tabuleiro
    public Cell[,] GridMatrix;
    public int GridColumns = 12;
    public int GridLines = 8;
    public GameObject GridCell;

    void Start () {
        // Define a dimensão da matriz de células
        GridMatrix = new Cell[GridLines, GridColumns];

        // Instância a matriz, visualmente e informacionalmente
        for (int i = 0; i < GridLines; ++i) {
            for (int j = 0; j < GridColumns; ++j) {
                GameObject tmp = Instantiate (GridCell, transform) as GameObject;
                tmp.transform.name = "Célula (" + i + "," + j + ")";
                tmp.transform.localPosition = new Vector3(tmp.transform.localPosition.x, tmp.transform.localPosition.y, 0f);
                tmp.transform.localScale = new Vector3(1f, 1f ,1f);
                tmp.GetComponent<Image>().color = Cell.DefaultColor;
                tmp.GetComponent<ButtonLoadScript>().CellCoordinates = new Vector3 (i, j, 0f);
                tmp.GetComponent<Button>().onClick.AddListener (
                        delegate () {
                            tmp.GetComponent<ButtonLoadScript>().OnClick();
                        }
                );
                GridMatrix[i, j] = new Cell(Cell.DefaultColor, tmp, new Vector3 (i, j, 0f));
            }
        }
    }
}
