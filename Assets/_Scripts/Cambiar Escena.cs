using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
   
public void NextScene(string nombre)
    {
        // Cargar la siguiente escena
        SceneManager.LoadScene(nombre);

        // Aplicar configuraciones después de cargar la nueva escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscar el GameManager en la nueva escena y aplicar las configuraciones
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AplicarConfiguraciones();
        }

        // Desuscribirse del evento para evitar múltiples llamadas
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
