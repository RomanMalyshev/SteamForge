using DefaultNamespace;

public class CampEncounter : Encounter
{
    
    public override void Activate()
    {
        Globals.Global.View.OnMapCampClick.Invoke();
        EncounterState = EncounterState.Visited;
    }
}
