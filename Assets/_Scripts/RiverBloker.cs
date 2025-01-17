using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverBloker : MonoBehaviour
{
    public string playerTag = "Player"; // Asegúrate de que el jugador tenga este tag

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Acceso al río bloqueado.");
            // Aquí puedes hacer algo más, como mostrar un mensaje al jugador
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
