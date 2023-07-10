using System.Collections.Generic;
using Game.Battle.Skills;

namespace DefaultNamespace
{
    public class Model
    {
        public SubscribableAction<int> OnNewBattleRound = new ();
        public SubscribableAction<string,List<SkillCommandHandler>> OnNewUnitTern = new ();
        public SubscribableAction<int> OnChangeUnitActionPoints = new ();
        public void Init()
        {

        }
    }
}