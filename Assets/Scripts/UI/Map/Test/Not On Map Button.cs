using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotOnMapButton : MonoBehaviour
{    
    private View _view;

    private void Start()
    {
        _view = Globals.Global.View;
    }
    
}
