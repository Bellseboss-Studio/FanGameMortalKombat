using MenuUI.SystemOfExtras;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class ContainerOfExtra : MonoBehaviour
{
    private IExtra _extra;
    [SerializeField] private Image imageToShow;
    [SerializeField] private Button buttonToAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Image imageOfResource;
    [SerializeField] private TextMeshProUGUI text;
    
    private async void Call()
    {
        animator.SetBool("show", true);
        imageOfResource.enabled = false;
        text.enabled = false;
        switch (_extra.GetTypeExtra())
        {
            case "text":
                text.text = _extra.GetSource();
                text.enabled = true;
                break;
            case "image":
                var spriteToPixel = Resources.Load<Sprite>(_extra.GetSource());
                if (_extra.GetSource().Contains("://"))
                {
                    var textureFromUrl = await GetRemoteTexture(_extra.GetSource());
                    Rect rec = new Rect(0, 0, textureFromUrl.width, textureFromUrl.height);
                    spriteToPixel = Sprite.Create(textureFromUrl,rec,new Vector2(0,0),.01f);
                }
                imageOfResource.sprite = spriteToPixel;
                imageOfResource.preserveAspect = true;
                imageOfResource.enabled = true;
                break;
            case "video":
                break;
            case "audio":
                break;
        }
    }

    public void Add(IExtra extra)
    {
        _extra = extra;
    }

    public void Configure()
    {
        if (_extra == null)
        {
            var spriteToPixel = Resources.Load<Sprite>("SinDatos");
            imageToShow.sprite = spriteToPixel;
        }
        else
        {
            var spriteToPixel = Resources.Load<Sprite>(_extra.GetIcon());
            imageToShow.sprite = spriteToPixel;
        }

        ConfigureActionsTuButton();
    }

    private void ConfigureActionsTuButton()
    {
        buttonToAction.onClick.AddListener(Call);
    }

    public void Clean()
    {
        _extra = null;
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
}