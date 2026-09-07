using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using _Scripts.Player;

namespace MortalKombat.Tests
{
    /// <summary>
    /// PlayMode tests for CharacterV2 hardening (spec character-configuration C3,
    /// player-character P7, design D3/D4):
    /// - Configure must fail fast with a clear error and skip registration when
    ///   required configuration is invalid (null refs, life &lt;= 0, speeds &lt; 0).
    /// - DisableControls must zero velocity, freeze rotation, and stop input
    ///   reading; EnableControls must restore them (P7 S3).
    /// </summary>
    public class PlayerV2HardeningTests
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

        private static StatisticsOfCharacter CreateStats(int life, float angry, float normal, float scared)
        {
            var stats = ScriptableObject.CreateInstance<StatisticsOfCharacter>();
            stats.life = life;
            stats.speedToMoveAngry = angry;
            stats.speedToMoveNormal = normal;
            stats.speedToMoveScared = scared;
            return stats;
        }

        /// <summary>
        /// Builds a CharacterV2 whose Start() never runs (inactive GO) so the test
        /// controls Configure() directly. stats may be null (all refs missing).
        /// </summary>
        private static CharacterV2 CreateBrokenPlayer(StatisticsOfCharacter stats)
        {
            var go = new GameObject("BrokenPlayer");
            go.SetActive(false);
            var cv = go.AddComponent<CharacterV2>();
            if (stats != null)
            {
                var field = typeof(CharacterV2).GetField("statisticsOfCharacter",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                field.SetValue(cv, stats);
            }

            return cv;
        }

        /// <summary>
        /// Sets a private serialized field on the given instance via reflection.
        /// Used to null ONE required reference on the fully-wired scene player so
        /// the fail-fast suite can prove each individual D3 ref is validated.
        /// </summary>
        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            var field = typeof(CharacterV2).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, $"Field {fieldName} must exist on CharacterV2.");
            field.SetValue(instance, value);
        }

        [UnityTest]
        public IEnumerator Configure_AllRefsNull_LogsError_AndSkipsRegistration()
        {
            LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex("statisticsOfCharacter is null"));
            // The scene player already registered during SetUp; clear it so the
            // assertion below proves the BROKEN player did not register itself.
            ServiceLocator.Instance.Reset();
            var broken = CreateBrokenPlayer(null);

            broken.Configure();

            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Fail-fast Configure must skip registration (C3 S2).");
            Object.Destroy(broken.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Configure_ZeroLifeStats_LogsError_AndSkipsRegistration()
        {
            LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex("statisticsOfCharacter is invalid"));
            ServiceLocator.Instance.Reset();
            var broken = CreateBrokenPlayer(CreateStats(0, 0f, 0f, 0f));

            broken.Configure();

            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Configure must reject life <= 0 without registering (C3 S2).");
            Object.Destroy(broken.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Configure_NegativeSpeedStats_LogsError_AndSkipsRegistration()
        {
            LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex("statisticsOfCharacter is invalid"));
            ServiceLocator.Instance.Reset();
            var broken = CreateBrokenPlayer(CreateStats(3, 0f, -1f, 0f));

            broken.Configure();

            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Configure must reject negative speeds without registering (C3 S2).");
            Object.Destroy(broken.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Configure_NullRequiredRef_LogsError_AndSkipsRegistration()
        {
            LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex("missing required reference: inputPlayerV2"));
            ServiceLocator.Instance.Reset();
            var broken = CreateBrokenPlayer(CreateStats(3, 0f, 0f, 0f));

            broken.Configure();

            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Configure must reject a missing required reference without registering (C3 S2).");
            Object.Destroy(broken.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Configure_NullFatalitySystem_LogsError_AndSkipsRegistration()
        {
            // Design D3: fatalitySystem belongs in the fail-fast non-null list.
            LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex("missing required reference: fatalitySystem"));
            ServiceLocator.Instance.Reset();
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain PlayerV2Update.");
            var cv = player.GetComponent<CharacterV2>();
            // The scene player wires every ref; null ONLY the fatality system so
            // this is the sole missing reference (all other D3 refs stay non-null).
            SetPrivateField(cv, "FatalitySystem", null);

            cv.Configure();

            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Configure must reject a missing fatalitySystem without registering (design D3).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Configure_NullTargetFocus_LogsError_AndSkipsRegistration()
        {
            // Design D3: targetFocus belongs in the fail-fast non-null list.
            LogAssert.Expect(LogType.Error,
                new System.Text.RegularExpressions.Regex("missing required reference: targetFocus"));
            ServiceLocator.Instance.Reset();
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain PlayerV2Update.");
            var cv = player.GetComponent<CharacterV2>();
            // Same isolation trick: the scene player wires every ref; null ONLY
            // targetFocus so it is the sole missing reference.
            SetPrivateField(cv, "targetFocus", null);

            cv.Configure();

            Assert.IsFalse(ServiceLocator.Instance.TryGetService<IPlayer>(out _),
                "Configure must reject a missing targetFocus without registering (design D3).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator DisableControls_ZeroesVelocity_FreezesRotation_AndStopsInputs()
        {
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain PlayerV2Update.");
            var cv = player.GetComponent<CharacterV2>();
            var rb = player.GetComponent<Rigidbody>();

            rb.linearVelocity = new Vector3(5f, 0f, 0f);
            cv.DisableControls();

            Assert.AreEqual(Vector3.zero, rb.linearVelocity, "DisableControls must zero velocity (P7).");
            Assert.IsTrue(rb.freezeRotation, "DisableControls must freeze rotation (P7).");
            Assert.IsFalse(cv.GetCanReadInputs(), "DisableControls must stop input reading (P7).");
            yield return null;
        }

        [UnityTest]
        public IEnumerator EnableControls_RestoresInputs_AndMovementResumes()
        {
            var player = GameObject.Find("PlayerV2Update");
            Assert.IsNotNull(player, "The test scene must contain PlayerV2Update.");
            var cv = player.GetComponent<CharacterV2>();
            var rb = player.GetComponent<Rigidbody>();

            cv.DisableControls();
            cv.EnableControls();

            Assert.IsTrue(cv.GetCanReadInputs(), "EnableControls must restore input reading (P7).");

            // P7 S3: movement resumes after EnableControls. Drive the direction
            // through the public MovementRigidbodyV2 API and let physics step.
            var movement = player.GetComponent<MovementRigidbodyV2>();
            movement.Direction(new Vector2(1f, 0f));
            for (var i = 0; i < 5; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            var horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            Assert.Greater(horizontal.magnitude, 0.1f,
                "Movement must resume after EnableControls (P7 S3).");
        }
    }
}