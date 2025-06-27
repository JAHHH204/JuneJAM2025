using UnityEngine;

public class DestroyState : PlayerInterface
{
     private float explosionRadius = 10f;
     private float explosionForce = 700f;
     private float destroyDelay = 2f;
     public LayerMask destroyLayer;
    private Material destroyMaterial;
    private SpriteCanvas spriteCanvas;

    public void EnterState(PlayerController player)
    {
        player.SetAnims("Destroy");
        player.ChangeMaterial(player.destroyMaterial);
        player.PlaySoundOnce(player.audioManager.destroySFX); 
        TriggerExplosion(player.transform.position, player.DestroyLayer);

    }

    public void UpdateState(PlayerController player)
    {
        player.StateTransition(new IdleState());


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