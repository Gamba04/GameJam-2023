using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private SpeakersSetup speakersSetup;
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
                StartCoroutine(DrawDialogue(dialogues[idx])); ;
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
        SFXPlayer.PlayRandomSFX(speakersSetup.GetSpeaker(msgInfo.speaker).voices);
        
        drawing = true;
        speaker.text = speakersSetup.GetSpeaker(msgInfo.speaker).name;
        speaker.color = speakersSetup.GetSpeaker(msgInfo.speaker).color; 

        foreach (char c in msgInfo.message)
        {
            concat += c;
            subtitle.text = concat;

            yield return new WaitForSeconds(0.1f);
        }

        concat = "";
        drawing = false;
    }
}