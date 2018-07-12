using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProvider : MonoBehaviour {
    private static GameProvider _instance;
    
    public static GameProvider Instance {
        get {
            if (_instance == null) {
                var newInstance = new GameObject("GameProvider");
                var component = newInstance.AddComponent<GameProvider>();
                _instance = component;
            }
            return _instance;
        }

        set {
            _instance = value;
        }
    }

    // Use this for initialization
    void Awake () {
        Debug.Log("game provider awakened");
		if (_instance == null) {
            _instance = this;
        } else {
            if (_instance != this) {
                Destroy(this.gameObject);
            }
        }
	}

    public GameObject GetGame(HackGame gameType) {
        var prefab = Resources.Load(gameType.prefabPath()) as GameObject;
        var gameObject = Instantiate(prefab) as GameObject;
        return gameObject;
    }
}
