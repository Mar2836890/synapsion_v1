using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTextDisplay : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform textDisplayRect;
    private Vector2 originalPosition;

    private bool isMoving = false;
    private Vector2 mouseStartPosition;
    private Vector2 objectStartPosition;
    private float moveSpeed = 1.2f;

    void Start()
    {
        textDisplayRect = GetComponent<RectTransform>();
        originalPosition = textDisplayRect.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartMovement();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isMoving && eventData.pointerId == -1)
        {
            MoveStructureByMouse();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isMoving && eventData.pointerId == -1)
        {
            StopMovement();
        }
    }

    void StartMovement()
    {
        isMoving = true;
        objectStartPosition = textDisplayRect.anchoredPosition;
        mouseStartPosition = Input.mousePosition;
    }

    void MoveStructureByMouse()
    {
        Vector2 mouseDelta = (Vector2)Input.mousePosition - mouseStartPosition;
        Vector2 moveDirection = new Vector2(mouseDelta.x, mouseDelta.y);

        moveDirection *= moveSpeed;

        // Calculate the new position without clamping
        Vector2 newPosition = objectStartPosition + moveDirection;

        textDisplayRect.anchoredPosition = newPosition;
    }

    void StopMovement()
    {
        isMoving = false;
    }

    public void ResetToOriginalPosition()
    {
        textDisplayRect.anchoredPosition = originalPosition;
    }
}
