using UnityEngine;

public class JumpState : PlayerInterface
{
    private float jumpForce = 12f;
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;
    float airControlMult = 0.4f;

    public void EnterState(PlayerController player)
    {
        verticalVelocity = jumpForce;
        player.SetAnims("Jump");
    }

    public void UpdateState(PlayerController player)
    {
        Vector3 move = new Vector3(player.moveInput.x,0,player.moveInput.y).normalized;
        player.RotateCharacter(move);
        verticalVelocity += gravity * Time.deltaTime;

        
        Vector3 velocity = move * 15f * airControlMult;
        velocity.y=verticalVelocity;

        player.MoveCharacter(velocity);

        if (player.characterController.isGrounded && verticalVelocity < 1)
        {
            player.SetAnims("Land");
            verticalVelocity = 0f;
            player.StateTransition(new IdleState());
        }

        if (player.isCreating)
        {
            player.StateTransition(new BuilderState());

        }

    }

    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");
    }


}
