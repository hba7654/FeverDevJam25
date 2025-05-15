using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool paused = false;
    public static bool journal = false;
    private PlayerInput controller;
    [SerializeField] GameObject journalPanel;
    [SerializeField] GameObject pausePanel;

    public void OnJournal(InputValue inputValue)
    {
        if (!paused) {
            journal = !journal;
            journalPanel.SetActive(journal);
        }
    }

    public void OnPause(InputValue inputValue)
    {
        journal = false;
        paused = !paused;

        Cursor.visible = paused;
        if (paused) { Cursor.lockState = CursorLockMode.None;}
        else { Cursor.lockState = CursorLockMode.Locked; }

        pausePanel.SetActive(paused);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Clicked!");
    }
}
