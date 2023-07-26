using DefaultNamespace;
using DefaultNamespace.Player;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMap
{
    public class Map : MonoBehaviour
    {
        [FormerlySerializedAs("_enconterOrder")][SerializeField] public List<EncounterColumn> _encounterOrder;
        [SerializeField] private PlayerFigure _playerFigure;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private int _currentColumn;
        [SerializeField] private Encounter _currentEncounter;

        private View _view;
        private Model _model;

        private void Start()
        {
            _view = Globals.Global.View;
            _model = Globals.Global.Model;

            if (!(_cameraController == null))
                _cameraController.FindPlayerFigure(_playerFigure);

            _model.Plyer.Subscribe((player) =>
            {
                FinalEncounterSelect(player);
            });

            _playerFigure.OnNewEncounterEnter.Subscribe((column) =>
            {
                PlayerFigireMoved(column);
            });

            _view.OnEncounterClick.Subscribe((currentEncounter) =>
            {
                SetCurrentEncounter(currentEncounter);
            });

            for (var index = 0; index < _encounterOrder.Count; index++)
            {
                var encounter = _encounterOrder[index];
                encounter.name = $"Colum  {index}";
            }

            foreach (var column in _encounterOrder)
            {
                foreach (var encounter1 in column.encounter)
                {
                    encounter1.OnEncounterSelect.Subscribe((transform, column1) =>
                    {
                        MovePlayerFigure(transform, column1);
                    });
                }
            }

            _currentEncounter = _encounterOrder[0].encounter[0];
            _playerFigure.SetCurrentEncounter(_currentEncounter);
            _playerFigure.GetStartingPoint();
        }

        private void MovePlayerFigure(Transform transform, int column)
        {
            _playerFigure.MoveToEncounter(transform, column);

            foreach (var column1 in _encounterOrder)
            {
                foreach (var encounter in column1.encounter)
                {
                    encounter.SetActive(false);
                }
            }
        }

        private void PlayerFigireMoved(int column)
        {
            _currentColumn = column;
            _currentEncounter.Activate();

            foreach (var column1 in _encounterOrder)
            {
                foreach (var encounter in column1.encounter)
                {
                    encounter.SetActive(true);

                    if ((encounter.Column - _currentColumn > 1) || (encounter.Column <= _currentColumn))
                    {
                        encounter.SetReeachable(false);
                    }
                    else encounter.SetReeachable(true);
                }
            }
        }

        private void SetCurrentEncounter(Encounter currentEncounter)
        {
            _currentEncounter = currentEncounter;
            _playerFigure.SetCurrentEncounter(currentEncounter);
        }

        private void FinalEncounterSelect(Player player)
        {
            Debug.Log(player.Moral);
            if (player.Moral > 0)
            {
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[0].gameObject.SetActive(true);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[1].gameObject.SetActive(false);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[2].gameObject.SetActive(false);
            }

            if (player.Moral == 0)
            {
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[0].gameObject.SetActive(false);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[1].gameObject.SetActive(true);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[2].gameObject.SetActive(false);
            }

            if (player.Moral < 0)
            {
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[0].gameObject.SetActive(false);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[1].gameObject.SetActive(false);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[2].gameObject.SetActive(true);
            }
        }
    }

    [Serializable]
    public class EncounterColumn
    {
        [HideInInspector] public string name;
        public List<Encounter> encounter;
    }
}