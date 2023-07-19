using System.Collections.Generic;
using DefaultNamespace.Player;
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

        public SubscribableField<Player.Player> Plyer = new();

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
                        UpPoints = 100,
                        Level = 1,                           
                    },
                    new Character()
                    {
                        Name = "Bug",
                        UpPoints = 90,
                        Level = 1,
                    },
                    new Character()
                    {
                        Name = "Caster",
                        UpPoints = 80,
                        Level = 1,
                    },
                    new Character()
                    {
                        Name = "SteamMachine",
                        UpPoints = 70,
                        Level = 1,
                    },
                }
            };
        }
    }
}