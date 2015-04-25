using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZoneScript : MonoBehaviour {

    public List<WorldItem> worldItemList { get; set; }
    public int selectedItem { get; set; }

    Text ZoneNameText { get; set; }
    Button BackButton { get; set; }
    

	// Use this for initialization
	void Start () {
	    
	}
        
    
    void OnLevelWasLoaded()
    {
        getWorldSceneData();

        getGameObjects();
        setUI();

    }

    private void getGameObjects()
    {
        ZoneNameText = GameObject.FindGameObjectWithTag("ZoneName").GetComponent<Text>();
   
          
    }

    private void setUI()
    {
        ZoneNameText.text = worldItemList[selectedItem].name;
        
    }

    private void getWorldSceneData()
    {
        var worldScript = GameObject.FindObjectOfType<WorldScript>();
        worldItemList = worldScript.worldItemList;
        selectedItem = worldScript.selectedItem;
        Destroy(worldScript);

    }
       
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void BackToWorld()
    {
        Application.LoadLevel(0);
    }
}
