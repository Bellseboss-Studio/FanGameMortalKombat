using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MenuUI.SystemOfExtras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class ExtraMediator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    [SerializeField] private Image imageOfResource;
    [SerializeField] private ControladorDeCinematica cinematic;
    public void SetExtraText(IExtra extra)
    {
        PreConditions();
        text.text = extra.GetSource();
        text.transform.parent.gameObject.SetActive(true);
        text.enabled = true;
        PostConditions(text.gameObject);
    }

    private void PostConditions(GameObject ofResource)
    {
    }

    private void PreConditions()
    {
        animator.gameObject.SetActive(true);
        animator.SetBool("show", true);
        imageOfResource.enabled = false;
        text.transform.parent.gameObject.SetActive(false);
        cinematic.StopVideo();
        cinematic.gameObject.SetActive(false);
    }

    public async void SetExtraImage(IExtra extra)
    {
        PreConditions();
        var spriteToPixel = Resources.Load<Sprite>(extra.GetSource());
        if (extra.GetSource().Contains("://"))
        {
            var textureFromUrl = await GetRemoteTexture(extra.GetSource());
            Rect rec = new Rect(0, 0, textureFromUrl.width, textureFromUrl.height);
            spriteToPixel = Sprite.Create(textureFromUrl,rec,new Vector2(0,0),.01f);
        }
        imageOfResource.sprite = spriteToPixel;
        imageOfResource.preserveAspect = true;
        imageOfResource.enabled = true;
        PostConditions(imageOfResource.gameObject);
    }


    public static async Task<Texture2D> GetRemoteTexture ( string url )
    {
        using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while( asyncOp.isDone==false )
                await Task.Delay( 1000/30 );//30 hertz
        
            // read results:
            if( www.isNetworkError || www.isHttpError )
                // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                // log error:
#if DEBUG
                Debug.Log( $"{www.error}, URL:{www.url}" );
#endif
            
                // nothing to return on error:
                return null;
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }

    public async void SetExtraVideo(IExtra extra)
    {
        PreConditions();
        cinematic.gameObject.SetActive(true);
        cinematic.LoadVideo(extra.GetSource());
        while (!cinematic.IsPrepared)
        {
            await Task.Delay(TimeSpan.FromSeconds(.2f));
        }
        cinematic.StartVideo();
        PostConditions(cinematic.gameObject);
    }
}
