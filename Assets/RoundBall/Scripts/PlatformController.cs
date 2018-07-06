using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    public float rotateSpeed = 90;
    public static PlatformController Instance;

    float deltaAngle = 0;
    float prevAngle = 0;

    [SerializeField]
    private Texture[] emissionTexture;

    [SerializeField] private Transform _circle;
    private SpriteRenderer loadCircle;
    void Awake()
    {
        loadCircle = transform.Find("Circle0").GetComponent<SpriteRenderer>();
        Instance = this;
        loadCircle.material.SetTexture("_EmissionMap", emissionTexture[0]);
    }
    public void FailGame()
    {
        _circle.GetComponent<SpriteRenderer>().color = new Color(231f / 255f, 127f / 255f, 127f/255f);
    }
    public void WinGame()
    {
        _circle.GetComponent<SpriteRenderer>().color = new Color(120 / 255f, 197 / 255f, 140 / 255f);
    }
    public void StartFlashingLock(int hit)
    {
        try
        {
            loadCircle.material.SetTexture("_EmissionMap", emissionTexture[hit]);
        }
        catch(System.IndexOutOfRangeException e)
        {
            loadCircle.material.SetTexture("_EmissionMap", emissionTexture[emissionTexture.Length - 1]);
        }
        StartCoroutine(FlashingLock());
    }
    public void HitPoint(int hit)
    {
        
    }
    private IEnumerator FlashingLock()
    {
        int i = 0;
        while (i < 10) 
        {
            _circle.localScale += 0.005f * Vector3.one;
            yield return null;
            i++;
        }
        i = 0;
        while (i < 5)
        {
            _circle.localScale -= 0.01f * Vector3.one;
            yield return null;
            i++;
        }
        yield return null;
    }

    private void Update ()
    {

#if !UNITY_EDITOR  
        deltaAngle = Camera.main.transform.eulerAngles.z - prevAngle;

        if (Camera.main.transform.eulerAngles.z > 3 && Camera.main.transform.eulerAngles.z < 180)
        {
            if (deltaAngle < 0)
            {
                deltaAngle = 0;
            }
        }
        else
        {
            if (Camera.main.transform.eulerAngles.z < 357 && Camera.main.transform.eulerAngles.z >= 180)
            {
                if (deltaAngle > 0)
                {
                    deltaAngle = 0;
                }
            }
            else
            {
                deltaAngle = 0;
            }
        }

        transform.Rotate(0, 0, deltaAngle * rotateSpeed / 50);
        prevAngle = Camera.main.transform.eulerAngles.z;
#else
        float x = Input.GetAxis("Horizontal");
        if (x > 0)
        {
            // Rotate Right
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotateSpeed));
        }
        else if (x < 0)
        {
            // Rotate Left
            transform.Rotate(new Vector3(0, 0, -Time.deltaTime * rotateSpeed));
        }     
#endif

	}
}
