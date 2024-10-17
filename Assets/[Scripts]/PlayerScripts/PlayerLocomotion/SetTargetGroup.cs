using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetTargetGroup : MonoBehaviour
{
    #region Singletone
    private static SetTargetGroup Instance;
    public static SetTargetGroup GetInstance() 
    { 
        return Instance;
    }
    #endregion

    [SerializeField] private Transform player;
    [SerializeField] private ThirdPersonController tpController;
    public static Transform enemy;
    
    private CinemachineTargetGroup group;
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
        
        group = GetComponent<CinemachineTargetGroup>();
        tpController = FindAnyObjectByType<ThirdPersonController>();
    }

    public void ChangeTargetGroup()
    {
        enemy = tpController.GetActiveEnemy();
        if (enemy != null)
        {
            group.m_Targets = new CinemachineTargetGroup.Target[]
            {
                new CinemachineTargetGroup.Target {target = player, weight = 1f, radius =  1f},
                new CinemachineTargetGroup.Target { target = enemy, weight = 0.2f, radius = 1f }
            };
            
        }
        else
        {
            group.m_Targets = new CinemachineTargetGroup.Target[]
            {
                new CinemachineTargetGroup.Target { target = player, weight = 1f, radius = 1f }
            };
        }
    }
}
