using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volumen : MonoBehaviour
{
    public Slider slider;
    public Image imagenMute;

    private void Start()
    {
        // Configurar el valor inicial del slider desde el GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            slider.value = gameManager.volumen;
        }

        ActualizarVolumen(slider.value);
    }

    public void ChangeSlider(float valor)
    {
        ActualizarVolumen(valor);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ActualizarVolumen(valor);
        }
    }

    private void ActualizarVolumen(float valor)
    {
        AudioListener.volume = valor;
        if (imagenMute != null)
        {
            imagenMute.enabled = valor == 0;
        }
    }
}
