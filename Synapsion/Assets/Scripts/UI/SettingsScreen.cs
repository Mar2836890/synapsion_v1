using UnityEngine;
using UnityEngine.UI;

public class SettingsPopUp : MonoBehaviour
{
    // Reference to the button inside the pop-up
    public Button closeButton;

    private void Start()
    {
        // Attach a listener to the button's onClick event
        closeButton.onClick.AddListener(ClosePopup);
    }

    // Method to close the pop-up
    private void ClosePopup()
    {
        // Deactivate or destroy the pop-up GameObject
        gameObject.SetActive(false); // Alternatively, you can use Destroy(gameObject);
    }
    public void ToggleSettings()
    {
        if (gameObject != null)
        {
            bool isActive = gameObject.activeSelf;
            gameObject.SetActive(!isActive);
        }
    }
}