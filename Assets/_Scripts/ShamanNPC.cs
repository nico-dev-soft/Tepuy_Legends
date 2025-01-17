using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShamanNPC : MonoBehaviour
{
   public string[] instructions = new string[]
    {
        "Saludos, viajero. Estas piedras no son simples rocas; son guardianas de los ecos de nuestra tierra y nuestra herencia ancestral.",
        "Cada piedra emite un sonido que representa uno de los instrumentos más sagrados para los pemones: el retumbar del tambor, el canto del marimare, y el eco del turas.",
        "Los pemones creemos que cada sonido tiene un espíritu, una conexión con la naturaleza que nos guía y protege.",
        "Escucha con atención, pues tu desafío será descifrar el orden correcto de estos sonidos para armonizar con los espíritus de la tierra.",
        "Si fallas, no temas; la naturaleza siempre es paciente. Inténtalo nuevamente, y muestra tu respeto y sabiduría para resolver este enigma."
    };

    public float[] messageTimings = new float[]
    {
        0f,    // Inicio del primer mensaje
        11f,   // Inicio del segundo mensaje
        23f,   // Inicio del tercer mensaje
        31f,   // Inicio del cuarto mensaje
        42f    // Inicio del quinto mensaje
    };

    public AudioSource shamanAudioSource; // Fuente de audio
    public TextMeshProUGUI uiText; // Referencia al texto en la UI

    private Coroutine messageCoroutine;

    public void GiveInstructions()
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine); // Detener cualquier corrutina previa
        }
        messageCoroutine = StartCoroutine(DisplayMessagesWithAudio());
    }

    private IEnumerator DisplayMessagesWithAudio()
    {
        if (shamanAudioSource != null)
        {
            shamanAudioSource.Play(); // Reproducir el audio completo
        }

        for (int i = 0; i < instructions.Length; i++)
        {
            uiText.text = instructions[i]; // Mostrar el mensaje correspondiente

            // Esperar hasta que sea el tiempo del siguiente mensaje
            float nextMessageTime = (i + 1 < messageTimings.Length) ? messageTimings[i + 1] : shamanAudioSource.clip.length;
            yield return new WaitForSeconds(nextMessageTime - messageTimings[i]);
        }

        // Al final, limpiar el texto
        uiText.text = "";
        messageCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GiveInstructions(); // Llama al método cuando el jugador entra en rango
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine); // Detener los mensajes al salir del rango
            }

            if (shamanAudioSource != null && shamanAudioSource.isPlaying)
            {
                shamanAudioSource.Stop(); // Detener el audio
            }

            uiText.text = ""; // Limpiar el texto
        }
    }
}
