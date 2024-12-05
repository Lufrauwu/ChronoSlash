using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject deadCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject deadButton;
    [SerializeField] private GameObject winButton;
    
    #region Singletone
    private static CanvasController Instance;
    public static CanvasController GetInstance()
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
    void Start()
    {
        pauseCanvas.SetActive(false);
        deadCanvas.SetActive(false);
        winCanvas.SetActive(false);
    }


    public void OpenDeathCanvas()
    {
        deadCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(deadButton);
    }
    

    public void OpenWinCanvas()
    {
        winCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(winButton);
    }
    
    
    public void OpenPauseMenu()
    {
        pauseCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseButton);
    }

    public void CloseAllMenus()
    {
        pauseCanvas.SetActive(false);
        deadCanvas.SetActive(false);
        winCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
