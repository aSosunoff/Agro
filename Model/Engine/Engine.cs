using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Engine
{
    class Engine : IEngine
    {
        protected Dictionary<Type, Object> Objects = new Dictionary<Type, Object>();
        public T Get<T>()
        {
            Type repositoryType = typeof(T);

            if (Objects.ContainsKey(repositoryType))
                return (T)Objects[repositoryType];

            throw new Exception("Key for EngineObject is not found");
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Objects.Clear();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
