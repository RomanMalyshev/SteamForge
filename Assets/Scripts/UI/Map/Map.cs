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
                    encounter1.OnEncounterSelect.Subscribe(rectTransform =>
                    {
                        MovePlayerFigure(rectTransform);
                    });
                }
            }
        }

        private void MovePlayerFigure(RectTransform rectTransform)
        {
            _playerFigure.MoveToEncounter(rectTransform);
        }
    }

    [Serializable]
    public class EncounterColumn
    {
        [HideInInspector]public string name;
        public List<EncounterButton> encounter;
    }
}