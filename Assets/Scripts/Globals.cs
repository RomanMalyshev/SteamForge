using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Globals : MonoBehaviour
    {
        public static Globals Global = null;
        public Model Model = new ();
        public View View = new ();
        public Presenter Presenter = new ();

        public void Awake()
        {
            if (!ReferenceEquals(Global, null))
            {
                Debug.LogWarning("It's another Globals!");
                return;
            }

            Global = this;
        }

        private void Start()
        {
            
            View.Init();
            Model.Init();
            Presenter.Init();           
        }
    }
}