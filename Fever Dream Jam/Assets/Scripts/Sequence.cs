using UnityEngine;

public class Sequence : MonoBehaviour
{
    [SerializeField] GameObject monsterGO;
    [SerializeField] Transform monsterSpawnPoint;
    [SerializeField] float monsterSpawnTimer;

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
    }
}
