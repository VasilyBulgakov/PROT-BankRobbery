using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class RBGameState : GameState
    {
        public RBGameState() { }

        public RBGameState(Transform tr) 
            : base(tr)
        {
            position = new Vector3(tr.position.x, Camera.main.transform.position.y - 0.5f * tr.localScale.y, tr.position.z);
            //rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x - 90, tr.eulerAngles.y, tr.eulerAngles.z);
        }

        public void UpdateState(RBGameState another)
        {
            base.UpdateState(another);
            //rotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x + 180, another.eulerAngles.y, another.eulerAngles.z);

            position = new Vector3(position.x, Camera.main.transform.position.y, position.z);

        }
    }
}