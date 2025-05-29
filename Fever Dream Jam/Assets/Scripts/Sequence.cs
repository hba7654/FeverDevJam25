using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sequence : MonoBehaviour
{
    [SerializeField] protected Player player;

    [SerializeField] GameObject monsterGO;
    [SerializeField] Transform monsterSpawnPoint;

    [SerializeField] Sequence nextSequence;

    [SerializeField] private GameObject sequenceDreamItems;


    [SerializeField] protected UnityEvent OnPuzzleCompleted;
    [SerializeField] protected UnityEvent OnStep1Completed;

    public static Monster monsterInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        monsterInstance = null;
    }

    // Update is called once per frame
    void Update()
    {
        //if puzzle completed
        // GoNextSequence();
    }

    public void GoNextSequence()
    {
        nextSequence.gameObject.SetActive(true);
        gameObject.SetActive(false);
        if (monsterInstance)
        {
            Destroy(monsterInstance);
            monsterInstance = null;
        }
    }

    public void SpawnMonster()
    {
        //if (monsterInstance != null)
      //  {
            monsterInstance = Instantiate(monsterGO, monsterSpawnPoint.position, Quaternion.identity).GetComponent<Monster>();
            monsterInstance.player = player;
      //  }

        print(monsterInstance);
    }

    public void ShowDO(bool val)
    {
        sequenceDreamItems.SetActive(val);
    }
}
