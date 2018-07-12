using SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour, ICallableScene {
    public Transform FAStartPoint;

    private MainState _state = new MainState();

  

    private void Awake()
    {
       
    }

    void Update() {
    }


    public void launchRB(Transform tr) {        
        tr.gameObject.SetActive(false); // here launch loading animation

        FindObjectOfType<ScenesManager>().LoadGame(
            HackGame.round_ball,
            new RBGameState(tr)
        );
    }
    public void launchPuzzle(Transform tr) {        
        tr.gameObject.SetActive(false); // here launch loading animation

        FindObjectOfType<ScenesManager>().LoadGame(
            HackGame.puzzle,
            new PuzzleGameState(tr)
        );
    }
    public void launchMonkey(Transform tr) {        
        tr.gameObject.SetActive(false); // here launch loading animation

        FindObjectOfType<ScenesManager>().LoadGame(
            HackGame.monkey,
            new MonkeyGameState(tr)
        );
    }

    public void launchGlobe(Transform tr) {        
        tr.gameObject.SetActive(false); // here launch loading animation

        FindObjectOfType<ScenesManager>().LoadGame(
            HackGame.globe,
            new GlobeGameState(tr)
        );
    }
    public void launchDisk(Transform tr) {        
        tr.gameObject.SetActive(false); // here launch loading animation

        FindObjectOfType<ScenesManager>().LoadGame(
            HackGame.disk,
            new DiskGameState(tr)
        );
    }

    public SceneState GetState() {
        return _state;
    }

    public void ReturnFromSceneWithState(SceneState st) {
        Debug.Log("returned from game with state: " + st.ToString());

        var state = st as GameState;
        if (state != null) {

            // Interface.InterfaceManager interfaceManager = FindObjectOfType<Interface.InterfaceManager>();
            // //Отправляем интерфейсу результат взлома
            // if(interfaceManager != null)
            //     interfaceManager.SetHackingGameState(state.gameResult);
            

            if (state.gameResult == GameResult.succeeded)
                Debug.Log("game succeed");          
            else if (state.gameResult == GameResult.failed)
                Debug.Log("gameFailed");
            else if (state.gameResult == GameResult.cancelled)
                Debug.Log("gameCancelled");

            state.callerButton.gameObject.SetActive(true);
        }
    }

    public void SetState(SceneState st) {
        var state = st as MainState;
        _state.UpdateState(state);
    }
}
