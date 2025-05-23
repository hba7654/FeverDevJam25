using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.UI;

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
    private bool aimingAtDO;
    private GameObject dreamItems;

    [HideInInspector] public bool dreaming;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private UnityEngine.UI.Image cursor;


    private LayerMask layerMask;
    private RaycastHit hit;
    private Ray ray;

    // Temporary Sequence variables 
    private List<string> sequence3Runes;
    private List<string> sequence3SelectedRunes;
    private bool puzzleComplete;

    // Materials
    // private Material lightBlue_Material;
    // private Material black_Material;

    [SerializeField] private Material lightBlue_Material;
    [SerializeField] private Material black_Material;

    private void Start()
    {
        cam = Camera.main;

        lookingBehind = false;
        controller = GetComponent<CharacterController>();
        camController = cam.GetComponent<CharacterController>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        dreaming = false;

        layerMask = LayerMask.GetMask("Interactable", "Character");

        // TEMP
        sequence3Runes = new List<string> { "Inguz", "Wunjo", "Othilla", "Algiz" };
        sequence3SelectedRunes = new List<string>();
        puzzleComplete = false;

        print(cursor);

        
       // lightBlue_Material = Resources.Load("Light Blue.mat", typeof(Material)) as Material;
       // black_Material = Resources.Load("3D Assets/Runes/Materials/Black.mat", typeof(Material)) as Material;
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
            Vector3 dreamMoveDir = cam.transform.TransformDirection(
                moveInput.x * moveSpeed * Time.deltaTime,
                0,
                moveInput.y * moveSpeed * Time.deltaTime);
            //dreamMoveDir.y = 0;
            camController.Move(dreamMoveDir);
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
            cursor.enabled = true;
           
        }
        // If it doesn't hit something
        else
        {
            //Debug.DrawRay(cam.transform.position, cam.transform.forward * 1000, Color.white);
            //Debug.Log("Did not Hit");

            aimingAtDO = false;
            cursor.enabled = false;
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

            if (puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                puzzleComplete = false;
            }
        }

        else if (other.tag == "Start Trigger")
        {
            Debug.Log("Hit Start");
            controller.enabled = false;
            transform.position -= new Vector3(14, 0, -30);
            controller.enabled = true;

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
        SequenceManager.Instance.ShowDreamObjects(dreaming);

    }

    public void OnInteract(InputValue inputValue)
    {
        // Check if it hit something
        if (aimingAtDO)
        {
            // Debug.Log("Interacted with " + hit.collider.gameObject.name);

            if (SequenceManager.Instance.GetSequenceNumber() == 1) //Sequence 3
            {
                // Debug.Log("We are in sequnence 3!"); 
                Sequence3Puzzle(hit.collider.gameObject.name);
            }
        }

    }

    // Handles sequence 3 puzzle
    // Checks if the runes they have interacted with are in the list and tells us when the puzzle is complete
    private void Sequence3Puzzle(string objectName)
    {
        Debug.Log(objectName);

        if (sequence3Runes.Contains(objectName))
        {
            sequence3SelectedRunes.Add(objectName);
            GameObject.Find(objectName + "(hint)").GetComponent<MeshRenderer>().material = lightBlue_Material;
            Debug.Log("Correct!!!!!");
        }

        else
        {

             foreach (string rune in sequence3SelectedRunes)
            {
                GameObject.Find(rune + "(hint)").GetComponent<MeshRenderer>().material = black_Material;
                Debug.Log(rune + "(hint)");
            }

            sequence3SelectedRunes.Clear();
            Debug.Log("Not correct");
        }

        if (sequence3SelectedRunes.Count == 3) 
        {
            Debug.Log("Puzzle complete");
            puzzleComplete = true;
        }
    }

}
