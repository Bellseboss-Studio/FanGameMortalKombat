using NUnit.Framework;

namespace MortalKombat.Tests
{
    /// <summary>
    /// EditMode tests for the ServiceLocator test-isolation seam (design D2):
    /// <c>TryGetService&lt;T&gt;</c> (non-throwing lookup) and <c>Reset()</c> (clear static registry
    /// between PlayMode tests). Spec P2/S1 and design D2 require these APIs.
    /// </summary>
    public class ServiceLocatorTests
    {
        private sealed class DummyService
        {
            public string Tag { get; }

            public DummyService(string tag)
            {
                Tag = tag;
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Registry is a static singleton: always clean up between tests.
            ServiceLocator.Instance.Reset();
        }

        [Test]
        public void TryGetService_RegisteredService_ReturnsTrueAndResolvesService()
        {
            var expected = new DummyService("player");
            ServiceLocator.Instance.RegisterService(expected);

            var found = ServiceLocator.Instance.TryGetService<DummyService>(out var resolved);

            Assert.IsTrue(found, "TryGetService must return true for a registered service.");
            Assert.AreSame(expected, resolved, "TryGetService must resolve the registered instance.");
        }

        [Test]
        public void TryGetService_MissingService_ReturnsFalseWithoutThrowing()
        {
            var found = ServiceLocator.Instance.TryGetService<DummyService>(out var resolved);

            Assert.IsFalse(found, "TryGetService must return false when the service is not registered.");
            Assert.IsNull(resolved, "The out parameter must be null when the service is missing.");
        }

        [Test]
        public void Reset_ClearsRegistryAndAllowsReRegistration()
        {
            var first = new DummyService("first");
            ServiceLocator.Instance.RegisterService(first);

            ServiceLocator.Instance.Reset();

            var foundAfterReset = ServiceLocator.Instance.TryGetService<DummyService>(out var resolvedAfterReset);
            Assert.IsFalse(foundAfterReset, "Reset must remove all registered services.");
            Assert.IsNull(resolvedAfterReset, "Reset must leave no resolvable service behind.");

            // If Reset had not actually cleared the dictionary, this duplicate
            // RegisterService would throw (Assert.IsFalse on existing key).
            var second = new DummyService("second");
            ServiceLocator.Instance.RegisterService(second);
            var foundAfterReRegister = ServiceLocator.Instance.TryGetService<DummyService>(out var resolvedReRegistered);
            Assert.IsTrue(foundAfterReRegister, "Registry must accept new registrations after Reset.");
            Assert.AreSame(second, resolvedReRegistered, "The newly registered service must resolve after Reset.");
        }

        [Test]
        public void UnregisterService_MissingService_IsNoOp()
        {
            // A player destroyed before Configure completes (or after Reset) unregisters
            // without having registered. That must be a no-op, not an assertion failure.
            ServiceLocator.Instance.UnregisterService<DummyService>();

            var found = ServiceLocator.Instance.TryGetService<DummyService>(out _);
            Assert.IsFalse(found, "Unregistering a missing service must not create or resolve anything.");
        }
    }
}