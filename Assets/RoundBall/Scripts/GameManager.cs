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
    public Text AttemptText;
	public Text HighscoreText;
	bool pause=false;
    List<GameObject> ActiveSpikes = new List<GameObject>();
    float PlatformRadius;
    public GameObject Tube;
    public Material Select;
    public Material Unselect;
    public GameObject spike3d;
    public GameObject dead;
    public GameObject WallHit;

    string[] selectSection;
    private int colSect = 3;
    private int countAttempt = 3;

    void Awake ()
    {
        Instance = this;     
	}

    void Start()
    {
		//get value of highscore and show on highscore text.
		HighscoreText.text = "Score " + Score.ToString ();
        AttemptText.text = "Attempt " + Attempt.ToString();

        Vector3 cubePos = cube.transform.localPosition;
        Vector3 centerPos = transform.localPosition;
        PlatformRadius = Mathf.Sqrt( Mathf.Pow(cubePos.x - centerPos.x, 2) + Mathf.Pow(cubePos.y - centerPos.y, 2) );

        //PlatformRadius = cube.transform.position.y - transform.position.y;
       
        //CreatePoint();
    }

    // Is called when the point is Hit
    public void HitPoint()
    {   //Add 1 score and display it.
        Score += 1;
        HighscoreText.text = Score.ToString();

        isWin();
        //start createpoint function.
        CreatePoint();
    }
    void UnselectedSection()
    {
        if (selectSection != null)
        for (int i = 0; i < selectSection.Length; i++)
        { 
            Tube.transform.Find(selectSection[i]).GetComponent<MeshRenderer>().material = Unselect;
            Tube.transform.Find(selectSection[i]).tag = "Tube";
        }
    }
    void isWin()
    {
        if (Score == 5)
        {
            StartCoroutine(WinGame());
        }
    }
    IEnumerator WinGame()
    {
        FindObjectOfType<PlatformController>().WinGame();
        yield return new WaitForSeconds(1.5f);
        //FindObjectOfType<SceneManagement.GameScene>().doWin();
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
        //FindObjectOfType<SceneManagement.GameScene>().doFail();
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

        HighscoreText.text = "Score " + Score.ToString();
        AttemptText.text = "Attempt " + Attempt.ToString();
        yield return new WaitForSeconds(1.5f);///
        Ball.GetComponent<BallController>().Launch();
    }
    // This creates the Point
    void CreatePoint()
	{

        //GameObject Point = ObjectPool.Instance.GetObjectForType("point", false);
        //Point.transform.parent = PlatformController.Instance.transform;
        //float radius = Random.Range(0.3f, 2f);   
        //float angle = Random.Range(0, 360);
        //angle *= Mathf.PI / 180;
        //Point.transform.position = new Vector3(radius * Mathf.Sin(angle), Mathf.Cos(angle));

        //создадим новый сектор, в который нужно попасть мячем

        UnselectedSection();
        selectSection = new string[colSect];

        int numSection = Random.Range(1, Tube.transform.childCount-(colSect-1));
        for (int i = numSection, k = 0; i < numSection + colSect; i++,k++)
        {
            if (i <= Tube.transform.childCount)
            {
                selectSection[k] = "Cube" + i;
                MeshRenderer meshRenderer = Tube.transform.Find("Cube" + i).GetComponent<MeshRenderer>();
                Tube.transform.Find("Cube" + i).tag = "Select";
                meshRenderer.material = Select;
            }
            else
            {
                selectSection[k] = "Cube" + i;
                MeshRenderer meshRenderer = Tube.transform.Find("Cube" + (i - Tube.transform.childCount)).GetComponent<MeshRenderer>();
                Tube.transform.Find("Cube" + (i - Tube.transform.childCount)).tag = "Select";
                meshRenderer.material = Select;
            }
        }
    }

    // Is called when the ball hits the boundary
    public void HittheBoundary()
    {        
        StartCoroutine(SpikeSession());
        //SpikeSession();
    }
		
    // Hides the present Spikes and brings a new one
    //void SpikeSession()
    IEnumerator SpikeSession()
    {   //this hides all the active spikes.
        if (ActiveSpikes.Count > 0)
        {
            foreach (GameObject g in ActiveSpikes)
                g.SendMessage("HideSpikes");
                //Destroy(g);

            ActiveSpikes.Clear();

            yield return new WaitForSeconds(0.05f);
        }
        //num_spikes is the number of spikes to be created.you can use your own algorithm.
        int num_spikes = 5 + (Score / 3);
        //int num_spikes = 0;
        float currentAngle = 0;
        float max_delta = 360 / num_spikes;
        float min_delta = (360 * 0.5f) / (2 * Mathf.PI * PlatformRadius);
        
        //Loop for creating spikes.
        for (int i = 0; i < num_spikes; i++)
        {
            //GameObject sp = Instantiate(spike3d);
            GameObject sp = ObjectPool.Instance.GetObjectForType("spike3d", false);

            sp.transform.parent = PlatformController.Instance.transform;
            float angle = Random.Range(min_delta + (max_delta - min_delta) / 2, max_delta);
            currentAngle += angle;
            sp.transform.localRotation = Quaternion.Euler(0, 0, 0 - currentAngle) ; //* transform.rotation
            float rad = currentAngle * Mathf.PI / 180;
            var scale = transform.parent.localScale.y * transform.parent.parent.localScale.y;
            var localScale = new Vector3(sp.transform.localScale.x * scale, sp.transform.localScale.y * scale, sp.transform.localScale.z * scale);
            sp.transform.localScale = localScale;
                var spikeOffset = sp.transform.localScale.y * scale * 40;
            sp.transform.localPosition = new Vector3(   (PlatformRadius - spikeOffset) * Mathf.Sin(rad), 
                                                        (PlatformRadius - spikeOffset) * Mathf.Cos(rad), 
                                                        Ball.transform.localPosition.z
                                                    ) + transform.localPosition;
            //* transform.parent.localScale.y* transform.parent.localScale.y
            //sp.transform.localScale = new Vector3(sp.transform.localScale.x , sp.transform.localScale.y , sp.transform.localScale.z );
            ActiveSpikes.Add(sp);
            sp.SendMessage("ShowSpikes");
        }
    }

    // This function shows all the spikes around on launching game.
    public IEnumerator ShowFullSpikes()
    {
		float circum = Mathf.PI * 2 * PlatformRadius;
        int numSpikes = (int)(circum / 0.05f); // 0.5f is the base-length of the spike
        float angleStep = 360f / numSpikes;     

		//Loop for creating all the spikes around the platform.
        for (int i = 0; i < numSpikes; i++)
        {
            //GameObject sp = Instantiate(spike3d);
            GameObject sp = ObjectPool.Instance.GetObjectForType("spike3d", false);

            sp.transform.parent = PlatformController.Instance.transform;
            float angle = i * angleStep;
            sp.transform.localRotation = Quaternion.Euler(0, 0, 0 - angle);
            angle *= Mathf.PI / 180;
            var scale = transform.parent.localScale.y * transform.parent.parent.localScale.y;
            sp.SendMessage("SaveScale");
            var localScale = new Vector3(sp.transform.localScale.x * scale, sp.transform.localScale.y * scale, sp.transform.localScale.z * scale);
            sp.transform.localScale = localScale;
            var spikeOffset = sp.transform.localScale.y * scale * 40    ;
			sp.transform.localPosition = new Vector3(   (PlatformRadius - spikeOffset) * Mathf.Sin(angle), 
                                                        (PlatformRadius - spikeOffset) * Mathf.Cos(angle), 
                                                        Ball.transform.localPosition.z
                                                    ) + transform.localPosition;
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
