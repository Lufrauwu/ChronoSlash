using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject postProcessingVolume;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Velocity", InputManager.GetInstance().MovementInput().magnitude);
    }

    public void SetNormalSlash()
    {
        animator.SetBool("Slash", false);
        GameManager.GetInstance().ChangeGameState(GAME_STATE.END_TURN);

    }
    
    public void DeactivatePostProcessVolume()
    {
        postProcessingVolume.SetActive(false);
    }
    
    public void ActivatePostProcessVolume()
    {
        //postProcessingVolume.SetActive(true);
    }

    public void ChangeToEndTurn()
    {
        Debug.Log("animationevent");
        GameManager.GetInstance().ChangeGameState(GAME_STATE.END_TURN);
    }

    public void ChangeToStartTurn()
    {
        Debug.Log("CAMBIO");
        GameManager.GetInstance().ChangeGameState(GAME_STATE.START_TURN);
    }
}
