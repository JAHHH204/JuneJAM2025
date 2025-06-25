using UnityEngine;

public class DestroyState : PlayerInterface
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] public LayerMask destroyLayer;

    public void EnterState(PlayerController player)
    {
        player.SetAnims("Destroy");
        TriggerExplosion(player.transform.position);
    }

    public void UpdateState(PlayerController player)
    {
        player.StateTransition(new IdleState());
    }

    public void ExitState(PlayerController player)
    {
        player.SetAnims("Idle");
        

    }

    private void TriggerExplosion(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius, destroyLayer);

        foreach(Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>(); 
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, position, explosionRadius);
                GameObject go = hit.gameObject;
                Object.Destroy(go, destroyDelay);
                Debug.Log(rb);
            }
        }

        Debug.Log("Explosion triggered at " + position);
    }
}