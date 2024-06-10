using MortalKombat.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.UI
{
    public class Amulets : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool isOver = false;
        public Image amulet;
        private float velocity = 12;
        private IAmuletPositioner _amuletPositioner;
        public bool amuletIsMoving;
        private AmuletsMediator _amuletsMediator;
        private IFmodManager m_FmodManager = new FmodManagerUI();

        private void Awake()
        {
            amuletIsMoving = false;
            amulet.enabled = true;
            _amuletPositioner = new MoveTowardsAdapter();
        }

        public void Configure(AmuletsMediator amuletsMediator)
        {
            _amuletsMediator = amuletsMediator;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("Mouse enter");
            MoveAmulet();
        }

        public void MoveAmulet()
        {
            isOver = true;
            if (isOver != true) return;
            amulet.enabled = true;
            m_FmodManager.PlaySfx(UISoundList.UI_MouseHover);
            _amuletsMediator.AnyScriptCanMove();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("Mouse exit");
            DontMoveAmulet();
        }

        public void DontMoveAmulet()
        {
            isOver = false;
        }

        private void Update()
        {
            amuletIsMoving = _amuletPositioner.MoveAmulet(amulet, velocity, gameObject, isOver, amuletIsMoving);
        }
    
        public void ExitGame()
        {
            Application.Quit();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Mouse click");
        }
    }
}