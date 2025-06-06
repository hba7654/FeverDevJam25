using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sequence_25 : Sequence
{
    [SerializeField] private Material glowMat;
    [SerializeField] private List<Texture> runePics;

    [SerializeField] private List<GameObject> inguzRunes;
    private List<GameObject> inguzRunesLeft;
    private GameObject selectedRune;

    private int index = 0;
    private int[] currentCombo = { 0, 0, 0, 0 };
    private int[] correctCombo = { 2, 3, 5, 7 };

    protected override void Start()
    {
        base.Start();
        inguzRunesLeft = new List<GameObject>(inguzRunes);
    }
    public void SetIndex(int val)
    {
        index = val;
    }

    private void Sequence25Door()
    {
        // Check to see if the current combo is correct
        // Any combo of the 4 correct runes is right and will open the door
        foreach (int rune in correctCombo)
        {
            if (currentCombo.Contains(rune))
            {
                // print("ok looking good");

            }
            else
            {
                //print("NOPE the combo is wrong");
                return;
            }

        }

        // print("Yeah thats the right combo right there alright alright alright");
        OnStep1Completed?.Invoke();
    }

    public void PadlockUp(RawImage img)
    {
        //if (currentCombo + 1 == 8)
        //{
        //    currentCombo = 0;
        //}
        //else
        //{
        //    currentCombo++;
        //}

        currentCombo[index]++;
        currentCombo[index] %= 8;

        //print(currentCombo[index]);
        img.texture = runePics[currentCombo[index]];

        Sequence25Door();
    }

    public void PadlockDown(RawImage img)
    {
        if (currentCombo[index] - 1 == -1)
        {
            currentCombo[index] = 7;
        }
        else
        {
            currentCombo[index]--;
        }
        // print(currentCombo[index]);

        img.texture = runePics[currentCombo[index]];

        Sequence25Door();
    }

    // Manages the puzzle for sequence 25 inside the lab
    // Sets of 3 runes will spawn in and the user has to click on the 3 runes to move onto the next round
    // Monster will spawn in when they start the game 
    // NOTE: maybe add a time limit later???? 
    public void Sequence25Puzzle()
    {
        // Check if any runes are still in this room
        if (inguzRunes.Count > 0)
        {
            foreach (GameObject rune in inguzRunes)
            {
                // If there is a rune that is still active in the scene then do nothing (return) 
                if (rune.activeInHierarchy == true)
                {
                    print("Theres a rune left somewhere");
                    return;
                }
            }

            print("no more runes now lets spawn more");

            // Spawn the second round in the game
            // The second round MUST be these three runes and nothing else
            if (inguzRunes.Count > 10)
            {
                inguzRunes[0].SetActive(true);
                inguzRunes[1].SetActive(true);
                inguzRunes[2].SetActive(true);
            }
            else
            {
                // Make sure the code knows which runes are left 
                inguzRunesLeft = new List<GameObject>(inguzRunes);

                // Repopulates the next round of runes if this is not the second round
                // The runes that are repopulated are random runes in the list
                for (int i = 0; i < 3; i++)
                {
                    selectedRune = inguzRunesLeft[Random.Range(0, (inguzRunesLeft.Count - 1))];

                    selectedRune.SetActive(true);
                    inguzRunesLeft.Remove(selectedRune);
                }
            }
        }

        // Check to see if the player got all the runes
        else 
        {
            print("complete SIRRRRRRRRRR");
            OnPuzzleCompleted?.Invoke();
        }

    }

    public void RuneCaptured(GameObject rune)
    {
        StartCoroutine(HandleRuneGlow(rune));
    }

    private IEnumerator HandleRuneGlow(GameObject rune)
    {
        var mat = rune.GetComponent<Renderer>();

        mat.material = glowMat;
        yield return new WaitForSeconds(2);
        inguzRunes.Remove(rune);
        rune.SetActive(false);

        Sequence25Puzzle();
    }

}
