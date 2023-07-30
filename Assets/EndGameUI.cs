using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public Button NewGame;
    public TMP_Text Result;

    private const string DefeatPeople =
        "Your robots reveled in their triumphant rule, believing their firm control would ensure progress and order. Despite the cost of freedom, they remained resolute in their conviction for a brighter steampunk future.";

    private const string DefeatRobots =
        "Your robots defended humanity, defeating malevolent steampunk robots. Their triumph brought unity and hope, leaving a legacy of compassion and progress.";

    public void Init()
    {
        Globals.Global.Model.Plyer.Subscribe(player =>
        {
            Result.text = player.Moral < 0 ? DefeatPeople : DefeatRobots;
        });
        
        NewGame.onClick.AddListener(Globals.Global.View.OnNewGame.Invoke);
    }
}