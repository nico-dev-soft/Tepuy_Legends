using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentLight : MonoBehaviour
{

    private static PersistentLight instance;

    private void Awake()
    {
        // Asegura que solo haya una instancia de la luz
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destruye duplicados
        }
    }


}
