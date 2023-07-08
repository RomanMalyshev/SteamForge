using DefaultNamespace;
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
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
            _view = Globals.Global.View;
        }

        private void OnButtonClick()
        {
            _view.OnMapButtonClick.Value = _type;
        }

        private void OnApplicationQuit()
        {
            _view.OnExitFromGame.Invoke(10);
        }
    }
}
