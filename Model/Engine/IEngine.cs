using System;

namespace Model.Engine
{
    public interface IEngine : IDisposable
    {
        T Get<T>(); 
    }
}