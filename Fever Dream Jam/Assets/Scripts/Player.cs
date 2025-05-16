using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Camera cam;
    private Vector2 moveInput;
    private Vector2 cameraRot;
    private CharacterController controller;
    private CharacterController camController;
    private float xRot = 0;
    private float yRot = 0;
    private bool lookingBehind;

    private bool dreaming;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;

    private void Start()
    {
        cam = Camera.main;

        lookingBehind = false;
        controller = GetComponent<CharacterController>();
        camController = cam.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dreaming = false;
    }

    private void Update()
    {
        //Movement - awake
        if (!dreaming)
        {
            controller.Move(transform.TransformDirection(
                moveInput.x * moveSpeed * Time.deltaTime,
                0,
                moveInput.y * moveSpeed * Time.deltaTime));
        }
        //Movement - dreaming
        else
        {
            camController.Move(transform.TransformDirection(
                moveInput.x * moveSpeed * Time.deltaTime,
                0,
                moveInput.y * moveSpeed * Time.deltaTime));
        }

        //Camera Look
        //FIX DREAMING ROTATION
        if (!dreaming)
        {
            cam.transform.localRotation = Quaternion.Euler(xRot, lookingBehind ? 180 : 0, 0);
            transform.Rotate(Vector3.up * cameraRot.x);
        }
        else
        {
            cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
            //cam.transform.Rotate(Vector3.up * cameraRot.x);
        }
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
        yRot += cameraRot.x;
    }

    public void OnLookBehind(InputValue inputValue)
    {
        lookingBehind = inputValue.isPressed;
    }

    public void OnDream(InputValue inputValue)
    {
        dreaming = !dreaming;
        camController.enabled = false;
        cam.transform.position = transform.position;
        camController.enabled = true;

        if(dreaming)
        {
            cam.transform.parent = null;
        }
        else
        {
            cam.transform.parent = transform;
        }

    }
}
