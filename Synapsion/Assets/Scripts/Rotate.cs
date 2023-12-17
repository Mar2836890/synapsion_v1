using UnityEngine;
using UnityEngine.EventSystems;

public class RotateZoomAndMove : MonoBehaviour
{
    private bool isRotating = false;
    private bool isMoving = false;
    private Vector3 mouseStartPosition;
    private Vector3 objectStartPosition;
    private float zoomSpeed = 0.3f;
    private float moveSpeed = 0.1f;

    // Reference to the information GameObject
    public GameObject informationObject;

    // Define the rectangular area where actions are allowed (in screen coordinates)
    public Rect allowedArea = new Rect(0.2f, 0.2f, 0.6f, 0.6f);

    void Update()
    {
        if (allowedArea.Contains(Input.mousePosition))
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
        }

        // Check if the cursor is over the informationObject
        bool isCursorOverInformation = IsCursorOverInformation();

        // Zoom with the mouse wheel only if the cursor is not over the informationObject
        if (!isCursorOverInformation)
        {
            float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
            ZoomStructure(zoomAmount);
        }
    }

    bool IsCursorOverInformation()
    {
        // Check if the cursor is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;  // The cursor is over a UI element; don't perform zooming
        }

        // Raycast from the mouse position to detect if it hits the informationObject
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Adjust the layer mask if necessary to only hit the informationObject
        int layerMask = LayerMask.GetMask("DisplayLayer");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.collider.gameObject == informationObject;
        }

        return false;
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
        // Adjust the zoom speed based on your preference
        float zoomFactor = 1.0f + zoomAmount * zoomSpeed;

        // Apply the zoom factor to the parent GameObject's scale
        transform.localScale *= zoomFactor;

        // Ensure the scale doesn't go below a certain threshold to avoid issues
        transform.localScale = Vector3.Max(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f));
    }


}
