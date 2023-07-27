using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using static Cinemachine.CinemachineTargetGroup;
using static RedBjorn.ProtoTiles.MapBorder;
using static UnityEngine.GraphicsBuffer;


namespace GameMap
{
    public class PlayerFigure : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private Encounter _currentEncounter;
        [SerializeField] private Transform _innerFigure;

        private Transform _transform;
        private bool _isMooving;
        private int _direction = 1;

        public SubscribableAction<int> OnNewEncounterEnter = new();

        private void Start()
        {
            _transform = GetComponent<Transform>();
        }

        public void MoveToEncounter(Transform transform, int column)
        {
            if (!_isMooving)
            {
                _isMooving = true;
                StartCoroutine(MovingBetveenEncounters(transform, column));
                float dist = Vector3.Distance(transform.position, _transform.position);
            }
        }

        private IEnumerator MovingBetveenEncounters(Transform movingPosition, int column)
        {
            var stepDir = (movingPosition.position - _transform.position);

            _transform.rotation = Quaternion.LookRotation(stepDir, Vector3.up);


            while (!(_transform.position.x == movingPosition.position.x) || !(_transform.position.z == movingPosition.position.z))
            {
                _transform.position = Vector3.MoveTowards(_transform.position, movingPosition.position, _speed);

                _innerFigure.Translate(0, _jumpSpeed * _direction * Time.deltaTime, 0);                
                
                if (_innerFigure.position.y >= _jumpHeight)
                {
                    _direction = -1;
                } 
                else if (_innerFigure.position.y <= 0.5f)
                {
                    _direction = 1;
                }

                yield return null;
            }

            OnNewEncounterEnter.Invoke(column);
            _innerFigure.localPosition = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            _isMooving = false;
            _direction = 1;
        }

        public void SetCurrentEncounter(Encounter encounter)
        {
            _currentEncounter = encounter;           
        }

        public void GetStartingPoint()
        {
            transform.position = _currentEncounter.transform.position;
        }

        public void LoadPosition(Transform loadPosition)
        {
            transform.position = loadPosition.position;
        }
    }
}
