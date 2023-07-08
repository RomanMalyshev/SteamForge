using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI.Map
{
    public class PlayerFigure : MonoBehaviour
    {       
        private RectTransform _rectTransform;

        private void Start()
        {            
            _rectTransform = GetComponent<RectTransform>(); 
        }

        public void MoveToEncounter(RectTransform encounterPosition)
        {
            _rectTransform.anchoredPosition = encounterPosition.anchoredPosition;            
        }

    }
}
