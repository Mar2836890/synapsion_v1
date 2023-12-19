// using UnityEngine;
// using UnityEngine.EventSystems;

// public class DraggableTextDisplay : MonoBehaviour, IPointerDownHandler, IDragHandler
// {
//     private RectTransform textDisplayRect;
//     private RectTransform draggablePart;
//     private Vector2 offset;
//     private Vector2 originalPosition;

//     public GameObject TopPart;
//     public Rect movementBounds;

//     private bool isMoving = false;
//     private Vector2 mouseStartPosition;
//     private Vector2 objectStartPosition;
//     private float moveSpeed = 0.1f;

//     void Start()
//     {
//         textDisplayRect = GetComponent<RectTransform>();
//         draggablePart = TopPart.GetComponent<RectTransform>();
//         originalPosition = textDisplayRect.anchoredPosition;
//     }

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         RectTransformUtility.ScreenPointToLocalPointInRectangle(
//             draggablePart, eventData.position, eventData.pressEventCamera, out offset);
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         Vector2 newPosition;

//         RectTransformUtility.ScreenPointToLocalPointInRectangle(
//             draggablePart, eventData.position, eventData.pressEventCamera, out newPosition);

//         newPosition = newPosition - offset;

//         newPosition.x = Mathf.Clamp(newPosition.x, movementBounds.x, movementBounds.x + movementBounds.width);
//         newPosition.y = Mathf.Clamp(newPosition.y, movementBounds.y, movementBounds.y + movementBounds.height);

//         textDisplayRect.anchoredPosition = newPosition;
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(1) && !isMoving)
//         {
//             StartMovement();
//         }

//         if (isMoving && Input.GetMouseButton(1))
//         {
//             MoveStructureByMouse();
//         }

//         if (Input.GetMouseButtonUp(1) && isMoving)
//         {
//             StopMovement();
//         }
//     }

//     void StartMovement()
//     {
//         isMoving = true;
//         objectStartPosition = textDisplayRect.anchoredPosition;
//         mouseStartPosition = Input.mousePosition;
//     }

//     void MoveStructureByMouse()
//     {
//         Vector2 mouseDelta = (Vector2)Input.mousePosition - mouseStartPosition;
//         Vector2 moveDirection = new Vector2(mouseDelta.x, mouseDelta.y);

//         moveDirection *= moveSpeed;

//         textDisplayRect.anchoredPosition = objectStartPosition + moveDirection;
//     }

//     void StopMovement()
//     {
//         isMoving = false;
//     }

//     public void ResetToOriginalPosition()
//     {
//         textDisplayRect.anchoredPosition = originalPosition;
//     }
// }
