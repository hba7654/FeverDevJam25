using UnityEngine;

public class Sequence : MonoBehaviour
{
    [SerializeField] GameObject monsterGO;
    [SerializeField] Transform monsterSpawnPoint;
    [SerializeField] float monsterSpawnTimer;

    [SerializeField] Sequence nextSequence;

    GameObject monsterInstance;
    float spawnTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = 0;
        monsterInstance = null;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if(monsterInstance != null && spawnTime >= monsterSpawnTimer)
        {
            monsterInstance = Instantiate(monsterGO, monsterSpawnPoint.position, Quaternion.identity);
        }

        //if puzzle completed
            // GoNextSequence();
    }

    void GoNextSequence()
    {
        nextSequence.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
