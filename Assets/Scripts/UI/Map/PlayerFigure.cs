using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static RedBjorn.ProtoTiles.MapBorder;
using static UnityEngine.GraphicsBuffer;


namespace UI.Map
{
    public class PlayerFigure : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpHeight;

        private Transform _transform;
        private bool _isMooving;

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
            //Vector3 jumpPoint = new Vector3(_transform.position.x, _transform.position.y + _jumpHeight, _transform.position.z);
           // _transform.position = Vector3.MoveTowards(_transform.position, jumpPoint, _jumpHeight);            

            while (!(_transform.position.x == movingPosition.position.x) || !(_transform.position.z == movingPosition.position.z))
            {
                _transform.position = Vector3.MoveTowards(_transform.position, movingPosition.position, _speed);               
                yield return null;                
            }

            OnNewEncounterEnter.Invoke(column);
            _isMooving = false;
        }

        
    }
}
