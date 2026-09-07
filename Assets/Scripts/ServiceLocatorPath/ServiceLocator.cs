using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
public class ServiceLocator
{
    public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());
    private static ServiceLocator _instance;

    private readonly Dictionary<Type, object> _services;

    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void RegisterService<T>(T service)
    {
        var type = typeof(T);
        Assert.IsFalse(_services.ContainsKey(type), 
            $"Service {type} already registered");
        
        _services.Add(type, service);
    }

    public T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            throw new Exception($"Service {type} not found");
        }

        return (T) service;
    }
    
    public void UnregisterService<T>()
    {
        var type = typeof(T);
        // Tolerant by design: unregistering a service that was never registered
        // (e.g. a player destroyed before Configure completed, or after Reset)
        // is a no-op, not a failure. Duplicate REGISTRATION is still asserted
        // in RegisterService because that indicates a real double-instance bug.
        _services.Remove(type);
    }

    /// <summary>
    /// Non-throwing lookup. Returns true and sets <paramref name="service"/> when the
    /// service is registered; returns false with a default value otherwise.
    /// </summary>
    public bool TryGetService<T>(out T service)
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var value))
        {
            service = (T) value;
            return true;
        }

        service = default;
        return false;
    }

    /// <summary>
    /// Clears the static registry. Used as the test-isolation seam between
    /// PlayMode tests (the singleton survives scene loads).
    /// </summary>
    public void Reset()
    {
        _services.Clear();
    }
}