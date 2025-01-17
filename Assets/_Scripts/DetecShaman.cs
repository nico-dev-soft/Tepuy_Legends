using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanInteraction : MonoBehaviour
{
    private ShamanNPC shamanNPC;

    void Start()
    {
        shamanNPC = GetComponent<ShamanNPC>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shamanNPC.GiveInstructions();
        }
    }
}
