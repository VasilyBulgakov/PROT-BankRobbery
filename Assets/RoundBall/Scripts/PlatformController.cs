using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    public float rotateSpeed = 90;
    public static PlatformController Instance;

    float deltaAngle = 0;
    float prevAngle = 0;

    [SerializeField] private Transform _circle;

    void Awake()
    {
        Instance = this;
    }
    public void FailGame()
    {
        _circle.GetComponent<SpriteRenderer>().color = new Color(231f / 255f, 127f / 255f, 127f/255f);
    }
    public void WinGame()
    {
        _circle.GetComponent<SpriteRenderer>().color = new Color(120 / 255f, 197 / 255f, 140 / 255f);
    }

    void Update () {

        // Check if there are any touches
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

        int sighDelta = 1;

        if (Camera.main.transform.eulerAngles.y < 240 && Camera.main.transform.eulerAngles.y >= 90) sighDelta = -1;

        transform.Rotate(0, 0, sighDelta * deltaAngle * rotateSpeed / 50);
        prevAngle = Camera.main.transform.eulerAngles.z;
#else
        {
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
        }
#endif
   
	}
}
