using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _player;

    [SerializeField] private float _movedSpeed, _gravity, _fallVelocity, _jumpForce;
    [SerializeField] private float mouseSensitivity = 100f; // Sensibilidad del mouse
    [SerializeField] private Transform playerCamera; // Cámara del jugador
    [SerializeField] private AudioSource footstepAudioSource; // Fuente de audio para los pasos
    [SerializeField] private AudioClip[] footstepClips; // Clips de audio para los pasos
    [SerializeField] private float footstepInterval = 0.5f; // Intervalo entre los pasos
    [SerializeField] private float interactionRange = 2.5f; // Rango de interacción con objetos
    [SerializeField] private LayerMask interactableLayer; // Capa de objetos interactuables

    private Vector3 _axis, _movePlayer;
    private float verticalRotation = 0f; // Controla la inclinación vertical
    private float footstepTimer = 0f; // Temporizador para los pasos

    private void Awake()
    {
        _player = GetComponent<CharacterController>();
    }

    void Start()
    {
        // Bloquea el cursor en la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook(); // Maneja la rotación con el ratón
        HandleMovement(); // Maneja el movimiento del jugador
        HandleInteraction(); // Maneja la interacción con objetos

         // Detecta la tecla Esc para volver al menú principal
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Regresando al menú principal...");
            SceneManager.LoadScene("mainmenu"); // Cambia "MenuPrincipal" por el nombre exacto de tu escena
        }
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
        if (_axis.magnitude > 1)
            _axis = transform.TransformDirection(_axis).normalized;
        else
            _axis = transform.TransformDirection(_axis);

        _movePlayer.x = _axis.x;
        _movePlayer.z = _axis.z;

        setGravity();
        _player.Move(_movePlayer * _movedSpeed * Time.deltaTime);

        HandleFootsteps();
    }

    private void HandleInteraction()
    {
        if (Input.GetMouseButtonDown(1)) // Botón derecho del mouse
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
            {
                Stone stone = hit.collider.GetComponent<Stone>();
                if (stone != null)
                {
                    stone.Activate(); // Activa la piedra
                }
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
}
