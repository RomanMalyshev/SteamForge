using UnityEngine;

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
        
        public void RunTest()
        {
            Globals.Global.View.OnEnterGame.Invoke();
            Globals.Global.View.OnMapButtonClick.Value = EncounterType.trade;
            Globals.Global.View.OnExitFromGame.Invoke(10);
        }
    }
}