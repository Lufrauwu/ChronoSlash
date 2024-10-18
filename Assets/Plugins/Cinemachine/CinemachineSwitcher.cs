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
    #endregion

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

    private Animator animator;
    private bool panoramicCamera;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchState()
    {
        if (panoramicCamera)
        {
            animator.Play("Menu");
        }
        else
        {
            animator.Play("Panoramic");
        }
        panoramicCamera = !panoramicCamera;
    }
}
