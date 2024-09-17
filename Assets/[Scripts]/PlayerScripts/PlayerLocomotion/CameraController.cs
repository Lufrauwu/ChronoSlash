using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform targetToLock;
    [SerializeField] private GameObject lockCamera;
    [SerializeField] private GameObject explorerCamera;
    [SerializeField] private CinemachineFreeLook lockedFreelookComponent;
    [SerializeField] private CinemachineFreeLook explorerFreelookComponent;
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private bool isTargetLocked = false;
    private bool isRestarted = false;
    private Vector2 cameraRotation = Vector2.zero;
    private Vector2 _playerTargetLocation = Vector2.zero;
    private float rotationSpeed = 7f;

    #region Singletone
    private static CameraController Instance;
    public static CameraController GetInstance() 
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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isTargetLocked)
        {
            HandleLockOnCamera();
        }
        else
        {
            if (!isRestarted)
            {
                SetCameraFreelookCameraRotation();
            }
            HandleExplorationCamera();
        }
    }

    public void HandleExplorationCamera()
    {
        lockCamera.SetActive(false);
        explorerCamera.SetActive(true);
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = InputManager.GetInstance().MovementInput().x;
        float verticalInput = InputManager.GetInstance().MovementInput().y;
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        

        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }

    private void SetCameraFreelookCameraRotation()
    {
        explorerFreelookComponent.m_XAxis.Value = lockedFreelookComponent.m_XAxis.Value;
        explorerFreelookComponent.m_YAxis.Value = lockedFreelookComponent.m_YAxis.Value;
        targetToLock = null;
        isRestarted = true;
    }

    public void HandleLockOnCamera()
    {
        targetToLock = ThirdPersonController.targetEnemy;
        explorerCamera.SetActive(false);
        lockCamera.SetActive(true);
        isRestarted = false;
        Vector3 viewDir = targetToLock.transform.position - playerObj.position;
        viewDir.y = 0;
        playerObj.forward += Vector3.Lerp(playerObj.forward, viewDir, Time.deltaTime * rotationSpeed);
        lockedFreelookComponent.m_XAxis.Value = playerObj.transform.rotation.eulerAngles.y;
    }

    public void SetLockOn()
    {
        isTargetLocked = true;
    }

    public void SetLockOff()
    {
        isTargetLocked = false;
    }
    
    
}
