using System;
using RedBjorn.ProtoTiles;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace.Battle
{
    public abstract class CommandHandler:MonoBehaviour
    {
        public abstract Action onHandlerEnd { get; set; }

        public abstract void Init(MapEntity mapEntity);
        
        public abstract void Activate();
        public abstract void Handle();
        public abstract void Deactivate();
    }
}