using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
  public void QuitGame()
    {

        // Cerrar la aplicación en una compilación
        Debug.Log("Cerrando el juego...");
        Application.Quit();
        
    }
}
