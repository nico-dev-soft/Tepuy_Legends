using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzlePedestal : MonoBehaviour
{
    public Pedestal[] pedestals; // Lista de pedestales en el puzzle
    public TextMeshProUGUI puzzleStatusText; // Referencia al texto en la UI
    private int stonesPlaced = 0; // Contador de piedras colocadas correctamente
    private bool puzzleCompleted = false;

    private void Start()
    {
        UpdatePuzzleStatus("Coloca las piedras en los pedestales correctos.");
    }

    public void UpdateStoneStatus()
    {
        // Recalcula cuántas piedras se han colocado correctamente
        stonesPlaced = 0;

        foreach (var pedestal in pedestals)
        {
            if (pedestal.IsStonePlaced())
                stonesPlaced++;
        }

        // Actualiza la UI con el progreso
        UpdatePuzzleStatus($"Piedras colocadas: {stonesPlaced}/{pedestals.Length}");

        // Comprueba si todas las piedras se colocaron
        if (stonesPlaced == pedestals.Length && !puzzleCompleted)
        {
            PuzzleCompleted();
        }
    }

    private void PuzzleCompleted()
    {
        puzzleCompleted = true;
        UpdatePuzzleStatus("¡Puzzle completado! Has resuelto el desafío final.");
        Debug.Log("¡Puzzle completado!");
        // Aquí puedes añadir efectos, sonidos o animaciones adicionales
    }

    private void UpdatePuzzleStatus(string message)
    {
        if (puzzleStatusText != null)
        {
            puzzleStatusText.text = message; // Actualiza el texto en la UI
        }
        else
        {
            Debug.LogWarning("No se asignó el texto de UI al PuzzleManager.");
        }
    }
}


