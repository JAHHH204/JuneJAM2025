using UnityEngine;

public class FogPainter : MonoBehaviour
{
    public RenderTexture fogMask;      // RenderTexture used to mask fog
    public Transform player;           // The player to follow
    public Camera cam;                 // Perspective camera
    public float radius = 3f;          // Radius in world units
    public Material brushMaterial;     // Material using FogRevealBrush shader

    private Texture2D brush;

    void Start()
    {
        brush = CreateFeatheredBrush(128);
        brushMaterial.mainTexture = brush;

        // Clear the fog mask ONCE to white (full fog)
        RenderTexture.active = fogMask;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;
    }

    void Update()
    {
        if (fogMask == null || player == null || cam == null)
            return;

        // Convert to screen space
        Vector3 screenPos = cam.WorldToScreenPoint(player.position);
        if (screenPos.z < 0) return;

        float px = screenPos.x / cam.pixelWidth * fogMask.width;
        float py = screenPos.y / cam.pixelHeight * fogMask.height;

        // Adjust pixel radius by distance
        float distance = Vector3.Distance(cam.transform.position, player.position);
        float pixelsPerUnit = (fogMask.height / (2f * Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2))) / distance;
        float pixelRadius = radius * pixelsPerUnit;
        Rect rect = new Rect(px - pixelRadius, py - pixelRadius, pixelRadius * 2, pixelRadius * 2);

        // Draw onto the fog mask
        RenderTexture.active = fogMask;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, fogMask.width, 0, fogMask.height);
        Graphics.DrawTexture(rect, brush, brushMaterial);
        GL.PopMatrix();
        RenderTexture.active = null;
    }


    // Feathered circular brush
    Texture2D CreateFeatheredBrush(int size)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        Color[] cols = new Color[size * size];
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float maxDist = center.x;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(center, new Vector2(x, y));
                float alpha = Mathf.Clamp01(1f - dist / maxDist); // 1 in center, fades out
                cols[y * size + x] = new Color(1, 1, 1, alpha);
            }
        }

        tex.SetPixels(cols);
        tex.Apply();
        return tex;
    }

}
