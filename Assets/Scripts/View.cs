using Game.Battle.Skills;
using RedBjorn.ProtoTiles;
using UnityEngine;

namespace DefaultNamespace
{
    public class View
    {
        public SubscribableAction<EncounterType> OnMapButtonClick = new();
        public SubscribableAction<MapSettings> OnMapBattleClick = new();
        public SubscribableAction OnMapTradeClick = new();
        public SubscribableAction<MapSettings> OnMapCampClick = new();

        public SubscribableAction<int> OnExitFromGame = new();
        public SubscribableAction OnEnterGame = new();
        
        public SubscribableAction<SkillCommandHandler> OnCommandSelect = new ();
        public SubscribableAction OnRestartBattle =  new ();
        public SubscribableField<MapSettings> ActiveBattle = new(null);

        public SubscribableAction<Transform> OnCameraTargetSelect = new();
        public SubscribableAction<bool> OnCameraStateChange = new();
        public void Init()
        {
 
        }
        
    }
}