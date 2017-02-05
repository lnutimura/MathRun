/*
 * Esse script não pode ser atrelado a um GameObject.
 * O intuito dele é servir de base para a construção
 * da ideia de uma célula no tabuleiro.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell {
    // Operações da célula
    public enum OperationType {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Mixed
    }

    // Cor padrão de uma célula
    public static Color DefaultColor = new Color (0.81f, 0.76f, 0.91f, 1f);
    public static Color MiddleCellColor = new Color(0.31f, 0.17f, 0.65f, 1f);
    public static Color SurroundingCellColor = new Color(0.39f, 0.12f, 1f, 1f);

    // Dados visuais da célula
    private Color m_color;
    private GameObject m_primitive;

    // Dados informacionais da célula
    private bool m_isSelected;
    private string m_question;
    private float m_answer;
    private int m_difficulty;
    private OperationType m_operation;
    private Vector3 m_position;

    // Construtor de célula vazia
    public Cell (Color color, GameObject primitive, Vector3 position) {
        m_isSelected = false;
        m_color = color;
        m_primitive = primitive;

        m_question = "";
        m_answer = 0f;
        m_difficulty = 1;
        m_operation = OperationType.Addition;

        m_position = position;
    }

    // Construtor de célula completa
    public Cell (Color color, GameObject primitive, Vector3 position, string question, float answer, int difficulty, OperationType operation) {
        m_isSelected = false;
        m_color = color;
        m_primitive = primitive;

        m_question = question;
        m_answer = answer;
        m_difficulty = difficulty;
        m_operation = operation;

        m_position = position;
    }

    // Seta os dados informativos da célula
    public void SetCell (string question, float answer, int difficulty, OperationType operation) {
        m_question = question;
        m_answer = answer;
        m_difficulty = difficulty;
        m_operation = operation;
    }

    // Reseta os dados informativos da célula
    public void ResetCell () {
        m_isSelected = false;

        m_question = "";
        m_answer = 0f;
        m_difficulty = 1;
        m_operation = OperationType.Addition;
    }

    // Métodos GET/SET
    public void SetStatus (bool status) {
        m_isSelected = status;
    }

    public bool GetStatus () {
        return m_isSelected;
    }

    public void SetColor (Color color) {
        m_color = color;
        m_primitive.GetComponent<Image>().color = color;
    }

    public Color GetColor () {
        return m_color;
    }

    public GameObject GetPrimitive () {
        return m_primitive;
    }

    public string GetQuestion () {
        return m_question;
    }

    public float GetAnswer () {
        return m_answer;
    }

    public int GetDifficulty () {
        return m_difficulty;
    }

    public OperationType GetOperationType () {
        return m_operation;
    }

    public Vector3 GetPosition () {
        return m_position;
    }
}
