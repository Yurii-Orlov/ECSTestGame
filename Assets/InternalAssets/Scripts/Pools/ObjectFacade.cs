using System;
using UnityEngine;
using Zenject;

namespace OrCor_GameName.Pool
{

    public abstract class ObjectFacade : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [Inject]
        PoolableManager _poolableManager;

        IMemoryPool _pool;

        public virtual void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _poolableManager.TriggerOnSpawned();
        }

        public virtual void OnDespawned()
        {
            _pool = null;
            _poolableManager.TriggerOnDespawned();
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

    }
}