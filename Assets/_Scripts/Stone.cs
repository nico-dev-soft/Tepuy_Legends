using UnityEngine;

public class Stone : MonoBehaviour
{
     public PuzzleManager puzzleManager; // Referencia al PuzzleManager
    public AudioClip stoneSound; // Sonido asignado a esta piedra
    public Material defaultMaterial; // Material por defecto
    public Material activeMaterial; // Material activado
    private AudioSource audioSource; // AudioSource de esta piedra
    private Renderer stoneRenderer; // Renderer de la piedra
    private bool isActivated = false; // Estado de la piedra

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
    }
}
