using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Battle;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private DebugBattleUI _debugBattleUI;
    [SerializeField] private PartyUpgrade _upgradeUI;
    [SerializeField] private OnMapUI _onMapUI;
    [SerializeField] private MainMenuUI _mainMenuUI;
    [SerializeField] private EndGameUI _endGameUI;


    private void Start()
    {
        _endGameUI.Init();
        _debugBattleUI.gameObject.SetActive(false);
        _upgradeUI.gameObject.SetActive(false);
        _onMapUI.gameObject.SetActive(false);
        _mainMenuUI.gameObject.SetActive(true);
        _endGameUI.gameObject.SetActive(false);

        Globals.Global.View.OnNewGame.Subscribe(() =>
        {
            _onMapUI.gameObject.SetActive(true);
            _mainMenuUI.gameObject.SetActive(false);
            _endGameUI.gameObject.SetActive(false);
        });

        Globals.Global.View.StartGame.Subscribe(() =>
        {
            _onMapUI.gameObject.SetActive(true);
            _mainMenuUI.gameObject.SetActive(false);

        });

        Globals.Global.View.ActiveBattle.Subscribe(map => 
        {
            _debugBattleUI.gameObject.SetActive(true);
            _onMapUI.gameObject.SetActive(false);
        });

        Globals.Global.View.ActiveBattle.Subscribe(map => { _debugBattleUI.gameObject.SetActive(true); });

        Globals.Global.View.OnMapCampClick.Subscribe(() =>
        {
            _upgradeUI.gameObject.SetActive(true);
            _onMapUI.gameObject.SetActive(false);
        });

        Globals.Global.View.ReturnToMap.Subscribe(() =>
        {
            _onMapUI.gameObject.SetActive(true);
            _upgradeUI.gameObject.SetActive(false);
            _debugBattleUI.gameObject.SetActive(false);
        });
        
        Globals.Global.View.OnEndGame.Subscribe(() =>
        {
            _debugBattleUI.gameObject.SetActive(false);
            _upgradeUI.gameObject.SetActive(false);
            _onMapUI.gameObject.SetActive(false);
            _mainMenuUI.gameObject.SetActive(false);
            _endGameUI.gameObject.SetActive(true);
        });
    }
}