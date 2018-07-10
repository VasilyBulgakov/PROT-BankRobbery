using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    Rigidbody rb;
    public static bool Alive = true;
	public AudioSource Point;
	public AudioSource Wall;
	public AudioSource GameOver;
    public GameObject WallHit;
    public GameObject Dead;

    private float _minTimeBetweenTouches = 0.2f;

    private float _timeLastTouch;

    /// <summary>
    /// количество совершенных касаний
    /// </summary>
    private int touch = 0;

    int AudioEnabled;
    
    Vector3 lastForce;
    Vector3 initPos;
    int boost = 60; 
    void Start()
    {
		AudioEnabled = PlayerPrefs.GetInt ("Audio",1);//get the saved audio value, 1 being enabled and 0 not enabled.
        rb = GetComponent<Rigidbody>();
        initPos = rb.transform.localPosition;
        var scale = transform.parent.localScale.x * transform.parent.parent.localScale.x;
        boost = (int)(boost * scale);
        GetComponent<TrailRenderer>().widthMultiplier = GetComponent<TrailRenderer>().widthMultiplier * transform.parent.localScale.x;
        StartCoroutine(Launch());

        var trail = GetComponent<TrailRenderer>();
        trail.widthMultiplier = 0.07f * scale; // trail.widthMultiplier * scale;
    }
    private void Update()
    {
        //защита отповторного срабатывания OnCollisionEnter
        //бывают случаи, когда он срабатывает несколько раз подряд
    }
    public IEnumerator Launch()
    {
		//starting the game
        Alive = true;
        rb.transform.localPosition = initPos;
        touch = 0;
        yield return new WaitForSeconds(0.1f);

        //fisrt show all spikes animation
        StartCoroutine(GameManager.Instance.ShowFullSpikes());
		//after 1 second move the ball with less speed.
        yield return new WaitForSeconds(0.8f);
        var scale = transform.parent.localScale.x * transform.parent.parent.localScale.x;
        rb.AddForce(-rb.transform.up * scale);
		//after 1 more second increase ball speeed
        yield return new WaitForSeconds(0.1f);
        lastForce = -rb.transform.up * 10 * scale;
        rb.AddForce(lastForce);

        _timeLastTouch = Time.time;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (Time.time - _timeLastTouch > _minTimeBetweenTouches)
        {
            switch (coll.gameObject.tag)
            {   
                case "Select":
                    _timeLastTouch = Time.time;
                    touch++;

                    //if Audio is enabled play wall hit sound.
                    if (AudioEnabled == 1) Wall.Play();

                    //добавление силы после отскока
                    Debug.Log(coll.transform.parent.name + " " + coll.transform.name + " " + touch);
                    rb.AddForce(-lastForce);
                    lastForce = coll.transform.up * rb.transform.localPosition.magnitude * (boost + touch);
                    rb.AddForce(lastForce);
                    rb.transform.up = -coll.transform.up;

                    GameManager.Instance.HitPoint();
                    GameManager.Instance.HittheBoundary();
                    if (AudioEnabled == 1) Point.Play();//if Audio is enabled play Point collection sound.
                    break;

                //on hitting the boundry of platform
                case "Tube":
                    //start HittheBoundry command of gamemanager.
                    GameManager.Instance.HittheBoundary();

                    _timeLastTouch = Time.time;
                    touch++;

                    //if Audio is enabled play wall hit sound.
                    if (AudioEnabled == 1) Wall.Play();

                    //добавление силы после отскока         
                    Debug.Log(coll.transform.parent.name + " " + coll.transform.name + " " + touch);
                    rb.AddForce(-lastForce);
                    lastForce = coll.transform.up * rb.transform.localPosition.magnitude * (boost + touch);
                    rb.AddForce(lastForce);
                    rb.transform.up = -coll.transform.up;
                    break;
                case "Spike":
                    _timeLastTouch = Time.time;

                    //if Audio is enabled play Game over sound.
                    if (AudioEnabled == 1) GameOver.Play();
                   
                    Destroy(Instantiate(Dead, this.transform.position, Quaternion.identity, this.transform.parent), 4);

                    //start GameOver command of gamemanager and destroy the ball.
                    GameManager.Instance.isLose();
                    break;
            }
        }
    }
}
