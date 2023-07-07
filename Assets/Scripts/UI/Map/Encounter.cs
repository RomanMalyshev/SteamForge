using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Map
{
    public class Encounter : MonoBehaviour
    {
        [SerializeField] private EncounterType _type;

        public Action<EncounterType> _onEncounterSelected;       

        public void Select()
        {
            _onEncounterSelected?.Invoke(_type);
        }
    }
}
