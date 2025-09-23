/* Service locator who returns which services exist and are registered. */

public static class ServiceLocator
{
    private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        if (_services.ContainsKey(typeof(T)))
            throw new InvalidOperationException(
                $"Service of type {typeof(T)} is already registered."
            );
        if (service is null)
        {
            throw new NullReferenceException($"Service of type {typeof(T)} is null.");
        }
        _services[typeof(T)] = service;
    }

    public static T Get<T>()
    {
        if (!_services.ContainsKey(typeof(T)))
            throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
        return (T)_services[typeof(T)];
    }

    public static void Unregister<T>()
    {
        if (_services.ContainsKey(typeof(T)))
            _services.Remove(typeof(T));
    }

    public static void Reset()
    {
        _services.Clear();
    }
}
