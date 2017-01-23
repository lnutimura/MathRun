using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {
    public int gridLines;
    public int gridColumns;
    public float skinWidth;

    public Cell[,] gridMatrix;
    public Cell[] selectedCells;

    public int numOfCells;
    public int numOfSelectedCells;

    private Cell lastSelectedCell;
    private SelectCell selectCellScript;

    void Start () {
        lastSelectedCell = null;
        selectCellScript = GameObject.Find("Game Manager").GetComponent<SelectCell>();

        selectedCells = new Cell[60];
        gridMatrix = new Cell[gridColumns, gridLines];

        numOfCells = gridLines * gridColumns;
        numOfSelectedCells = 0;

        for (int j = 0; j < gridLines; j++) {
            for (int i = 0; i < gridColumns; i++) {
                GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Quad);
                
                tmp.transform.SetParent(transform);
                tmp.transform.localScale = new Vector3(1f - skinWidth, 1f - skinWidth, 1f);
                tmp.transform.localPosition = new Vector3(i, j, 1f);
                tmp.transform.name = "Cell (" + i + ", " + j + ")";

                Cell cell = new Cell (tmp, tmp.transform.localPosition);
            
                gridMatrix[i, j] = cell;
            }
        }
    }

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(camRay, out hit)) {
                bool isValid = false;
                int tmpX = (int) hit.transform.localPosition.x;
                int tmpY = (int) hit.transform.localPosition.y;

                if (!gridMatrix[tmpX, tmpY].GetCellStatus()) {
                    if (lastSelectedCell == null) {
                        gridMatrix[tmpX, tmpY].SetCellStatus(true);
                        gridMatrix[tmpX, tmpY].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, (numOfSelectedCells * 1f) / numOfCells);
                        isValid = true;
                    } else {
                        int diffX, diffY;

                        diffX = (int) Mathf.Abs(tmpX - lastSelectedCell.GetCellPosition().x);
                        diffY = (int) Mathf.Abs(tmpY - lastSelectedCell.GetCellPosition().y);

                        if (((diffX == 1 && diffY == 0) || (diffX == 0 && diffY == 1)) && Check(gridMatrix[tmpX, tmpY])) {
                          
                            gridMatrix[tmpX, tmpY].SetCellStatus(true);
                            gridMatrix[tmpX, tmpY].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.blue, (numOfSelectedCells * 1f) / numOfCells);
                            isValid = true;
                        }
                    }

                    if (isValid && lastSelectedCell != gridMatrix[tmpX, tmpY]) {
                        ClearGrid();
                        DrawPossiblePaths(tmpX, tmpY);
                        selectedCells[numOfSelectedCells] = gridMatrix[tmpX, tmpY];

                        ++numOfSelectedCells;
                        lastSelectedCell = gridMatrix[tmpX, tmpY];
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            Rollback();
        }
    }

    public void Rollback () {
        if (numOfSelectedCells != 0) {
            --numOfSelectedCells;
            lastSelectedCell.SetCellStatus(false);
            lastSelectedCell.GetCellPrimitive().GetComponent<Renderer>().material.color = Color.white;
            lastSelectedCell.ResetCell();

            selectedCells[numOfSelectedCells] = null;

            ClearGrid();
            lastSelectedCell = null;
            selectCellScript.currentSelectedCell = null;
            if (numOfSelectedCells != 0) {
                lastSelectedCell = selectedCells[numOfSelectedCells - 1];
                selectCellScript.currentSelectedCell = selectedCells[numOfSelectedCells - 1];

                int tmpX = (int)selectedCells[numOfSelectedCells - 1].GetCellPosition().x;
                int tmpY = (int)selectedCells[numOfSelectedCells - 1].GetCellPosition().y;

                selectCellScript.LoadCellInfo(lastSelectedCell);
                DrawPossiblePaths(tmpX, tmpY);
            }
        }
    }

    public void DrawPossiblePaths (int tmpX, int tmpY) {
        if (tmpX > 0 && !gridMatrix[tmpX - 1, tmpY].GetCellStatus() && Check(gridMatrix[tmpX - 1, tmpY])) {
            gridMatrix[tmpX - 1, tmpY].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.green;
        }

        if (tmpX < gridColumns - 1 && !gridMatrix[tmpX + 1, tmpY].GetCellStatus() && Check(gridMatrix[tmpX + 1, tmpY])) {
            gridMatrix[tmpX + 1, tmpY].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.green;
        }

        if (tmpY > 0 && !gridMatrix[tmpX, tmpY - 1].GetCellStatus() && Check(gridMatrix[tmpX, tmpY - 1])) {
            gridMatrix[tmpX, tmpY - 1].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.green;
        }

        if (tmpY < gridLines - 1 && !gridMatrix[tmpX, tmpY + 1].GetCellStatus() && Check(gridMatrix[tmpX, tmpY + 1])) {
            gridMatrix[tmpX, tmpY + 1].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public void ClearGrid () {
        for (int j = 0; j < gridLines; j++) {
            for (int i = 0; i < gridColumns; i++) {
                if (gridMatrix[i, j].GetCellPrimitive().GetComponent<Renderer>().material.color == Color.green) {
                    gridMatrix[i, j].GetCellPrimitive().GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    public bool Check (Cell cell) {
        Vector3 tmp = cell.GetCellPosition();

        int selectedCount = 0;
        int tmpX = (int)tmp.x;
        int tmpY = (int)tmp.y;

        for (int i = tmpX - 1; i <= tmpX + 1; ++i) {
            for (int j = tmpY - 1; j <= tmpY + 1; ++j) {
                if (((i >= 0 && i < gridColumns) && (j >= 0 && j < gridLines)) &&gridMatrix[i, j].GetCellStatus()) {
                    ++selectedCount;
                }

                if (selectedCount == 3) {
                    return false;
                }
            }
        }

        return true;
    }
}
