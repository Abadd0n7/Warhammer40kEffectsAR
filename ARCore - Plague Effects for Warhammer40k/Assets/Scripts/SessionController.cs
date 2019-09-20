using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SessionController : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private ARSessionOrigin sessionOrigin;
    private ARPlaneManager planeManager;

    private List<ARRaycastHit> hits;

    private bool isInstantiated = false;
    private ButtonValue currValue;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<ButtonState> StateButtons;

    [SerializeField]
    private GameObject RainPrefab, BlizzardPrefab, 
        FirePrefab, TornadoPrefab, MeteorPrefab, 
        TsunamiPrefab, PlaguePrefab;

    void Start()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        hits = new List<ARRaycastHit>();
        audioSource.volume = 0.3f;

        foreach(ButtonState button in StateButtons)
        {
            button.OnButtonClick += StateHandler;
        }
    }

    void Update()
    {
        planeManager.enabled = !planeManager.enabled;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began && raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon) && isInstantiated == false)
            {
                Pose pose = hits[0].pose;

                GameObject btnPrefab = null;

                if (currValue == ButtonValue.Rain)
                {
                    btnPrefab = RainPrefab;
                }
                else if(currValue == ButtonValue.Blizzard)
                {
                    btnPrefab = BlizzardPrefab;
                }
                else if (currValue == ButtonValue.Fire)
                {
                    btnPrefab = FirePrefab;
                }
                else if (currValue == ButtonValue.Tornado)
                {
                    btnPrefab = TornadoPrefab;
                }
                else if (currValue == ButtonValue.Meteor)
                {
                    btnPrefab = MeteorPrefab;
                }
                else if (currValue == ButtonValue.Tsunami)
                {
                    btnPrefab = TsunamiPrefab;
                }
                else if(currValue == ButtonValue.Plague)
                {
                    btnPrefab = PlaguePrefab;
                }
                
                var go = Instantiate(btnPrefab, pose.position, pose.rotation);
                sessionOrigin.MakeContentAppearAt(go.transform, go.transform.position, go.transform.rotation);
                isInstantiated = true;
                audioSource.volume = 0.1f;
                SetAllPlanesActive(false);
                StartCoroutine(OnDestroyPrefab(go));
            }
        }
    }
    IEnumerator OnDestroyPrefab(GameObject go)
    {
        yield return new WaitForSeconds(15f);
        Destroy(go);
        isInstantiated = false;
        SetAllPlanesActive(true);
        audioSource.volume = 0.3f;
    }

    void StateHandler(ButtonValue bv)
    {
        currValue = bv;
    }

    void OnDestroy()
    {
        foreach (ButtonState button in StateButtons)
        {
            button.OnButtonClick -= StateHandler;
        }
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(value);
    }

}
