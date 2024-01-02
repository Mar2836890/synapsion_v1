using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Button CloseButton;
    public GameObject TutorialScreen;

    private void Start()
    {
        CloseButton.onClick.AddListener(ClosePopup);
    }   

    private void OpenPopup()
    {
        gameObject.SetActive(true); 
    }

    // Open or close popup depending on current state
    public void ToggleTutorial()
    {
        if (TutorialScreen != null)
        {
            bool isActive = TutorialScreen.activeSelf;
            TutorialScreen.SetActive(!isActive);
        }
    }

    // Method to close the pop-up
    private void ClosePopup()
    {
        // Deactivate or destroy the pop-up GameObject
        gameObject.SetActive(false);
    }
}

