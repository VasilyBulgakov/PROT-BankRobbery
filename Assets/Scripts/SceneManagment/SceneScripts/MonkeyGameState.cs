using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class MonkeyGameState : GameState
    {
        public MonkeyGameState() { }

        public MonkeyGameState(Transform tr) 
            : base(tr)
        {
            
        }

        public void UpdateState(RBGameState another)
        {
            base.UpdateState(another);
           
        }
    }
}