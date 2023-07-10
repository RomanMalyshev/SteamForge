using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RedBjorn.ProtoTiles.MapBorder;
using static UnityEngine.GraphicsBuffer;


namespace UI.Map
{
    public class PlayerFigure : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        private Transform _transform;
        [SerializeField] private bool _isMooving;

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
            }            
        }

        private IEnumerator MovingBetveenEncounters(Transform movingPosition, int column)
        {
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
