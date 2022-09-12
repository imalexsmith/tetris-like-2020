using UnityEngine;
using UltEvents;

// ========================
// Revision 2020.10.19
// ========================

namespace NightFramework
{
    public class PoolableObject : PoolableObject<PoolableObject> { }

    [DisallowMultipleComponent]
    public abstract class PoolableObject<T> : MonoBehaviour where T : PoolableObject<T>
    {
        // ========================================================================================
        public UltEvent OnWakeUp = new UltEvent();
        public UltEvent OnSleep = new UltEvent();

        [Space]
        public bool ManualActivation;

        internal ObjectPool<T> Pool { get; set; }


        // ========================================================================================
        public virtual void WakeUp()
        {
            if (!ManualActivation)
                gameObject.SetActive(true);

            OnWakeUp.Invoke();
        }

        public virtual void Sleep()
        {
            OnSleep.Invoke();

            if (!ManualActivation)
                gameObject.SetActive(false);
        }

        public virtual void ReturnToPool()
        {
            Pool.Push(this as T);
        }
    }
}