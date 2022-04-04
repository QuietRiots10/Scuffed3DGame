using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS SCRIPT CONTROLS THE HITMARKER UI ELEMENT, AND THE ANIMATION OF IT SPAWNING AND DISAPPEARING

public class HitMarkerScript : MonoBehaviour
{
    //Objects
    RawImage HitMarker;
    public float HitMarkerSize;
    
    // Start is called before the first frame update
    void Start()
    {
        HitMarker = gameObject.GetComponent<RawImage>();
        HitMarker.enabled = false;
    }

    //Coroutines

    //Calls the hit marker
    public void StartHitMarker()
    {
        StopCoroutine(HitMarkerAnimation());
        StartCoroutine(HitMarkerAnimation());
    }

    //Controls the hit marker animation (growing and shrinking)
    IEnumerator HitMarkerAnimation()
    {
        transform.localScale = new Vector3(0, 0, 1);
        HitMarker.enabled = true;

        //Scale the hitmarker up
        float count = 0;
        while (count < 0.04f)
        {
            transform.localScale = new Vector3((HitMarkerSize / 0.04f) * count, (HitMarkerSize / 0.04f) * count, 1);
            count = count + Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.1f);


        //Scale the hitmarker up
        count = 0.04f;
        while (count > 0)
        {
            transform.localScale = new Vector3((HitMarkerSize / 0.04f) * count, (HitMarkerSize / 0.04f) * count, 1);
            count = count - Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        yield return null;
    }
}
