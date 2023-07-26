using System;
using System.Collections.Generic;

namespace DefaultNamespace.Player
{
    [Serializable]
    public class Player
    {
        public int PeopleMoral;
        public int Moral;
        public int Currency;
        public int Gears;
        public List<Character> Party;
    }
}