using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // Necesario para usar UnityEvent

public class Pasos : MonoBehaviour
{
    [Range(0, 20f)]
    public float frequency = 10.0f;
    public UnityEvent onFootStep;
    private float sinValue;
    private bool isTriggered = false; // Declaración corregida

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la magnitud del input del jugador
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartFootSteps();
        }
    }

    private void StartFootSteps()
    {
        // Usa Mathf.Sin para cálculos
        sinValue = Mathf.Sin(Time.time * frequency);

        // Activa el evento de paso si el valor del seno supera cierto umbral
        if (sinValue > 0.97f && isTriggered == false)
        {
            isTriggered = true;
            Debug.Log("Tic");
            onFootStep.Invoke();
        }
        else if (isTriggered == true && sinValue < -0.97f)
        {
            isTriggered = false;
        }
    }
}
