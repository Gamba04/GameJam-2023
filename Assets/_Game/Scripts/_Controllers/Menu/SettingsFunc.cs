using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsFunc : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public void SetMaster(float value)
    {
        audioMixer.SetFloat("Master", value);
    }

    public void SetSFX(float value)
    {
        audioMixer.SetFloat("SFX", value);
    }

    public void SetMusic(float value)
    {
        audioMixer.SetFloat("Music", value);
    }
}
