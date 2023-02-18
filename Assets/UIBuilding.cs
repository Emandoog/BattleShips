using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBuilding : MonoBehaviour
{

     private TextMeshProUGUI buildingLog;
    // Start is called before the first frame update
    void Start()
    {
		buildingLog = GetComponent<TextMeshProUGUI>();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuildingLog(string textToSet)
    { 
    buildingLog.text = textToSet;
    
    }
}
