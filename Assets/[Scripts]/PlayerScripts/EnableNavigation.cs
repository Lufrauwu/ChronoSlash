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
        // Selecciona el primer botón cuando el canvas se activa
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    
    
}
