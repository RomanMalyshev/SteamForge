using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UI.Map
{
    public class PlayerFigure : MonoBehaviour
    {
        private int _currentColumn;
        private RectTransform _rectTransform;

        private void Start()
        {
            _currentColumn = 1;
            _rectTransform = GetComponent<RectTransform>(); ;

        }

        public void MoveToEncounter(RectTransform encounterPosition)
        {
            _rectTransform.anchoredPosition = encounterPosition.anchoredPosition;
        }

    }
}
