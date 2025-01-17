using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSoundTrigger : MonoBehaviour
{

    private AudioSource riverAudioSource; // Fuente de sonido del río
    public string playerTag = "Player";   // Tag del jugador

    private void Start()
    {
        // Obtén el AudioSource del objeto asociado
        riverAudioSource = GetComponent<AudioSource>();
        if (riverAudioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el objeto del río.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reproduce el sonido cuando el jugador entra en el área
        if (other.CompareTag(playerTag))
        {
            if (riverAudioSource != null && !riverAudioSource.isPlaying)
            {
                riverAudioSource.Play();
                Debug.Log("El jugador se acercó al río. Sonido activado.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detén el sonido cuando el jugador salga del área
        if (other.CompareTag(playerTag))
        {
            if (riverAudioSource != null && riverAudioSource.isPlaying)
            {
                riverAudioSource.Stop();
                Debug.Log("El jugador se alejó del río. Sonido detenido.");
            }
        }
    }
  

    // Update is called once per frame
    void Update()
    {
        
    }
}
