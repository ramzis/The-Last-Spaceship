using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextboxManager : MonoBehaviour
{
    public static TextboxManager Instance;

    public Queue<string> TextQueue;
    public TMP_Text TextPrinter;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TextQueue = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartText(string[] newText)
    {
        TextQueue.Clear();
        foreach (string text in newText)
        {
            TextQueue.Enqueue(text);
        }

        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        while (TextQueue.Count > 0)
        {
            string str = TextQueue.Dequeue();
            TextPrinter.text = str;

            for(int i = 1; i <= TextPrinter.text.Length; i++)
            {
                TextPrinter.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            yield return new WaitForSecondsRealtime(2.5f);
        }

        TextPrinter.text = "";
    }

    public void TestText()
    {
        StartText(new string[]{
                    "Hello There!",
                    "This is a test text on a test textbox.",
                    "Try saying that out loud three times!" } );
    }
}
