using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Sequence_25 : Sequence
{
    //[SerializeField] private GameObject padLock;
    [SerializeField] private List<Texture> runePics;

    private int index = 0;
    private int[] currentCombo = { 0, 0, 0, 0 };
    private int[] correctCombo = { 2, 3, 5, 7 };

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
                print("ok looking good");

            }
            else
            {
                print("NOPE the combo is wrong");
                return;
            }

        }

        print("Yeah thats the right combo right there alright alright alright");
        OnStep1Completed.Invoke();
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


}
