using System.Collections;
using System.Collections.Generic;
using UI.Map;
using UnityEngine;

//TODO: delete this
public class CameraConroller : MonoBehaviour
{
    [SerializeField] private float _height;
    [SerializeField] private float _distance;    

    private PlayerFigure _playerFigure;

    
    void Update()
    {
        if (!(_playerFigure == null))
        {
            transform.position = new Vector3(_playerFigure.transform.position.x - _distance, _playerFigure.transform.position.y + _height, transform.position.z);
        }        
    }

    public void FindPlayerFigure(PlayerFigure playerFigure)
    {
        _playerFigure = playerFigure;
    }
}
