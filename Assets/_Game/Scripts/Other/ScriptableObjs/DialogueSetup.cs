using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MessageInfo
{
    [SerializeField, HideInInspector] private string name;

    public DialogueSpeaker speaker;
    public string message;


    public void SetName()
    {
        int previewLength = Mathf.Min(message.Length, 5);

        string messagePreview = message.Substring(0, previewLength);

        name = $"{speaker}: {messagePreview}...";
    }
}

[CreateAssetMenu(fileName = "DialogueSetup", menuName = "Setup/Dialogue Setup", order = 2)]
public class DialogueSetup : ScriptableObject
{
    [SerializeField]
    private List<MessageInfo> messages;

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        messages.ForEach(message => message.SetName());
    }

#endif

    #endregion

    public List<MessageInfo> Messages => messages;
}