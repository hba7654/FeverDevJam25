using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraTPer : MonoBehaviour
{
    CharacterController controller;
    Player player; 

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "End Trigger")
        {
            Debug.Log("Hit End");
            controller.enabled = false;
            transform.position += new Vector3(14, 0, -30);
            controller.enabled = true;

            if (player.puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                transform.parent.GetComponent<Player>().puzzleComplete = false;
            }
        }

        else if (other.tag == "Start Trigger")
        {
            Debug.Log("Hit Start");
            controller.enabled = false;
            transform.position -= new Vector3(14, 0, -30);
            controller.enabled = true;

            if (player.puzzleComplete)
            {
                SequenceManager.Instance.NextSequence();
                transform.parent.GetComponent<Player>().puzzleComplete = false;
            }
        }
    }
}
