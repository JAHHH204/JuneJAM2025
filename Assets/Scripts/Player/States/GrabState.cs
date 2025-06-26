using UnityEngine;

public class GrabState : PlayerInterface
{
    public void EnterState(PlayerController player)
    {
    if (player.grabbedObject == null)
    {
        TryGrab(player);
    }

    player.isGrabbing = false; 
    }
    public void UpdateState(PlayerController player)
    {
        if (player.grabbedObject != null)
        {
            Vector3 targetPos = player.grabPoint.position;
            player.grabbedObject.transform.position = Vector3.Lerp(
                player.grabbedObject.transform.position,
                targetPos,
                Time.deltaTime * player.grabLerpSpeed
            );
        }
        else
        {
            // Don't auto-exit if object is dropped manually
            player.StateTransition(new IdleState());
        }
    }


    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");
    }

    private void TryGrab(PlayerController player)
    {
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, player.grabDistance, player.grabbableLayer))
        {
            GameObject target = hit.collider.gameObject;
            Rigidbody rb = target.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;

                player.grabbedObject = target;
                target.transform.position = player.grabPoint.position;
                target.transform.parent = player.grabPoint;
            }
        }
    }
}
