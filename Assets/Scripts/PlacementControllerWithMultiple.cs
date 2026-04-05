using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementControllerWithMultiple : MonoBehaviour
{
    [SerializeField]
    private Button VelocyBtn;
    [SerializeField]
    private Button PachyBtn;
    private GameObject placedPrefab;
    [SerializeField]
    private GameObject PachyFacts;
    [SerializeField]
    private GameObject VelocyFacts;
    private bool showPachyFacts = false;
    private bool showVelocyFacts = false;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private int numOfSpawnedVelocy = 0;
    private int numOfSpawnedPachy = 0;
    private bool helpBtnClicked = false;

    // Start is called before the first frame update
    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        VelocyBtn.onClick.AddListener(() => ChangePrefabTo("toScaleVelocy"));
        PachyBtn.onClick.AddListener(() => ChangePrefabTo("toScalePachy"));
    }

    // Update is called once per frame
    void Update()
    {
        if(placedPrefab == null) 
        {
            return;
        }
        if (Input.touchCount > 0 ) { 
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var touchPosition = touch.position;
                bool isOverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                Debug.Log(isOverUI);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Debug.Log("blocked raycast");
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if ((Physics.Raycast(ray, out hit)) &&  (hit.collider.tag == "Dino"))
                {
                    Debug.Log("raycast");
                    if (Input.touchCount == 2)
                    {
                        if (hit.transform.parent.tag == "Pachy")
                        {
                            Destroy(hit.transform.gameObject);
                            numOfSpawnedPachy--;
                        }
                        if (hit.transform.parent.tag == "Velocy")
                        {
                            Destroy(hit.transform.gameObject);
                            numOfSpawnedVelocy--;
                        }
                    }
                    if (Input.touchCount == 1)
                    {
                        if (hit.transform.parent.tag == "Pachy")
                        {
                            if (showPachyFacts == false) // display facts
                            {
                                showPachyFacts = true;
                                PachyFacts.SetActive(true); 
                            }
                            else
                            {
                                showPachyFacts = false;
                                PachyFacts.SetActive(false);
                            }
                        }
                        if (hit.transform.parent.tag == "Velocy")
                        {
                            if (showVelocyFacts == false) // display facts
                            {
                                showVelocyFacts = true;
                                VelocyFacts.SetActive(true);
                            }
                            else
                            {
                                showVelocyFacts = false;
                                VelocyFacts.SetActive(false);
                            }
                        }
                    }
                }
                else if (!isOverUI && arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                {
                    Debug.Log("arraycast");
                    var hitPose = hits[0].pose;
                    
                    if ((placedPrefab.gameObject.name == "toScalePachy") && (numOfSpawnedPachy < 1))
                    {
                        if (helpBtnClicked == false)
                        {
                            Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                            numOfSpawnedPachy++;
                        }
                    }
                    if ((placedPrefab.gameObject.name == "toScaleVelocy") && (numOfSpawnedVelocy < 1))
                    {
                        if (helpBtnClicked == false)
                        {
                            Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                            numOfSpawnedVelocy++;
                        }
                    }
                }
            }
        }
    }

    void ChangePrefabTo(string prefabName)
    {
        placedPrefab = Resources.Load<GameObject>($"prefabs/{prefabName}");
        if( placedPrefab == null )
        {
            Debug.LogError($"Prefab with name {prefabName} could not be loaded, make sure you check the naming of your prefabs.");
        }
        Color velc = VelocyBtn.image.color;
        Color pachyc = PachyBtn.image.color;
        Debug.Log("is velocy" + velc.a + " " + pachyc.a);
        switch (prefabName)
        {
            case "toScaleVelocy":
                velc.a = 1f;
                pachyc.a = 0.1f;
                break;
            case "toScalePachy":
                Debug.Log("is pachy");
                velc.a = 0.1f;
                pachyc.a = 1f;
                break;
        }
        VelocyBtn.image.color = velc;
        PachyBtn.image.color = pachyc;
    }
}
