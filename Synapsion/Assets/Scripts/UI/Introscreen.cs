using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : MonoBehaviour
{
    // ref close button
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
    }   


    // Method to close the pop-up
    public void ClosePopup()
    {
        gameObject.SetActive(false); 
    }
}
