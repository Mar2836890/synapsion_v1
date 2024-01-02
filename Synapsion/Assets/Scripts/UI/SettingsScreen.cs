using UnityEngine;
using UnityEngine.UI;

public class SettingsPopUp : MonoBehaviour
{
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
    }

    // Method to close the pop-up
    private void ClosePopup()
    {
        gameObject.SetActive(false); 
    }

    // Open or close popup depending on current state
    public void ToggleSettings()
    {
        if (gameObject != null)
        {
            bool isActive = gameObject.activeSelf;
            gameObject.SetActive(!isActive);
        }
    }
}