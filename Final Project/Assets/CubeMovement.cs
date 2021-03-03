using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    Vector3 newPosition;
    private float speed = 5.0f;
    void Start () {
        newPosition = transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                newPosition = hit.point;
                transform.position = Vector3.Lerp (transform.position, newPosition, speed * Time.deltaTime);
            }
        }
    }
}