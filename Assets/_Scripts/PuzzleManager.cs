using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro
using UnityEngine.SceneManagement; // Necesario para cargar escenas (opcional)

public class PuzzleManager : MonoBehaviour
{
    public AudioClip[] correctSequence; // Secuencia correcta de sonidos
    private List<AudioClip> playerSequence = new List<AudioClip>(); // Secuencia ingresada por el jugador
    public GameObject[] stones; // Las piedras en la escena
    public float resetDelay = 6f; // Tiempo antes de reiniciar el puzzle tras un fallo
    public TextMeshProUGUI puzzleStatusText; // Referencia al texto del Canvas
    public GameObject messagePanel; // Referencia al panel que contiene el mensaje (añadido)

    public bool isFinalPuzzle = false; // Indica si este es el último puzzle
    private bool puzzleCompleted = false; // Flag para evitar repeticiones

    private Material[] originalMaterials; // Almacena los materiales originales

    private void Start()
    {
        // Almacena los materiales iniciales de cada piedra
        originalMaterials = new Material[stones.Length];
        for (int i = 0; i < stones.Length; i++)
        {
            Renderer renderer = stones[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterials[i] = renderer.material;
            }
        }

        if (messagePanel != null)
        {
            ConfigurePanel(); // Configura la posición y visibilidad inicial del panel
        }

        if (isFinalPuzzle)
            UpdatePuzzleStatus("Resuelve el segundo puzzle:\n Los Pedestales Ancestrales.");
        else
            UpdatePuzzleStatus("Resuelve el primer puzzle:\n El Canto del Tepuy.");
    }

    public void AddToSequence(AudioClip clip)
    {
        if (puzzleCompleted) return; // Evita continuar si el puzzle ya está resuelto

        playerSequence.Add(clip);
        Debug.Log($"Se añadió el sonido {clip.name} a la secuencia del jugador.");
        UpdatePuzzleStatus($"Secuencia en progreso: {playerSequence.Count}/{correctSequence.Length}");

        if (playerSequence.Count == correctSequence.Length)
        {
            CheckSequence();
        }
    }

    private void CheckSequence()
    {
        bool isCorrect = true;

        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            PuzzleSolved();
        }
        else
        {
            Debug.Log("Secuencia incorrecta.");
            UpdatePuzzleStatus("Secuencia incorrecta.Reiniciando...");
            StartCoroutine(RestartAfterDelay(10f));
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia al inicio del juego
        }
    }

    private IEnumerator ResetPuzzle()
    {
        yield return new WaitForSeconds(resetDelay);
        playerSequence.Clear();

        // Reinicia las texturas de las piedras
        for (int i = 0; i < stones.Length; i++)
        {
            Renderer renderer = stones[i].GetComponent<Renderer>();
            if (renderer != null && originalMaterials[i] != null)
            {
                renderer.material = originalMaterials[i];
            }
        }

        UpdatePuzzleStatus("Puzzle reiniciado. Intenta de nuevo.");
        Debug.Log("Puzzle reiniciado.");
    }

    private void PuzzleSolved()
    {
        puzzleCompleted = true; // Marca el puzzle como resuelto
        Debug.Log("¡Puzzle resuelto!");
        UpdatePuzzleStatus("¡Puzzle resuelto!");

        // Muestra el mensaje durante 8 segundos antes de reiniciar
        StartCoroutine(RestartAfterDelay(8f));
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia al inicio del juego
    }

    private void UpdatePuzzleStatus(string message)
    {
        if (puzzleStatusText != null)
        {
            puzzleStatusText.text = message; // Actualiza el texto en el Canvas
            Debug.Log(message);
        }
        else
        {
           Debug.LogWarning("No se asignó el texto del Canvas al PuzzleManager.");
        }

        // Hacer visible el panel si está oculto
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }
    }

    private void ConfigurePanel()
    {
        if (messagePanel != null)
        {
            RectTransform panelRect = messagePanel.GetComponent<RectTransform>();

            // Configurar posición y tamaño inicial del panel
            panelRect.sizeDelta = new Vector2(400f, 300f); // Tamaño del panel
            panelRect.anchorMin = new Vector2(1, 0);       // Ancla en la esquina inferior izquierda
            panelRect.anchorMax = new Vector2(1, 0);
            panelRect.pivot = new Vector2(1, 0);           // Pivot en la esquina inferior izquierda
            panelRect.anchoredPosition = new Vector2(-40, panelRect.anchoredPosition.y + 10); // Margen desde los bordes
        }
    }
}
