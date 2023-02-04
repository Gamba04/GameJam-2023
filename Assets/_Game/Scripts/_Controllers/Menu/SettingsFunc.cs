using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsFunc : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public void SetMaster(float value)
    {
        audioMixer.SetFloat("Master", GambaFunctions.VolumeToDB(value));
    }

    public void SetSFX(float value)
    {
        audioMixer.SetFloat("SFX", GambaFunctions.VolumeToDB(value));
    }

    public void SetMusic(float value)
    {
        audioMixer.SetFloat("Music", GambaFunctions.VolumeToDB(value));
    }
}
