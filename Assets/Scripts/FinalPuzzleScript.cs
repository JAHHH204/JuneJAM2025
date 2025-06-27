using UnityEngine;

public class FinalPuzzleScript : MonoBehaviour
{
    public GameObject targetToWatchDie;
    public Vector3 targetPosition;
    public float moveSpeed = 1f;

    void Start()
    {
        // Fix: Replace OnDestroy() with a Unity event subscription to detect when the GameObject is destroyed.
        if (targetToWatchDie != null)
        {
            var targetDestroyNotifier = targetToWatchDie.AddComponent<DestroyNotifier>();
            targetDestroyNotifier.OnDestroyed += OnTargetDestroyed;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTargetDestroyed()
    {
        // Move the object to the target position
        StartCoroutine(MoveToTargetPosition());
    }

    private System.Collections.IEnumerator MoveToTargetPosition()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        // Optionally, you can stop the movement when the target position is reached
        transform.position = targetPosition;
    }
}

// Helper class to notify when a GameObject is destroyed
public class DestroyNotifier : MonoBehaviour
{
    public event System.Action OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
