using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Encounter : MonoBehaviour
{    
    private View _view;
    private bool _isReechable = false;
    private Transform _transform;
    private Renderer _renderer;
    private Color _color;
    private bool _isActive = true;

    public int Column;

    public SubscribableAction<Transform, int> OnEncounterSelect = new();



    private void Start()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
        _view = Globals.Global.View;
        _color = _renderer.material.color;

        if ((Column == 0) || (Column == 1))
        {
            _isReechable = true;
        }

        if (!_isReechable)
        {
            _renderer.material.color = Color.grey;
        }
    }

    private void OnMouseDown()
    {
        OnButtonClick();
    }

    private void OnButtonClick()
    {
        if (_isActive)
        {
            if (_isReechable)
            {
                EncounterSelected();
                //_view.OnMapButtonClick.Invoke(_type);
                OnEncounterSelect.Invoke(_transform, Column);
            }
        }

        _view.OnCameraTargetSelect.Invoke(_transform);
    }

    public abstract void EncounterSelected();    

    private void OnMouseOver()
    {
        if (_isActive)
        {
            GetComponent<Renderer>().material.color = _color + new Color(10f, 10f, 10f);
        }
    }

    private void OnMouseExit()
    {
        if (_isActive)
        {
            if (_isReechable)
            {
                _renderer.material.color = _color;
            }
            else _renderer.material.color = Color.grey;
        }
    }

    private void OnApplicationQuit()
    {
        _view.OnExitFromGame.Invoke(10);
    }

    public void SetReeachable(bool isReeachable)
    {
        _isReechable = isReeachable;

        if (_isReechable)
        {
            _renderer.material.color = _color;
        }
        else _renderer.material.color = Color.grey;
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }
}
