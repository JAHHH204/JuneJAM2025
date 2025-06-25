using UnityEngine;

public class PlatformTimer : MonoBehaviour
{
    [SerializeField] private GameObject Platform;
    [SerializeField] private float destroyTimer = 10f;

    private PlayerController playerController;

    public void SetOwner(PlayerController owner)
    {
        playerController = owner;
    }

    void Update()
    {
        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0f)
        {
            if (Platform != null)
                Destroy(Platform); // Or Destroy(gameObject)

            if (playerController != null)
                playerController.maxObjects--;
        }
    }
}
