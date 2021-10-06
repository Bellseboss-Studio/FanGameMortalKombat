using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Rest
{
    public class RestGet
    {
        
        public delegate void RestGetOk<T>(T result);
        public delegate void RestGetImageOk<T>(T result);
        public delegate void RestGetBad();
        
        public static IEnumerator GetRequest<T>(string uri, RestGetOk<T> ok, RestGetBad bad)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    //All bad
                    Debug.Log(webRequest.error);
                    bad();
                }
                else
                {
                    //All good
                    //Convert this function in Template function or Template class
                    Debug.Log(webRequest.downloadHandler.text);
                    var convert = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    ok(convert);
                }
            }
        }
        
        public static IEnumerator GetImageRequest(string uri, RestGetImageOk<Sprite> ok, RestGetBad bad)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    //All bad
                    Debug.Log(webRequest.error);
                    bad();
                }
                else
                {
                    Texture2D myTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                    var sprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    ok(sprite);
                }
            }
        }
    }
}