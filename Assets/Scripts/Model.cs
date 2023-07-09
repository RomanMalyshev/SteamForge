using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class Model
    {
        public SubscribableAction<int> OnNewBattleRound = new ();
        public SubscribableAction<string> OnNewUnitTern = new ();
        public SubscribableAction<int> OnChangeUnitActionPoints = new ();
        public void Init()
        {

        }
    }
}