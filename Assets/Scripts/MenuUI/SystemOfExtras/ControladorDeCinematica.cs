using System;
using System.Collections;
using System.Collections.Generic;
using MortalKombat.Audio;
using Rest;
using UnityEngine;
using UnityEngine.Video;


public class ControladorDeCinematica : MonoBehaviour
{
    public delegate void Respuesta(string url);
    [SerializeField] private VideoPlayer video;
    [SerializeField] private bool isPreparedVideo;
    [SerializeField] private GameObject canvas;
    private IFmodManager _fmodManager;

    public void LoadVideo(string urlVideo)
    {
        _fmodManager = new FmodManagerUI();
        canvas.SetActive(true);
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
        
    }

    public bool IsPrepared => isPreparedVideo;
    
    public void StopVideo()
    {
        video.Stop();
        isPreparedVideo = false;
        canvas.SetActive(false);
    }
}