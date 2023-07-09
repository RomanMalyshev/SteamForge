using System;
using DefaultNamespace.Battle;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace DefaultNamespace.Player
{
    public class Unit : MonoBehaviour
    {
        public int InitiativeTest = 1;
        public int ActionPoints = 20;

        public Action OnActionPointsEnd;
        public CommandHandler Handler;

        private int _currentActionPoints;
        private Model _model;

        public void Init(MapEntity battleFieldFieldEntity)
        {
            _model = Globals.Global.Model;
            Handler.onHandlerEnd += OnHandlerEnd;

            Handler.Init(battleFieldFieldEntity);
            Debug.Log($"Init {gameObject.name}. AP - {_currentActionPoints}");
        }

        public void Activate()
        {
            Debug.Log($"Activate {gameObject.name}. AP - {_currentActionPoints}");
            _currentActionPoints = ActionPoints;
            _model.OnNewUnitTern.Invoke(gameObject.name);
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);

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
            _model.OnChangeUnitActionPoints.Invoke(_currentActionPoints);
            Debug.Log($"End handle {gameObject.name}. AP - {_currentActionPoints}");
            if (_currentActionPoints <= 0)
                OnActionPointsEnd?.Invoke();
        }
    }
}