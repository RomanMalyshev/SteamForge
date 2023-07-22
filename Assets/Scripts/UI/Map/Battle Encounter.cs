using DefaultNamespace;
using RedBjorn.ProtoTiles;
using UnityEngine;

public class BattleEncounter : Encounter
{
    [SerializeField] private MapSettings _battleground;

    public override void Activate()
    {
        Globals.Global.View.ActiveBattle.Value = _battleground;
    }
}

