using UnityEngine;

public class SphereGrabAndRotate : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Sphere"))
                {
                    StartDragging(hit.collider.gameObject);
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            RotateSphere();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }

    void StartDragging(GameObject sphere)
    {
        isDragging = true;
        offset = sphere.transform.position - GetMouseWorldPos();
    }

    void RotateSphere()
    {
        GameObject grabbedSphere = GetGrabbedSphere();
        if (grabbedSphere != null)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;
            grabbedSphere.transform.position = Vector3.Lerp(grabbedSphere.transform.position, targetPos, 10f * Time.deltaTime);
        }
    }

    void StopDragging()
    {
        isDragging = false;
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        plane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    GameObject GetGrabbedSphere()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Sphere"))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }
}
