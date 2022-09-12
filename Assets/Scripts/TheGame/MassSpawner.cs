using System;
using NightFramework;

// ========================
// Revision 2020.10.19
// ========================

namespace TheGame
{
    public class MassSpawner : Singleton<MassSpawner>
    {
        [Serializable]
        public struct PreloadRecord
        {
            public PoolableObject Prefab;
            public int Count;
        }


        // ========================================================================================
        public PreloadRecord[] Preload;

        private ObjectPoolCollection<PoolableObject> _pools;


        // ========================================================================================
        public T Spawn<T>(T prefab) where T : PoolableObject
        {
            return _pools.GetPool(prefab, transform).Pop() as T;
        }

        protected override void Awake()
        {
            base.Awake();

            _pools = new ObjectPoolCollection<PoolableObject>();
        }

        protected void Start()
        {
            foreach (var item in Preload)
                _pools.GetPool(item.Prefab, transform, item.Count);
        }
    }
}