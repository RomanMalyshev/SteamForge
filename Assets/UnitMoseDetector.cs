using DefaultNamespace;
using DefaultNamespace.Player;
using UnityEngine;

public class UnitMoseDetector : MonoBehaviour
{
    private Unit _unit;
    private View _view;
    
   public void Init(Unit unit)
   {
       _view = Globals.Global.View;
       _unit = unit;
   }

   private void OnMouseOver()
   {
       _view.mouseOverUnit.Invoke(_unit);
   }
   
   private void OnMouseExit()
   {
       _view.mouseOverUnit.Invoke(null);
   }
}
