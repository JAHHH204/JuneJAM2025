using UnityEngine;

public class GrabState : PlayerInterface
{
    public void EnterState(PlayerController player)
    {
        player.SetAnims("Grabbing");
    }

    public void UpdateState(PlayerController player)
    {
        RaycastHit hit;
        float grabDistance = 10f;
        int grabLayer = LayerMask.GetMask("Grabbable");

        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, grabDistance,grabLayer))
        {
            GameObject grabbedObject = hit.collider.gameObject;
            Debug.Log("Grabbed:" + grabbedObject.name);
        }
        player.StateTransition(new IdleState());
    }

    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");


    }
}
