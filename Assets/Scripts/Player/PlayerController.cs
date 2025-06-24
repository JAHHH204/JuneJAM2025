using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private PlayerInterface currentState;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerControls playerInput;
    [SerializeField] public CharacterController characterController;

    [SerializeField] public Vector2 moveInput {  get; private set; }
    [SerializeField] public bool isJumping { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 gravity = Vector3.zero;
    private void Awake()
    {
        playerInput = new PlayerControls();
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput.Inputs.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Inputs.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Inputs.Jump.performed += ctx => isJumping=true;
    }

    private void OnEnable() => playerInput.Enable();
    private void OnDisable() => playerInput.Disable();

    void Start()
    {
        StateTransition(new IdleState());  
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState(this);
        isJumping = false;
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
        //Debug.Log("Calling MoveCharacter. Direction: " + moveDirection);

        if (characterController.isGrounded && gravity.y < 0)
            gravity.y = -2f;

        gravity.y += Physics.gravity.y * Time.deltaTime;
        Vector3 fullMove = (moveDirection + gravity) * Time.deltaTime;

        //Debug.Log("Final move vector: " + fullMove);
        characterController.Move(fullMove);
    }


}
