using DefaultNamespace;

public class TradeEncounter : Encounter
{   
    public override void Activate()
    {
        Globals.Global.View.OnMapTradeClick.Invoke();
        EncounterState = EncounterState.Visited;
    }
}
