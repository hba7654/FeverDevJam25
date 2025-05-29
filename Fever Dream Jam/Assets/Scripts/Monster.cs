using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Monster : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Material visible;
    [SerializeField] Material invisible;

    [Header("Vignette Settings")]
    [SerializeField] VolumeProfile volumeProfile;
    [SerializeField] float maxDistanceFromPlayer;
    [SerializeField] float minDistanceFromPlayer;

    private bool canMove;
    private bool forceFreeze;
    private float distanceToPlayer;
    private Vignette vignette;

    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("found vignette? " + volumeProfile.TryGet<Vignette>(out vignette));
    }

    public void ForceFreeze(bool val)
    {
        forceFreeze = val;
    }

    // Update is called once per frame
    private void Update()
    {
        if(canMove && !forceFreeze)
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

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position)/(maxDistanceFromPlayer - minDistanceFromPlayer);

        vignette.intensity.value = Mathf.Clamp(1 - distanceToPlayer, 0f, 1f);
        //vignette.intensity.
        //print("mons is " + distanceToPlayer + " away, vignette strength is " + vignette.intensity);
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
