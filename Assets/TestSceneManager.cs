using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TestSceneManager : MonoBehaviour {
	public UnityEngine.UI.Button buttonPrefab;

	public GameObject contentContainer;

	// Use this for initialization


	// Use this for initialization
	void Start () {
		makeButtons();

		DontDestroyOnLoad(gameObject);
	}


	

	private void makeButtons()
	{
		RectTransform thisTr = GetComponent<RectTransform>();
		float btnHeight = buttonPrefab.GetComponent<RectTransform>().sizeDelta.y;
		
		int sceneCount = SceneManager.sceneCountInBuildSettings;
		for(int i =0; i < sceneCount; i++)
		{
			var btn = GameObject.Instantiate(buttonPrefab);	
			var btnTr = btn.GetComponent<RectTransform>();

			btnTr.parent = contentContainer.transform;	
			btnTr.localPosition = new Vector3(0, -50 - 1.1f * i * btnHeight, 0);

			string path = SceneUtility.GetScenePathByBuildIndex(i);
			btn.GetComponentInChildren<UnityEngine.UI.Text>().text = path.Substring(path.LastIndexOf("/")+1);						
		}		
	}


	
	public void restartCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
