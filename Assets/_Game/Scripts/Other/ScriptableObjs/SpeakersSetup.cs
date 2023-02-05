using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueSpeaker
{
    SageTree,
    Mushroom1,
    Mushroom2,
    Mushroom3,
    Player
}

[CreateAssetMenu(fileName = "SpeakersSetup", menuName = "Setup/Speakers Setup", order = 3)]
public class SpeakersSetup : ScriptableObject
{

    #region Serializable

    [Serializable]
    public class SpeakerInfo
    {
        [SerializeField, HideInInspector] private string displayName;

        public string name;
        public Color color = Color.white;

        [Space]
        public List<SFXTag> voices;

        public void SetName(DialogueSpeaker speaker)
        {
            displayName = speaker.ToString();
        }
    }

    #endregion

    [SerializeField]
    private List<SpeakerInfo> speakers;

    public SpeakerInfo GetSpeaker(DialogueSpeaker speaker) => speakers[(int)speaker];

    #region Editor

#if UNITY_EDITOR

    private void OnValidate()
    {
        speakers.Resize<SpeakerInfo, DialogueSpeaker>();
        speakers.ForEach((speaker, index) => speaker.SetName((DialogueSpeaker)index));
    }

#endif

    #endregion

}