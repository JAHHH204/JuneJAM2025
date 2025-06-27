using UnityEngine;
using UnityEngine.UI;

public class TriggerBlock : MonoBehaviour
{
    [SerializeField] private GameObject wallObject;
    [SerializeField] private GameObject confirmCanvas;
    [SerializeField] private Button acceptButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

        acceptButton.onClick.AddListener(DestroyWall);
        confirmCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            ShowConfirm();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            HideConfirm();
        }
    }

    private void ShowConfirm()
    {
        Time.timeScale = 0;
        confirmCanvas.SetActive(true);

    }

    private void HideConfirm()
    {
        confirmCanvas.SetActive(false);
    }
    private void DestroyWall()
    {
        Destroy(wallObject);
        Destroy(this);
        HideConfirm();
        Time.timeScale = 1;
    }
    
}
