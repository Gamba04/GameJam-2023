using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicController : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer MyVideoPlayer;
    
    public void PlayVideo(VideoClip cinematic)
    {
        MyVideoPlayer.clip = cinematic;
        GplayUI.SetFade(true, FadeColor.Cinematic, onTransitionEnd: MyVideoPlayer.Play);
        Timer.CallOnDelay(() => GplayUI.SetFade(false, FadeColor.Cinematic), (float)MyVideoPlayer.clip.length);
    }
}
