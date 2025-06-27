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
        spriteGreen.SetActive(false);
        spriteBlue.SetActive(false);
        spriteRed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRedSprite()
    {
        spriteRed.SetActive(true);
        spriteGreen.SetActive(false);
        spriteBlue.SetActive(false);
    }

    public void SetGreenSprite()
    {
        spriteGreen.SetActive(true);
        spriteBlue.SetActive(false);
        spriteRed.SetActive(false);
    }

    public void SetBlueSprite()
    {
        spriteGreen.SetActive(false);
        spriteBlue.SetActive(true);
        spriteRed.SetActive(false);
    }

    public void SetNoSprite()
    {
        spriteGreen.SetActive(false);
        spriteBlue.SetActive(false);
        spriteRed.SetActive(false);
    }
}
