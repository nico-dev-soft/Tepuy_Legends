using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;

    [SerializeField] private float _movedSpeed, _gravity, _fallVelocity, _jumpForce;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Terrain terrain;
    [SerializeField] private Transform puzzleFocusPoint;
    [SerializeField] private float focusSpeed = 2f;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private GameObject pauseMenu;

    private Vector3 _axis, _movePlayer;
    private float verticalRotation = 0f;
    private float footstepTimer = 0f;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isFocusing = false;
    private bool isPaused = false;

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
    }

    void Start()
    {
        ShowGameStartMessage();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalCameraPosition = playerCamera.localPosition;
        originalCameraRotation = playerCamera.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (isPaused) return;

        HandleMouseLook();
        HandleMovement();
        HandleInteraction();
        ClampPlayerPositionToTerrain();

        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCameraFocus();
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            ResetCameraFocus();
        }
    }

    private void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ClosePauseMenu()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainmenu");
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        _axis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _axis = _axis.magnitude > 1 ? transform.TransformDirection(_axis).normalized : transform.TransformDirection(_axis);

        _movePlayer.x = _axis.x;
        _movePlayer.z = _axis.z;

        setGravity();
        _player.Move(_movePlayer * _movedSpeed * Time.deltaTime);

        HandleFootsteps();
    }

    private void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
            {
                Stone stone = hit.collider.GetComponent<Stone>();
                stone?.Activate();
            }
        }
    }

    private void HandleFootsteps()
    {
        if (_player.isGrounded && _movePlayer.magnitude > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            footstepAudioSource.PlayOneShot(clip);
        }
    }

    private void setGravity()
    {
        if (_player.isGrounded)
        {
            _fallVelocity = -_gravity * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            {
                _fallVelocity = _jumpForce;
            }
        }
        else
        {
            _fallVelocity -= _gravity * Time.deltaTime;
        }
        _movePlayer.y = _fallVelocity;
    }

    private void ClampPlayerPositionToTerrain()
    {
        if (terrain == null)
        {
            Debug.LogWarning("El terreno no está asignado al PlayerController.");
            return;
        }

        Vector3 playerPosition = transform.position;
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPosition = terrain.transform.position;
        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;

        float minX = terrainPosition.x;
        float maxX = terrainPosition.x + terrainWidth;
        float minZ = terrainPosition.z;
        float maxZ = terrainPosition.z + terrainLength;

        playerPosition.x = Mathf.Clamp(playerPosition.x, minX, maxX);
        playerPosition.z = Mathf.Clamp(playerPosition.z, minZ, maxZ);

        transform.position = playerPosition;
    }

    private void StartCameraFocus()
    {
        if (puzzleFocusPoint != null && !isFocusing)
        {
            isFocusing = true;
            originalCameraPosition = playerCamera.localPosition;
            originalCameraRotation = playerCamera.localRotation;
            StartCoroutine(MoveCameraToPuzzle());
        }
    }

    private void ResetCameraFocus()
    {
        if (isFocusing)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCameraBack());
        }
    }

    private IEnumerator MoveCameraToPuzzle()
    {
        float elapsedTime = 0f;
        float duration = 5f;

        Vector3 startPosition = playerCamera.localPosition;
        Quaternion startRotation = playerCamera.localRotation;

        Vector3 adjustedFocusPosition = transform.InverseTransformPoint(puzzleFocusPoint.position + new Vector3(0, 2f, -5f));
        adjustedFocusPosition.y = Mathf.Max(adjustedFocusPosition.y, terrain.SampleHeight(puzzleFocusPoint.position) - transform.position.y + 1f);
        Quaternion adjustedFocusRotation = Quaternion.LookRotation((transform.InverseTransformPoint(puzzleFocusPoint.position) - adjustedFocusPosition).normalized);

        while (elapsedTime < duration)
        {
            playerCamera.localPosition = Vector3.Lerp(startPosition, adjustedFocusPosition, elapsedTime / duration);
            playerCamera.localRotation = Quaternion.Lerp(startRotation, adjustedFocusRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime * focusSpeed;
            yield return null;
        }

        playerCamera.localPosition = adjustedFocusPosition;
        playerCamera.localRotation = adjustedFocusRotation;
    }

    private IEnumerator MoveCameraBack()
    {
        float elapsedTime = 0f;
        float duration = 5f;

        Vector3 startPosition = playerCamera.localPosition;
        Quaternion startRotation = playerCamera.localRotation;

        while (elapsedTime < duration)
        {
            playerCamera.localPosition = Vector3.Lerp(startPosition, originalCameraPosition, elapsedTime / duration);
            playerCamera.localRotation = Quaternion.Lerp(startRotation, originalCameraRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime * focusSpeed;
            yield return null;
        }

        playerCamera.localPosition = originalCameraPosition;
        playerCamera.localRotation = originalCameraRotation;
        isFocusing = false;
    }

    private void ShowGameStartMessage()
    {
        if (messagePanel != null && messageText != null)
        {
            messagePanel.SetActive(true);
            messageText.text = "Bienvenido al Parque Canaima.Clic (V) Visualiza el objetivo ";
            StartCoroutine(HideMessageAfterDelay(8f));
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messagePanel?.SetActive(false);
    }
}
