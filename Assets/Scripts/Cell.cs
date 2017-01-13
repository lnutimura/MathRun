using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public enum OperationType
    {
        Soma,
        Subtracao,
        Multiplicacao,
        Divisao
    }

    private bool selected;

    private GameObject primitive;
    private Vector3 position;

    private OperationType type;
    private int difficulty;
    private string question;
    private float answer;

    public Cell (GameObject primitive, Vector2 position)
    {
        selected = false;

        this.primitive = primitive;
        this.position = position;
    }

    public void SetCell (OperationType type, int difficulty, string question, float answer)
    {
        this.type = type;
        this.difficulty = difficulty;
        this.question = question;
        this.answer = answer;
    }

    public void SetCellStatus (bool status)
    {
        selected = status;
    }

    public bool GetCellStatus ()
    {
        return selected;
    }

    public GameObject GetCellPrimitive ()
    {
        return primitive;
    }

    public Vector3 GetCellPosition ()
    {
        return position;
    }

    public OperationType GetCellType ()
    {
        return type;
    }

    public int GetDifficulty ()
    {
        return difficulty;
    }

    public string GetCellQuestion ()
    {
        return question;
    }

    public float GetCellAnswer ()
    {
        return answer;
    }
}
