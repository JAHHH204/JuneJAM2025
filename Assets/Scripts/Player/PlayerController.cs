using System.Collections.Generic;
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

    public bool isFallingDown { get; set; }


    [Header("Extra")]
    private Vector3 gravity = Vector3.zero;
     public int maxObjects;
    public GameObject lastSpawnedPlatform;
    public ParticleSystem destroyParticles;
    public ParticleSystem grabParticles;
    public ParticleSystem dustParticles;
    [SerializeField] private SkinnedMeshRenderer playerMesh;



    [Header("Jumping")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float verticalVelocity = 0f;
    [SerializeField] private float airControlMult = 0.4f;
    [SerializeField] private bool isFalling => verticalVelocity < 0f;

    [Header("Grabbing")]
    public Transform grabPoint; 
    public float grabDistance = 3f;
    public float grabLerpSpeed = 10f;
    public LayerMask grabbableLayer;
    public GameObject grabbedObject;

    [Header("SFX")]
    public AudioManager audioManager;

    [Header("Materials")]
    public Material destroyMaterial;
    public Material grabMaterial;
    public Material createMaterial;
    public Material deadMaterial;

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


        playerInput.Inputs.Pause.performed += ctx => pauseMenu.TogglePauseMenu();

        playerInput.Inputs.Grab.performed += ctx => {
            if (grabbedObject == null)
            {
                isGrabbing = true;
            }
            else
            {
                DropObject();
                StateTransition(new IdleState()); // Exit GrabState after dropping
            }
        };



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

        HandleMovement();

        if (isJumping && characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
            SetAnims("Jump");


            if (grabbedObject != null)
            {
                grabbedObject.transform.position += Vector3.up * 0.2f;
            }
        }

        ApplyGravity();

        currentState?.UpdateState(this);

        isJumping = false;

        isCreating = false;
        isDestroying = false;
        isFallingDown = false;



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

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        RotateCharacter(move);

        float moveSpeed = characterController.isGrounded ? 10f : 10f * airControlMult;

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        MoveCharacter(velocity);

        if (moveInput == Vector2.zero && characterController.isGrounded && verticalVelocity <= 0f)
        {
            StateTransition(new IdleState());
        }

        if (isCreating && characterController.isGrounded)
        {
            TryBuild();
            StateTransition(new IdleState());
        }

        if (isDestroying)
        {
            StateTransition(new DestroyState());
        }


    }

    public void MoveCharacter(Vector3 moveDirection)
    {

            characterController.Move(moveDirection * Time.deltaTime /1.5f);
        
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
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Stick to ground
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }



    public void CameraFollow()
    {
        if (cameraTransform == null) return;

        Vector3 desiredPosition = transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed * Time.deltaTime);

        cameraTransform.position = smoothedPosition;
        cameraTransform.LookAt(transform); 
    }

    public void TryBuild()
    {
        SetAnims("Create");
        if (maxObjects < 1)
        {
            if (lastSpawnedPlatform != null)
                GameObject.Destroy(lastSpawnedPlatform);

            Vector3 spawnPos = grabPoint.transform.position -grabPoint.transform.forward +transform.right;
            GameObject spawnedPlatform = GameObject.Instantiate(buildingObject, spawnPos, grabPoint.transform.rotation);
            lastSpawnedPlatform = spawnedPlatform;
            isCreating = false;
        }
    }

    public void DropObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            grabbedObject.transform.parent = null;
            grabbedObject = null;
            isGrabbing = false;
        }
    }

    public void PlayParticles(string particleName)
    {
        if (particleName == "Destroy")
            destroyParticles.Play();
        else if (particleName == "Grab")
            grabParticles.Play();
        else if (particleName == "Land")
            dustParticles.Play();
    }

    public void PlaySoundOnce(AudioClip clipName)
    {
        audioManager.PlayClip(clipName);
    }

    public void ChangeMaterial(Material playerMaterial)
    {
        playerMesh.material = playerMaterial;
    }
}
