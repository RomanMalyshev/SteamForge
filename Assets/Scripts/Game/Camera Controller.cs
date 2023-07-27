using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using GameMap;
using UnityEngine;
using UnityEngine.UIElements;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _speed = 0.06f;
    [SerializeField] private float _targetSwapSpeed = 0.01f;
    [SerializeField] private float _zoomSpeed = 10.0f;
    [SerializeField] private float _rotateSpeed = 0.1f;
    [SerializeField] private float _maxHeight = 25f;
    [SerializeField] private float _minHeight = 1f;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxZ;
    [SerializeField] private CameraState _cameraState;
    [SerializeField] private Transform _target;    
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
    public bool _isActiveControl;
    public bool _isMooving;
    private PlayerFigure _playerFigure;    

    private void Start()
    {        
        _view = Globals.Global.View;
        _model = Globals.Global.Model;
        _view.OnCameraTargetSelect.Subscribe((target) => { GetTarget(target); });
        
        _view.ReturnToMap.Subscribe(() =>
        {
            SetCameraOnMapState(CameraState.OnMap);
        });
        
        _view.ActiveBattle.Subscribe(value =>
        {
            SetCameraOnMapState(CameraState.InBattle);
        });
        
        _view.OnMapCampClick.Subscribe(() =>
        {
            SetCameraOnMapState(CameraState.InCamp);
        });

        _model.OnUnitStartTern.Subscribe((unit, skills) => { GetTarget(unit.transform); });
        _view.OnUnitInBattleSelect.Subscribe(unit => { GetTarget(unit.transform); });

        _newPosition = transform.position;
        _newZoom = _cameraTransform.localPosition;
        _cameraTransform.localPosition = new Vector3(0, _minHeight, -_minHeight);

        SetCameraOnMapState(CameraState.OnMap);
    }


    private void Update()
    {
        if ((Input.GetAxis("Horizontal") != 0) || (Input.GetAxis("Vertical") != 0) || (Input.GetMouseButtonDown(1)))
        {
            _isActiveControl = true;
        }

        if (_cameraState == CameraState.OnMap)
        {
            OnMapCameraMovement();
        }
        else if ((_cameraState == CameraState.InBattle) && !_isMooving)
        {
            if (_target != null && !_isActiveControl)
            {
                transform.position = new Vector3(_target.position.x,transform.position.y , _target.position.z);
            }
            else
            {
                GetCameraMovement();
            }            

            GetCameraRotation();
            GetCameraZoom();            
        }        
    }

    private void GetCameraMovement()
    {
        Vector3 lateralMove = transform.right * _speed * Input.GetAxis("Horizontal") * Time.deltaTime;
        Vector3 forwardMove = transform.forward * _speed * Input.GetAxis("Vertical") * Time.deltaTime;


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

        if (transform.position.x + _newPosition.x >= _maxX)
        {
            _newPosition.x = 0;
        }

        if (transform.position.x + _newPosition.x < _minX)
        {
            _newPosition.x =0;
        }

        if (transform.position.z + _newPosition.z >= _maxZ)
        {
            _newPosition.z = 0;
        }

        if (transform.position.z + _newPosition.z < _minZ)
        {
            _newPosition.z = 0;
        }

        transform.position += _newPosition;
    }

    private void GetCameraZoom()
    {
        Vector3 zoomAmount = new Vector3(0, -_zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime,
            _zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);

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

            float dx = (_rotateEndPoint - _rotateStartPoint).x * _rotateSpeed * Time.deltaTime;
            float dy = (_rotateEndPoint - _rotateStartPoint).y * _rotateSpeed * Time.deltaTime;

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
        transform.position = new Vector3(_playerFigure.transform.position.x,
            _playerFigure.transform.position.y + _onMapHeight, _mapPoint.z - _onMapDistance);
    }       

    public void SetCameraOnMapState(CameraState state)
    {
        _cameraState = state;

        if (_cameraState == CameraState.OnMap)
        {
            _target = null;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            _cameraTransform.localPosition = new Vector3(0, _minHeight, -_minHeight);
        }            
        else if (_cameraState == CameraState.InBattle)
        {
            transform.position = _target.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            _cameraTransform.localPosition = new Vector3(0, _minHeight * 2 , -_minHeight * 2);
        }
        else if (_cameraState == CameraState.InCamp)
        {
            transform.position = _campPoint;
            _cameraTransform.localPosition = new Vector3(0, _minHeight * 2, -_minHeight * 2);
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        }
    }

    private void GetTarget(Transform target)
    {
        if (!_isMooving)
        {
            _target = target;
            _isActiveControl = false;
            _isMooving = true;
            
            StartCoroutine(TarrgetSwapping(_target));
            _cameraTransform.localPosition = new Vector3(0, _minHeight * 2, -_minHeight * 2);
        }        
    }

    public void FindPlayerFigure(PlayerFigure playerFigure)
    {
        _playerFigure = playerFigure;
    }

    private IEnumerator TarrgetSwapping(Transform target)
    {
        while ((target != null) && ((transform.position.x != target.position.x) || (transform.position.z != target.position.z)))
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _targetSwapSpeed * Time.deltaTime);
                yield return null;
        }
        
        _isMooving = false;
    }
}

public enum CameraState
{
    OnMap,
    InBattle,
    InCamp
}