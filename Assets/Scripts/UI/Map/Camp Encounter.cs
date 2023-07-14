using DefaultNamespace;
using RedBjorn.ProtoTiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampEncounter : Encounter
{
    [SerializeField] private MapSettings _camp;

    public override void EncounterSelected()
    {
        Globals.Global.View.OnMapCampClick.Invoke(_camp);
    }

}
