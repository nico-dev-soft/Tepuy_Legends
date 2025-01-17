using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaCompleta : MonoBehaviour
{
    public Toggle toggle;
    
    // Start is called before the first frame update
    void Start()
    {
        if(Screen.fullScreen){
            toggle.isOn = true;
        }
        else{
            toggle.isOn = false;
        }
        
    }

    public void ActivarPantallaCompleta(bool pantallaCompleta){
        Screen.fullScreen = pantallaCompleta;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
