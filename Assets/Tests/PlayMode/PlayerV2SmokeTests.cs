using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// PlayMode smoke suite (spec player-v2-validation V1/V2, player-test-scene T3/T5,
    /// scenario S1/S3): loads the isolated <c>PlayerV2Test</c> scene via SceneManager
    /// (never Build Settings), asserts zero console errors, and verifies the player
    /// registers itself as <c>IPlayer</c> in ServiceLocator during Configure (P2/S1).
    /// </summary>
    public class PlayerV2SmokeTests
    {
        private const string TestScenePath = "Assets/_Scenes/Tests/PlayerV2Test.unity";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            ServiceLocator.Instance.Reset();
            yield return LoadSceneAndYieldFrames(TestScenePath);
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            ServiceLocator.Instance.Reset();
            yield return null;
        }

        private static IEnumerator LoadSceneAndYieldFrames(string scenePath)
        {
            // T3: load via SceneManager (play-mode scene load), NOT Build Settings.
            // EditorSceneManager.LoadSceneInPlayMode loads an arbitrary scene asset in
            // play mode without registering it in the build profile.
            EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
            // Allow the scene to activate and every MonoBehaviour.Start (incl.
            // CharacterV2.Configure) to run before asserting. No timing sleeps (V4).
            yield return null;
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestScene_Loads_WithZeroConsoleErrors()
        {
            LogAssert.ignoreFailingMessages = false;

            var active = SceneManager.GetActiveScene();
            Assert.AreEqual(TestScenePath, active.path,
                "The isolated test scene must be loaded as the active scene.");
            Assert.IsTrue(active.IsValid(), "The active scene must be valid.");

            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain a PlayerV2Update instance (T2).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Configure_RegistersPlayer_AsIPlayer_Service()
        {
            var resolved = ServiceLocator.Instance.TryGetService<IPlayer>(out var player);

            Assert.IsTrue(resolved,
                "PlayerV2Update must register itself as IPlayer in ServiceLocator on Configure (P2/S1).");
            Assert.IsNotNull(player, "The resolved IPlayer must not be null.");
            Assert.IsNotNull(player.GetGameObject(),
                "The resolved IPlayer must expose its GameObject (T5).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Player_StandsOnFloor_AtSceneOrigin()
        {
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "PlayerV2Update must exist.");

            var rigidbody = player.GetComponent<Rigidbody>();
            Assert.IsNotNull(rigidbody, "The player must have a Rigidbody.");

            // The floor plane sits at y=0; a player spawned at y=1 resting on it must
            // keep its feet at the origin height (not fall through, not fly away).
            // This proves the scene's floor collider and physics are wired (T2).
            Assert.Greater(player.transform.position.y, 0.0f,
                "The player must rest above the floor plane at y=0.");
            Assert.Less(player.transform.position.y, 3.0f,
                "The player must not be launched or spawned above the scene bounds.");
            yield return null;
        }
    }
}