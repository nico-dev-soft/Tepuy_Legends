using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleValidator : MonoBehaviour
{
 
 public Pedestal[] pedestals; // Referencia a todos los pedestales del puzzle

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Tecla de prueba para validar el puzzle
        {
            CheckPuzzleCompletion();
        }
    }

    public void CheckPuzzleCompletion()
    {
        foreach (Pedestal pedestal in pedestals)
        {
            if (!pedestal.IsStonePlaced())
            {
                Debug.Log("El puzzle aún no está completo.");
                return;
            }
        }

        Debug.Log("¡Puzzle completado! Se activará el altar sagrado.");
        ActivateAltar();
    }

    private void ActivateAltar()
    {
        // Lógica para activar el altar o la secuencia final
        Debug.Log("Altar sagrado activado. Fin del puzzle.");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

   
}
