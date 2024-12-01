using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject postProcessingVolume;
    [SerializeField] private ThirdPersonController thrdPersonController;
    [SerializeField] private bool isplayer;
    [SerializeField] private List<ParticleSystem> particles;
    [SerializeField] private GameObject objectToDeactivate;
    private ParticleSystem particleInstance;
    private string currentAnimation = "";


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("Velocity", InputManager.GetInstance().MovementInput().magnitude);
        if (isplayer && thrdPersonController.isInCombat == false)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.EXPLORATION);
        }
        CheckAnimation();
    }

    private void CheckAnimation()
    {
        if (currentAnimation == "A_00000")
        {
            return;
        }
        if (InputManager.GetInstance().MovementInput().y == 1)
        {
            ChangeAnimation("Run_anim");
        }
        else if (InputManager.GetInstance().MovementInput().y == -1)
        {
            ChangeAnimation("Walk_Backward_anim");
        }
        else
        {
            ChangeAnimation("Idle_anim");
        }
    }

    public void ChangeAnimation(string animation, float crossfadeTime = 0.2f)
    {
        if (currentAnimation != animation)
        {   
            currentAnimation = animation;
            animator.CrossFade(animation, crossfadeTime);
        }
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

    public void DeactivateObject(GameObject obj)
    {
        obj = objectToDeactivate;
        objectToDeactivate.SetActive(false);
    }

    public void PlayParticles()
    {
        particles[0].Play();
    }
    public void DeactivateAttack()
    {
        animator.SetBool("Attack", false);
    }

    public void SpawnParticles()
    {
        particleInstance = Instantiate(particles[0], transform.position, Quaternion.identity);
    }
}
