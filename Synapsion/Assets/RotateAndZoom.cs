// using UnityEngine;

// public class RotateAndZoom : MonoBehaviour
// {
//     private bool isRotating = false;
//     private Vector3 mouseStartPosition;
//     private float zoomSpeed = 5.0f;

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0) && !isRotating)
//         {
//             StartRotation();
//         }

//         if (isRotating && Input.GetMouseButton(0))
//         {
//             RotateStructureByMouse();
//         }

//         if (Input.GetMouseButtonUp(0) && isRotating)
//         {
//             StopRotation();
//         }

//         // Zoom with the mouse wheel
//         float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
//         ZoomStructure(zoomAmount);
//     }

//     void StartRotation()
//     {
//         isRotating = true;
//         mouseStartPosition = Input.mousePosition;
//     }

//     void RotateStructureByMouse()
//     {
//         Vector3 mouseDelta = Input.mousePosition - mouseStartPosition;

//         // Adjust the rotation speed based on your preference
//         float rotationSpeed = 0.1f;

//         // Invert the mouseDelta values here
//         float deltaX = -mouseDelta.x * rotationSpeed;
//         float deltaY = mouseDelta.y * rotationSpeed;

//         // Rotate the parent GameObject around the vertical (up) axis
//         transform.Rotate(Vector3.up, deltaX, Space.World);

//         // Rotate the parent GameObject around the horizontal axis
//         transform.Rotate(Vector3.right, deltaY, Space.World);

//         // Update the mouse start position for the next frame
//         mouseStartPosition = Input.mousePosition;
//     }

//     void StopRotation()
//     {
//         isRotating = false;
//     }

//     void ZoomStructure(float zoomAmount)
//     {
//         // Adjust the zoom speed based on your preference
//         float zoomFactor = 1.0f + zoomAmount * zoomSpeed;

//         // Apply the zoom factor to the parent GameObject's scale
//         transform.localScale *= zoomFactor;

//         // Ensure the scale doesn't go below a certain threshold to avoid issues
//         transform.localScale = Vector3.Max(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f));
//     }
// }
