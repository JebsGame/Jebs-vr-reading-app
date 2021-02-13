using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JebsReadingGame.Pools
{
    public interface IPooledObject
    {
        void OnInstantiation();
        void OnRespawn();
    }
}
