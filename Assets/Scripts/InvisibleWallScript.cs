using UnityEngine;
using System.Collections;

public class InvisibleWallScript : MonoBehaviour
{
    public Collider wallCollider;
    public MeshRenderer wallMeshRenderer;
    public bool permVisible = false;
    public float fadeDuration = 1.0f;

    private Coroutine fadeCoroutine;
    void Start()
    {
        if (wallCollider == null)
        {
            wallCollider = GetComponent<Collider>();
        }
        if (wallMeshRenderer == null)
        {
            wallMeshRenderer = GetComponent<MeshRenderer>();
        }
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade(0f); // Start fading to transparent
            Debug.Log("Player has passed through the invisible wall.");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !permVisible)
        {
            StartFade(1f); // Start fading back to opaque
            Debug.Log("Player has exited the invisible wall area.");
        }
    }
    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeTo(targetAlpha));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Material mat = wallMeshRenderer.material;
        Color color = mat.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            mat.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        mat.color = new Color(color.r, color.g, color.b, targetAlpha);

    }
}
