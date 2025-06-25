using UnityEngine;

public class JumpState : PlayerInterface
{
    private float jumpForce = 10f;
    private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    public void EnterState(PlayerController player)
    {
        verticalVelocity = jumpForce;
        player.SetAnims("Jump");
    }

    public void UpdateState(PlayerController player)
    {
        Vector3 move = new Vector3(player.moveInput.x,0,player.moveInput.y);
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * 15f;
        velocity.y=verticalVelocity;

        player.MoveCharacter(velocity);

        if (player.characterController.isGrounded && verticalVelocity < 0.1)
        {
            player.StateTransition(new IdleState());
        }
    }

    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");
    }
}
