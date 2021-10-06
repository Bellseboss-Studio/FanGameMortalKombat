using System;
using System.Collections;
using System.Collections.Generic;
using Rest;
using UnityEngine;
using UnityEngine.Video;


public class ControladorDeCinematica : MonoBehaviour
{
    public delegate void Respuesta(string url);
    [SerializeField] private VideoPlayer video;
    [SerializeField] private bool isPreparedVideo;
    [SerializeField] private GameObject canvas;

    public void LoadVideo(string urlVideo)
    {
        video.source = VideoSource.Url;
        video.url = urlVideo;
        video.prepareCompleted += source =>
        {
            isPreparedVideo = true;
            video.Pause();
        };
    }

    public void StartVideo()
    {
        video.Play();
        canvas.SetActive(true);
    }

    public bool IsPrepared => isPreparedVideo;
    
    public void StopVideo()
    {
        video.Stop();
        video.source = VideoSource.VideoClip;
        video.url = "";
        canvas.SetActive(false);
    }
}