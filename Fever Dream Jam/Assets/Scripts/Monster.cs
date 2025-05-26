using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float speed;
    [SerializeField] Material visible;
    [SerializeField] Material invisible;

    [HideInInspector] public bool canMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    private void Update()
    {
        if(canMove)
        {
            Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(playerPos, Vector3.up);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if(player.dreaming)
        {
            GetComponent<Renderer>().material = visible;
        }
        else
        {
            GetComponent<Renderer>().material = invisible;
        }
    }

    private void OnBecameVisible()
    {
        canMove = false;
    }
    private void OnBecameInvisible()
    {
        canMove = true;
    }
}
