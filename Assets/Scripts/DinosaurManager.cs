using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DinosaurManager : MonoBehaviour
{
    private List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();
    public Camera arCamera;
    public GameObject PachycephalasaurusPrefab;
    private Vector2 touchPosition = default;
    public ARRaycastManager arRaycastManager;
    private int numberOfPachycephalasaurus = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (Input.touchCount == 1)
                {
                    if (arRaycastManager.Raycast(touch.position, aRRaycastHits))
                    {
                        if (numberOfPachycephalasaurus < 1)
                        {
                            var pose = aRRaycastHits[0].pose;
                            CreateDino(pose.position);
                            numberOfPachycephalasaurus++;
                            return;
                        }
                    }
                    
                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.tag == "Pachycephalasaurus") { 
                            DeleteDino(hit.collider.gameObject);
                            numberOfPachycephalasaurus--;
                        }
                    }
                }
            }
        }
    }

    private void CreateDino(Vector3 position)
    {
        Instantiate(PachycephalasaurusPrefab, position, Quaternion.identity);
    }

    private void DeleteDino(GameObject dinoObject)
    {
        Handheld.Vibrate();
        Destroy(dinoObject);
    }
}
