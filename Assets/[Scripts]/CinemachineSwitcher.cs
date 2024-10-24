using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    #region Singletone
    private static CinemachineSwitcher Instance;
    public static CinemachineSwitcher GetInstance() 
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

    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    private Animator animator;
    private bool panoramicCamera = true;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        Debug.Log(panoramicCamera);
        Debug.Log(GameManager.GetInstance().GetGameState());
    }

    public void SwitchState()
    {
        if (panoramicCamera)
        {
            animator.Play("Menu");
            mainMenuCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
            GameManager.GetInstance().ChangeGameState(GAME_STATE.MAINMENU);
        }
        panoramicCamera = !panoramicCamera;
    }

    public void SettingsCamera()
    {
        animator.Play("Settings");
        settingsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        panoramicCamera = !panoramicCamera;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
