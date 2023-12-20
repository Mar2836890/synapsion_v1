using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : MonoBehaviour
{
    // Reference to the button inside the pop-up
    public Button closeButton;

    private void Start()
    {
        // Attach a listener to the button's onClick event
        closeButton.onClick.AddListener(ClosePopup);
    }   


    // Method to close the pop-up
    public void ClosePopup()
    {
        // Deactivate or destroy the pop-up GameObject
        gameObject.SetActive(false); // Alternatively, you can use Destroy(gameObject);
    }
}
