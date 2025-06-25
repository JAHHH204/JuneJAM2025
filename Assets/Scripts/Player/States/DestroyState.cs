using UnityEngine;

public class DestroyState : PlayerInterface
{
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] public LayerMask destroyLayer;

    public void EnterState(PlayerController player)
    {
        player.SetAnims("Destroy");
        TriggerExplosion(player.transform.position, player.DestroyLayer);
    }

    public void UpdateState(PlayerController player)
    {
        player.StateTransition(new IdleState());
        player.SetAnims("Idle");
    }

    public void ExitState(PlayerController player)
    {
        

    }

    private void TriggerExplosion(Vector3 position, LayerMask destroyLayer)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius, destroyLayer);

        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.DrawLine(position, hit.transform.position, Color.red, 1f);
                rb.AddExplosionForce(explosionForce, position, explosionRadius);
                Object.Destroy(hit.gameObject, destroyDelay);
            }
        }

    }

}