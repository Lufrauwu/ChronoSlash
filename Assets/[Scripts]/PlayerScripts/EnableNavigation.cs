using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnableNavigation : MonoBehaviour
{
    
    public GameObject firstButton; 
    // Start is called before the first frame update
    private void OnEnable()
    {
        firstButton = GameObject.FindGameObjectWithTag("FirstButton");

    }
    
    
}
