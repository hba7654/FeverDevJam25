using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    public static UIController Instance { get { return _instance; } }

    public bool paused = false;
    public bool journal = false;
    public bool inUI = false;
    private PlayerInput controller;

    private int currentSequenceNum;
    private int pageNum;
    [SerializeField] GameObject journalPanel;
    [SerializeField] GameObject pausePanel;

    [SerializeField] private Image journalImg;
    [HideInInspector] public List<Sprite> journals;

    [SerializeField] Player player;

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
        journals = new List<Sprite>();
    }

    public void OnJournal(InputValue inputValue)
    {
        if (!paused) {
            journal = !journal;
            journalPanel.SetActive(journal);

            Freeze(journal);

            currentSequenceNum = SequenceManager.Instance.GetSequenceNumber();
            pageNum = currentSequenceNum;
            journalImg.sprite = journals[pageNum];
        }
    }

    public void Pause()
    {
        journal = false;
        paused = !paused;

        Freeze(paused || inUI);
        //Cursor.visible = paused;
        //if (paused) { Cursor.lockState = CursorLockMode.None;}
        //else { Cursor.lockState = CursorLockMode.Locked; }

        pausePanel.SetActive(paused);
    }

    public void OnPause(InputValue inputValue)
    {
        Pause();
    }

    public void Freeze(bool val)
    {
        player.canRotate = !val;
        Sequence.monsterInstance?.ForceFreeze(val);
    }
    public void OpenedUI(bool val)
    {
        inUI = val;
    }

    public void SwitchJournalPage(bool val) 
    {
        if(val)
        {
            pageNum++;
            if (pageNum > currentSequenceNum) pageNum = 0;
        }
        else
        {
            pageNum--;
            if (pageNum < 0) pageNum = currentSequenceNum;
        }

        journalImg.sprite = journals[pageNum];
    }

}
