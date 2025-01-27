using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brillo : MonoBehaviour
{
    public Slider slider;
    public Image panelBrillo;

    private void Start()
    {
        // Configurar el valor inicial del slider desde el GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            slider.value = gameManager.brillo;
        }

        ActualizarBrillo(slider.value);
    }

    public void ChangeSliderValue(float valor)
    {
        ActualizarBrillo(valor);

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ActualizarBrillo(valor);
        }
    }

    private void ActualizarBrillo(float valor)
    {
        if (panelBrillo != null)
        {
            panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, valor);
        }
    }
}
