using UnityEngine;

namespace View
{
    public class LookAtCameraForever : MonoBehaviour
    {
        private GameObject camera;
        private void Update()
        {
            //var rotation = camera.transform;
            transform.LookAt(camera.transform);
        }

        public void SetCameraRoot(GameObject cameraRoot)
        {
            camera = cameraRoot;
        }
    }
}
