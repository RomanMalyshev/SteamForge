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
        [SerializeField] private int _currentColumn;


        private void Start()
        {
            for (var index = 0; index < _encounterOrder.Count; index++)
            {
                var encounter = _encounterOrder[index];
                encounter.name = $"Colum  {index}";
            }

            foreach (var column in _encounterOrder)
            {
                foreach (var encounter1 in column.encounter)
                {
                    encounter1.OnEncounterSelect.Subscribe((rectTransform, column1) =>
                    {
                        MovePlayerFigure(rectTransform, column1);
                    });
                }
            }
        }

        private void MovePlayerFigure(RectTransform rectTransform, int column)
        {
            _playerFigure.MoveToEncounter(rectTransform);
            _currentColumn = column;

            foreach (var column1 in _encounterOrder)
            {
                foreach (var encounter1 in column1.encounter)
                {
                    if ((encounter1.Column - _currentColumn > 1) || (encounter1.Column - _currentColumn < -1))
                    {
                        encounter1.SetActive(false);
                    }
                    else encounter1.SetActive(true);
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