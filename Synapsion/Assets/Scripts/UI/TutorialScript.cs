using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    // Reference to the button inside the pop-up
    public Button CloseButton;
    public GameObject TutorialScreen;

    private void Start()
    {
        // Attach a listener to the button's onClick event
        CloseButton.onClick.AddListener(ClosePopup);
    }   

    private void OpenPopup()
    {
        gameObject.SetActive(true); 
    }

    public void ToggleTutorial()
    {
        if (TutorialScreen != null)
        {
            // Check if the legend is currently active
            bool isActive = TutorialScreen.activeSelf;

            // Toggle the visibility of the legend
            TutorialScreen.SetActive(!isActive);
        }
    }

    // Method to close the pop-up
    private void ClosePopup()
    {
        // Deactivate or destroy the pop-up GameObject
        gameObject.SetActive(false); // Alternatively, you can use Destroy(gameObject);
    }
}

