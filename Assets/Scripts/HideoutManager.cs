using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Time {
    day, night
}

public class HideoutManager : MonoBehaviour {
    public static HideoutManager instance;

    public Player player;
    public int elapse = 1;
    public Time time;
    public List<Weather> lst_weather;

    [Header("Interaction")]
    public Button btn_raid;
    public Button btn_stay;
    public Button btn_sleep;

    [Header("Equiped")]
    public Image img_weapon;
    public Image img_body;
    public Image img_bag;

    [Header("Belonging")]
    public GameObject pnl_belonging;
    public GameObject[] arr_belonging;

    [Header("Inventory")]
    public GameObject pnl_inventory;
    public GameObject[] arr_inventory;

    [Header("Context")]
    public GameObject pnl_invisibleClose;
    public Transform trans_context;
    public GameObject prefab_contextMenu;


    public void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
            return;
        }

        btn_raid.onClick.AddListener(() => btn_Raid());
        btn_stay.onClick.AddListener(() => btn_Stay());
        btn_sleep.onClick.AddListener(() => btn_Sleep());

        pnl_invisibleClose.GetComponent<Button>().onClick.AddListener(() => CloseContext());
    }

    public void Start() {
        print("하이드 아웃 진입");
        LoadData();

        SetArr();

        Update_All();
    }

    private void SetArr() {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in pnl_belonging.transform) {
            if (child.gameObject != pnl_belonging) {
                children.Add(child.gameObject);
            }
        }
        arr_belonging = children.ToArray();

        List<GameObject> children2 = new List<GameObject>();
        foreach (Transform child in pnl_inventory.transform) {
            if (child.gameObject != pnl_inventory) {
                children2.Add(child.gameObject);
            }
        }
        arr_inventory = children2.ToArray();
    }

    private void LoadData() {
        SaveData data = StaticFunctions.LoadData();
        elapse = data.elapse;
        time = data.time;
        player = data.player;
        lst_weather = data.lst_weather;
    }

    public void Update_All() {
        Update_PlayerState();
        Update_Inventory();
        Update_GeneratorState();
        Update_Interation();
    }

    public void Update_PlayerState() {

    }

    public void Update_Inventory() {
        //착용 장비 표시
        img_weapon.GetComponent<ItemHolder>().init();
        img_body.GetComponent<ItemHolder>().init();
        img_bag.GetComponent<ItemHolder>().init();
        foreach (var equiped in player.lst_equiped) {
            if (equiped.Value.Count == 0) continue;
            switch (equiped.Key) {
                case "weapon":
                    img_weapon.GetComponent<ItemHolder>().SetItem(equiped.Value[0], player.lst_equiped["weapon"]);
                    break;
                case "body":
                    img_body.GetComponent<ItemHolder>().SetItem(equiped.Value[0], player.lst_equiped["body"]);
                    break;
                case "bag":
                    img_bag.GetComponent<ItemHolder>().SetItem(equiped.Value[0], player.lst_equiped["bag"]);
                    break;
            }
        }

        //소지품 표시
        foreach(var btn in arr_belonging) {
            btn.GetComponent<ItemHolder>().init();
        }
        for (int i = 0; i < player.lst_belonging.Count; i++) {
            arr_belonging[i].GetComponent<ItemHolder>().SetItem(player.lst_belonging[i], player.lst_belonging);
        }

        //인벤토리 표시
        foreach (var btn in arr_inventory) {
            btn.GetComponent<ItemHolder>().init();
        }
        for (int i = 0; i < player.lst_inventory.Count; i++) {
            arr_inventory[i].GetComponent<ItemHolder>().SetItem(player.lst_inventory[i], player.lst_inventory);
        }

    }

    public void Update_GeneratorState() {

    }

    public void Update_Interation() {
        btn_raid.gameObject.SetActive(time == Time.day);
        btn_stay.gameObject.SetActive(time == Time.day);
        btn_sleep.gameObject.SetActive(time == Time.night);
    }

    //외출
    public void btn_Raid() {
        StaticFunctions.SaveData(new SaveData(elapse, player, time, lst_weather));
        StaticFunctions.ChangeScene(StaticFunctions.Scene_Raid);
    }

    //수면
    public void btn_Sleep() {
        StaticFunctions.SaveData(new SaveData(elapse, player, time, lst_weather));
        print("수면");
        elapse++;
        time = Time.day;
        //날씨 차감
        lst_weather.RemoveAt(0);
        if (lst_weather.Count <= 0) {
            StaticFunctions.ExtendWeather(ref lst_weather);
        }
        //todo 밤사이 이벤트 발생 구현
        StaticFunctions.SaveData(new SaveData(elapse, player, time, lst_weather));

        Update_All();
    }

    //집에 머물기
    public void btn_Stay() {
        print("집에 있기");
        time = Time.night;
        StaticFunctions.SaveData(new SaveData(elapse, player, time, lst_weather));

        Update_All();
    }

    public void OpenContext(HashSet<string> actions, Item item, List<Item> from) {
        pnl_invisibleClose.gameObject.SetActive(true);
        trans_context.gameObject.SetActive(true);

        //컨텍스트 위치 조절
        RectTransform rect_context = trans_context.GetComponent<RectTransform>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect_context.parent as RectTransform, // 부모 기준
            Input.mousePosition,
            null, // 카메라 (Screen Space Overlay일 때는 null)
            out pos
        );
        rect_context.anchoredPosition = pos;

        //컨텍스트 메뉴 추가
        foreach (string action in actions) {
            if (action == "tobelonging" && from == player.lst_belonging) continue;
            if (action == "toinventory" && from == player.lst_inventory) continue; 
            GameObject menu = Instantiate(prefab_contextMenu, trans_context);
            menu.GetComponentInChildren<TMP_Text>().text = action;
            menu.GetComponent<Button>().onClick.AddListener(() => {
                StaticFunctions.ClickContextMenu(action, item, from);
                CloseContext();
            });
        }
    }

    public void CloseContext() {
        pnl_invisibleClose.gameObject.SetActive(false);
        trans_context.gameObject.SetActive(false);

        StaticFunctions.ClearChild(trans_context);
    }
}
