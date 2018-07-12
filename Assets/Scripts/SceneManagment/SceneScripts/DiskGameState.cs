using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class DiskGameState : GameState
    {
        public DiskGameState() { }

        public DiskGameState(Transform tr) 
            : base(tr)
        {
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        }

        public void UpdateState(GlobeGameState another)
        {
            base.UpdateState(another);
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        }
    }
}