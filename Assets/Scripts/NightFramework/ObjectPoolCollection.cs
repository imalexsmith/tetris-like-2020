using System.Collections.Generic;
using UnityEngine;

// ========================
// Revision 2020.10.19
// ========================

namespace NightFramework
{
    public class ObjectPoolCollection<T> where T : PoolableObject<T>
    {
        // ========================================================================================
        private readonly Dictionary<T, ObjectPool<T>> _pools;


        // ========================================================================================
        public ObjectPoolCollection()
        {
            _pools = new Dictionary<T, ObjectPool<T>>();
        }

        public ObjectPool<T> GetPool(T prefab, Transform poolParent = null, int preallocate = 5)
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                var root = new GameObject($"pool<{typeof(T).Name}>: {prefab.gameObject.name}");
                root.transform.SetParent(poolParent);
                pool = new ObjectPool<T>(prefab, root.transform, preallocate);
                _pools.Add(prefab, pool);
            }

            return pool;
        }
    }
}