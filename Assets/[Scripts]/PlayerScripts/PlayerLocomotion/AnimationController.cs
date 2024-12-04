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
        if (currentAnimation == "A_000000" || currentAnimation == "A_0000" || currentAnimation == "A_0101" || currentAnimation == "A_1000" || currentAnimation == "A_1100"
            || currentAnimation == "A_00000" || currentAnimation == "A_00011" || currentAnimation == "A_11000" || currentAnimation == "A_001100" || currentAnimation == "A_000111" 
            || currentAnimation == "A_110011" || currentAnimation == "A_111111"|| currentAnimation == "A_null" || currentAnimation == "WallRunLeft" || currentAnimation == "WallRunRight")
        {
            return;
        }
        if (GameManager.GetInstance().currentGameState == GAME_STATE.EXPLORATION && InputManager.GetInstance().MovementInput().magnitude != 0 )
        {
            ChangeAnimation("Run_anim");
        }
        else if (InputManager.GetInstance().MovementInput().y == -1)
        {
            ChangeAnimation("Walk_Backward_anim");
        }
        else if (InputManager.GetInstance().MovementInput().y == 1)
        {
            ChangeAnimation("Enfrente");
        }
        else if (InputManager.GetInstance().MovementInput().x == -1)
        {
            ChangeAnimation("izquierda");
        }
        else if (InputManager.GetInstance().MovementInput().x == 1)
        {
            ChangeAnimation("derecha");
        }
        else
        {
            ChangeAnimation("Idle_anim");
        }
    }

    public void ChangeAnimation(string animation, float crossfadeTime = 0.2f, float time= 0)
    {
        if (time > 0) StartCoroutine(Wait());
        else Validate();

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(time );
            Validate();
        }

        void Validate()
        {
            if (currentAnimation != animation)
            {   
                currentAnimation = animation;

                if (currentAnimation == "")
                {
                    CheckAnimation();
                }
                else
                {
                    animator.CrossFade(animation, crossfadeTime);
                }
            }
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
        GameManager.GetInstance().ChangeGameState(GAME_STATE.END_TURN);
        Debug.Log("end turn");
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

    public void SpawnParticles(int index)
    {
        Vector3 spawnPosition = transform.position + Vector3.up;
        particleInstance = Instantiate(particles[index], spawnPosition , transform.rotation);
    }
}
