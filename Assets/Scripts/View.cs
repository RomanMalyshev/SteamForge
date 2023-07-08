using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class View
    {
        public readonly SubscribableAction<EncounterType> OnMapButtonClick = new();
        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();

        public void Init()
        {
 
        }
        
    }
}