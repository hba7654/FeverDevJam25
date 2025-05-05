using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 cameraRot;
    private CharacterController controller;
    private float xRot = 0;
    private bool lookingBehind;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;

    private void Start()
    {
        lookingBehind = false;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Movement
        controller.Move(transform.TransformDirection(
            moveInput.x * moveSpeed * Time.deltaTime,
            0, 
            moveInput.y * moveSpeed * Time.deltaTime));

        //Camera Look
        Camera.main.transform.localRotation = Quaternion.Euler(xRot, lookingBehind ? 180 : 0, 0);
        transform.Rotate(Vector3.up * cameraRot.x);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End Trigger")
        {
            Debug.Log("Hit End");
            controller.enabled = false;
            transform.position += new Vector3(14, 0, -30);
            controller.enabled = true;
        }

        else if (other.tag == "Start Trigger")
        {
            Debug.Log("Hit Start");
            controller.enabled = false;
            transform.position -= new Vector3(14, 0, -30);
            controller.enabled = true;
        }
    }

    public void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    public void OnLook(InputValue inputValue)
    {
        cameraRot = inputValue.Get<Vector2>() * mouseSensitivity * Time.deltaTime;
        xRot -= cameraRot.y;
        xRot = Mathf.Clamp(xRot, -60, 60);
    }

    public void OnLookBehind(InputValue inputValue)
    {
        lookingBehind = inputValue.isPressed;
    }
}
