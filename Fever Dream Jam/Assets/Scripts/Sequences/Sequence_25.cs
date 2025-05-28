using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sequence_25 : Sequence
{
    //[SerializeField] private GameObject padLock;
    [SerializeField] private List<Texture> runePics;

    private int index = 0;
    private int[] currentCombo = { 0, 0, 0, 0 };

    public void SetIndex(int val)
    {
        index = val;
    }

    private void Sequence25Door()
    {

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

        Debug.Log(currentCombo[index]);
        img.texture = runePics[currentCombo[index]];
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
        print(currentCombo[index]);

        img.texture = runePics[currentCombo[index]];
    }
}
