using System.Collections.Generic;
using UnityEngine;

public class Sequence_3 : Sequence
{
    private List<string> sequence3Runes;
    private List<string> sequence3SelectedRunes;
    [HideInInspector] public bool seq3Complete;

    [SerializeField] private Material lightBlue_Material;
    [SerializeField] private Material black_Material;

    //if you wanna add new events for more steps you do so like this
    //[SerializeField] private UnityEvent OnStepXCompleted
    //
    //and then call it wherever you want it to happen like this
    //OnStepXCompleted?.Invoke();
    //
    //The '?' is making sure OnStepXCompleted is not null, the same as
    //  doing if(OnStepXCompleted != null) {OnStepXCompleted.Invoke();}

    protected override void Start()
    {
        base.Start();

        sequence3Runes = new List<string> { "Inguz", "Wunjo", "Othilla", "Algiz" };
        sequence3SelectedRunes = new List<string>();
        seq3Complete = false;

        SpawnMonster();
    }

    public void Sequence3Puzzle(string objectName)
    {
        // Debug.Log(objectName);

        if (sequence3Runes.Contains(objectName))
        {
            sequence3SelectedRunes.Add(objectName);
            GameObject.Find(objectName + "(hint)").GetComponent<MeshRenderer>().material = lightBlue_Material;
            Debug.Log("Correct!!!!!");
        }

        else
        {

            foreach (string rune in sequence3SelectedRunes)
            {
                GameObject.Find(rune + "(hint)").GetComponent<MeshRenderer>().material = black_Material;
                Debug.Log(rune + "(hint)");
            }

            sequence3SelectedRunes.Clear();
            Debug.Log("Not correct");
        }

        if (sequence3SelectedRunes.Count == 4)
        {
            Debug.Log("Puzzle complete");
            seq3Complete = true;
            player.puzzleComplete = true;

            OnPuzzleCompleted?.Invoke();
        }
    }
}
