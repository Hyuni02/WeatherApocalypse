using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour {
    public static RaidManager instance;

    public Player player;
    public int elapse = 1;
    public Time time;
    public List<Weather> lst_weather;

    public Sunny curWeather;
    public Item curItem = null;
    public Animal curAnimal = null;

    [Header("Component")]
    public Button btn_return;
    public Button btn_search;
    public Button btn_inventory;
    public Button btn_closeInventory;
    public Button btn_pick;
    public Button btn_run;
    public Button btn_attack;
    public GameObject pnl_inventory;

    [Header("Equiped")]
    public Image img_weapon;
    public Image img_body;
    public Image img_bag;

    [Header("Belonging")]
    public GameObject pnl_belonging;
    public GameObject[] arr_belonging;

    [Header("Context")]
    public GameObject pnl_invisibleClose;
    public Transform trans_context;
    public GameObject prefab_contextMenu;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
            return;
        }

        btn_return.onClick.AddListener(() => btn_Return());
        btn_search.onClick.AddListener(() => btn_Search());
        btn_inventory.onClick.AddListener(() => btn_Inventory());
        btn_pick.onClick.AddListener(() => btn_Pick());
        btn_run.onClick.AddListener(() => btn_Run());
        btn_attack.onClick.AddListener(() => btn_Attack());
        btn_closeInventory.onClick.AddListener(() => btn_CloseInventory());

        pnl_invisibleClose.GetComponent<Button>().onClick.AddListener(() => CloseContext());
    }

    private void Start() {
        print("레이드 진입");
        LoadData();

        SetArr();

        SetWeather();

        Update_All();

        pnl_inventory.SetActive(false);
    }

    public void Update_All() {
        Update_PlayerState();
        Update_Inventory();
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
        foreach (var btn in arr_belonging) {
            btn.GetComponent<ItemHolder>().init();
        }
        for (int i = 0; i < player.lst_belonging.Count; i++) {
            arr_belonging[i].GetComponent<ItemHolder>().SetItem(player.lst_belonging[i], player.lst_belonging);
        }
    }

    private void SetArr() {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in pnl_belonging.transform) {
            if (child.gameObject != pnl_belonging) {
                children.Add(child.gameObject);
            }
        }
        arr_belonging = children.ToArray();
    }

    private void SetWeather() {
        switch (lst_weather[0]) {
            case Weather.sunny:
                curWeather = new Sunny();
                break;
            case Weather.heatwave:
                curWeather = new Heatwave();
                break;
            case Weather.rain:
                curWeather = new Rain();
                break;
            case Weather.heavyrain:
                curWeather = new Heavyrain();
                break;
            case Weather.fog:
                curWeather = new Fog();
                break;
            case Weather.windy:
                curWeather = new Windy();
                break;
            case Weather.snow:
                curWeather = new Snow();
                break;
        }
    }

    private void LoadData() {
        SaveData data = StaticFunctions.LoadData();
        elapse = data.elapse;
        time = data.time;
        player = data.player;
        lst_weather = data.lst_weather;
    }

    public void btn_Return() {
        time = Time.night;
        StaticFunctions.SaveData(new SaveData(elapse, player, time, lst_weather));
        StaticFunctions.ChangeScene(StaticFunctions.Scene_Hideout);
    }

    public void btn_Search(int rate = 1, bool run = false) {
        //버튼 초기화
        btn_pick.gameObject.SetActive(false);
        btn_attack.gameObject.SetActive(false);
        btn_run.gameObject.SetActive(false);

        //이동 체력 계산
        int requireHP = player.energyConsum_move;
        requireHP += curWeather.GetProperity("energyConsum_move") * rate;
        //todo 다리 상태 적용

        //이동용 체력 소모
        if(player.curHP >= requireHP) {
            player.curHP -= requireHP;
            if (run) print("무사히 도망쳤다");
        }
        else {
            print(!run ? "체력 부족" : "도망칠 수 없다.");
            return;
        }
        //날씨에 해당하는 아이템 풀에서 탐색
        string type = curWeather.GetItem();
        switch (type) {
            case "food":
            case "water":
            case "material":
            case "medical":
            case "equip":
                FindItem(type);
                break;
            case "animal":
                EncounterAnimal();
                break;
            default:
                print($"아무것도 찾지 못했다.");
                curItem = null;
                break;
        }
    }

    private void FindItem(string type) {
        curItem = StaticFunctions.GetItem(type);
        print(curItem.name);
        btn_pick.gameObject.SetActive(true);
    }

    private void EncounterAnimal(bool keep = false) {
        //기존 동물 조우 여부
        if(!keep) curAnimal = StaticFunctions.GetAnimal();
        print(curAnimal.name);
        btn_attack.gameObject.SetActive(true);
        btn_run.gameObject.SetActive(true);
    }

    public void btn_Inventory() {
        pnl_inventory.SetActive(true);
        //todo 소지품 칸 초기화
        //todo 장비 칸 초기화
        //todo 몸상태 칸 초기화
        //todo 소지품 칸 표시
        //todo 장비 칸 표시
        //todo 몸상태 칸 표시
    }

    public void btn_CloseInventory() {
        pnl_inventory.SetActive(false);
    }

    public void btn_Pick() {
        //todo 가방 용량 확인
        btn_pick.gameObject.SetActive(false);
        player.lst_belonging.Add(curItem);
        curItem = null;
    }

    public void btn_Attack() {
        //공격 체력 계산
        int requireHP = player.energyConsum_attack;
        //todo 추가 소모 체력 확인

        //공격용 체력 소모
        if(player.curHP >= requireHP) {
            player.curHP -= requireHP;
        }
        else {
            print("체력 부족");
            return;
        }
        //공격 적중 계산
        System.Random rand = new System.Random();
        if(rand.Next(0, 100) < player.acc) {
            //대상에게 피해 주기
            if (curAnimal.GetDmg(player.dmg)) {
                print("동물 사살");
                btn_attack.gameObject.SetActive(false);
                btn_run.gameObject.SetActive(false);
                
                //전리품 열기
                OpenDrop();

                curAnimal = null;
            }
            else {
                //동물의 공격
                curAnimal.Attack(ref player);
                EncounterAnimal(true);
            }
        }
        else {
            print("공격이 빗나감");
            //동물의 공격
            curAnimal.Attack(ref player);
            EncounterAnimal(true);
        }
    }

    public void OpenDrop() {
        //버튼 초기화
        btn_pick.gameObject.SetActive(true);
        btn_attack.gameObject.SetActive(false);
        btn_run.gameObject.SetActive(false);

        System.Random random = new System.Random();
        int i = random.Next(0, StaticFunctions.GetTotalWeight(ref curAnimal.dic_drop));
        foreach (var item in curAnimal.dic_drop) {
            i -= item.Value;
            if (i < 0) {
                curItem = item.Key;
            }
        }

        print(curItem.name);
    }

    public void btn_Run() {
        System.Random rand = new System.Random();
        if (rand.Next(0, 100) < curAnimal.acc) {
            btn_Search(2, true);
        }
        else {
            print("도주 실패");
            curAnimal.Attack(ref player);
            EncounterAnimal(true);
        }
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
            if (action == "toinventory") continue;
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
