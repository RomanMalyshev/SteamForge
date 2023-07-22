using DefaultNamespace;
using RedBjorn.ProtoTiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeEncounter : Encounter
{   


    public override void Activate()
    {
        Globals.Global.View.OnMapTradeClick.Invoke();
    }
}
