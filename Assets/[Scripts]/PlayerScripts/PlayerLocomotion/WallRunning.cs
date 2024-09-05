using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{

    [Header("Wall Running")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float maxWallRunTime;
    private float wallRunTimer;

    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")] 
    [SerializeField] private float wallcheckDisance;
    [SerializeField] private float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;
    
    
    [Header("References")]
    [SerializeField] private Transform orientation;
    private Rigidbody rigidbody;
    private ThirdPersonController thirdPersonController;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }


    void Update()
    {
        
    }

    private void CheckWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out leftWallHit, wallcheckDisance, wallLayer);
    }
}
