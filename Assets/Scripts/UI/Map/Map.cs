using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Map
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Encounter[] _encounters;

        private void OnEnable()
        {           
            foreach (Encounter encounter in _encounters)
            {
                encounter._onEncounterSelected += EncounterSelected;
            }
        }

        private void OnDisable()
        {
            foreach (Encounter encounter in _encounters)
            {
                encounter._onEncounterSelected -= EncounterSelected;
            }
        }

        private void EncounterSelected(EncounterType type)
        {
            Debug.Log(type);
        }
    }    
}