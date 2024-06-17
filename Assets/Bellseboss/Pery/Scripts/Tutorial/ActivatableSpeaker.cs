using System.Collections;
using UnityEngine;


namespace MortalKombat.Audio
{
    public class ActivatableSpeaker : ActivableTutorial, IFmodClient
    {
        [SerializeField] private NarratorDialogues m_Id;
        private FmodFacade m_FmodFacade;

        private void Start()
        {
            m_FmodFacade = new FmodFacade("event:/DX/", m_Id.ToString());
        }

        public override void Activate()
        {
            try
            {
                if (IsFinished) return;
                StartCoroutine(WentToFinish(m_FmodFacade.PlayToGetMilliseconds()));
            }
            catch
            {
                Debug.Log("error on Activatable Speaker");
            }
            
        }

        private IEnumerator WentToFinish(int milliseconds = 0)
        {
            yield return new WaitForSeconds(milliseconds / 1000);
            Finish();
        }
    }

    public interface IFmodClient
    {
    }
}



