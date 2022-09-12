using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ========================
// Revision 2020.10.19
// ========================

namespace NightFramework
{
    public class ObjectPool<T> where T : PoolableObject<T>
    {
        // ========================================================================================
        private readonly Stack<T> _pool;
        private readonly T _prefab;
        private readonly Transform _parent;


        // ========================================================================================
        public ObjectPool(T prefab, Transform parent = null, int preallocate = 5)
        {
            _pool = new Stack<T>();
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < preallocate; i++)
                Push(Instantiate());
        }

        public void Clear()
        {
            if (_pool != null && _pool.Count > 0)
            {
                while (_pool.Count > 0)
                    _pool.Pop().DestroyGameObject();
            }
        }

        public T Pop()
        {
            if (_pool.Count > 0)
            {
                T obj = _pool.Pop();
                obj.WakeUp();
                return obj;
            }

            var newObj = Instantiate();
            newObj.WakeUp();
            return newObj;
        }

        public void Push(T obj)
        {
            obj.Sleep();
            _pool.Push(obj);
        }

        private T Instantiate()
        {
            var inst = Object.Instantiate(_prefab, _parent);
            inst.Pool = this;
            return inst;
        }
    }
}