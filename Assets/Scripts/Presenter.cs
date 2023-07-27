using Game.Battle;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace DefaultNamespace
{
    public class Presenter
    {
        private int cashAction;

        public void Init()
        {
            Globals.Global.View.OnMapBattleClick.Subscribe(encounter => { Debug.Log($"{encounter} - on click!"); });

            Globals.Global.View.OnMapTradeClick.Subscribe(() => { Debug.Log($"Camp - on click!"); });

            Globals.Global.View.OnExitFromGame.Subscribe(count => { Debug.Log($"{count} - on exit!"); });

            Globals.Global.View.OnEnterGame.Subscribe(() => { Debug.Log("Enter game"); });


            Globals.Global.Model.OnBattleEnd.Subscribe(side =>
            {
                if (side != UnitSide.Player) return;

                var party = Globals.Global.Model.Plyer.Value.Party;
                var isHumanEnemy = Globals.Global.View.ActiveBattle.Value.isHumanEnemy;
                Globals.Global.Model.Plyer.Value.Moral += isHumanEnemy ? -1 : +1;
                Globals.Global.Model.ChangedPlayerExp.Value -= 100;
                Globals.Global.Model.ChangedPlayerMoral.Value = isHumanEnemy ? -1 : +1;
                
                
                foreach (var character in party)
                {
                    character.Exp += 100;
                    if (character.Exp >= 100)
                    {
                        character.Level += 1;
                        character.Exp = 0;
                        character.UpPoints += 10;
                        Debug.Log($"{character.Name} Level UP!now level {character.Level}");
                    }
                }

                Globals.Global.Model.Plyer.Invoke(Globals.Global.Model.Plyer.Value);
            });
            
            Globals.Global.Model.Plyer.Subscribe(player =>
            {
                if (player != null)
                {
                    var playerJson = JsonUtility.ToJson(player);
                    PlayerPrefs.SetString("player",playerJson);
                }
            } );
        }
    }
}