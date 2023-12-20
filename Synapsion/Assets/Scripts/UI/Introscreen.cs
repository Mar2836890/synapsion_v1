using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : MonoBehaviour
{
    // Reference to the button inside the pop-up
    public Button closeButton;
    public Button openButton;

    private void Start()
    {
        // Attach a listener to the button's onClick event
        closeButton.onClick.AddListener(ClosePopup);
        openButton.onClick.AddListener(OpenPopup);
    }   

    private void OpenPopup()
    {
        gameObject.SetActive(true); 
    }

    // Method to close the pop-up
    private void ClosePopup()
    {
        // Deactivate or destroy the pop-up GameObject
        gameObject.SetActive(false); // Alternatively, you can use Destroy(gameObject);
    }
}
