using System.Collections;
using NUnit.Framework;
using ServiceLocatorPath;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using View.Installers;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// PlayMode tests for the guarded pause/UI wiring (design D5, task 4.3/4.4):
    /// Configure must not throw when no pause/UI services are registered
    /// (isolated scene), and when services ARE present it must subscribe the
    /// pause menu (freeze/resume) and bind the observer UI.
    /// Orchestrator decision: no installers are added to 03_Game — wiring stays
    /// guarded and the absent-registrant path is the default for the test scene.
    /// </summary>
    public class PlayerV2PauseUiWiringTests
    {
        private const string TestScenePath = "Assets/_Scenes/Tests/PlayerV2Test.unity";

        private sealed class FakeObserverUi : IObserverUI
        {
            public bool Observed;
            public ICharacterUi BoundCharacterUi;
            public ICharacterV2 BoundCharacterV2;

            public void Observer(ICharacterUi character, ICharacterV2 characterV2)
            {
                Observed = true;
                BoundCharacterUi = character;
                BoundCharacterV2 = characterV2;
            }
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            ServiceLocator.Instance.Reset();
            yield return null;
        }

        private static IEnumerator LoadSceneAndYieldFrames(string scenePath)
        {
            // T3: load via scene manager in play mode, NOT Build Settings.
            EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
            yield return null;
            yield return null;
        }

        private static CharacterV2 GetScenePlayer()
        {
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain PlayerV2Update.");
            return player.GetComponent<CharacterV2>();
        }

        [UnityTest]
        public IEnumerator Configure_WithoutUiService_DoesNotThrow_AndRegisters()
        {
            ServiceLocator.Instance.Reset();
            yield return LoadSceneAndYieldFrames(TestScenePath);

            // D5 gate: with NO IObserverUI registrant Configure must not throw
            // (the old code called GetService<IObserverUI>() unconditionally) and
            // the player still registers itself as IPlayer. NOTE: IPauseMainMenu
            // IS present — the player prefab's own PausaMenuV2 child self-registers
            // it on Awake — so this test covers the absent-IObserverUI branch.
            Assert.IsTrue(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Configure must register the player even without an IObserverUI service (D5).");
            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IObserverUI>(out _),
                "The isolated scene must NOT have an IObserverUI registrant (D6).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator PauseService_Present_PauseFreezesControls_AndResumeRestoresThem()
        {
            ServiceLocator.Instance.Reset();
            yield return LoadSceneAndYieldFrames(TestScenePath);

            // The PlayerV2Update prefab's own PausaMenuV2 child registers
            // IPauseMainMenu on Awake, so a real registrant IS present in the
            // isolated scene. Configure (guarded wiring, D5) must have subscribed
            // OnPausaMenu to it. This validates the "when present" branch.
            var pauseService = ServiceLocator.Instance.GetService<IPauseMainMenu>();
            Assert.IsNotNull(pauseService,
                "The player prefab self-registers IPauseMainMenu via its PausaMenuV2 child.");

            var cv = GetScenePlayer();
            var rb = cv.gameObject.GetComponent<Rigidbody>();

            // Give the player a velocity so freezing is observable (P7 S3).
            rb.linearVelocity = new Vector3(5f, 0f, 0f);
            pauseService.onPause?.Invoke(true);

            Assert.AreEqual(Vector3.zero, rb.linearVelocity,
                "Pause must freeze the player (DisableControls) via guarded wiring (D5).");
            Assert.IsTrue(rb.freezeRotation,
                "Pause must freeze rotation (P7).");
            Assert.IsFalse(cv.GetCanReadInputs(),
                "Pause must stop input reading (P7 S3).");

            pauseService.onPause?.Invoke(false);

            Assert.IsTrue(cv.GetCanReadInputs(),
                "Resume must restore input reading (P7 S3).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator ObserverUi_Present_BindsToPlayer()
        {
            ServiceLocator.Instance.Reset();
            var fakeUi = new FakeObserverUi();
            ServiceLocator.Instance.RegisterService<IObserverUI>(fakeUi);
            yield return LoadSceneAndYieldFrames(TestScenePath);

            var cv = GetScenePlayer();

            // D5 gate: when an IObserverUI is registered, Configure must bind it to
            // the player. (Damage-payload propagation is covered separately by
            // PlayerV2DeathTests; calling ReceiveDamage here would start the
            // StunSystem TeaTime coroutine, which outlives scene teardown and can
            // write NaN positions into the next test's scene.)
            Assert.IsTrue(fakeUi.Observed, "Configure must bind a registered IObserverUI (D5).");
            Assert.AreSame(cv, fakeUi.BoundCharacterUi,
                "The observer must be bound to the player's ICharacterUi.");
            Assert.AreSame(cv, fakeUi.BoundCharacterV2,
                "The observer must be bound to the player's ICharacterV2.");
            yield return null;
        }
    }
}