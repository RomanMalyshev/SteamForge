using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(() => Globals.Global.View.SaveButtonClick.Invoke());
    }
}

    
