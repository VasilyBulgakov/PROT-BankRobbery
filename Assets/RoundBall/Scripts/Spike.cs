using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {
    
    int anim_speed = 2;
    Vector3 initScale;
    public void SaveScale() {

        initScale = transform.localScale;
    }
    //public void ShowSpikes()
    //{   //Function is called from gamemanager to start showing spikes aniamtion.
    //    ShowHide(true);
    //}


    //public void HideSpikes()
    //{//Function is called from gamemanager to start hiding spikes aniamtion.
    //    //hide();
    //    Destroy(this);
    //}

    //void hide()
    //{
    //    ShowHide(false);
    //    ObjectPool.Instance.PoolObject(gameObject);
    //}

    //// This Handles Animation of spikes
    //void ShowHide(bool show)
    //{    //if show is true, animation of showing spikes starts.
    //    if (show)
    //    {
    //        ////initScale = transform.localScale;
    //        //Vector3 scale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
    //        //transform.localScale = scale;

    //        //float h = Mathf.Abs(initScale.y - scale.y) / anim_speed;

    //        //while (scale.y < initScale.y)
    //        //{
    //        //    scale.y += h;// anim_speed * Time.deltaTime/2;
    //        //    transform.localScale = scale;
    //        //yield return null;
    //        //}                        
    //    }//if show is false, animation of hiding spikes starts. 
    //    else
    //    {
    //        ////initScale = transform.localScale;
    //        //Vector3 scale = transform.localScale;
    //        //transform.localScale = scale;
    //        //float h = Mathf.Abs(initScale.y - 0) / anim_speed;

    //        //while (scale.y > 0)
    //        //{
    //        //    scale.y -= h;// anim_speed * Time.deltaTime/2;
    //        //    transform.localScale = scale;
    //       // yield return null;
    //        //}                        
    //    }
    //}
    public void ShowSpikes()
    {   //Function is called from gamemanager to start showing spikes aniamtion.
        StartCoroutine(ShowHide(true));
    }


    public void HideSpikes()
    {//Function is called from gamemanager to start hiding spikes aniamtion.
        StartCoroutine(hide());
    }

    IEnumerator hide()
    {
        yield return StartCoroutine(ShowHide(false));
        ObjectPool.Instance.PoolObject(gameObject);
    }

    // This Handles Animation of spikes
    IEnumerator ShowHide(bool show)
    {    //if show is true, animation of showing spikes starts.
        if (show)
        {
            ////initScale = transform.localScale;
            //Vector3 scale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
            //transform.localScale = scale;

            //float h = Mathf.Abs(initScale.y - scale.y) / anim_speed;

            //while (scale.y < initScale.y)
            //{
            //    scale.y += h;// anim_speed * Time.deltaTime/2;
            //    transform.localScale = scale;
            yield return null;
            //}                        
        }//if show is false, animation of hiding spikes starts. 
        else
        {
            ////initScale = transform.localScale;
            //Vector3 scale = transform.localScale;
            //transform.localScale = scale;
            //float h = Mathf.Abs(initScale.y - 0) / anim_speed;

            //while (scale.y > 0)
            //{
            //    scale.y -= h;// anim_speed * Time.deltaTime/2;
            //    transform.localScale = scale;
            yield return null;
            //}                        
            transform.localScale = initScale;
        }
    }
}
