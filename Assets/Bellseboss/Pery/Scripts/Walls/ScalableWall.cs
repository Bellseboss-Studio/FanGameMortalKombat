using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class ScalableWall : MonoBehaviour,IFocusTarget
{
    [SerializeField] private TargetFocus targetFocus;
    [SerializeField] private float forceToGravitate;
    private CharacterV2 _characterV2;
    void Start()
    {
        targetFocus.Configure(this);
        targetFocus.EnableCollider();
        targetFocus.CollisionEnter += TargetFocusOnCollisionEnter;
        targetFocus.CollisionExit += TargetFocusOnCollisionExit;
    }

    private void TargetFocusOnCollisionExit(GameObject obj)
    {
        //Debug.Log($"ScalableWall: TargetFocusOnCollisionExit: obj: {obj.name}");
        _characterV2.LeaveGround(false, 0);
        _characterV2 = null;
    }

    private void TargetFocusOnCollisionEnter(GameObject obj)
    {
        //Debug.Log($"ScalableWall: TargetFocusOnCollisionEnter: obj: {obj.name}");
        _characterV2 = obj.GetComponent<CharacterV2>();
        //levitate the character
        _characterV2.LeaveGround(true, forceToGravitate);
    }
    
}
