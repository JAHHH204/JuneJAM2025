using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInterface currentState;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerControls playerInput;
     public CharacterController characterController;
     public GameObject buildingObject;
    [SerializeField] private LayerMask destroyLayer;
    [SerializeField] private PauseMenu pauseMenu;
    public LayerMask DestroyLayer => destroyLayer;
    [Header ("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 5f, -10f);
    [SerializeField] private float cameraSmoothSpeed = 5f;
    [Header("Checks")]
     public Vector2 moveInput {  get; private set; }
     public bool isJumping { get; set; }

    public bool isCreating { get; set; }

     public bool isGrabbing { get; set; }

     public bool isDestroying { get; set; }

    [Header("Extra")]
    private Vector3 gravity = Vector3.zero;
     public int maxObjects;
    public GameObject lastSpawnedPlatform;
    private void Awake()
    {
        maxObjects = 0;
        playerInput = new PlayerControls();
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        pauseMenu = GetComponent<PauseMenu>();
        playerInput.Inputs.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Inputs.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Inputs.Jump.performed += ctx => isJumping=true;

        playerInput.Inputs.Build.performed += ctx => isCreating=true;
        playerInput.Inputs.Destroy.performed += ctx => isDestroying = true;
        playerInput.Inputs.Grab.performed += ctx => isGrabbing = true;

        playerInput.Inputs.Pause.performed += ctx => pauseMenu.TogglePauseMenu();
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();

    void Start()
    {
        StateTransition(new IdleState());  
    }

    void Update()
    {

        if (isDestroying)
        {
            StateTransition(new DestroyState());
        }

        currentState?.UpdateState(this);
        isJumping = false;
        isGrabbing = false;
        isCreating = false;
        isDestroying = false;
        ApplyGravity();



    }
    private void LateUpdate()
    {
        CameraFollow();
    }


    public void StateTransition(PlayerInterface newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetAnims(string animName)
    {
        if (playerAnimator != null) {
            playerAnimator.Play(animName);
        }
    }

    public void MoveCharacter(Vector3 moveDirection)
    {

            characterController.Move(moveDirection * Time.deltaTime);
        
    }

    public void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float rotationSpeed = 1080f; // degrees per second
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
        }
    }
    private void ApplyGravity()
    {
        if (characterController.isGrounded && gravity.y < 0.1)
            gravity.y = -2f;

        gravity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(gravity * Time.deltaTime);
    }


    public void CameraFollow()
    {
        if (cameraTransform == null) return;

        Vector3 desiredPosition = transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);

        cameraTransform.position = smoothedPosition;
        cameraTransform.LookAt(transform); 
    }

}
