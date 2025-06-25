using UnityEngine;

public class GrabState : PlayerInterface
{
    private GameObject grabbedObject;
    private Rigidbody grabbedRb;
    private Transform grabPoint;
    [SerializeField] private float throwForce = 500f;

    private bool isHolding = false;

    public void EnterState(PlayerController player)
    {
        player.SetAnims("Grab");

        // Create the grab point (if not created already)
        if (grabPoint == null)
        {
            grabPoint = new GameObject("GrabPoint").transform;
            grabPoint.SetParent(player.transform);
            grabPoint.localPosition = new Vector3(0, 1.5f, 1.5f); // Adjust as needed
        }

        RaycastHit hit;
        float grabDistance = 10f;
        LayerMask grabLayer = LayerMask.GetMask("Grabbable");

        if (Physics.Raycast(player.transform.position + Vector3.up, player.transform.forward, out hit, grabDistance, grabLayer))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedRb = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedRb != null)
            {
                grabbedRb.useGravity = false;
                grabbedRb.isKinematic = false; // Use physics
                grabbedRb.linearDamping = 10f; // Smooth drag effect while held
            }

            grabbedObject.transform.SetParent(grabPoint);
            grabbedObject.transform.localPosition = Vector3.zero;
            isHolding = true;
        }
        else
        {
            player.StateTransition(new IdleState());
        }
    }

    public void UpdateState(PlayerController player)
    {
        // Keep the object dragged toward the grab point
        if (isHolding && grabbedObject != null && grabbedRb != null)
        {
            Vector3 direction = grabPoint.position - grabbedObject.transform.position;
            grabbedRb.AddForce(direction * 50f); // Drag force

            // If player presses grab again, throw the object
            if (player.isGrabbing)
            {
                ThrowObject(player);
                player.StateTransition(new IdleState());
            }
        }
    }

    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");
    }

    private void ThrowObject(PlayerController player)
    {
        if (grabbedObject != null && grabbedRb != null)
        {
            grabbedObject.transform.SetParent(null);
            grabbedRb.useGravity = true;
            grabbedRb.linearDamping = 0f;

            grabbedRb.linearVelocity = Vector3.zero; // Clear old movement
            Vector3 throwDirection = (player.transform.forward + Vector3.up * 0.3f).normalized;
            grabbedRb.AddForce(throwDirection * throwForce);

            Debug.Log("Thrown: " + grabbedObject.name);
        }

        grabbedObject = null;
        grabbedRb = null;
        isHolding = false;
    }
}
