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
    //переменная для дебага
    int touch = 0;
    //********************

    int AudioEnabled;
    int ticker = 0;
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
        switch (ticker)
        {
            case 0:
                break;
            case 20:
                ticker = 0;
                break;
            default:
                ticker++;
                break;
        }
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
    }


    //void OnCollisionEnter(Collision coll)
    //{
    //    if (coll.gameObject.tag == "Select" && ticker == 0)
    //    {   //start HittheBoundry command of gamemanager.
    //        GameManager.Instance.HittheBoundary();
    //        ticker++;
    //        touch++;

    //        //if Audio is enabled play wall hit sound.
    //        if (AudioEnabled == 1) Wall.Play();
    //        //create the wall hit effect.
    //        //GameObject particl = ObjectPool.Instance.GetObjectForType("WallHit", false);
    //        //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 0.5f));
    //        //добавление силы после отскока
    //        //coll.
    //        //Debug.Log(coll.transform.position.ToString());
    //        //Debug.Log(rb.transform.position.ToString());
    //        //Debug.Log("vect " + new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0));
    //        //Debug.Log("mag " + Vector3.Magnitude(new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0)));

    //        Debug.Log(coll.transform.parent.name + " " + coll.transform.name + " " + touch);

    //        //rb.AddForce(new Vector3(0, 15, 0));
    //        //rb.AddForce(new Vector3(-coll.transform.position.x,  -coll.transform.position.y , 0),ForceMode.Impulse);
    //        rb.AddForce(-lastForce);//, ForceMode.Impulse
    //        lastForce = coll.transform.right * rb.transform.localPosition.magnitude * (boost + touch);
    //        rb.AddForce(lastForce);//, ForceMode.Impulse
    //                               //rb.ad
    //        GameManager.Instance.HitPoint();
    //        if (AudioEnabled == 1) Point.Play();//if Audio is enabled play Point collection sound.
    //    }
    //    else
    //    //on hitting the boundry of platform
    //    if (coll.gameObject.tag == "Tube" && ticker == 0)
    //    {   //start HittheBoundry command of gamemanager.
    //        GameManager.Instance.HittheBoundary();
    //        ticker++;
    //        touch++;

    //        //if Audio is enabled play wall hit sound.
    //        if (AudioEnabled == 1) Wall.Play();
    //        //create the wall hit effect.
    //        //GameObject particl = ObjectPool.Instance.GetObjectForType("WallHit", false);
    //        //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 0.5f));
    //        //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(WallHit, transform.position, 0.5f));
            
            
    //        //добавление силы после отскока
    //        //coll.
    //        //Debug.Log(coll.transform.position.ToString());
    //        //Debug.Log(rb.transform.position.ToString());
    //        //Debug.Log("vect " + new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0));
    //        //Debug.Log("mag " + Vector3.Magnitude(new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0)));

    //        Debug.Log(coll.transform.parent.name + " " + coll.transform.name + " " + touch);

    //        //rb.AddForce(new Vector3(0, 15, 0));
    //        //rb.AddForce(new Vector3(-coll.transform.position.x,  -coll.transform.position.y , 0),ForceMode.Impulse);
    //        rb.AddForce(-lastForce);//, ForceMode.Impulse
    //        lastForce = coll.transform.right * rb.transform.localPosition.magnitude * (boost + touch);
    //        rb.AddForce(lastForce);//, ForceMode.Impulse
    //        //rb.ad
    //    }
    //    else if(coll.gameObject.tag == "Spike" && ticker == 0)
    //    {
    //        ticker++;
    //        //if Audio is enabled play Game over sound.
    //        if (AudioEnabled == 1) GameOver.Play();
    //        //create the dead(exploding effect).
    //        GameObject particl = ObjectPool.Instance.GetObjectForType("Dead", false);
    //        GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 2));


    //        //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(Dead, transform.position, 2));
    //        //start GameOver command of gamemanager and destroy the ball.
    //        GameManager.Instance.isLose();
    //        //GameManager.Instance.StartCoroutine(GameManager.Instance.RestartGame());
    //        //Destroy(this.gameObject);
    //    }
    //}
    void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.tag == "Select" && ticker == 0)
        {   //start HittheBoundry command of gamemanager.
            GameManager.Instance.HittheBoundary();
            ticker++;
            touch++;

            //if Audio is enabled play wall hit sound.
            if (AudioEnabled == 1) Wall.Play();
            //create the wall hit effect.
            //GameObject particl = ObjectPool.Instance.GetObjectForType("WallHit", false);
            //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 0.5f));
            //добавление силы после отскока
            //coll.
            //Debug.Log(coll.transform.position.ToString());
            //Debug.Log(rb.transform.position.ToString());
            //Debug.Log("vect " + new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0));
            //Debug.Log("mag " + Vector3.Magnitude(new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0)));

            Debug.Log(coll.transform.parent.name + " " + coll.transform.name + " " + touch);

            //rb.AddForce(new Vector3(0, 15, 0));
            //rb.AddForce(new Vector3(-coll.transform.position.x,  -coll.transform.position.y , 0),ForceMode.Impulse);
            rb.AddForce(-lastForce);//, ForceMode.Impulse
            lastForce = coll.transform.right * rb.transform.localPosition.magnitude * (boost + touch);
            rb.AddForce(lastForce);//, ForceMode.Impulse
                                   //rb.ad
            GameManager.Instance.HitPoint();
            if (AudioEnabled == 1) Point.Play();//if Audio is enabled play Point collection sound.
        }
        else
        //on hitting the boundry of platform
        if (coll.gameObject.tag == "Tube" && ticker == 0)
        {   //start HittheBoundry command of gamemanager.
            GameManager.Instance.HittheBoundary();
            ticker++;
            touch++;

            //if Audio is enabled play wall hit sound.
            if (AudioEnabled == 1) Wall.Play();
            //create the wall hit effect.
            //GameObject particl = ObjectPool.Instance.GetObjectForType("WallHit", false);
            //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 0.5f));
            //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(WallHit, transform.position, 0.5f));


            //добавление силы после отскока
            //coll.
            //Debug.Log(coll.transform.position.ToString());
            //Debug.Log(rb.transform.position.ToString());
            //Debug.Log("vect " + new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0));
            //Debug.Log("mag " + Vector3.Magnitude(new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0)));

            Debug.Log(coll.transform.parent.name + " " + coll.transform.name + " " + touch);

            //rb.AddForce(new Vector3(0, 15, 0));
            //rb.AddForce(new Vector3(-coll.transform.position.x,  -coll.transform.position.y , 0),ForceMode.Impulse);
            rb.AddForce(-lastForce);//, ForceMode.Impulse
            lastForce = coll.transform.right * rb.transform.localPosition.magnitude * (boost + touch);
            rb.AddForce(lastForce);//, ForceMode.Impulse
            //rb.ad
        }
        else if (coll.gameObject.tag == "Spike" && ticker == 0)
        {
            ticker++;
            //if Audio is enabled play Game over sound.
            if (AudioEnabled == 1) GameOver.Play();
            //create the dead(exploding effect).
            GameObject particl = ObjectPool.Instance.GetObjectForType("Dead", false);
            particl.transform.parent = transform.parent;
            GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 2));


            //GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(Dead, transform.position, 2));
            //start GameOver command of gamemanager and destroy the ball.
            GameManager.Instance.isLose();
            //GameManager.Instance.StartCoroutine(GameManager.Instance.RestartGame());
            //Destroy(this.gameObject);
        }
        //     if (col.gameObject.tag == "Select" && ticker == 0)
        //     {   //start HittheBoundry command of gamemanager.
        //         GameManager.Instance.HittheBoundary();
        //         ticker++;
        //         touch++;

        //         //if Audio is enabled play wall hit sound.
        //         if (AudioEnabled == 1) Wall.Play();
        //         //create the wall hit effect.
        //         GameObject particl = ObjectPool.Instance.GetObjectForType("WallHit", false);
        //         GameManager.Instance.StartCoroutine(GameManager.Instance.StartParticleEffect(particl, transform.position, 0.5f));
        //         //добавление силы после отскока
        //         //coll.
        //         //Debug.Log(coll.transform.position.ToString());
        //         //Debug.Log(rb.transform.position.ToString());
        //         //Debug.Log("vect " + new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0));
        //         //Debug.Log("mag " + Vector3.Magnitude(new Vector3(-coll.transform.position.x * 50, -coll.transform.position.y * 50, 0)));

        //         Debug.Log(col.transform.parent.name + " " + col.transform.name + " " + touch);

        //         //rb.AddForce(new Vector3(0, 15, 0));
        //         //rb.AddForce(new Vector3(-coll.transform.position.x,  -coll.transform.position.y , 0),ForceMode.Impulse);
        //         rb.AddForce(-lastForce);//, ForceMode.Impulse
        //         lastForce = col.transform.right * rb.transform.localPosition.magnitude * (50 + touch);
        //         rb.AddForce(lastForce);//, ForceMode.Impulse
        //         //rb.ad
        //     }
        //     //on hitting the point, destroy point and start HitPoint command of gamemanager.
        //     if (col.gameObject.name == "point")
        //     {
        //         //Destroy(col.gameObject);
        //         GameManager.Instance.HitPoint();
        //if(AudioEnabled==1) Point.Play ();//if Audio is enabled play Point collection sound.
        //     }
    }
}
