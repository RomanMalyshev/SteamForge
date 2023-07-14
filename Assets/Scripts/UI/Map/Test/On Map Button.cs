using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMapButton : MonoBehaviour
{
    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;
    }

    public void Click()
    {
        _view.OnCameraStateChange.Invoke(true);
    }
}
