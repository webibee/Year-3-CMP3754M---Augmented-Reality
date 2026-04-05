using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour
{
    public GameObject helpbutton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        if (helpbutton.activeInHierarchy == true)
        {
            helpbutton.SetActive(false);
        }
        else
        {
            helpbutton.SetActive(true);
        }
            
    }
}
