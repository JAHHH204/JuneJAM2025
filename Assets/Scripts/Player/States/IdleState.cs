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
    }

    public void ExitState(PlayerController player)
    {

    }
}


