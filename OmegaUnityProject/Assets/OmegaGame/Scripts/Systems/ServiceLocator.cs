using System;
using System.Collections.Generic;
using Omega.Services;
using UnityEngine;

namespace Omega.Systems
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance { get; private set; }

        private readonly Dictionary<string, IService> _services;

        private ServiceLocator()
        {
            _services = new Dictionary<string, IService>();
        }

        public static void Initialize()
        {
            Instance = new ServiceLocator();
        }
        
        public T GetService<T>() where T : IService
        {
            var key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                return (T) _services[key];
            }
            Debug.LogError($"{key} not registered with {GetType().Name}");
            throw new InvalidOperationException();

        }

        public void Register<T>(T service) where T : IService
        {
            var key = typeof(T).Name;
            if (_services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>() where T : IService
        {
            var key = typeof(T).Name;
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }
            
            _services[key].Dispose();
            _services.Remove(key);
        }
    }
}