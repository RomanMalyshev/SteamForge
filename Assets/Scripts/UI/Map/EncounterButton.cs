using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    [RequireComponent(typeof(Button))]
    public class EncounterButton : MonoBehaviour
    {
        [SerializeField] private EncounterType _type;
        [SerializeField] private int _column;

        private Button _button;
        private View _view;
        private bool _isReachable;
        private RectTransform _rectTransform;

        public SubscribableAction<RectTransform> OnEncounterSelect = new();

        private void Start()
        {
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            _button.onClick.AddListener(OnButtonClick);
            _view = Globals.Global.View;            
        }

        private void OnButtonClick()
        {            
            _view.OnMapButtonClick.Invoke(_type);
            OnEncounterSelect.Invoke(_rectTransform);
        }

        private void OnApplicationQuit()
        {
            _view.OnExitFromGame.Invoke(10);
        }
    }
}
