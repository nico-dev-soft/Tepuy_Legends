using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBloker : MonoBehaviour
{
   [SerializeField] private string playerTag = "Player"; // Asegúrate de que el jugador tenga este tag
    [SerializeField] private GameObject warningMessage; // Panel o mensaje para mostrar al jugador

    private bool isBlocked = true; // Estado inicial: acceso bloqueado

    private void OnTriggerEnter(Collider other)
    {
        if (isBlocked && other.CompareTag(playerTag))
        {
            Debug.Log("Acceso al río bloqueado.");
            ShowWarning();
        }
    }

    private void ShowWarning()
    {
        if (warningMessage != null)
        {
            warningMessage.SetActive(true); // Activa el mensaje de advertencia
            Invoke("HideWarning", 3f); // Oculta el mensaje después de 3 segundos
        }
    }

    private void HideWarning()
    {
        if (warningMessage != null)
        {
            warningMessage.SetActive(false);
        }
    }

    // Método público para desbloquear el acceso (llámalo desde otros scripts)
    public void UnlockAccess()
    {
        isBlocked = false;
        Debug.Log("Acceso al río desbloqueado.");
    }
}
