using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class RBGameScene : GameScene
    {

        private new RBGameState _state = new RBGameState();

        public override SceneState GetState()
        {
            return _state;
        }

        public override void SetState(SceneState st)
        {
            if (st != null)
            {
                var state = st as RBGameState;
                _state.UpdateState(state);

                mSceneObject.transform.position = _state.position;
                mSceneObject.transform.rotation = _state.rotation;
                mSceneObject.transform.localScale = _state.scale;

            }
        }

        public new void doWin()
        {
            Debug.Log("I win");
            _state.gameResult = GameResult.succeeded;
            returnToCaller();
        }

        public new void doFail()
        {
            _state.gameResult = GameResult.failed;
            returnToCaller();
        }

        private void Update()
        {
            mSceneObject = this.transform;
            var distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            //this.DebugLog("distance to game: " + distance.ToString());
            if (distance > 5)
                doCancel();
        }
    }
}