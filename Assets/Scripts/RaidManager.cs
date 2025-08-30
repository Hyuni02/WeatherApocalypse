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
        print("���̵� ����");
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
        //�̵��� ü�� �Ҹ�
        if(player.curHP >= player.energyConsum_move) {
            player.curHP -= player.energyConsum_move;
        }
        else {
            print("ü�� ����");
            return;
        }
        //������ �ش��ϴ� ������ Ǯ���� Ž��
        string pick = curWeather.Pick();
        switch (pick) {
            case "food":
            case "water":
            case "material":
            case "medical":
            case "equip":
                print("������ ����");
                break;
            case "animal":
                print("�߻����� ����");
                break;
            default:
                print($"Pick : {pick}");
                break;
        }

    }

    public void btn_Inventory() {
        pnl_inventory.SetActive(true);
        //����ǰ ĭ �ʱ�ȭ
        //��� ĭ �ʱ�ȭ
        //������ ĭ �ʱ�ȭ
        //����ǰ ĭ ǥ��
        //��� ĭ ǥ��
        //������ ĭ ǥ��
    }

    public void btn_CloseInventory() {
        pnl_inventory.SetActive(false);
    }
}
