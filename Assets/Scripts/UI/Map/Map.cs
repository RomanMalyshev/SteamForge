using DefaultNamespace;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Map
{
    public class Map : MonoBehaviour
    {
        [FormerlySerializedAs("_enconterOrder")][SerializeField] public List<EncounterColumn> _encounterOrder;
        [SerializeField] private PlayerFigure _playerFigure;
        [SerializeField] private CameraConroller _cameraController;
        [SerializeField] private int _currentColumn;

        private void Start()
        {
            _cameraController.FindPlayerFigure(_playerFigure);
            _playerFigure.OnNewEncounterEnter.Subscribe( (column)=>
            {
                PlayerFigireMoved(column);
            });

            for (var index = 0; index < _encounterOrder.Count; index++)
            {
                var encounter = _encounterOrder[index];
                encounter.name = $"Colum  {index}";
            }

            foreach (var column in _encounterOrder)
            {
                foreach (var encounter1 in column.encounter)
                {
                    encounter1.OnEncounterSelect.Subscribe((transform, column1) =>
                    {
                        MovePlayerFigure(transform, column1);
                    });
                }
            }
        }

        private void MovePlayerFigure(Transform transform, int column)
        {
            _playerFigure.MoveToEncounter(transform, column);

            foreach (var column1 in _encounterOrder)
            {
                foreach (var encounter in column1.encounter)
                {
                    encounter.SetActive(false);                    
                }
            }
        }

        private void PlayerFigireMoved(int column)
        {
            _currentColumn = column;

            foreach (var column1 in _encounterOrder)
            {
                foreach (var encounter in column1.encounter)
                {
                    encounter.SetActive(true);

                    if ((encounter.Column - _currentColumn > 1) || (encounter.Column - _currentColumn < -1))
                    {
                        encounter.SetReeachable(false);
                    }
                    else encounter.SetReeachable(true);
                }
            }           
        }
    }

    [Serializable]
    public class EncounterColumn
    {
        [HideInInspector]public string name;
        public List<EncounterButton> encounter;
    }
}