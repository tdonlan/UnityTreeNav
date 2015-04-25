using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.UI;


public class WorldScript : MonoBehaviour {

    public List<WorldItem> worldItemList { get; set; }
    public System.Random r { get; set; }
    public int maxItems = 4;

    public GameObject worldItemPrefab { get; set; }
    public GameObject linePrefab { get; set; }
    public GameObject zoneInfoPrefab { get; set; }

    public GameObject zoneContentPanel { get; set; }

    public int selectedItem { get; set; }

	// Use this for initialization
	void Start () {
        r = new System.Random();

        InitPrefabs();
        InitWorldItemList();

        LoadWorldItems();
        //LoadLines();

        DontDestroyOnLoad(this);
	}

    private void InitPrefabs()
    {
        worldItemPrefab = Resources.Load<GameObject>("WorldItemPrefab");
        linePrefab = Resources.Load<GameObject>("LinePrefab");
        zoneInfoPrefab = Resources.Load<GameObject>("ZoneInfoPrefab");

        zoneContentPanel = GameObject.FindGameObjectWithTag("ZoneInfoContentPanel");
    }

    private void InitWorldItemList()
    {
        worldItemList = new List<WorldItem>();

        for(int i=0;i<maxItems;i++)
        {
            worldItemList.Add(new WorldItem() { index = i, name = string.Format("Item{0}", i), linkList = getSequentialList(i,maxItems-1) });
        }
    }

    private List<int> getSequentialList(int index, int max)
    {
        if(index < max)
        {
            return new List<int>() { index + 1 };
        }
        return new List<int>() { 0 };
    }

    private List<int> getSampleList(int num)
    {
        List<int> linkList = new List<int>();
        for(int i=0;i<r.Next(2)+1;i++)
        {
            linkList.Add(r.Next(num));
        }
        return linkList;
    }

    private void LoadWorldItems()
    {
        var worldContentPanel = GameObject.FindGameObjectWithTag("WorldContentPanel");

        foreach(var item in worldItemList)
        {
            var worldItem = (GameObject)Instantiate(worldItemPrefab);
            UpdateWorldItem(worldItem, item);
          

            int localIndex = item.index;
            var button = worldItem.GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate { ClickWorldItem(localIndex); });

            //worldItem = UIHelpers.UpdateButtonOnClick(worldItem, delegate { ClickWorldItem(item.index); }, item.name);
            //UIHelpers.AddClickToGameObject(worldItem, ClickWorldItem, UnityEngine.EventSystems.EventTriggerType.PointerClick, item.index);

            worldItem.transform.position = item.position;
            worldItem.transform.SetParent(worldContentPanel.transform);

        }
    }

    private void LoadLines()
    {
        foreach(var item in worldItemList)
        {
            foreach (var link in item.linkList)
            {
                LoadLine(item, worldItemList[link]);
            }
        }
        
    }

    //create a line between two items
    private void LoadLine(WorldItem item1, WorldItem item2)
    {
        var height = Vector2.Distance(item1.position, item2.position);

        var angle = Mathf.Atan2(item1.position.y - item2.position.y, item1.position.x - item2.position.x);

        var midPtX = (item2.position.x - (item1.position.x /2));
        var midPtY = (item2.position.y - (item1.position.y / 2));

        Vector3 midPos = new Vector3(midPtX,midPtY);

        var line = (GameObject)Instantiate<GameObject>(linePrefab);

        var lineRectTransform = line.GetComponent<RectTransform>();

        lineRectTransform.position = item1.position;
        lineRectTransform.eulerAngles = new Vector3(0,0,angle * Mathf.Rad2Deg);
        //lineRectTransform.sizeDelta = new Vector2(0, height);

        var worldContentPanel = GameObject.FindGameObjectWithTag("WorldContentPanel");

        line.transform.SetParent(worldContentPanel.transform);
    }

    private void LoadZoneInfo(int zoneIndex)
    {
        UIHelpers.DestroyAllChildren(zoneContentPanel.transform);

        var zoneInfo = (GameObject)Instantiate(zoneInfoPrefab);
        UIHelpers.UpdateTextComponent(zoneInfo, "ZoneName", worldItemList[zoneIndex].name);

        var zoneInfoButton = zoneInfo.GetComponentInChildren<Button>();
        zoneInfoButton.onClick.AddListener(delegate { EnterZone(zoneIndex); });

        zoneInfo.transform.SetParent(zoneContentPanel.transform);
    }


    private void UpdateWorldItem(GameObject worldItemObject, WorldItem item)
    {
        UpdateTextComponent(worldItemObject, "Text", item.name);
    }

    public static void UpdateTextComponent(GameObject parent, string componentName, string text)
    {
        foreach (var comp in parent.GetComponentsInChildren<Text>())
        {
            if (comp.name == componentName)
            {
                comp.text = text;
            }
        }
    }

    public static void UpdateSpriteComponent(GameObject parent, string componentName, Sprite sprite)
    {
        foreach (var comp in parent.GetComponentsInChildren<Image>())
        {
            if (comp.name == componentName)
            {
                comp.sprite = sprite;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateDebugText();
	}

    private void UpdateDebugText()
    {
        var mousePos = Input.mousePosition;
        var debugText = GameObject.FindGameObjectWithTag("DebugText");

        Text debugTxtComp = debugText.GetComponent<Text>();
        debugTxtComp.text = string.Format("{0},{1}", mousePos.x, mousePos.y);
    }

    public void EnterZone(int index)
    {
        selectedItem = index;
        Application.LoadLevel(1);
    
    }

    public void ClickWorldItem(int index)
    {
        LoadZoneInfo(index);
    }


}
