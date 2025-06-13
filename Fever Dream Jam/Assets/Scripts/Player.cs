using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Rendering;

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
    private bool sprinting;
    private bool aimingAtDO;
    private bool flashlightOn;
    private GameObject dreamItems;

    [HideInInspector] public bool dreaming;
    [HideInInspector] public bool canRotate;
    [HideInInspector] public bool puzzleComplete;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMult;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private UnityEngine.UI.Image cursor;
    [SerializeField] private GameObject flashlight;


    private LayerMask layerMask;
    private RaycastHit hit;
    private Ray ray;


    [SerializeField] private GameObject globalVolumeGO;
    private Volume[] volumes;

    // Temporary Sequence variables 

    // Sequence 25 variables


    // Materials
    // private Material lightBlue_Material;
    // private Material black_Material;

    private void Start()
    {
        cam = Camera.main;

        lookingBehind = false;
        sprinting = false;
        canRotate = true;
        flashlightOn = false;
        controller = GetComponent<CharacterController>();
        camController = cam.GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        dreaming = false;

        layerMask = LayerMask.GetMask("Interactable", "Character");

        volumes = globalVolumeGO.GetComponents<Volume>();
        

        print(cursor);


        // lightBlue_Material = Resources.Load("Light Blue.mat", typeof(Material)) as Material;
        // black_Material = Resources.Load("3D Assets/Runes/Materials/Black.mat", typeof(Material)) as Material;
    }

    private void Update()
    {

        if (canRotate)
        {
            //Movement - awake
            if (!dreaming)
            {
                controller.SimpleMove(transform.TransformDirection(
                    moveInput.x * moveSpeed * Time.deltaTime * (sprinting ? sprintMult : 1),
                    0,
                    moveInput.y * moveSpeed * Time.deltaTime * (sprinting ? sprintMult : 1)
                    ));
            }
            //Movement - dreaming
            else
            {
                Vector3 dreamMoveDir = cam.transform.TransformDirection(
                    moveInput.x * moveSpeed / 100 * Time.deltaTime,
                    0,
                    moveInput.y * moveSpeed / 100 * Time.deltaTime);
                //dreamMoveDir.y = 0;
                camController.Move(dreamMoveDir);
            }
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            //Camera Look
            if (!dreaming)
            {
                cam.transform.localRotation = Quaternion.Euler(xRot, lookingBehind ? 180 : 0, 0);
                transform.localRotation = Quaternion.Euler(0, yRot, 0);
            }
            else
            {
                cam.transform.localRotation = Quaternion.Euler(xRot, yRot, 0);
                //cam.transform.Rotate(Vector3.up * cameraRot.x);
            }
        }
        else
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void FixedUpdate()
    {
        // Handles Raycasting 

        // If it hits something
        ray = new Ray(transform.position, transform.forward);

        //Make a Sphere and throwing it in a direction seeing if anythinghits hit (.2 is sphere radius and 1 is the distnace it will travel)
        if (Physics.SphereCast(cam.transform.position, .2f, cam.transform.forward, out hit, 1f, layerMask))
        {
            // Debug.DrawRay(cam.transform.position, cam.transform.forward * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            // Debug.Log(hit.distance);

            aimingAtDO = true;
            cursor.gameObject.SetActive(true);

        }
        // If it doesn't hit something
        else
        {
            //Debug.DrawRay(cam.transform.position, cam.transform.forward * 1000, Color.white);
            //Debug.Log("Did not Hit");

            aimingAtDO = false;
            cursor.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End Trigger")
        {
            Debug.Log("Hit End");
            GetComponent<CapsuleCollider>().enabled = false;
            controller.enabled = false;
            transform.position += new Vector3(14, 0, -30);
            controller.enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;

            if (puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                puzzleComplete = false;
            }
        }

        else if (other.tag == "Start Trigger")
        {
            GetComponent<CapsuleCollider>().enabled = false;
            Debug.Log("Hit Start");
            controller.enabled = false;
            transform.position -= new Vector3(14, 0, -30);
            controller.enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;

            if (puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                puzzleComplete = false;
            }
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
    public void OnSprint(InputValue inputValue)
    {
        sprinting = inputValue.isPressed;
    }

    public void OnDream(InputValue inputValue)
    {
        dreaming = !dreaming;
        camController.enabled = false;
        cam.transform.position = transform.position;
        camController.enabled = true;

        if (dreaming)
        {
            cam.transform.parent = null;
        }
        else
        {
            cam.transform.parent = transform;
        }
        cam.GetComponent<CameraTPer>().enabled = dreaming;
        cam.GetComponent<BoxCollider>().enabled = dreaming;
        SequenceManager.Instance.ShowDreamObjects(dreaming);

        volumes[0].weight = dreaming ? 0.75f : 1;
        volumes[1].enabled = dreaming;

    }

    public void OnInteract(InputValue inputValue)
    {
        // Check if it hit something
        if (aimingAtDO)
        {
            Debug.Log("Interacted with " + hit.collider.gameObject.name);
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            interactable?.Interact();

        }

    }

    public void OnFlashlight(InputValue inputValue)
    {
        if (!dreaming)
        {
            flashlightOn = !flashlightOn;
            flashlight.SetActive(flashlightOn);
        }

    }
}
