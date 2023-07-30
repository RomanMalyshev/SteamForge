using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Player;
using Game.Battle;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugBattleUI : MonoBehaviour
{
    public TMP_Text Round;
    public TMP_Text UnitName;
    public TMP_Text AP;

    public TMP_Text Side;

    public Button Prefab;
    public Button Restart;
    public Button RunTestField1;
    public Button RunTestField2;
    public Button RunTestField3;
    
    public Button SkipTurn;
    
    public Button TestWin;

    public GameObject TurnMenu;
    
    public MapSettings Field1;
    public MapSettings Field2;
    public MapSettings Field3;

    public Transform ButtonsParents;

    [Header("Battle line")] public UIUnitLine UnitLinePrefab;
    public Transform Content;
    public GameObject RoundSeparator;

    [Header("Result")] public GameObject ResultPopup;
    public Button RestartResult;
    public Button NextBattleTest;
    public Button ReturnToMap;
    public TMP_Text ResultLabel;
    public TMP_Text MoralResult;
    public TMP_Text ExpResult;

    [Header("Skill Settings")] public Sprite MoveIcon;
    public Sprite AttackIcon;
    public Sprite HealIcon;
    
    private readonly Dictionary<Unit, UIUnitLine> _unitsLine = new();
    private readonly Dictionary<Unit, UIUnitLine> _unitsNextRoundLine = new();

    private Model _model;
    private View _view;
    private readonly List<Button> _unitCommands = new();

    private readonly List<MapSettings> _testMapsOrder = new();
    private GameObject _separator;

    public void Init()
    {
        _model = Globals.Global.Model;
        _view = Globals.Global.View;

        _testMapsOrder.Add(Field1);
        _testMapsOrder.Add(Field2);
        //_testMapsOrder.Add(Field3);   

        SkipTurn.onClick.AddListener(() =>
        {
            _view.onBattleSkipTurn.Invoke();
        });
        
        _model.OnNewBattleRound.Subscribe(round =>
        {
            foreach (var unitLine in _unitsLine)
                unitLine.Value.gameObject.SetActive(true);

            Round.text = $"Round: {round}";
        });

        _model.OnUnitStartTern.Subscribe((unit, commands) =>
        {
            foreach (var unitCommand in _unitCommands)
                Destroy(unitCommand.gameObject);

            _unitCommands.Clear();

            UnitName.text = $"{unit.gameObject.name}";
            Side.text = unit.UnitSide == UnitSide.Player ? "Player Turn" : "Enemy Turn";

            TurnMenu.gameObject.SetActive(unit.UnitSide == UnitSide.Player);
            if (unit.UnitSide != UnitSide.Player) return;

            foreach (var command in commands)
            {
                var button = Instantiate(Prefab, ButtonsParents);
                _unitCommands.Add(button);
                button.onClick.AddListener(() => { _view.OnCommandSelect.Invoke(command); });
                if (command is Movable)
                {
                    button.image.sprite = MoveIcon;
                }
                if (command is Attacker)
                {
                    button.image.sprite = AttackIcon;
                }
            }
            
        });

        _model.UnitBattleOrder.Subscribe(units =>
        {
            LineInstance(units, _unitsLine);
            _separator = Instantiate(RoundSeparator, Content);
            LineInstance(units, _unitsNextRoundLine);
        });

        _model.OnUnitHealthChange.Subscribe(unit =>
        {
            if (_unitsLine.TryGetValue(unit, out var unitLine))
                unitLine.Health.text = unit.CurrentHealth.ToString();
            else
                Debug.LogWarning("Unit has not line prefab on battle UI!");

            if (_unitsNextRoundLine.TryGetValue(unit, out var nexRoundUnitLine))
                nexRoundUnitLine.Health.text = unit.CurrentHealth.ToString();
            else
                Debug.LogWarning("Unit has not line prefab on battle UI!");
        });

        _model.OnUnitEndTern.Subscribe(unit =>
        {
            if (_unitsLine.TryGetValue(unit, out var value))
                value.gameObject.SetActive(false);
            else
                Debug.LogWarning("Unit has not line prefab on battle UI!");
        });

        _model.OnChangeUnitActionPoints.Subscribe(ap => { AP.text = $"Action points: {ap}"; });

        ResultPopup.gameObject.SetActive(false);
        RestartResult.onClick.AddListener(() =>
        {
            ResultPopup.gameObject.SetActive(false);
            _view.OnRestartBattle.Invoke();
        });

        ReturnToMap.onClick.AddListener(() =>
        {
            ResultPopup.gameObject.SetActive(false);
            _view.ReturnToMap.Invoke();
        });

        NextBattleTest.onClick.AddListener(() =>
        {
            ResultPopup.gameObject.SetActive(false);
            _view.ActiveBattle.Value = (_testMapsOrder[Random.Range(0, _testMapsOrder.Count - 1)], false);
        });
        TestWin.onClick.AddListener(() =>
        {
            _view.OnTestWin.Invoke();
        });
        _model.OnBattleEnd.Subscribe(winSide =>
        {
            Debug.LogWarning(winSide);
            RestartResult.gameObject.SetActive(winSide == UnitSide.Enemy);
            ReturnToMap.gameObject.SetActive(winSide == UnitSide.Player);
            ResultLabel.text = winSide == UnitSide.Player ? "Win!" : "Lose!";
            ResultPopup.gameObject.SetActive(true);
        });
        
        _model.ChangedPlayerExp.Subscribe(value => { ExpResult.text = "Exp +" +value.ToString(); });
        
        _model.ChangedPlayerMoral.Subscribe(value =>
        {
            var symbol = value > 0 ? "+" : "";
            MoralResult.text = "Moral " + symbol + value.ToString();
        });
        
        _view.mouseOverUnit.Subscribe(unit =>
        {
            if (unit == null)
            {
                foreach (var unitLine in _unitsLine)
                    unitLine.Value.MouseTargetIcon.sprite = unitLine.Value.SpriteUnitMouseExit;
                foreach (var unitLine in _unitsNextRoundLine)
                    unitLine.Value.MouseTargetIcon.sprite = unitLine.Value.SpriteUnitMouseExit;
                
                return;
            }

            if (_unitsLine.TryGetValue(unit, out var currentLine))
            {
                currentLine.MouseTargetIcon.sprite = currentLine.SpriteUnitMouseOver;
            }
            if (_unitsNextRoundLine.TryGetValue(unit, out var nexRoundLine))
            {
                nexRoundLine.MouseTargetIcon.sprite = nexRoundLine.SpriteUnitMouseOver;
            }
        });
        
        Restart.onClick.AddListener(() => { _view.OnRestartBattle.Invoke(); });

        RunTestField1.onClick.AddListener(() => { _view.ActiveBattle.Value = (Field1, false); });
        RunTestField2.onClick.AddListener(() => { _view.ActiveBattle.Value = (Field2, false); });
        RunTestField3.onClick.AddListener(() => { _view.ActiveBattle.Value = (Field3, false); });
    }

    private void LineInstance(List<Unit> units, Dictionary<Unit, UIUnitLine> unitLines)
    {
        for (var i = 0; i < units.Count; i++)
        {
            var unitLine = Instantiate(UnitLinePrefab, Content);
            var unit = units[i];

            unitLine.Button.onClick.AddListener(() => _view.OnUnitInBattleSelect.Invoke(unit));
            unitLine.Label.text = unit.UnitSide.ToString();
            unitLine.Health.text = unit.Health.ToString();

            unit.OnUnitDead += deadUnit =>
            {
                unitLines.Remove(deadUnit);
                Destroy(unitLine.gameObject);
            };

            unitLines.Add(unit, unitLine);
        }
    }

    public void Reset()
    {
        foreach (var unitsLine in _unitsLine)
        {
            Destroy(unitsLine.Value.gameObject);
        }

        foreach (var unitsLine in _unitsNextRoundLine)
        {
            Destroy(unitsLine.Value.gameObject);
        }

        _unitsNextRoundLine.Clear();
        _unitsLine.Clear();

        Destroy(_separator);
    }
}