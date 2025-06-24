using UnityEngine;

public class MoveState : PlayerInterface
{
    public void EnterState(PlayerController player)
    {
        player.SetAnims("Walk");
    }

    public void UpdateState(PlayerController player)
    {
        Vector3 move = new Vector3(player.moveInput.x, 0, player.moveInput.y).normalized;
        player.MoveCharacter(move * 10f);
        //Debug.Log("MoveInput: " + player.moveInput);

        if (player.moveInput == Vector2.zero)
        {
            player.StateTransition(new IdleState());
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