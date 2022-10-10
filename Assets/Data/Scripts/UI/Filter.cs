using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Filter : MonoBehaviour, IPointerClickHandler
{
    public GameObject toggleBar;
    bool isOn = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isOn == false)
        {
            toggleBar.SetActive(true);
            isOn = true;
        }
        else if (isOn == true)
        {
            toggleBar.SetActive(false);
            isOn = false;
        }
    }

}
