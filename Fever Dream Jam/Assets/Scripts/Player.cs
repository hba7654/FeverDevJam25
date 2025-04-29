using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector3 moveDir;
    private Vector2 moveInput;
    private Vector2 cameraRot;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;

    private void Update()
    {
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        cameraRot.x = Mathf.Clamp(cameraRot.x, -45, 45);

        transform.Rotate(new Vector3(0f, cameraRot.x * Time.deltaTime * mouseSensitivity, 0f), Space.World);
        transform.Rotate(new Vector3(-cameraRot.y * Time.deltaTime * mouseSensitivity, 0f, 0f), Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End Trigger")
        {
            Debug.Log("Hit End");
            transform.position += new Vector3(10, 0, -30);
        }

        else if (other.tag == "Start Trigger")
        {
            Debug.Log("Hit Start");
            transform.position -= new Vector3(10, 0, -30);
        }
    }

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        moveDir.y = 0;
    }

    public void OnLook(InputValue inputValue)
    {
        Debug.Log("looking");
        cameraRot = inputValue.Get<Vector2>();
    }
}
