using System.Collections.Generic;
using DefaultNamespace.Player;
using Game.Battle;
using Game.Battle.Skills;
using UnityEngine;

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

           

          
        }
    }
}