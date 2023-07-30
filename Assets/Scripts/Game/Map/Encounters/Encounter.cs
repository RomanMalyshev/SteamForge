using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public abstract class Encounter : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private View _view;
    public bool _isReechable = false;
    private Transform _transform;   
    private Color _color;
    public bool _isActive = true;

    public int Column;
    public EncounterState EncounterState;

    public SubscribableAction<Transform, int> OnEncounterSelect = new();

    private void Start()
    {
        _transform = GetComponent<Transform>();        
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
        _view.OnEncounterClick.Invoke(this);
        if (_isActive)
        {
            if (_isReechable)
            {
                OnEncounterSelect.Invoke(_transform, Column);
            }
        }
    }

    private void OnMouseOver()
    {
        if (_isActive)
        {
            _renderer.material.color = _color + new Color(10f, 10f, 10f);
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

    public void SetEncounterState(EncounterState encounterState)
    {
        EncounterState = encounterState;
    }


    public abstract void Activate();
}
