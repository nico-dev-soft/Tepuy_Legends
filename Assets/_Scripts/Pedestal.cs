using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pedestal : MonoBehaviour
{
    private bool stonePlaced = false; // Variable para indicar si una piedra ha sido colocada
    public string expectedStoneID;    // ID de la piedra esperada en este pedestal
    private string currentStoneID;    // ID de la piedra colocada
    public TextMeshProUGUI puzzleStatusText; // Referencia al texto en la UI

    // Método para comprobar si hay una piedra colocada
    public bool IsStonePlaced()
    {
        return stonePlaced;
    }

    // Método para verificar si la piedra colocada es la correcta
    public bool CheckStone(MovableStone stone)
    {
        if (stone.stoneID == expectedStoneID) // Comprueba si el ID coincide
        {
            stonePlaced = true;
            currentStoneID = stone.stoneID;
            UpdatePuzzleStatus($"Piedra {stone.stoneID} colocada correctamente en {gameObject.name}.");
            Debug.Log($"Piedra {stone.stoneID} colocada correctamente en {gameObject.name}.");
            return true;
        }
        else
        {
            UpdatePuzzleStatus("Piedra incorrecta. Inténtalo de nuevo.");
            Debug.LogWarning($"Piedra {stone.stoneID} no coincide con el pedestal {gameObject.name}.");
            return false;
        }
    }

    // Método auxiliar para actualizar el texto de estado del puzzle
    private void UpdatePuzzleStatus(string message)
    {
        if (puzzleStatusText != null)
        {
            puzzleStatusText.text = message;
        }
        else
        {
            Debug.LogWarning("No se asignó un TextMeshProUGUI al pedestal.");
        }
    }
}
