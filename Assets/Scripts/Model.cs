using System.Collections.Generic;
using DefaultNamespace.Player;
using Game.Battle;
using Game.Battle.Skills;

namespace DefaultNamespace
{
    public class Model
    {
        public SubscribableAction<int> OnNewBattleRound = new();
        public SubscribableAction<Unit, List<SkillCommandHandler>> OnUnitStartTern = new();
        public SubscribableAction<Unit> OnUnitEndTern = new();
        public SubscribableAction<int> OnChangeUnitActionPoints = new();
        public SubscribableField<List<Unit>> UnitBattleOrder = new();
        public SubscribableAction<UnitSide> OnBattleEnd = new();
        public SubscribableAction<Unit> OnUnitHealthChange = new();

        public SubscribableField<Player.Player> Plyer = new();

        public SubscribableField<int> ChangedPlayerExp = new ();
        public SubscribableField<int> ChangedPlayerMoral = new ();


        public void Init()
        {
            Plyer.Value = new Player.Player()
            {
                Currency = 1000,
                Gears = 10000,
                Party = new List<Character>()
                {
                    new Character()
                    {
                        Name = "Tank",
                        Level = 1,                           
                    },
                    new Character()
                    {
                        Name = "Bug",
                        Level = 1,
                    },
                    new Character()
                    {
                        Name = "Caster",
                        Level = 1,
                    },
                    new Character()
                    {
                        Name = "SteamMachine",
                        Level = 1,
                    },
                }
            };
        }
    }
}