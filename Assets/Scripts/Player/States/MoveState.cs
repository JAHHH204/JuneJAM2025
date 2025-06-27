using UnityEngine;

public class MoveState : PlayerInterface
{
    public Material deadMaterial;
    public void EnterState(PlayerController player)
    {
        deadMaterial=player.deadMaterial;

        player.SetAnims("Move");

        player.ChangeMaterial(deadMaterial);
        
    }

    public void UpdateState(PlayerController player)
    {
        if (player.characterController.isGrounded == true)
        {
            Vector3 move = new Vector3(player.moveInput.x, 0, player.moveInput.y);
            player.MoveCharacter(move * 10f);
            player.RotateCharacter(move);
        }  

            if (player.moveInput == Vector2.zero)
            {
                player.StateTransition(new IdleState());
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
        player.SetAnims("Idle");
    }
}