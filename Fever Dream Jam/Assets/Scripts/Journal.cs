using System.Collections;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField] private Sprite[] journals;
    [SerializeField] private GameObject text;
    private Sprite currentJournal;
    private int curPage;

    private void Start()
    {
        text.SetActive(false);
        curPage = 0;
        currentJournal = journals[0];
    }

    public void AddPage()
    {
        UIController.Instance.journals.Add(currentJournal);
        curPage++;
        currentJournal = journals[curPage];

        StartCoroutine(ShowText());
    }

    public IEnumerator ShowText()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(3);
        text.SetActive(false);
    }
}
