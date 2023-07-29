using System.Collections.Generic;
using DefaultNamespace.Player;
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
                    if (character.Exp >= character.NextLevelExp)
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
                    PlayerPrefs.SetString("player", playerJson);
                }
            });

            if (PlayerPrefs.HasKey("player"))
            {
                Globals.Global.Model.Plyer.Value = JsonUtility.FromJson<Player.Player>(PlayerPrefs.GetString("player"));
            }
            else
            {
                Globals.Global.Model.Plyer.Value = new Player.Player()
                {
                    Currency = 1000,
                    Gears = 10000,
                    Party = new List<Character>()
                    {
                        new Character()
                        {
                            Name = "Tank",
                            Level = 1,
                            Hull = { Value = 40 },
                            Strength = { Value = 40 },
                            Agile = { Value = 40 },
                            Speed = { Value = 40 },
                            Reaction = { Value = 40 },
                        },
                        new Character()
                        {
                            Name = "Bug",
                            Level = 1,
                            Hull = { Value = 40},
                            Strength = { Value = 40 },
                            Agile = { Value = 40 },
                            Speed = { Value = 40 },
                            Reaction = { Value = 40 },
                        },
                        new Character()
                        {
                            Name = "Caster",
                            Level = 1,
                            Hull = { Value = 40 },
                            Strength = { Value = 40 },
                            Agile = { Value = 40 },
                            Speed = { Value = 40 },
                            Reaction = { Value = 40 },
                        },
                        new Character()
                        {
                            Name = "SteamMachine",
                            Level = 1,
                            Hull = { Value = 40 },
                            Strength = { Value = 40 },
                            Agile = { Value = 40 },
                            Speed = { Value = 40 },
                            Reaction = { Value = 40 },
                        },
                    }
                };
            }
        }
    }
}