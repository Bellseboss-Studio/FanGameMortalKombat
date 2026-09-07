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
    /// PlayMode tests for the death pipeline (spec player-character P6 S2,
    /// design D4): damage to life &lt;= 0 must fire <c>OnDead</c> exactly once,
    /// ignore later damage, and drive the death animation through
    /// HealComponent -&gt; StartDeadAction -&gt; PlayDeath.
    /// </summary>
    public class PlayerV2DeathTests
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
            // T3: load via scene manager in play mode, NOT Build Settings.
            EditorSceneManager.LoadSceneInPlayMode(scenePath, new LoadSceneParameters(LoadSceneMode.Single));
            yield return null;
            yield return null;
        }

        private CharacterV2 GetScenePlayer()
        {
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain PlayerV2Update.");
            return player.GetComponent<CharacterV2>();
        }

        /// <summary>
        /// Builds a well-formed StunInfo for the damage tests. Zeroed StunInfo
        /// values make StunSystem's TeaTime math divide by zero (NaN positions);
        /// positive times with zero forces keep the player stationary while the
        /// stun loop runs, so the death assertions stay deterministic (V4).
        /// </summary>
        private static StunInfo CreateSafeStun()
        {
            var stun = ScriptableObject.CreateInstance<StunInfo>();
            stun.timeToAttack = 0.1f;
            stun.timeToDecreasing = 0.1f;
            stun.timeToSustain = 0.1f;
            stun.timeToRelease = 0.1f;
            stun.forceToAttack = 0f;
            stun.forceToDecreasing = 0f;
            stun.maxDistance = 0f;
            return stun;
        }

        [UnityTest]
        public IEnumerator ReceiveDamage_FatalDamage_OnDeadFiresOnce_AndLaterDamageIgnored()
        {
            var cv = GetScenePlayer();
            var stun = CreateSafeStun();
            var deadCount = 0;
            cv.OnDead += _ => deadCount++;

            // The scene player's statistics (StatisticsOfCharacter_Player) have life 3.
            cv.ReceiveDamage(3, cv.gameObject, stun);

            Assert.AreEqual(1, deadCount, "OnDead must fire exactly once at life <= 0 (P6).");
            Assert.LessOrEqual(cv.GetLife(), 0f, "Life must be <= 0 after fatal damage.");

            cv.ReceiveDamage(1, cv.gameObject, stun);

            Assert.AreEqual(1, deadCount, "Later damage must be ignored after death (P6).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator ReceiveDamage_FatalDamage_RaisesDamageEvent_AndPlaysDeathAnimation()
        {
            var cv = GetScenePlayer();
            var stun = CreateSafeStun();
            var lastDamageEvent = float.MinValue;
            cv.OnEnterDamageEvent += value => lastDamageEvent = value;

            cv.ReceiveDamage(3, cv.gameObject, stun);

            Assert.LessOrEqual(lastDamageEvent, 0f,
                "OnEnterDamageEvent must carry life <= 0 after fatal damage (P6/S2).");

            // HealComponent (sole OnDead bridge) -> StartDeadAction -> PlayDeath.
            // The player model animator (V3.controller) exposes a "Dead" state.
            // Read the live Animator through the public Model3DInstance and accept
            // either the current or the in-transition (next) state, since
            // CrossFade reports the destination via GetNextAnimatorStateInfo.
            var modelAnimator = cv.Model3DInstance.GetComponentInChildren<Animator>();
            Assert.IsNotNull(modelAnimator, "The instantiated player model must have an Animator.");

            var reachedDead = false;
            var observedStates = "";
            for (var i = 0; i < 60 && !reachedDead; i++)
            {
                yield return null;
                var current = modelAnimator.GetCurrentAnimatorStateInfo(0);
                var next = modelAnimator.GetNextAnimatorStateInfo(0);
                observedStates = $"{current.IsName("Dead")}/{next.IsName("Dead")}";
                reachedDead = current.IsName("Dead") || next.IsName("Dead");
            }

            Assert.IsTrue(reachedDead,
                $"Death animation must play via HealComponent -> StartDeadAction -> PlayDeath (D4). " +
                $"Last observed current/next Dead state: {observedStates}.");
        }
    }
}