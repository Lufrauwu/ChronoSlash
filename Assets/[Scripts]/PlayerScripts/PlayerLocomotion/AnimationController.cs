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
    }
    
    public void DeactivatePostProcessVolume()
    {
        postProcessingVolume.SetActive(false);
    }

    public void ChangeToEnemyTurn()
    {
        GameManager.GetInstance().ChangeGameState(GAME_STATE.ENEMYTURN);
    }
}
