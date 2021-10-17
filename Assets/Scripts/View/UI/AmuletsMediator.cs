using System;
using MenuUI;
using UnityEngine;

namespace View.UI
{
    public class AmuletsMediator : MonoBehaviour
    {
        [SerializeField] private Amulets playButton;
        [SerializeField] private Amulets optionsButton;
        [SerializeField] private Amulets extrasButton;
        [SerializeField] private Amulets creditsButton;
        [SerializeField] private Amulets quitButton;

        private void Awake()
        {
            playButton.Configure(this);
            optionsButton.Configure(this);
            extrasButton.Configure(this);
            creditsButton.Configure(this);
            quitButton.Configure(this);
        }

        public void AnyScriptCanMove()
        {
            playButton.amuletIsMoving = false;
            optionsButton.amuletIsMoving = false;
            extrasButton.amuletIsMoving = false;
            creditsButton.amuletIsMoving = false;
            quitButton.amuletIsMoving = false;
        }
    }
}