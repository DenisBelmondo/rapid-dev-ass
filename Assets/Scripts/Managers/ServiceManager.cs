using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Managers
{
    public class ServiceManager : Singleton<ServiceManager>
    {
        private Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public void RegisteredService<T>(T service) where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                Debug.Log($"Service of type {typeof(T)} has been already registered.");
                return;
            }
            _services.Add(typeof(T), service);
        }

        public T GetService<T>() where T : class, IService
        {
            if (!_services.ContainsKey(typeof(T)))
            {
                Debug.Log($"Service of type {typeof(T)} is not registered.");
                return null;
            }
            return (T)_services[typeof(T)];
        }

        public void UnregisterService<T>() where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _services.Remove(typeof(T));
            }
        }
    }
}
