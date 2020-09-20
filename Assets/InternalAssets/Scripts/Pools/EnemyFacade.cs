using CustomLogger;
using System;
using UnityEngine;
using Zenject;

namespace OrCor_GameName.Pool
{

    public class EnemyFacade : ObjectFacade
    {

        public override void OnSpawned(IMemoryPool pool)
        {
            base.OnSpawned(pool);
            DebugLogger.Log(null, "Enemy spawned", LogColor.Orange);
        }

        public class Factory : PlaceholderFactory<EnemyFacade>
        {
        }
    }
}