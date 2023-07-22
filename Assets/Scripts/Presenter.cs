using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace DefaultNamespace
{
    public class Presenter
    {
        private int cashAction;
        public void Init()
        {
            Globals.Global.View.OnMapButtonClick.Subscribe(encounter =>
            {
                Debug.Log($"{encounter} - on click!");
            });

            Globals.Global.View.OnMapBattleClick.Subscribe(encounter =>
            {
                Debug.Log($"{encounter} - on click!");
            });

            Globals.Global.View.OnMapTradeClick.Subscribe(() =>
            {
                Debug.Log($"Camp - on click!");
            });

            Globals.Global.View.OnExitFromGame.Subscribe(count =>
            {
                Debug.Log($"{count} - on exit!");
            });

            Globals.Global.View.OnEnterGame.Subscribe(() =>
            {
                Debug.Log("Enter game");
            });
        }        
    }
}