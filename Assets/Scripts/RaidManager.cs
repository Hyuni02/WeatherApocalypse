using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour {
    public Player player;
    public int elapse = 1;
    public Time time;
    public List<Weather> lst_weather;

    public Sunny curWeather;

    [Header("Component")]
    public Button btn_return;
    public Button btn_search;
    public Button btn_inventory;
    public Button btn_closeInventory;
    public GameObject pnl_inventory;

    private void Awake() {
        btn_return.onClick.AddListener(() => btn_Return());
        btn_search.onClick.AddListener(() => btn_Search());
        btn_inventory.onClick.AddListener(() => btn_Inventory());
        btn_closeInventory.onClick.AddListener(() => btn_CloseInventory());
    }

    private void Start() {
        print("레이드 진입");
        LoadData();

        SetWeather();
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

    public void btn_Search() {
        //이동 체력 계산
        int requireHP = player.energyConsum_move;
        requireHP += curWeather.GetProperity("energyConsum_move");
        //todo 다리 상태 적용

        //이동용 체력 소모
        if(player.curHP >= requireHP) {
            player.curHP -= requireHP;
        }
        else {
            print("체력 부족");
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
                print("야생동물 조우");
                break;
            default:
                print($"아무것도 찾지 못했다.");
                break;
        }
    }

    private void FindItem(string type) {
        Item item = StaticFunctions.GetItem(type);
        print(item.name);
    }

    private void EncounterAnimal() {

    }

    public void btn_Inventory() {
        pnl_inventory.SetActive(true);
        //소지품 칸 초기화
        //장비 칸 초기화
        //몸상태 칸 초기화
        //소지품 칸 표시
        //장비 칸 표시
        //몸상태 칸 표시
    }

    public void btn_CloseInventory() {
        pnl_inventory.SetActive(false);
    }
}
