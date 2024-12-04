using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject[] canvases; // Array de todos los canvases
    public GameObject firstButton; 
    
    #region Singletone
    private static CanvasManager Instance;
    public static CanvasManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private void OnEnable()
    {
        firstButton = GameObject.FindGameObjectWithTag("FirstButton");
    }

    public void SwitchCanvas(int index)
    {
        
        // Desactiva todos los canvases
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }

        // Activa el canvas deseado
        canvases[index].SetActive(true);

        // Selecciona el primer botón del nuevo canvas
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstButton);
    }
}
