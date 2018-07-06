using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    public GameObject Ball;
	public GameObject newPoint;
    public GameObject cube;
    int Score = 0;
	int Attempt = 1;
	bool pause=false;

    List<GameObject> ActiveSpikes = new List<GameObject>();
    List<Transform> ActivePanel = new List<Transform>();
    List<GameObject> ActiveGreenEffect = new List<GameObject>();

    float PlatformRadius;
    public GameObject Tube;
    public Material Select;
    public Material Unselect;
    public Material Spike;
    public GameObject spike3d;
    public GameObject dead;
    public GameObject WallHit;
    public int NumberTouches;
    string[] selectSection;
    private int colSect = 3;
    private int countAttempt = 3;

    void Awake ()
    {
        Instance = this;     
	}

    void Start()
    {
        PlatformRadius = cube.transform.position.y - transform.position.y;
        //CreatePoint();
    }

    // Is called when the point is Hit
    public void HitPoint()
    {   //Add 1 score and display it.
        Score += 1;

        FindObjectOfType<PlatformController>().StartFlashingLock(Score);

        isWin();
        //start createpoint function.
        CreatePoint();
    }
    void UnselectedSection()
    {
        foreach(GameObject go in ActiveGreenEffect)
        {
            ObjectPool.Instance.PoolObject(go);
        }
        ActiveGreenEffect.Clear();

        if (selectSection != null)
        for (int i = 0; i < selectSection.Length; i++)
        {
            Tube.transform.Find(selectSection[i]).Find("Forward").GetComponent<MeshRenderer>().material = Unselect;
            //Tube.transform.Find(selectSection[i]).GetComponent<MeshRenderer>().material = Unselect;
            Tube.transform.Find(selectSection[i]).Find("Forward").tag = "Tube";
        }
    }
    void isWin()
    {
        if (Score == NumberTouches)
        {
            StartCoroutine(WinGame());
        }
    }
    IEnumerator WinGame()
    {
        FindObjectOfType<PlatformController>().WinGame();
        yield return new WaitForSeconds(1.5f);

        //FindObjectOfType<SceneManagement.RBGameScene>().doWin();
    }
    public void isLose()
    {
        //if (Attempt < countAttempt)
        //{
        //    StartCoroutine(RestartGame());
        //}
        //else
        //{
            StartCoroutine(FailGame());
        //}
        
    }
    IEnumerator FailGame()
    {
        //
        Destroy(Ball);
        yield return new WaitForSeconds(0.3f);
        FindObjectOfType<PlatformController>().FailGame();
        yield return new WaitForSeconds(1.5f);
        //FindObjectOfType<SceneManagement.RBGameScene>().doFail();
    }
    public IEnumerator RestartGame()
    {
        //yield return new WaitForSeconds(0.3f);
        foreach (GameObject g in ActiveSpikes)
            g.SendMessage("HideSpikes");
        ActiveSpikes.Clear();

        Attempt++;

        Score = 0;
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1.5f);///
        Ball.GetComponent<BallController>().Launch();
    }

    // This creates the Point
    void CreatePoint()
	{
        DeactiveSpikes();

        //создадим новый сектор, в который нужно попасть мячем

        UnselectedSection();
        selectSection = new string[colSect];

        int numSection = Random.Range(1, Tube.transform.childCount-(colSect-1));
        for (int i = numSection, k = 0; i < numSection + colSect; i++,k++)
        {
            selectSection[k] = "Panel" + i;
            Transform panel = Tube.transform.Find("Panel" + i).Find("Forward");
            panel.tag = "Select";
            panel.GetComponent<MeshRenderer>().material = Select;
            if (k == 1)
            {
                GameObject re = ObjectPool.Instance.GetObjectForType("greenEffect", false);

                re.transform.parent = PlatformController.Instance.transform;
                re.transform.position = panel.position;
                re.transform.rotation = panel.rotation;
                ActiveGreenEffect.Add(re);
            }
        }
    }

    // Is called when the ball hits the boundary
    public void HittheBoundary()
    {        
        StartCoroutine(SpikeSession());
        //SpikeSession();
    }
	private void DeactiveSpikes()
    {
        if (ActiveSpikes.Count > 0)
        {
            foreach (GameObject go in ActiveSpikes)
            {
                go.SendMessage("HideSpikes");
            }
            ActiveSpikes.Clear();
        }
        if (ActivePanel.Count > 0)
        {
            foreach (Transform tr in ActivePanel)
            {
                tr.tag = "Tube";
                tr.GetComponent<MeshRenderer>().material = Unselect;
            }
            ActivePanel.Clear();
        }
    }
    private bool NewWarningPanel(int numPanel)
    {
        Transform panel = Tube.transform.Find("Panel" + numPanel).Find("Forward");
        if (panel.tag != "Select" && panel.tag != "Spike")
        {
            panel.tag = "Spike";
            MeshRenderer meshRenderer = panel.GetComponent<MeshRenderer>();
            meshRenderer.material = Spike;
            ActivePanel.Add(panel);
            GameObject sp = ObjectPool.Instance.GetObjectForType("redEffect", false);

            sp.transform.parent = PlatformController.Instance.transform;
            sp.transform.position = panel.position;
            sp.transform.localRotation = Quaternion.Euler(0, 180, 0);
            sp.transform.rotation = panel.rotation;
            sp.SendMessage("ShowSpikes");
            ActiveSpikes.Add(sp);

            return true;
        }
        else
        {
            return false;
        }      
    }

    // Hides the present Spikes and brings a new one
    private IEnumerator SpikeSession()
    {  
        //this hides all the active spikes.
        DeactiveSpikes();
        
        int num_spikes = 5 + (Score / 3);
        int numFirstSection = 0;
        int countPanel = Tube.transform.childCount;

        while(true)
        {
            numFirstSection = Random.Range(1, countPanel + 1);
            if (NewWarningPanel(numFirstSection))
            {
                yield return null;
                break;
            }
        }

        int step = countPanel / num_spikes;

        for (int i = 1; i < num_spikes;)
        {
            Debug.Log((numFirstSection + (step * i) - (int)(step / 2)).ToString() + " " + (numFirstSection + (step * i) + (int)(step / 2) - ((step % 2 == 0 && step > 4) ? 1 : 0) - 1).ToString());
                                                                                                                       //classify = (input > 0) ? "positive" : "negative";  
            int numCurrentSection = Random.Range(numFirstSection + (step * i) - (int)(step / 2), numFirstSection +(step * i) + (int)(step / 2) - ((step % 2 == 0 && step > 4) ? 1 : 0));
            if(numCurrentSection > countPanel)
            {
                numCurrentSection -= countPanel;
            }
            if (NewWarningPanel(numCurrentSection))
            {
                yield return null;
                i++;
            }
            //selectSection[k] = "Cube" + i;
            //MeshRenderer meshRenderer = Tube.transform.Find("Cube" + i).GetComponent<MeshRenderer>();
            //Tube.transform.Find("Cube" + i).tag = "Select";          
        }

        yield return null;
    }
    //IEnumerator SpikeSession()
    //{   //this hides all the active spikes.
    //    if (ActiveSpikes.Count > 0)
    //    {
    //        foreach (GameObject g in ActiveSpikes)
    //            g.SendMessage("HideSpikes");
    //        //Destroy(g);

    //        ActiveSpikes.Clear();

    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    //num_spikes is the number of spikes to be created.you can use your own algorithm.
    //    int num_spikes = 5 + (Score / 3);

    //    float currentAngle = 0;
    //    float max_delta = 360 / num_spikes;
    //    float min_delta = (360 * 0.5f) / (2 * Mathf.PI * PlatformRadius);

    //    //Loop for creating spikes.
    //    for (int i = 0; i < num_spikes; i++)
    //    {
    //        //GameObject sp = Instantiate(spike3d);
    //        GameObject sp = ObjectPool.Instance.GetObjectForType("RedSpike", false);

    //        sp.transform.parent = PlatformController.Instance.transform;
    //        float angle = Random.Range(min_delta + (max_delta - min_delta) / 2, max_delta);
    //        currentAngle += angle;
    //        sp.transform.rotation = Quaternion.Euler(0, 0, 0 - currentAngle) * transform.rotation;
    //        float rad = currentAngle * Mathf.PI / 180;
    //        var scale = transform.parent.localScale.y * transform.parent.parent.localScale.y;
    //        var localScale = new Vector3(sp.transform.localScale.x * scale, sp.transform.localScale.y * scale, sp.transform.localScale.z * scale);
    //        sp.transform.localScale = localScale;
    //        var spikeOffset = sp.transform.localScale.y * scale / 7;
    //        //sp.transform.position = new Vector3((PlatformRadius - spikeOffset) * Mathf.Sin(rad), (PlatformRadius - spikeOffset) * Mathf.Cos(rad), Ball.transform.localPosition.z) + transform.position;
    //        sp.transform.position = new Vector3((PlatformRadius - spikeOffset) * Mathf.Sin(rad), (PlatformRadius - spikeOffset) * Mathf.Cos(rad), Ball.transform.localPosition.z / 2) + transform.position;
    //        //* transform.parent.localScale.y* transform.parent.localScale.y
    //        //sp.transform.localScale = new Vector3(sp.transform.localScale.x , sp.transform.localScale.y , sp.transform.localScale.z );
    //        ActiveSpikes.Add(sp);
    //        sp.SendMessage("ShowSpikes");
    //    }
    //}
    // This function shows all the spikes around on launching game.
    public IEnumerator ShowFullSpikes()
    {
        int numSpikes = 10;
        int step = Tube.transform.childCount / numSpikes;

        //Loop for creating all the spikes around the platform.
        for (int i = 0; i < numSpikes; i++)
        {
            NewWarningPanel(step * i + 1);
        }
        //After a second hiding all the spikes and then starting the spikes sessions.
        yield return new WaitForSeconds(0.5f);

        DeactiveSpikes();

        yield return new WaitForSeconds(0.1f);

        CreatePoint();
        StartCoroutine(SpikeSession());
        //SpikeSession();
    }

    /*
    public IEnumerator ShowFullSpikes()
    {
        float circum = Mathf.PI * 2 * PlatformRadius;
        //int numSpikes = (int)(circum / 0.02f); // 0.5f is the base-length of the spike
        int numSpikes = 10;
        float angleStep = 360f / numSpikes;

        //Loop for creating all the spikes around the platform.
        for (int i = 0; i < numSpikes; i++)
        {
            //GameObject sp = Instantiate(spike3d);
            GameObject sp = ObjectPool.Instance.GetObjectForType("redEffect", false);

            sp.transform.parent = PlatformController.Instance.transform;
            float angle = i * angleStep;
            sp.transform.localRotation = Quaternion.Euler(0, 0, 0 - angle);
            angle *= Mathf.PI / 180;
            var scale = transform.parent.localScale.y * transform.parent.parent.localScale.y;
            sp.SendMessage("SaveScale");
            var localScale = new Vector3(sp.transform.localScale.x * scale, sp.transform.localScale.y * scale, sp.transform.localScale.z * scale);
            sp.transform.localScale = localScale;
            var spikeOffset = sp.transform.localScale.y * scale / 7;
            //var spikeOffset = sp.transform.localScale.y * scale * 20;
            sp.transform.position = new Vector3((PlatformRadius) * Mathf.Sin(angle), (PlatformRadius) * Mathf.Cos(angle), Ball.transform.localPosition.z / 2) + transform.position;
            //sp.transform.localScale = new Vector3(sp.transform.localScale.x * transform.parent.localScale.x, sp.transform.localScale.y * transform.parent.localScale.y, sp.transform.localScale.z * transform.parent.localScale.y);
            ActiveSpikes.Add(sp);
            sp.SendMessage("ShowSpikes");
        }
        //After a second hiding all the spikes and then starting the spikes sessions.
        yield return new WaitForSeconds(0.5f);

        foreach (GameObject g in ActiveSpikes)
            g.SendMessage("HideSpikes");
        // Destroy(g);
        ActiveSpikes.Clear();

        yield return new WaitForSeconds(0.1f);

        CreatePoint();
        StartCoroutine(SpikeSession());
        //SpikeSession();
    }
    */

    //for creating particle effects(dead, wallhit). 
    public IEnumerator StartParticleEffect(GameObject obj, Vector3 pos, float sec_befrPool)
    {
        obj.transform.position = pos;
        yield return new WaitForSeconds(sec_befrPool);
        ObjectPool.Instance.PoolObject(obj);
    }
	//this is the function performed on game over
	public IEnumerator GameOver()
	{  //save the current score in playerprefs so it can be displayed in the next scene. 
		PlayerPrefs.SetInt ("Score", Score);
		//after 1 second begin the fading effect.
		yield return new WaitForSeconds(1);
		//float fadeTime = GameObject.Find ("Main Camera").GetComponent<Fading> ().BeginFade (1);
		//after the fade time load the menu scene
		//yield return new WaitForSeconds(fadeTime);
		//SceneManager.LoadScene("RoundBall_menu");
	}
	//function performed on pause button.
	public void PauseButton(){
		//if the game is already paused, set pause bool to false and time scale to normal.
		if (pause) {
			pause = false;
			Time.timeScale = 1;
		}//else set pause bool to true and stop time. 
		else {
			pause = true;
			Time.timeScale = 0;
		}
	}
    public void StartButton()
    {
        ///roundBall.SetActive(true);
    }
    public void StopButton()
    {
        //roundBall.SetActive(false);
    }
}
