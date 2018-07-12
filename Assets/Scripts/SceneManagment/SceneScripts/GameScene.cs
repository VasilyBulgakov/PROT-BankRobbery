using UnityEngine;

namespace SceneManagement {
    public class GameScene : MonoBehaviour, ICallableScene {
        public Transform mSceneObject;

        protected GameState _state = new GameState();

        public virtual SceneState GetState() {
            return _state;
        }

        public void ReturnFromSceneWithState(SceneState st) {

        }

        public virtual void SetState(SceneState st) {
            if (st != null) {
                var state = st as GameState;
                _state.UpdateState(state);

                mSceneObject.transform.position = _state.position;
                mSceneObject.transform.rotation = _state.rotation;
                mSceneObject.transform.localScale = _state.scale;
            }
        }

        public void doWin() {
            Debug.Log("I win");
            _state.gameResult = GameResult.succeeded;
            returnToCaller();
        }

        public void doFail() {
            _state.gameResult = GameResult.failed;
            returnToCaller();
        }

        public void doCancel() {
            //_state.gameResult = GameResult.cancelled;
            //returnToCaller();
        }

        protected void returnToCaller() {
            FindObjectOfType<ScenesManager>().UnloadGame(gameObject);
        }

        private void Update() {
            var distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            //this.DebugLog("distance to game: " + distance.ToString());
            if (distance > 10)
                doCancel();
        }
    }
}

// INTERFACE FOR GAMES:
//FindObjectOfType<GameScene>().doWin();
//FindObjectOfType<GameScene>().doFail();
