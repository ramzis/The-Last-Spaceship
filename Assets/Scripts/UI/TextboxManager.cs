using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextboxManager : MonoBehaviour
{
    public Queue<string> TextQueue;
    public TMP_Text TextPrinter;

    void Awake()
    {
        TextQueue = new Queue<string>();
        StartCoroutine(TypeText());
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
        while (true)//(TextQueue.Count > 0)
        {
            if (TextQueue.Count < 1)
            {
                TextPrinter.text = "";
                yield return new WaitForSecondsRealtime(1f);
                continue;
            }

            string str = TextQueue.Dequeue();
            TextPrinter.text = str;

            for(int i = 1; i <= TextPrinter.text.Length; i++)
            {
                TextPrinter.maxVisibleCharacters = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            yield return new WaitForSecondsRealtime(2.5f);
        }
    }

    public void TestText()
    {
        StartText(new string[]{
                    "Hello There!",
                    "This is a test text on a test textbox.",
                    "Try saying that out loud three times!" } );
    }
}
