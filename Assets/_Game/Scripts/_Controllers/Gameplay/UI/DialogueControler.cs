using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

[Serializable]
public class MessageInfo
{
    private string speaker;
    private string message;
    private Color colour;
    
    public String GetSpeaker() { return speaker; }
    public String GetMessage() { return message; }
    public Color GetColor() { return colour; }
}
public class DialogueControler : MonoBehaviour
{
    [SerializeField]
    private Text subtitle;
    [SerializeField]
    private Text speaker;
    private string concat = "";
    private int idx = 0;
    private List<MessageInfo> dialogues;
    private bool drawing = false;

    public event Action onFinishedDialogue;

    public void Update()
    {
        if (Input.GetButtonDown("Skip"))
        {
            StartDialogue();
        }
    }

    public void SetDialogue(List<MessageInfo> str) 
    {
        dialogues = str;
        StartDialogue();
        
    }
     private void StartDialogue()
    {
        if (!drawing)
        {
            if (idx < dialogues.Count)
            {
                StartCoroutine(DrawDialogue(dialogues[idx]));
                idx++;
            }
            else
            {
                onFinishedDialogue?.Invoke();
                speaker.text = "";
                subtitle.text = "";
            }
        }
    }
    private IEnumerator DrawDialogue(MessageInfo msgInfo)
    {   
        drawing = true;
        speaker.text = msgInfo.GetSpeaker();
        speaker.color = msgInfo.GetColor(); 

        foreach (char c in msgInfo.GetMessage())
        {
            concat += c;
            subtitle.text = concat;
            yield return new WaitForSeconds(0.1f);
        }
        concat = "";
        drawing = false;
    }
}
