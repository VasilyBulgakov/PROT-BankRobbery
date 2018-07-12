using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockableInterface : MonoBehaviour {
    public InterfaceAnimManager activeState;
    public InterfaceAnimManager blockedState;

    public bool ActiveState = true;

	// Use this for initialization
	void Start () {
        updateState();
	}

    public void setStateBlocked() {
        activeState.startDisappear();
        blockedState.startAppear();
    }

    public void setStateActive() {
        activeState.startAppear();
        blockedState.startDisappear();
    }

    public void updateState() {
        if (ActiveState)
            setStateActive();
        else
            setStateBlocked();
    }
}
