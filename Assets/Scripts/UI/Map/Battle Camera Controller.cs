using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using UI.Map;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//TODO: rename
public class BattleCameracontroller : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _speed = 0.06f;
    [SerializeField] private float _zoomSpeed = 10.0f;
    [SerializeField] private float _rotateSpeed = 0.1f;
    [SerializeField] private float _maxHeight = 25f;
    [SerializeField] private float _minHeight = 1f;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _isOnMap;
    [SerializeField] private float _onMapHeight;
    [SerializeField] private float _onMapDistance;
    [SerializeField] private Map _map;

    [SerializeField] private Vector3 _battlePoint;
    [SerializeField] private Vector3 _campPoint;
    [SerializeField] private Vector3 _mapPoint;
    
    

    private View _view;
    private Model _model;
    private Vector3 _newPosition;
    private Vector2 _rotateStartPoint;
    private Vector2 _rotateEndPoint;
    private Vector3 _newZoom;
    private Vector3 _dragStartPoint;
    private Vector3 _dragStopPosition;
    private PlayerFigure _playerFigure;
    private Transform _onMapCameraTransform;

    private void Start()
    {
        _mapPoint = new Vector3(29, -31, 220);
        //_campPoint = new Vector3(-6.3f, 10.2f, -317.82f);
        //_battlePoint = new Vector3(-6.3f, -10.2f, -317.82f);

        
        
        _view = Globals.Global.View;
        _model = Globals.Global.Model;
        _view.OnCameraTargetSelect.Subscribe((target) => { GetTarget(target); });
        
        
        _view.ReturnToMap.Subscribe(() =>
        {
            SetCameraOnMapState(true);
        });
        
        _view.ActiveBattle.Subscribe(value =>
        {
            SetCameraOnMapState(false);
        });
        
        _view.OnMapCampClick.Subscribe(() =>
        {
            GetCampCameraPosition();
        });
        
        _model.OnBattleEnd.Subscribe(winSide =>
        {
            if(winSide == UnitSide.Player)
                SetCameraOnMapState(true);
        });
        
        _model.OnUnitStartTern.Subscribe((unit, skills) => { GetTarget(unit.transform); });

        _view.OnUnitInBattleSelect.Subscribe(unit => { GetTarget(unit.transform); });
        _newPosition = transform.position;
        _newZoom = _cameraTransform.localPosition;
        _onMapCameraTransform = transform;

        _cameraTransform.localPosition = new Vector3(0, _minHeight, -_minHeight);

        SetCameraOnMapState(true);
    }


    private void Update()
    {
        if (_isOnMap)
        {
            OnMapCameraMovement();
        }
        else
        {
            if (_target != null)
            {
                transform.position = _target.position;
            }
            else
            {
                GetCameraMovement();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _target = null;
            }

            GetCameraRotation();
            GetCameraZoom();
        }
    }

    private void GetCameraMovement()
    {
        Vector3 lateralMove = transform.right * _speed * Input.GetAxis("Horizontal");
        Vector3 forwardMove = transform.forward * _speed * Input.GetAxis("Vertical");


        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                _dragStartPoint = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                _dragStopPosition = ray.GetPoint(entry);
            }
        }
        else
        {
            _dragStartPoint = _dragStopPosition;
        }

        _newPosition = lateralMove + forwardMove + _dragStartPoint - _dragStopPosition;

        transform.position += _newPosition;
    }

    private void GetCameraZoom()
    {
        Vector3 zoomAmount = new Vector3(0, -_zoomSpeed * Input.GetAxis("Mouse ScrollWheel"),
            _zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
        _newZoom = zoomAmount;

        if (((_cameraTransform.localPosition.y + _newZoom.y) >= _minHeight) &&
            ((_cameraTransform.localPosition.y + _newZoom.y) <= _maxHeight))
        {
            _cameraTransform.localPosition += _newZoom;
        }
    }

    private void GetCameraRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _rotateStartPoint = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            _rotateEndPoint = Input.mousePosition;

            float dx = (_rotateEndPoint - _rotateStartPoint).x * _rotateSpeed;
            float dy = (_rotateEndPoint - _rotateStartPoint).y * _rotateSpeed;

            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));

            var rotation = transform.GetChild(0).transform.rotation * Quaternion.Euler(new Vector3(-dy, 0, 0));
            
            if (rotation.eulerAngles.x > 0 && rotation.eulerAngles.x < 40)
                transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));

            if (rotation.eulerAngles.x > -40 && rotation.eulerAngles.x <= 0)
                transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));
            
            if (rotation.eulerAngles.x > 320 && rotation.eulerAngles.x <= 360)
                transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));
            
            
            _rotateStartPoint = _rotateEndPoint;
        }
    }

    private void OnMapCameraMovement()
    {
        if (_playerFigure == null) return;
        transform.position = new Vector3(_playerFigure.transform.position.x - _onMapDistance,
            _playerFigure.transform.position.y + _onMapHeight, _mapPoint.z);
    }

    private void GetCampCameraPosition()
    {
        SetCameraOnMapState(false);
        transform.position = _campPoint;
        transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
    }

    private void GetTarget(Transform target)
    {
        _target = target;
    }

    public void FindPlayerFigure(PlayerFigure playerFigure)
    {
        _playerFigure = playerFigure;
    }

    public void SetCameraOnMapState(bool state)
    {
        _isOnMap = state;

        if (_isOnMap)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            _cameraTransform.localPosition = new Vector3(0, _minHeight, -_minHeight);
        }
        else
        {
            _target = null;
        }
    }
    
    
}