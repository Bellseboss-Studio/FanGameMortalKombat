using UnityEngine;

public class OptionsFacade : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string idShow = "show";

    public void ShowOptions()
    {
        animator.SetBool(idShow,true);
    }

    public void HideOptions()
    {
        animator.SetBool(idShow,false);   
    }
}