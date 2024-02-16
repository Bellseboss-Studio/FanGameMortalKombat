using System;
using Bellseboss.Pery.Scripts.Input;
using Cinemachine;
using UnityEngine;

public class CameraBehaviourWithReel : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath path;
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private CameraCollider[] cameraColliders;
    [SerializeField] private CameraCollider roomCollider;
    private ICameraBehaviour _cameraBehaviour;
    private int _index;
    public Action<int, bool, CinemachineVirtualCamera> ChangeCamera { get; set; }
    public CinemachineVirtualCameraBase Camera => camera;

    private void Start()
    {
        Debug.Log($"CameraBehaviourWithReel:Start - {path.m_Waypoints.Length}");
        for (var i = 0; i < cameraColliders.Length; i++)
        {
            var cameraCollider = cameraColliders[i];
            var index = i;
            cameraCollider.ColliderEnter += (o) => OnColliderEnter(index);
        }
        roomCollider.ColliderEnter += EntreToRoom;
        roomCollider.ColliderExit += ExitToRoom;
    }
    
    private void EntreToRoom(GameObject o)
    {
        ChangeCamera?.Invoke(_index, true, camera);
    }

    private void ExitToRoom(GameObject o)
    {
        ChangeCamera?.Invoke(_index, false, camera);
    }

    private void OnColliderEnter(int i)
    {
        //Debug.Log($"CameraBehaviourWithReel:OnColliderEnter: {i}");
        CinemachineTrackedDolly trackedDolly = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        if (trackedDolly != null)
        {
            trackedDolly.m_PathPosition = i;
        }else
        {
            //Debug.LogError("No CinemachineTrackedDolly component found in camera body.");
        }
    }

    public void Config(ICameraBehaviour cameraBehaviour, int index)
    {
        _index = index;
        _cameraBehaviour = cameraBehaviour;
    }
}
