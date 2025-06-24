using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private PlayerInterface currentState;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerControls playerInput;
    [SerializeField] private CharacterController characterController;

    [SerializeField] public Vector2 moveInput {  get; private set; }
    [SerializeField] public bool isJumping { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        playerInput = new PlayerControls();
        characterController = new CharacterController();
        playerAnimator = GetComponent<Animator>();

        playerInput.Inputs.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Inputs.Move.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Inputs.Jump.performed += ctx => isJumping=false;
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


}
