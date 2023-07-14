using DefaultNamespace;
using RedBjorn.ProtoTiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEncounter : Encounter
{
    [SerializeField] private MapSettings _battleground;

    public override void EncounterSelected()
    {
        Globals.Global.View.OnMapBattleClick.Invoke(_battleground);
    }
}

