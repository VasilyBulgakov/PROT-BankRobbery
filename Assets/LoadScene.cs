﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	const string SCENES_FOLDER = "Assets/Scenes";

	// Use this for initialization
	void Start () {
		GetComponent<UnityEngine.UI.Button>().onClick.AddListener(load);
	}

	private void load()
	{
		string path = SCENES_FOLDER + "/" + GetComponentInChildren<UnityEngine.UI.Text>().text;
		
		int index = SceneUtility.GetBuildIndexByScenePath(path);
		SceneManager.LoadScene(index);
		//SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
	}

}
