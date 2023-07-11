using Game.Battle.Skills;
using RedBjorn.ProtoTiles;

namespace DefaultNamespace
{
    public class View
    {
        public readonly SubscribableAction<EncounterType> OnMapButtonClick = new();
        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();
        
        public SubscribableAction<SkillCommandHandler> OnCommandSelect = new ();
        public SubscribableAction OnRestartBattle =  new ();
        public SubscribableField<MapSettings> ActiveBattle = new(null);
        public void Init()
        {
 
        }
        
    }
}