using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class GlobeGameState : GameState
    {
        public GlobeGameState() { }

        public GlobeGameState(Transform tr) 
            : base(tr)
        {
            
        }

        public void UpdateState(GlobeGameState another)
        {
            base.UpdateState(another);
           
        }
    }
}