using UnityEngine;

public class SpriteCanvas : MonoBehaviour
{

    [SerializeField] private GameObject spriteCanvas;
    [SerializeField] private GameObject spriteGreen;
    [SerializeField] private GameObject spriteBlue;
    [SerializeField] private GameObject spriteRed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteGreen.SetActive(true);
        spriteBlue.SetActive(true);
        spriteRed.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRedSprite()
    {
        spriteRed.SetActive(false);
        spriteGreen.SetActive(true);
        spriteBlue.SetActive(true);
    }

    public void SetGreenSprite()
    {
        spriteGreen.SetActive(false);
        spriteBlue.SetActive(true);
        spriteRed.SetActive(true);
    }

    public void SetBlueSprite()
    {
        spriteGreen.SetActive(true);
        spriteBlue.SetActive(false);
        spriteRed.SetActive(true);
    }

    public void SetNoSprite()
    {
        spriteGreen.SetActive(true);
        spriteBlue.SetActive(true);
        spriteRed.SetActive(true);
    }
}
