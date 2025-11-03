using System;

namespace Core
{
    public interface IService : IDisposable
    {
        void Initialize();
    }
}