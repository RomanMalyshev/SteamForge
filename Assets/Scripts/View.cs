using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class View
    {
        public readonly SubscribableActionField<EncounterType> OnButtonClick = new(EncounterType.camp);
        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();
        
        public void Init()
        {
 
        }

    
    }
}