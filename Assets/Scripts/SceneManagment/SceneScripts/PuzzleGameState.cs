using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class PuzzleGameState : GameState
    {
        public PuzzleGameState() { }

        public PuzzleGameState(Transform tr) 
            : base(tr)
        {
            
        }

        public void UpdateState(RBGameState another)
        {
            base.UpdateState(another);
           
        }
    }
}