using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {
    
    int anim_speed = 2;
    Vector3 initScale;
    public void SaveScale() {

        initScale = transform.localScale;
    }
    public void ShowSpikes()
    {   
        //Function is called from gamemanager to start showing spikes aniamtion.
        StartCoroutine(ShowHide(true));
    }


    public void HideSpikes()
    {
        //Function is called from gamemanager to start hiding spikes aniamtion.
        StartCoroutine(hide());
    }

    private IEnumerator hide()
    {
        StartCoroutine(ShowHide(false));
        yield return null;
        ObjectPool.Instance.PoolObject(gameObject);
    }

    // This Handles Animation of spikes
    private IEnumerator ShowHide(bool show)
    {    //if show is true, animation of showing spikes starts.
        if (show)
        {
            yield return null;
        }//if show is false, animation of hiding spikes starts. 
        else
        {
            yield return null;
            //transform.localScale = initScale;
        }
    }
}
