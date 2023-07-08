using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace DefaultNamespace
{
    public class Presenter
    {
        public void Init()
        {
            Globals.Global.View.OnMapButtonClick.Subscribe(encounter =>
            {
                Debug.Log($"{encounter} - on click!");
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