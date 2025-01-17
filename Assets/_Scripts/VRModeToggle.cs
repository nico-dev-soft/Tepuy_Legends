using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRModeToggle  : MonoBehaviour
{
   public Camera mainCamera; // Cámara principal del juego
    public Camera leftEyeCamera; // Cámara para el ojo izquierdo
    public Camera rightEyeCamera; // Cámara para el ojo derecho
    private bool isStereoEnabled = false; // Estado del modo estereoscópico

    void Start()
    {
        // Asegúrate de que solo la cámara principal esté activa al inicio
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
        }
        if (leftEyeCamera != null && rightEyeCamera != null)
        {
            leftEyeCamera.enabled = false;
            rightEyeCamera.enabled = false;
        }
    }

    void Update()
    {
        // Detecta si se presiona la tecla 'v'
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleStereoVision();
        }
    }

    private void ToggleStereoVision()
    {
        isStereoEnabled = !isStereoEnabled; // Cambia el estado

        if (isStereoEnabled)
        {
            // Activa la visión estereoscópica
            if (mainCamera != null)
            {
                mainCamera.enabled = false;
            }
            if (leftEyeCamera != null && rightEyeCamera != null)
            {
                leftEyeCamera.enabled = true;
                rightEyeCamera.enabled = true;
            }
        }
        else
        {
            // Desactiva la visión estereoscópica
            if (mainCamera != null)
            {
                mainCamera.enabled = true;
            }
            if (leftEyeCamera != null && rightEyeCamera != null)
            {
                leftEyeCamera.enabled = false;
                rightEyeCamera.enabled = false;
            }
        }

        Debug.Log($"Modo estereoscópico {(isStereoEnabled ? "activado" : "desactivado")}");
    }
}
