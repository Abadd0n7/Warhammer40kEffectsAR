using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class First : MonoBehaviour
{
    private ARSessionOrigin sessionOrigin;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits;

    public GameObject PrefabToPlay;

    // Start is called before the first frame update
    void Start()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        raycastManager = GetComponent<ARRaycastManager>();
        hits = new List<ARRaycastHit>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                if(raycastManager.Raycast(touch.position, hits, TrackableType.FeaturePoint))
                {
                    Pose pose = hits[0].pose;
                    Instantiate(PrefabToPlay, pose.position, pose.rotation);
                    //Destroy(PrefabToPlay, 5f);
                }
            }
        }
    }
}
