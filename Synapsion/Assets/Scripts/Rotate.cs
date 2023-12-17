using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class RotateZoomAndMove : MonoBehaviour
{
    private bool isRotating = false;
    private bool isMoving = false;
    private Vector3 mouseStartPosition;
    private Vector3 objectStartPosition;
    private float zoomSpeed = 0.3f;
    private float moveSpeed = 0.1f;

    // Define the rectangular area where actions are allowed (in screen coordinates)
    public Rect allowedArea = new Rect(0.2f, 0.2f, 0.6f, 0.6f);

    private GameObject displayObj; // Reference to the text display object
    private SphereRandomGenerator sphereRandomGenerator;
    public GameObject yourTextDisplayGameObject;

    void Start()
    {
        // Find the SphereRandomGenerator script on the same GameObject
        sphereRandomGenerator = GetComponent<SphereRandomGenerator>();
    }

    void Update()
    {
        if (displayObj == null)
        {
            displayObj = yourTextDisplayGameObject;
        }

        if (allowedArea.Contains(Input.mousePosition) && displayObj != null && !displayObj.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) && !isRotating)
            {
                StartRotation();
            }

            if (isRotating && Input.GetMouseButton(0))
            {
                RotateStructureByMouse();
            }

            if (Input.GetMouseButtonUp(0) && isRotating)
            {
                StopRotation();
            }

            if (Input.GetMouseButtonDown(1) && !isMoving)
            {
                StartMovement();
            }

            if (isMoving && Input.GetMouseButton(1))
            {
                MoveStructureByMouse();
            }

            if (Input.GetMouseButtonUp(1) && isMoving)
            {
                StopMovement();
            }

            if (IsMouseOverTextDisplay())
            {
                // If the mouse is over the text display, prevent rotation, movement, and zoom
                return;
            }
        }

        // Zoom with the mouse wheel
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
        ZoomStructure(zoomAmount);
    }

    void StartRotation()
    {
        isRotating = true;
        mouseStartPosition = Input.mousePosition;
    }

    void RotateStructureByMouse()
    {
        Vector3 mouseDelta = Input.mousePosition - mouseStartPosition;

        // Adjust the rotation speed based on your preference
        float rotationSpeed = 0.2f;

        // Invert the mouseDelta values here
        float deltaX = -mouseDelta.x * rotationSpeed;
        float deltaY = mouseDelta.y * rotationSpeed;

        // Rotate the parent GameObject around the vertical (up) axis
        transform.Rotate(Vector3.up, deltaX, Space.World);

        // Rotate the parent GameObject around the horizontal axis
        transform.Rotate(Vector3.right, deltaY, Space.World);

        // Update the mouse start position for the next frame
        mouseStartPosition = Input.mousePosition;
    }

    void StopRotation()
    {
        isRotating = false;
    }

    void StartMovement()
    {
        isMoving = true;
        objectStartPosition = transform.position;
        mouseStartPosition = Input.mousePosition;
    }

    void MoveStructureByMouse()
    {
        Vector3 mouseDelta = Input.mousePosition - mouseStartPosition;

        // Calculate the movement in world space
        Vector3 moveDirection = new Vector3(mouseDelta.x, mouseDelta.y, 0);

        // Adjust the movement speed based on your preference
        moveDirection *= moveSpeed;

        // Apply the movement to the object's position
        transform.position = objectStartPosition + moveDirection;
    }

    void StopMovement()
    {
        isMoving = false;
    }

    void ZoomStructure(float zoomAmount)
    {
        if (IsMouseOverTextDisplay())
        {
            // If the mouse is over the text display, prevent zooming
            return;
        }

        // Adjust the zoom speed based on your preference
        float zoomFactor = 1.0f + zoomAmount * zoomSpeed;

        // Apply the zoom factor to the parent GameObject's scale
        transform.localScale *= zoomFactor;

        // Ensure the scale doesn't go below a certain threshold to avoid issues
        transform.localScale = Vector3.Max(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f));
    }

    private bool IsMouseOverTextDisplay()
    {
        // Check if the mouse is over a UI element
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, new List<RaycastResult>());
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Check if any of the UI elements under the mouse are your text display
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == yourTextDisplayGameObject)
            {
                return true;
            }
        }

        return false;
    }
}
