using UnityEngine;

public class CameraController : MonoBehaviour
{

    void Update()
    {
        HandleMovement();
        HandleScroll();
    }

    private void HandleMovement()
    {
        Vector2 dir = new();
        if (Input.GetKey(KeyCode.W))
        {
            dir.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x += 1;
        }
        transform.position = new Vector3(transform.position.x + dir.x * 5f * Time.deltaTime, transform.position.y, transform.position.z + dir.y * 5f * Time.deltaTime);
    }

    private void HandleScroll() { }
}
