using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class ScalableWall : MonoBehaviour,IFocusTarget
{
    [SerializeField] private GetDataWentCollisionWithPlayer targetFocus;
    [SerializeField] private float forceToGravitate;
    private CharacterV2 _characterV2;
    void Start()
    {
        targetFocus.Configure(this);
        targetFocus.EnableCollider();
        targetFocus.CollisionEnter += TargetFocusOnCollisionEnter;
        targetFocus.CollisionExit += TargetFocusOnCollisionExit;
    }

    private void TargetFocusOnCollisionExit(GameObject obj, Vector3 point)
    {
        //Debug.Log($"ScalableWall: TargetFocusOnCollisionExit: obj: {obj.name}");
        _characterV2.LeaveGround(false, 0, gameObject);
        _characterV2 = null;
    }

    private void TargetFocusOnCollisionEnter(GameObject obj, Vector3 point)
    {
        //Debug.Log($"ScalableWall: TargetFocusOnCollisionEnter: obj: {obj.name}");
        _characterV2 = obj.GetComponent<CharacterV2>();
        //levitate the character
        _characterV2.LeaveGround(true, forceToGravitate, gameObject);
        //pintar una linea roja desde el punto de colision hacia arriba
        Debug.DrawRay(point, Vector3.up * 10, Color.red, 10);
    }
    
}
