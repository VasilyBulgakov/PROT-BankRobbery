namespace SceneManagement {
    interface ICallableScene {
        SceneState GetState();
        void SetState(SceneState st);
        void ReturnFromSceneWithState(SceneState st);
    }
}
