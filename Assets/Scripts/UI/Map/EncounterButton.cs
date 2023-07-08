using DefaultNamespace;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    [RequireComponent(typeof(Button))]
    public class EncounterButton : MonoBehaviour
    {
        [SerializeField] private EncounterType _type;        

        private Button _button;
        private View _view;
        private bool _isActive = false;
        private RectTransform _rectTransform;
        private Image _image;
        private Color _color;

        public int Column;

        public SubscribableAction<RectTransform, int> OnEncounterSelect = new();

        private void Start()
        {
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            _button.onClick.AddListener(OnButtonClick);
            _view = Globals.Global.View;
            _image = GetComponent<Image>();
            _color = _image.color;

            if ((Column == 0) || (Column == 1))
            {
                _isActive = true;
            }

            if (!_isActive)
            {
                _image.color = Color.grey;
            }
        }

        private void OnButtonClick()
        {
            if (_isActive)
            {
                _view.OnMapButtonClick.Invoke(_type);
                OnEncounterSelect.Invoke(_rectTransform, Column);
            }                
        }

        private void OnApplicationQuit()
        {
            _view.OnExitFromGame.Invoke(10);
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;

            if (_isActive)
            {
                _image.color = _color;
            }
            else _image.color = Color.grey;
        }
    }
}
