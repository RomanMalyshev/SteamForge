﻿using DefaultNamespace.Player;
using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace DefaultNamespace
{
    public class View
    {        
        public SubscribableAction<MapSettings> OnMapBattleClick = new();
        public SubscribableAction OnMapTradeClick = new();
        public SubscribableAction OnMapCampClick = new();
        public SubscribableAction<Encounter> OnEncounterClick = new();

        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();
        
        public SubscribableAction<SkillCommandHandler> OnCommandSelect = new ();
        public SubscribableAction OnRestartBattle =  new ();
        public SubscribableField<MapSettings> ActiveBattle = new(null);

        public SubscribableAction<Transform> OnCameraTargetSelect = new();
        
        public SubscribableAction<Unit> OnUnitInBattleSelect = new();

        public SubscribableAction ReturnToMap = new();
        public void Init()
        {
 
        }
        
    }
}