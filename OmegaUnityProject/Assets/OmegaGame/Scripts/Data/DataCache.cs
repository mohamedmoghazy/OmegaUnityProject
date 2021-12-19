using System;
using System.Collections.Generic;

public class DataCache<T> : IDisposable
{
    private readonly Dictionary<Type, T> _cache = new Dictionary<Type, T>();

    public void Add<TK>(TK dataCache) where TK : T
    {
        _cache.Add(typeof(TK), dataCache);
    }

    public void Remove<TK>() where TK : T
    {
        Remove(typeof(TK));
    }

    public void Remove(Type type)
    {
        _cache.Remove(type);
    }

    public TK Get<TK>() where TK : T
    {
        T data;

        if (!_cache.TryGetValue(typeof(TK), out data))
        {
            return default;
        }

        return (TK) data;
    }

    public bool Contains<TK>() where TK : T
    {
        return _cache.ContainsKey(typeof(TK));
    }

    public void Dispose()
    {
        _cache.Clear();
    }
}