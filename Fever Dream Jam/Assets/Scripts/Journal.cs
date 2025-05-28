using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField] private Sprite[] journals;
    private Sprite currentJournal;
    private int curPage;

    private void Start()
    {
        curPage = 0;
        currentJournal = journals[0];
    }

    public void AddPage()
    {
        UIController.Instance.journals.Add(currentJournal);
        curPage++;
        currentJournal = journals[curPage];
    }
}
