using UnityEngine;

public class IdleState: PlayerInterface 
{
    public void EnterState(PlayerController player)
    {
        player.SetAnims("Idle");
    }

    public void UpdateState(PlayerController player)
    {
        if (player.moveInput != Vector2.zero)
        {
            player.StateTransition(new MoveState()); 
        }

        if (player.isJumping)
        {
            player.StateTransition(new JumpState());
        }

        if (player.isCreating)
        {
            player.StateTransition(new BuilderState());
        }

        if (player.isDestroying)
        {
            player.StateTransition(new DestroyState());
        }

        if (player.isGrabbing)
        {
            player.StateTransition(new GrabState());
        }
    }

    public void ExitState(PlayerController player)
    {

    }
}


