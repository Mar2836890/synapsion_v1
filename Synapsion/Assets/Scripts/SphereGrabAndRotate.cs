using UnityEngine;

public class SphereGrabAndRotate : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private GameObject grabbedStructure;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Structure"))
                {
                    StartDragging(hit.collider.gameObject);
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                MoveStructure();
            }
            else
            {
                RotateSphere();
            }
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }

    void StartDragging(GameObject structure)
    {
        isDragging = true;
        grabbedStructure = structure;
        offset = grabbedStructure.transform.position - GetMouseWorldPos();
    }

    void RotateSphere()
    {
        if (grabbedStructure != null)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            grabbedStructure.transform.position = Vector3.Lerp(grabbedStructure.transform.position, targetPos, 10f * Time.deltaTime);
        }
    }

    void MoveStructure()
    {
        if (grabbedStructure != null)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            grabbedStructure.transform.position = Vector3.Lerp(grabbedStructure.transform.position, targetPos, 10f * Time.deltaTime);
        }
    }

    void StopDragging()
    {
        isDragging = false;
        grabbedStructure = null;
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
