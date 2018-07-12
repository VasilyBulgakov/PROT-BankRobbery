using UnityEngine;

namespace SceneManagement {
    public enum GameResult {
        notFinished,
        succeeded,
        failed,
        cancelled,
    }


    public class GameState : SceneState {
        public GameResult gameResult = GameResult.notFinished;

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public Transform callerButton;

        public GameState() { }

        public GameState(Transform tr) {
            position = tr.position;
            rotation = tr.rotation;
            scale = tr.localScale;

            callerButton = tr;
        }

        public void UpdateState(GameState another) {
            gameResult = another.gameResult;
            position = another.position;
            rotation = another.rotation;
            scale = another.scale;

            callerButton = another.callerButton;
        }
    }
}
