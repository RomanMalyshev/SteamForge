using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class View
    {
        public readonly SubscribableField<EncounterType> OnMapButtonClick = new(EncounterType.camp);
        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();

        public void Init()
        {
 
        }
        
    }
}