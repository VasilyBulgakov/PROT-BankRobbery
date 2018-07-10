using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FreakyArchitect;

public class UIController : MonoBehaviour
{
    public static event System.Action WatchToContinueConfirmed = delegate{};

    public GameController gameController;
    public GameObject cubePrefab;
    public Text totalCubes;
    public Text height;
    public Text count;
    public Text speed;
    public Text random;
    public Text follow;
    public Text wall;
    public GameObject heightArea;
    public GameObject gameOverUI;

    void OnEnable()
    {
        GameController.NewGameEvent += OnNewGameEvent;
        ScoreManager.HeightUpdated += OnHeightUpdated;
    }

    void OnDisable()
    {
        GameController.NewGameEvent -= OnNewGameEvent;
        ScoreManager.HeightUpdated -= OnHeightUpdated;
    }

    // Use this for initialization
    void Start()
    {
        gameOverUI.SetActive(true);
    }
	
    // Update is called once per frame
    void Update()
    {       
        totalCubes.text = ScoreManager.Instance.Score.ToString();
        height.text = ScoreManager.Instance.Height.ToString();
        /// bestTotalCubes.text = ScoreManager.Instance.HighScore.ToString();
        //.text = ScoreManager.Instance.BestHeight.ToString();
        //nextLevelHeight.text = gameController.nextLevelStoreys.ToString();
        if (gameOverUI.activeSelf)
        {
            count.text = gameController.cubeCount.ToString();
            speed.text = gameController.speed.ToString();
            if (gameController.isRandom)
            {
                random.color = count.color;
                follow.color = new Color(0.5f, 0.5f, 0.5f);
            }
            else
            {
                random.color = new Color(0.5f, 0.5f, 0.5f);
                follow.color = count.color;
            }
            if (gameController.wall.activeInHierarchy)
            {
                wall.color = count.color;
            }
            else
            {
                wall.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    void OnHeightUpdated(int newHeight)
    {
        // Play anim
        heightArea.GetComponent<Animator>().Play("ReachNewHeight");
    }

    void OnNewGameEvent(GameEvent newEvent)
    {
        if (newEvent == GameEvent.START)
        {
            gameOverUI.SetActive(false);
        }
        else if (newEvent == GameEvent.GAMEOVER)
        {
            StartCoroutine(CRShowGameOverUI());
        }
        else if (newEvent == GameEvent.WINGAME)
        {
            StartCoroutine(CRShowWinGameUI());
        }
    }

    IEnumerator CRShowGameOverUI(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        // Update demo cube
        //demoCube.transform.position = demoCubePos.transform.position;
        //UpdateDemoCube();

        // Show buttons
        gameOverUI.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        //demoCube.SetActive(true);
    }

    IEnumerator CRShowWinGameUI(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        // Update demo cube
        //demoCube.transform.position = demoCubePos.transform.position;
        //UpdateDemoCube();

        // Show buttons
        gameOverUI.SetActive(true);

        yield return new WaitForSeconds(0.3f);
    }
}
