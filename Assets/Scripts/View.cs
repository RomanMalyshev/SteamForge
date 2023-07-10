using Game.Battle.Skills;

namespace DefaultNamespace
{
    public class View
    {
        public readonly SubscribableAction<EncounterType> OnMapButtonClick = new();
        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();
        
        public SubscribableAction<SkillCommandHandler> OnCommandSelect = new ();
        public SubscribableAction OnRestartBattle =  new ();

        public void Init()
        {
 
        }
        
    }
}