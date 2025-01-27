using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  private static GameManager instance;

    public float brillo = 0.5f;
    public float volumen = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Cargar configuraciones desde PlayerPrefs
        brillo = PlayerPrefs.GetFloat("brillo", 0.5f);
        volumen = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AplicarConfiguraciones();
    }

    public void ActualizarBrillo(float nuevoBrillo)
    {
        brillo = nuevoBrillo;
        PlayerPrefs.SetFloat("brillo", brillo);
        AplicarBrillo();
    }

    public void ActualizarVolumen(float nuevoVolumen)
    {
        volumen = nuevoVolumen;
        PlayerPrefs.SetFloat("volumenAudio", volumen);
        AplicarVolumen();
    }

    public void AplicarConfiguraciones()
    {
        // Aplica el brillo
        Image panelBrillo = FindObjectOfType<Brillo>()?.panelBrillo;
        if (panelBrillo != null)
        {
            panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, brillo);
        }

        // Aplica el volumen
        AudioListener.volume = volumen;
    }

  
    private void AplicarBrillo()
    {
        Image panelBrillo = FindObjectOfType<Brillo>()?.panelBrillo;
        if (panelBrillo != null)
        {
            panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, brillo);
        }
    }

    private void AplicarVolumen()
    {
        AudioListener.volume = volumen;
    }
}
