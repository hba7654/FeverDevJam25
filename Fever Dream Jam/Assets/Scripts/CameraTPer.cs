using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraTPer : MonoBehaviour
{
    CharacterController controller;
    [SerializeField ] Player player; 

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End Trigger")
        {
            Debug.Log("Cam Hit End");
            GetComponent<BoxCollider>().enabled = false;
            controller.enabled = false;
            transform.position += new Vector3(14, 0, -30);
            controller.enabled = true;
            GetComponent<BoxCollider>().enabled = true;

            if (player.puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                transform.parent.GetComponent<Player>().puzzleComplete = false;
            }
        }

        else if (other.tag == "Start Trigger")
        {
            Debug.Log("Cam Hit Start");
            GetComponent<BoxCollider>().enabled = false;
            controller.enabled = false;
            transform.position -= new Vector3(14, 0, -30);
            controller.enabled = true;
            GetComponent<BoxCollider>().enabled = true;

            if (player.puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                transform.parent.GetComponent<Player>().puzzleComplete = false;
            }
        }
    }
}
