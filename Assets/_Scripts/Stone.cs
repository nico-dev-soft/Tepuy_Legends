using System.Collections; // Necesario para usar IEnumerator
using UnityEngine;
using TMPro;

public class Stone : MonoBehaviour
{
    public PuzzleManager puzzleManager; // Referencia al PuzzleManager
    public AudioClip stoneSound; // Sonido asignado a esta piedra
    public Material defaultMaterial; // Material por defecto
    public Material activeMaterial; // Material activado
    public GameObject handModel; // Modelo 3D de la mano
    public Animator handAnimator; // Animator para animar la mano
    public GameObject messagePanel; // Panel de mensaje de UI
    public TextMeshProUGUI messageText; // Texto del mensaje en el panel

    private AudioSource audioSource; // AudioSource de esta piedra
    private Renderer stoneRenderer; // Renderer de la piedra
    private bool isActivated = false; // Estado de la piedra
    private bool isPlayerNearby = false; // Para comprobar si el jugador está cerca
    public string playerTag = "Player"; // Tag del jugador

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stoneRenderer = GetComponent<Renderer>();

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.clip = stoneSound;
        }

        if (stoneRenderer != null && defaultMaterial != null)
        {
            stoneRenderer.material = defaultMaterial;
        }

        if (handModel != null)
        {
            handModel.SetActive(false); // Asegurarse de que la mano esté oculta al inicio
        }

        if (messagePanel != null)
        {
            messagePanel.SetActive(false); // Ocultar el panel al inicio
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetMouseButtonDown(1)) // Clic derecho
        {
            Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerNearby = true;
            ShowHandAnimation();
            ShowMessage("Presiona clic derecho para activar la piedra y escuchar su sonido ancestral.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerNearby = false;
            HideHandAnimation();
            HideMessage();
        }
    }

    private void ShowHandAnimation()
    {
        if (handModel != null)
        {
            handModel.SetActive(true);
            if (handAnimator != null)
            {
                handAnimator.SetTrigger("Show"); // Suponiendo que hay un trigger llamado "Show" en el Animator
            }
        }
    }

    private void HideHandAnimation()
    {
        if (handModel != null)
        {
            if (handAnimator != null)
            {
                handAnimator.SetTrigger("Hide"); // Suponiendo que hay un trigger llamado "Hide" en el Animator
            }
            StartCoroutine(HideHandWithDelay(1f)); // Ocultar la mano después de 1 segundo
        }
    }

    private IEnumerator HideHandWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        handModel.SetActive(false);
    }

    private void ShowMessage(string message)
    {
        if (messagePanel != null && messageText != null)
        {
            messagePanel.SetActive(true);
            messageText.text = message;
        }
    }

    private void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;

        if (stoneRenderer != null && activeMaterial != null)
        {
            stoneRenderer.material = activeMaterial;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (puzzleManager != null)
        {
            puzzleManager.AddToSequence(stoneSound);
        }

        HideHandAnimation();
        HideMessage();
    }
}
