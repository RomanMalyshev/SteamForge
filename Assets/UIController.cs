using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Battle;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private DebugBattleUI _debugBattleUI;
    [SerializeField] private PartyUpgrade _upgradeUI;


    private void Start()
    {
        Globals.Global.View.ActiveBattle.Subscribe(map => { _debugBattleUI.gameObject.SetActive(true); });

        Globals.Global.Model.OnBattleEnd.Subscribe(map =>
        {
            if (map == UnitSide.Player)
                _debugBattleUI.gameObject.SetActive(false);
        });
        
        Globals.Global.View.OnMapCampClick.Subscribe(() =>{
            _upgradeUI.gameObject.SetActive(true);
        });
        
        Globals.Global.View.ReturnToMap.Subscribe(() =>{
            _upgradeUI.gameObject.SetActive(false);
            _debugBattleUI.gameObject.SetActive(false);
        });
        
    }
}