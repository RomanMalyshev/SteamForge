using DefaultNamespace;
using DefaultNamespace.Player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
        private string _ecounterStatesSaveData = "EncounterStatesSaveData";
        private string _currentEncounterSaveData = "CurrentEncounterSaveData";

        private void Start()
        {
            _view = Globals.Global.View;
            _model = Globals.Global.Model;

            if (_cameraController != null)
                _cameraController.FindPlayerFigure(_playerFigure);

            if (_model.Plyer != null)
            {
                FinalEncounterSelect(_model.Plyer.Value);
            }

            _model.Plyer.Subscribe((player) =>
            {
                FinalEncounterSelect(player);
            });

            _playerFigure.OnNewEncounterEnter.Subscribe((column) =>
            {
                PlayerFigireMoved(column);
            });

            _view.OnNewGame.Subscribe(() =>
            {
                StartNewGame();                
            });           

            _view.OnEncounterClick.Subscribe((currentEncounter) =>
            {
                SetCurrentEncounter(currentEncounter);
            });

            _view.ReturnToMap.Subscribe(() =>
            {
                SaveData();
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

            if (PlayerPrefs.HasKey(_ecounterStatesSaveData))
            {
                LoadData();
            }
            else
            {
                _playerFigure.GetStartingPoint();
            }

            _cameraController.GetOnMapBorderPoint(_encounterOrder[_encounterOrder.Count - 1].encounter[0]);
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

            for (var i = 0; i < _currentColumn; i++)
            {
                for (var a = 0; a < _encounterOrder[i].encounter.Count; a++)
                {
                    if (_encounterOrder[i].encounter[a].EncounterState == EncounterState.NonVisited)
                    {
                        _encounterOrder[i].encounter[a].EncounterState = EncounterState.Skipped;
                    }
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
            if (player.Moral > 0)
            {
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[0].gameObject.SetActive(true);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[1].gameObject.SetActive(false);                
            }

            if (player.Moral == 0)
            {
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[0].gameObject.SetActive(true);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[1].gameObject.SetActive(true);                
            }

            if (player.Moral < 0)
            {
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[0].gameObject.SetActive(false);
                _encounterOrder[_encounterOrder.Capacity - 1].encounter[1].gameObject.SetActive(true);                
            }
        }

        private void SaveData()
        {
            var encountersStateSaveData = new EncountersStateList();

            for (var i = 0; i < _encounterOrder.Count; i++)
            {
                EncounterCollumsStateList newCollum = new();
                encountersStateSaveData.EncountersState.Add(newCollum);

                for (var a = 0; a < _encounterOrder[i].encounter.Count; a++)
                {
                    EncounterState state = _encounterOrder[i].encounter[a].EncounterState;
                    encountersStateSaveData.EncountersState[i].EncountersCollums.Add(state);
                }
            }
            
            var outputEncountersStateString = JsonUtility.ToJson(encountersStateSaveData);

            PlayerPrefs.SetString(_ecounterStatesSaveData, outputEncountersStateString);

            var currentEncaunterIndex = _encounterOrder[_currentColumn].encounter.IndexOf(_currentEncounter, 0);
            var currentEncounterSaveData = new CurrentEncounterIndex();

            currentEncounterSaveData.CurrentEncounter = new Vector2Int(_currentColumn, currentEncaunterIndex);
            var outputCurrentEncounterString = JsonUtility.ToJson(currentEncounterSaveData);
           
            PlayerPrefs.SetString(_currentEncounterSaveData, outputCurrentEncounterString);
            PlayerPrefs.Save();
        }

        private void LoadData()
        {
            var inputEncountersStateString = PlayerPrefs.GetString(_ecounterStatesSaveData);
            var encountersStateLoadData = JsonUtility.FromJson<EncountersStateList>(inputEncountersStateString);            

            for (var i = 0; i < _encounterOrder.Count; i++)
            {
                for (var a = 0; a < _encounterOrder[i].encounter.Count; a++)
                {
                    _encounterOrder[i].encounter[a].EncounterState = encountersStateLoadData.EncountersState[i].EncountersCollums[a];
                }
            }

            var inputCurrentEncounterString = PlayerPrefs.GetString(_currentEncounterSaveData);
            CurrentEncounterIndex currentEncounterLoadData = JsonUtility.FromJson<CurrentEncounterIndex>(inputCurrentEncounterString);
            _currentEncounter = _encounterOrder[currentEncounterLoadData.CurrentEncounter.x].encounter[currentEncounterLoadData.CurrentEncounter.y];
            
            _playerFigure.LoadPosition(_currentEncounter.transform);
            LoadMapState(currentEncounterLoadData.CurrentEncounter.x);
        }
        private void LoadMapState(int column)
        {
            _currentColumn = column;            

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

        private void StartNewGame()
        {
            _currentEncounter = _encounterOrder[0].encounter[0];
            _playerFigure.SetCurrentEncounter(_currentEncounter);
            _playerFigure.GetStartingPoint();
            _encounterOrder[1].encounter[0].SetReeachable(true);
            _encounterOrder[1].encounter[1].SetReeachable(true);

            foreach (var column1 in _encounterOrder)
            {
                foreach (var encounter in column1.encounter)
                {
                    encounter.SetActive(true);
                    encounter.EncounterState = EncounterState.NonVisited;                    
                }
            }           

            _playerFigure.SetCurrentEncounter(_currentEncounter);
            _playerFigure.GetStartingPoint();
        }
    }

    [Serializable]
    public class EncounterColumn
    {
        [HideInInspector] public string name;
        public List<Encounter> encounter;
    }

    [Serializable]
    public class EncountersStateList
    {
        public List<EncounterCollumsStateList> EncountersState = new();
    }

    [Serializable]
    public class EncounterCollumsStateList
    {
        public List<EncounterState> EncountersCollums = new();
    }

    [Serializable]
    public class CurrentEncounterIndex
    {
        public Vector2Int CurrentEncounter;
    }
}