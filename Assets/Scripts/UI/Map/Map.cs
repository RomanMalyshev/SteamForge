using System;
using Unity.VisualScripting;

namespace UI.Map
{
    //Example 
    public class Map
    {
        public enum typeOfEvent
        {
            battle,
            rest
        }
    }

    public class MapButton
    {
        public Action<GameEvent> _onButtonClick;
    }
}