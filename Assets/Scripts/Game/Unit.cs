using System;
using DefaultNamespace.Battle;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Unit:MonoBehaviour
    {
        public int InitiativeTest = 1;
        public int ActionPoints = 20;

        public Action OnActionPointsEnd;
        public CommandHandler Handler;

        private int _currentActionPoints;
        public void Init(MapEntity battleFieldFieldEntity)
        {
            Handler.Init(battleFieldFieldEntity);
            Handler.onHandlerEnd += OnHandlerEnd;
            Debug.Log($"Init {gameObject.name}. AP - {_currentActionPoints}");
        }

        public void Activate()
        {
            Debug.Log($"Activate {gameObject.name}. AP - {_currentActionPoints}");
            _currentActionPoints = ActionPoints;
            Handler.Activate();
        }
        
        public void Deactivate()
        {
            Debug.Log($"Deactivate {gameObject.name}. AP - {_currentActionPoints}");

            Handler.Deactivate();
        }
        
        private void OnHandlerEnd()
        {
            _currentActionPoints -= 5;
            Debug.Log($"End handle {gameObject.name}. AP - {_currentActionPoints}");
            if (_currentActionPoints <= 0)
                OnActionPointsEnd?.Invoke();
        }
        
   

      
    }
}