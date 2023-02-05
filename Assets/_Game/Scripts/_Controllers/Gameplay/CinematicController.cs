using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicController : MonoBehaviour
{
    private VideoPlayer MyVideoPlayer;

    public void PlayVideo(VideoClip cinematic)
    {
        MyVideoPlayer.clip = cinematic;
        MyVideoPlayer.Play();
    }
}
