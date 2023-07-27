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
                        Health = {Value = 100},
                        MoveRange = {Value = 2},
                        ActionPoints = {Value = 8},
                        Damage = {Value = 4},
                        AttackRange = {Value = 2},
                        Initiative = {Value = 1},
                    },
                    new Character()
                    {
                        Name = "Bug",
                        Level = 1,
                        Health = {Value = 90},
                        MoveRange = {Value = 3},
                        ActionPoints = {Value = 12},
                        Damage = {Value = 15},
                        AttackRange = {Value = 1},
                        Initiative = {Value = 1},
                    },
                    new Character()
                    {
                        Name = "Caster",
                        Level = 1,
                        Health = {Value = 150},
                        MoveRange = {Value = 4},
                        ActionPoints = {Value = 9},
                        Damage = {Value = 5},
                        AttackRange = {Value = 1},
                        Initiative = {Value = 0},
                    },
                    new Character()
                    {
                        Name = "SteamMachine",
                        Level = 1,
                        Health = {Value = 75},
                        MoveRange = {Value = 3},
                        ActionPoints = {Value = 18},
                        Damage = {Value = 25},
                        AttackRange = {Value = 1},
                        Initiative = {Value = 1},
                    },
                }
            };
        }
    }
}