using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Map
{
    public class Map : MonoBehaviour
    {
        [FormerlySerializedAs("_enconterOrder")] [SerializeField] public List<EncounterColumn> _encounterOrder;

        private void Start()
        {
            for (var index = 0; index < _encounterOrder.Count; index++)
            {
                var encounter = _encounterOrder[index];
                encounter.name = $"Colum  {index}";
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