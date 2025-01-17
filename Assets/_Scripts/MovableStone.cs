using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovableStone : MonoBehaviour
{
  private bool isHeld = false; // Si el jugador sostiene la piedra
    private Transform player; // Referencia al jugador
    public string stoneID; // ID único de esta piedra
    public TextMeshProUGUI puzzleStatusText; // Mensajes en la UI
    private AudioSource audioSource; // Sonido al interactuar con la piedra

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning($"No se encontró AudioSource en {gameObject.name}. Añádelo si deseas sonidos.");
        }
    }

    private void Update()
    {
        if (isHeld && player != null)
        {
            transform.position = player.position + player.forward * 2f; // La piedra sigue al jugador
        }

        if (isHeld && Input.GetMouseButtonDown(1)) // Soltar con clic derecho
        {
            ReleaseStone();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Recoger la piedra
        if (!isHeld && other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PickStone(other.transform);
        }
        // Colocar la piedra en el pedestal
        else if (isHeld && other.CompareTag("Pedestal") && Input.GetMouseButtonDown(1))
        {
            PlaceStoneOnPedestal(other);
        }
    }

    private void PickStone(Transform playerTransform)
    {
        isHeld = true;
        player = playerTransform;
        Debug.Log($"Piedra {stoneID} recogida.");
        UpdatePuzzleStatus($"Piedra {stoneID} recogida.");
        PlaySound();
    }

    private void PlaceStoneOnPedestal(Collider pedestalCollider)
    {
        Pedestal pedestal = pedestalCollider.GetComponent<Pedestal>();
        if (pedestal != null)
        {
            if (pedestal.CheckStone(this))
            {
                gameObject.SetActive(false); // Desactivar la piedra si es correcta
                UpdatePuzzleStatus($"¡Piedra {stoneID} colocada correctamente!");
                Debug.Log($"Piedra {stoneID} colocada correctamente en {pedestal.gameObject.name}.");
            }
            else
            {
                UpdatePuzzleStatus("Piedra incorrecta. Intenta de nuevo.");
                Debug.LogWarning($"Piedra {stoneID} no coincide con el pedestal {pedestal.gameObject.name}.");
            }
            ReleaseStone();
        }
    }

    private void ReleaseStone()
    {
        isHeld = false;
        player = null;
        Debug.Log($"Piedra {stoneID} soltada.");
        UpdatePuzzleStatus($"Piedra {stoneID} soltada.");
        PlaySound();
    }

    private void UpdatePuzzleStatus(string message)
    {
        if (puzzleStatusText != null)
        {
            puzzleStatusText.text = message; // Actualiza el texto en la UI
        }
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // Reproduce el sonido asignado al AudioSource
        }
    }
}
