using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.UI
{
    public class ChangeInputMap : MonoBehaviour
    {
        [SerializeField] private GameObject newInputSelected;

        private void Start()
        {
            GetComponent<Button>()?.onClick.AddListener(ChangeInputMapToNew);
        }

        public void ChangeInputMapToNew()
        {
            EventSystem.current.SetSelectedGameObject(newInputSelected);
        }
        
        public void ChangeInputMapToNew(GameObject newInput)
        {
            newInputSelected = newInput;
            ChangeInputMapToNew();
        }
    }
}