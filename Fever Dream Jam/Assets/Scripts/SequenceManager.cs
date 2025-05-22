using System;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    private static SequenceManager _instance;

    public static SequenceManager Instance { get { return _instance; } }
    //public static int SequenceNum{ get { return _sequenceNumber; } set { _sequenceNumber = value; } }


    private int sequenceNumber;
    private Sequence curSequence;

    [SerializeField] private int startingSequenceNum;
    [SerializeField] private Sequence[] sequences;




    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        sequenceNumber = startingSequenceNum;
        curSequence = sequences[sequenceNumber];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetSequenceNumber()
    {
        return sequenceNumber;
    }

    public void NextSequence()
    {
        curSequence.GoNextSequence();

        curSequence = sequences[++sequenceNumber];
    }

    public void ShowDreamObjects(bool show)
    {
        curSequence.transform.GetChild(0).gameObject.SetActive(show);
    }
}
