using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameEvent
{
    START,
    PAUSED,
    GAMEOVER,
    WINGAME
}

public class GameController : MonoBehaviour
{
    public static event System.Action<GameEvent> NewGameEvent = delegate {};

    public GameObject cubePrefab;
    public GameObject firstCubePrefab;
    public GameObject firstCube;
    public GameObject targetCube;
    public GameObject wall;

    public UIController uiController;
    public float cubeRandomizeInterval = 10;
    [Range(0f, 1f)]
    public float intervalDecrementFactor = 0.5f;
    public CubeType currentType;
    public int speed = 5;

    public bool isWinGame = false;
    public bool isRandom = true;
    public int cubeCount = 10;
    GameObject randomizedCube;
    Vector3 initialFirstCubePos;
    bool touchDisabled = true;
    bool hasFallen = false;
    float maxY;
    int cubeNum = 10;
    int storey = 1;
    bool firstTouch = true;
    GameObject lastCube;
    IEnumerator randomCubeCoroutine;
    Vector3 lastLocalFreePos = Vector3.zero;
    Rigidbody rigidBody;
    List<Mesh> prevMeshes = new List<Mesh>();
    int maxStoredMeshes = 20;
    bool isGameStart = true;
    // Use this for initialization
    void Start()
    {
        // Setting things up
        currentType.material.DisableKeyword("_EMISSION");
        initialFirstCubePos = firstCube.transform.position;
        lastCube = firstCube;
        FreakyArchitect.ScoreManager.Instance.Reset();
        FreakyArchitect.ScoreManager.Instance.SetScore(cubeNum);
        FreakyArchitect.ScoreManager.Instance.SetHeight(storey);
        maxY = initialFirstCubePos.y;
        rigidBody = GetComponent<Rigidbody>();

        rigidBody.isKinematic = true;
        rigidBody.AddForce(Vector3.zero);
        Renderer rndCube = targetCube.GetComponent<Renderer>();
        rndCube.material.DisableKeyword("_EMISSION");
        storey = 1;
        FreakyArchitect.ScoreManager.Instance.SetHeight(storey);
        
        //if (firstCube)
        //    Destroy(firstCube);
    }
 
    public void SetupFinishPosition(Vector3 pos) {
        targetCube.transform.position = transform.position + pos;
    }

    IEnumerator StartGameAfter(float sec) {
        yield return new WaitForSeconds(sec);        
        StartGame();
    }
    void StartGame()
    {                
        if (firstCube)
            Destroy(firstCube);

        Debug.Log("Game start");
        cubeNum = cubeCount;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        firstCube = Instantiate(firstCubePrefab, transform) as GameObject;
        
        currentType.material.DisableKeyword("_EMISSION");
        lastLocalFreePos = Vector3.zero;

        lastCube = firstCube;
        FreakyArchitect.ScoreManager.Instance.Reset();
        FreakyArchitect.ScoreManager.Instance.SetScore(cubeNum);
        FreakyArchitect.ScoreManager.Instance.SetHeight(storey);
        maxY = initialFirstCubePos.y;
       
        rigidBody.isKinematic = true;
        rigidBody.AddForce(Vector3.zero);

        Renderer rndCube = targetCube.GetComponent<Renderer>();
        rndCube.material.DisableKeyword("_EMISSION");

        storey = 1;
        FreakyArchitect.ScoreManager.Instance.SetHeight(storey);
           
        // Start!
        StartRandomizeCube();
    }
    void OnEnable()
    {
        StartCoroutine(StartGameAfter(1.5f));
    }

    void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {       
        rigidBody.constraints = RigidbodyConstraints.FreezePositionX;
        rigidBody.constraints = RigidbodyConstraints.None;
        
        if (!hasFallen)
        {
            if (!isGameStart && cubeNum > 0)
            {
                isGameStart = true;
                isWinGame = false;
                StartGame();
            }
            else if (CheckFalling())   // check game over based on position change
            {
                hasFallen = true;
                PreGameOver();
            }
            else if (Input.GetMouseButtonDown(0) && !touchDisabled)
            {                
                StartCoroutine(TouchHandler());
            }
        }

        if (isGameStart && cubeNum == 0)
        {
            PreGameOver();
        }

        if (transform.position.y < -100 && rigidBody != null)
        {
            Destroy(rigidBody);
            rigidBody = null;
        }
    }
    public void EnableWall()
    {
        if (!wall.activeInHierarchy)
        {
            wall.SetActive(true);
        }
        else
        {
            wall.SetActive(false);
        }
    }
    public void RandomYes()
    {
        isRandom = true;
    }
    public void RandomNo()
    {
        isRandom = false;
    }
    public void CubeCountIncrement()
    {
        if (cubeCount < 30)
        {
            cubeCount++;
        }

    }
    public void CubeCountDecrement()
    {
        if (cubeCount > 10)
        {
            cubeCount--;
        }
    }
    public void SpeedIncrement()
    {
        if (speed < 7)
        {
            speed++;
        }

    }
    public void SpeedDecrement()
    {
        if (speed > 1)
        {
            speed--;
        }
    }
    // Determine if the structure is starting to fall
    bool CheckFalling()
    {
        
        float x, z;
        if (firstCube.transform.rotation.eulerAngles.z > 180)
        {
            z = 360 - firstCube.transform.rotation.eulerAngles.z;
        }
        else
        {
            z = firstCube.transform.rotation.eulerAngles.z;
        }
        if (firstCube.transform.rotation.eulerAngles.x > 180)
        {
            x = 360 - firstCube.transform.rotation.eulerAngles.x;
        }
        else
        {
            x = firstCube.transform.rotation.eulerAngles.x;
        }

        var rotVector = new Vector2(x, z);

        //var rotVector = new Vector2(firstCube.transform.rotation.eulerAngles.x, firstCube.transform.rotation.eulerAngles.z);

        //return (rotVector.magnitude) >= 0.05f;//0.015f
        return (firstCube.transform.position.y - initialFirstCubePos.y) >= 0.05f || (rotVector.magnitude) >= 0.5f;//       
    }

    void StartRandomizeCube()
    {
        randomCubeCoroutine = RandomizeCube();
        StartCoroutine(randomCubeCoroutine);
    }

    void StopRandomizeCube()
    {
        StopCoroutine(randomCubeCoroutine);
    }

    IEnumerator RandomizeCube()
    {
        // Find free positions
        RaycastHit hit;

        Transform lastCubeTf = lastCube.transform;
        var cubeExtent = lastCube.GetComponent<BoxCollider>().bounds.extents.z;
        float maxRay = cubeExtent * 3;
                   
        Ray rayBack = new Ray(lastCubeTf.position, Vector3.back);
        Ray rayForward = new Ray(lastCubeTf.position, Vector3.forward);
        Ray rayUp = new Ray(lastCubeTf.position, Vector3.up);
        Ray rayDown = new Ray(lastCubeTf.position, Vector3.down);
        Ray rayLeft = new Ray(lastCubeTf.position, Vector3.left);
        Ray rayRight = new Ray(lastCubeTf.position, Vector3.right);

        bool hitBack = (Physics.Raycast(rayBack, out hit, maxRay));
        if (hit.transform != null && hit.transform.name.Contains("TargetCube"))
        {
            isWinGame = true;
        }

        bool hitForward = (Physics.Raycast(rayForward, out hit, maxRay));
        if (hit.transform != null && hit.transform.name.Contains("TargetCube"))
        {
            isWinGame = true;
        }

        bool hitUp = (Physics.Raycast(rayUp, out hit, maxRay));
        if (hit.transform != null && hit.transform.name.Contains("TargetCube"))
        {
            isWinGame = true;
        };

        bool hitDown = (Physics.Raycast(rayDown, out hit, maxRay));
        if (hit.transform != null && hit.transform.name.Contains("TargetCube"))
        {
            isWinGame = true;
        };

        bool hitLeft = (Physics.Raycast(rayLeft, out hit, maxRay));
        if (hit.transform != null && hit.transform.name.Contains("TargetCube"))
        {
            isWinGame = true;
        };

        bool hitRight = (Physics.Raycast(rayRight, out hit, maxRay));
        if (hit.transform != null && hit.transform.name.Contains("TargetCube"))
        {
            isWinGame = true;
        };

        if (!isWinGame)
        {
            // Create a list of free positions in lastCube local space
            List<Vector3> localFreePos = new List<Vector3>(6);

            if (!hitBack || firstCube.transform.childCount == 0)
            {
                localFreePos.Add(Vector3.back);
            }
            if (!hitForward)
            {
                localFreePos.Add(Vector3.forward);
            }
            if (!hitUp)
            {
                localFreePos.Add(Vector3.up);
            }
            if (!hitDown && !(lastCubeTf.TransformPoint(Vector3.down).y < firstCube.transform.position.y - 0.1f) && firstCube.transform.childCount >0)   // not going downward!
            {
                localFreePos.Add(Vector3.down);
            }
            if (!hitLeft)
            {
                localFreePos.Add(Vector3.left);
            }
            if (!hitRight)
            {
                localFreePos.Add(Vector3.right);
            }

            if (localFreePos.Count > 0)
            {
                touchDisabled = false;

                // Create cube
                randomizedCube = (GameObject)Instantiate(cubePrefab, Vector3.zero, Quaternion.identity, transform);
                randomizedCube.GetComponent<BoxCollider>().enabled = false;    // temporarily disable collider so it won't mess with previous cubes
                Renderer meshRdr = randomizedCube.GetComponent<Renderer>();
                meshRdr.material = currentType.material;
                meshRdr.material.DisableKeyword("_EMISSION");

                int lastIndex = 0;

                while (!CheckFalling())
                {
                    // Move cube through random positions
                    if (isRandom)
                    {
                        int randomIndex = Random.Range(0, localFreePos.Count);

                        while (localFreePos.Count > 1 && randomIndex == lastIndex)
                        {
                            randomIndex = Random.Range(0, localFreePos.Count);  // no repeat random index
                        }

                        lastLocalFreePos = localFreePos[randomIndex];
                        randomizedCube.transform.position = lastCubeTf.TransformPoint(lastLocalFreePos);
                        randomizedCube.transform.rotation = lastCubeTf.rotation;
                        lastIndex = randomIndex;

                    }
                    else
                    {
                        if (lastIndex < localFreePos.Count)
                        {
                            lastLocalFreePos = localFreePos[lastIndex];
                            randomizedCube.transform.position = lastCubeTf.TransformPoint(lastLocalFreePos);
                            randomizedCube.transform.rotation = lastCubeTf.rotation;
                            lastIndex++;
                            if (lastIndex == localFreePos.Count)
                            {
                                lastIndex = 0;
                            }
                        
                        }

                        
                    }

                    yield return new WaitForSeconds(cubeRandomizeInterval / speed);
                }
            }
            else
            {
                // No more way to go!
                PreGameOver();
                yield break;
            }
        }
        else
        {
            PreWinGame();
            yield break;
        }
    }

    void PreWinGame()
    {
        touchDisabled = true;
        //isGameStart = false;
        //firstTouch = true;

        Renderer rndCube = targetCube.GetComponent<Renderer>();
        rndCube.material.EnableKeyword("_EMISSION");

        StopRandomizeCube();

        if (randomizedCube)
            Destroy(randomizedCube);

        FreakyArchitect.SoundManager.Instance.PlaySound(FreakyArchitect.SoundManager.Instance.levelUp);
        
        Invoke("WinGame", 1f);
        
    }

    void WinGame()
    {
        Debug.Log("WIN!!!");

        // Fire event
        NewGameEvent(GameEvent.WINGAME);
    }

    void PreGameOver()
    {
        touchDisabled = true;
        isGameStart = false;
        firstTouch = true;

        StopRandomizeCube();
        FreakyArchitect.ScoreManager.Instance.SetScore(cubeNum);
        Renderer rend = firstCube.GetComponent<Renderer>();

        if (cubeNum != 0)
        {
            rend.material.DisableKeyword("_EMISSION");

            Renderer[] cldRends = firstCube.GetComponentsInChildren<Renderer>();
            foreach (Renderer rnd in cldRends)
            {
                rnd.material.DisableKeyword("_EMISSION");
            }

            foreach (Cube cube in firstCube.GetComponentsInChildren<Cube>())
            {
                cube.StartParticle(currentType.particleEffect, 0.5f);
            }
        }

        if (randomizedCube)
            Destroy(randomizedCube);

        FreakyArchitect.SoundManager.Instance.PlaySound(FreakyArchitect.SoundManager.Instance.gameOver);
        
        Invoke("GameOver", 1f);
    }

    void GameOver()
    {
        Debug.Log("GAME OVER!!!");

        // Fire event
        NewGameEvent(GameEvent.GAMEOVER);
    }

    public void RestartGame()
    {
        for (int i = firstCube.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(firstCube.transform.GetChild(i).gameObject);
        }
        cubeNum = 5;

        if (firstCube)
            Destroy(firstCube);

        hasFallen = false;
        NewGameEvent(GameEvent.START);

        isGameStart = false;
    }

    IEnumerator TouchHandler()
    {
        
        touchDisabled = true;

        if (firstTouch)
        {
            firstTouch = false;
            //rigidBody.isKinematic = false;
            // Fire event
            NewGameEvent(GameEvent.START);
        }
        
        // Stop last coroutine
        StopRandomizeCube();
        
        // Create new reference for the cube in case game over occurs newCube won't be destroyed
        GameObject newCube = randomizedCube;
        randomizedCube = null;
        Cube cube = newCube.GetComponent<Cube>();
        cube.particle = currentType.particleEffect;
        // Adjust position
        newCube.transform.position = lastCube.transform.TransformPoint(lastLocalFreePos);
        newCube.transform.rotation = lastCube.transform.rotation;

        // Enable necessary component
        Renderer newCubeRdr = newCube.GetComponent<Renderer>();

        newCubeRdr.material.EnableKeyword("_EMISSION");

        newCube.GetComponent<BoxCollider>().enabled = true;     // activate collider
        newCube.gameObject.transform.SetParent(firstCube.transform);    // add it to the current structure to form the whole big physic body
        
        
        //decrease counter
        cubeNum--;

        if (cubeNum == 0)
        {
            yield break;
        }

        // Set cube order
        newCube.GetComponent<Cube>().orderInStructure = cubeNum - 1;

        // Update last cube pointer
        lastCube = newCube;

        // Store meshes data for restoration purpose, but don't store too many
        Mesh prevMesh = firstCube.GetComponent<MeshFilter>().mesh;

        if (prevMeshes.Count >= maxStoredMeshes)
        {
            for (int i = 0; i < prevMeshes.Count - 1; i++)
            {
                prevMeshes[i] = prevMeshes[i + 1];
            }

            prevMeshes[prevMeshes.Count - 1] = prevMesh;    // add the latest mesh
        }
        else
        {
            prevMeshes.Add(prevMesh);
        }

        yield return null;
        
        // Play particle
        newCube.GetComponent<Cube>().StartParticle(currentType.particleEffect, 1);

        yield return null;

        // Check if the structure reaches new height
        if (newCube.transform.position.y > maxY + 0.5f)
        {
            maxY = newCube.transform.position.y;

            // Update height
            storey++;
            FreakyArchitect.ScoreManager.Instance.SetHeight(storey);

            FreakyArchitect.SoundManager.Instance.PlaySound(FreakyArchitect.SoundManager.Instance.placeCubeUp);
        }
        else
        {
            FreakyArchitect.SoundManager.Instance.PlaySound(FreakyArchitect.SoundManager.Instance.placeCubeUp);
        }

        // Update scores
        FreakyArchitect.ScoreManager.Instance.SetScore(cubeNum);
        
        // Wait and then repeat the whole process
        yield return new WaitForSeconds(0.5f);

        if (!CheckFalling())
        {
            StartRandomizeCube();
        }   
       
    }
}
