//-----------------------------------------------------------------------
// <copyright file="SceneSwitcher.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement {
    /// <summary>
    /// Script that displays a scene switching UI.
    /// </summary>
    public class ScenesManager : MonoBehaviour {
        public bool debug = false;
        /// <summary>
        /// A delegate callback fired before a scene is loaded by SceneSwitcher.
        /// </summary>
        public System.Action<string> m_onBeforeLoadScene;
        public System.Action<string> m_onAfterLoadScene;
        private const int SCENE_BUTTON_SIZE_X = 300;
        private const int SCENE_BUTTON_SIZE_Y = 65;
        private const int SCENE_BUTTON_GAP_X = 5;
        private const int SCENE_BUTTON_GAP_Y = 3;

        /// <summary>
        /// The names of all the scenes this can switch between.
        /// </summary>
        //[SerializeField] private string[] m_sceneNames = {
        //    "MainScene",
        //    "Room1",
        //    "Room2"
        //};

        /// <summary>
        /// The Unity awake method.
        /// </summary>
        private void Awake() {
            // Assure there is only ever one active scene switcher.
            if (FindObjectsOfType<ScenesManager>().Length > 1) {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Unity start method.
        /// </summary>
        private void Start() {
            DontDestroyOnLoad(this);
        
            QualitySettings.vSyncCount = 2;
            Application.targetFrameRate = 30;
        }

        private void Update() {
            // if (Input.GetKey(KeyCode.Escape)) {
            //     if ((ProjectScenes)SceneManager.GetActiveScene().buildIndex == ProjectScenes.main) {
            //         // This is a fix for a lifecycle issue where calling
            //         // Application.Quit() here, and restarting the application
            //         // immediately results in a deadlocked app.
            //         AndroidHelper.AndroidQuit();
            //     } else {
            //         ReturnScene();
            //     }
            // }
        }

        /// <summary>
        /// Scene switching GUI.
        /// </summary>
        private void OnGUI() {
            if (debug) {
                var scenesCount = SceneManager.sceneCountInBuildSettings;
                for (int it = 0; it < scenesCount; ++it) {
                    Rect buttonRect = new Rect(Screen.width - SCENE_BUTTON_GAP_X - SCENE_BUTTON_SIZE_X,
                                               SCENE_BUTTON_GAP_Y + ((SCENE_BUTTON_GAP_Y + SCENE_BUTTON_SIZE_Y) * it),
                                               SCENE_BUTTON_SIZE_X,
                                               SCENE_BUTTON_SIZE_Y);
#pragma warning disable 618
                    var sceneName = SceneManager.GetSceneByBuildIndex(it).name;
                    if (GUI.Button(buttonRect, "<size=20>" + sceneName + "</size>")
                        && Application.loadedLevelName != sceneName) {
                        if (m_onBeforeLoadScene != null) {
                            m_onBeforeLoadScene(sceneName);
                        }

                        Application.LoadLevel(sceneName);

                    }
#pragma warning restore 618
                }
            }
        }

        // private ILogger logger = Debug.logger;

        // List<ProjectScenes> scenesStack = new List<ProjectScenes>();
        // List<SceneState> statesStack = new List<SceneState>();
        // public void RunScene (ProjectScenes scene, SceneState state = null) {
        //     var sceneToLoad = scene.sceneIndex();

        //     var currentSceneState = (GameObject.Find("SceneCallbacks") as GameObject).GetComponent<ICallableScene>().GetState();
        //     var curScene = SceneManager.GetActiveScene().name;

        //     UnityEngine.Events.UnityAction<Scene, Scene> loadCallback = null;
        //     loadCallback = (Scene prev, Scene cur) => {
        //         logger.Log("SceneManagement", "activeSceneChanged callback " + curScene + " -> " + sceneToLoad.ToString(), this);
        //         logger.Log("SceneManagement", "prev: " + prev.name, this);
        //         logger.Log("SceneManagement", "cur: " + cur.name, this);

        //         var newSceneCallable = GameObject.Find("SceneCallbacks").GetComponent<ICallableScene>();
        //         newSceneCallable.SetState(state == null ? null : state);
        //         SceneManager.activeSceneChanged -= loadCallback;
        //     };
        //     SceneManager.activeSceneChanged += loadCallback;

        //     scenesStack.Add((ProjectScenes)SceneManager.GetActiveScene().buildIndex);
        //     statesStack.Add(currentSceneState);
        //     SceneManager.LoadScene(scene.sceneIndex());
        // }

        // public void ReturnScene () {
        //     var sceneToLoad = scenesStack[scenesStack.Count-1]; scenesStack.RemoveAt(scenesStack.Count - 1);
        //     var stateToLoad = statesStack[statesStack.Count-1]; statesStack.RemoveAt(statesStack.Count - 1);

        //     var currentSceneState = (GameObject.Find("SceneCallbacks") as GameObject).GetComponent<ICallableScene>().GetState();
        //     var curScene = SceneManager.GetActiveScene().name;

        //     UnityEngine.Events.UnityAction<Scene, Scene> loadCallback = null;
        //     loadCallback = (Scene prev, Scene cur) => {
        //         logger.Log("SceneManagement", "activeSceneChanged callback " + curScene + " -> " + sceneToLoad.ToString());
        //         logger.Log("SceneManagement", "prev: " + prev.name, this);
        //         logger.Log("SceneManagement", "cur: " + cur.name, this);

        //         var newSceneCallable = GameObject.Find("SceneCallbacks").GetComponent<ICallableScene>();
        //         newSceneCallable.SetState(stateToLoad);
        //         newSceneCallable.ReturnFromSceneWithState(currentSceneState);
        //         SceneManager.activeSceneChanged -= loadCallback;
        //     };
        //     SceneManager.activeSceneChanged += loadCallback;

        //     SceneManager.LoadScene(sceneToLoad.sceneIndex());
        // }

        public void LoadGame (HackGame game, SceneState state = null) {
            var currentSceneState = (GameObject.Find("SceneCallbacks") as GameObject).GetComponent<ICallableScene>().GetState();
            var currentScene = SceneManager.GetActiveScene();

            var gameObject = GameProvider.Instance.GetGame(game);//Instantiate(lazorsGame) as GameObject;
            
            var newSceneCallable = gameObject.GetComponent<ICallableScene>();
            newSceneCallable.SetState(state);

            //SceneManager.SetActiveScene(currentScene);
        }

        public void UnloadGame (GameObject gameGO) {

            var gameSceneCallable = gameGO.GetComponent<ICallableScene>();
            var gameState = gameSceneCallable.GetState();

            Destroy(gameGO);

            var currentScene = SceneManager.GetActiveScene();
            var currentSceneCallable = currentScene.GetRootGameObjects()
                .First(go => go.name == "SceneCallbacks")
                .GetComponent<ICallableScene>();
            //sceneCallable.SetState(stateToLoad);
            currentSceneCallable.ReturnFromSceneWithState(gameState);
        }
    }
}
