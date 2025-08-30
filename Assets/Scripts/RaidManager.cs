using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour {
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

    private void Awake() {
        btn_return.onClick.AddListener(() => btn_Return());
        btn_search.onClick.AddListener(() => btn_Search());
        btn_inventory.onClick.AddListener(() => btn_Inventory());
        btn_pick.onClick.AddListener(() => btn_Pick());
        btn_run.onClick.AddListener(() => btn_Run());
        btn_attack.onClick.AddListener(() => btn_Attack());
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

    public void btn_Search(int rate = 1, bool run = false) {
        //��ư �ʱ�ȭ
        btn_pick.gameObject.SetActive(false);
        btn_attack.gameObject.SetActive(false);
        btn_run.gameObject.SetActive(false);

        //�̵� ü�� ���
        int requireHP = player.energyConsum_move;
        requireHP += curWeather.GetProperity("energyConsum_move") * rate;
        //todo �ٸ� ���� ����

        //�̵��� ü�� �Ҹ�
        if(player.curHP >= requireHP) {
            player.curHP -= requireHP;
            if (run) print("������ �����ƴ�");
        }
        else {
            print(!run ? "ü�� ����" : "����ĥ �� ����.");
            return;
        }
        //������ �ش��ϴ� ������ Ǯ���� Ž��
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
                print($"�ƹ��͵� ã�� ���ߴ�.");
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
        //���� ���� ���� ����
        if(!keep) curAnimal = StaticFunctions.GetAnimal();
        print(curAnimal.name);
        btn_attack.gameObject.SetActive(true);
        btn_run.gameObject.SetActive(true);
    }

    public void btn_Inventory() {
        pnl_inventory.SetActive(true);
        //todo ����ǰ ĭ �ʱ�ȭ
        //todo ��� ĭ �ʱ�ȭ
        //todo ������ ĭ �ʱ�ȭ
        //todo ����ǰ ĭ ǥ��
        //todo ��� ĭ ǥ��
        //todo ������ ĭ ǥ��
    }

    public void btn_CloseInventory() {
        pnl_inventory.SetActive(false);
    }

    public void btn_Pick() {
        //todo ���� �뷮 Ȯ��
        btn_pick.gameObject.SetActive(false);
        player.lst_belonging.Add(curItem);
        curItem = null;
    }

    public void btn_Attack() {
        //���� ü�� ���
        int requireHP = player.energyConsum_attack;
        //todo �߰� �Ҹ� ü�� Ȯ��

        //���ݿ� ü�� �Ҹ�
        if(player.curHP >= requireHP) {
            player.curHP -= requireHP;
        }
        else {
            print("ü�� ����");
            return;
        }
        //���� ���� ���
        System.Random rand = new System.Random();
        if(rand.Next(0, 100) < player.acc) {
            //��󿡰� ���� �ֱ�
            if (curAnimal.GetDmg(player.dmg)) {
                print("���� ���");
                btn_attack.gameObject.SetActive(false);
                btn_run.gameObject.SetActive(false);
                
                //����ǰ ����
                OpenDrop();

                curAnimal = null;
            }
            else {
                //������ ����
                curAnimal.Attack(ref player);
                EncounterAnimal(true);
            }
        }
        else {
            print("������ ������");
            //������ ����
            curAnimal.Attack(ref player);
            EncounterAnimal(true);
        }
    }

    public void OpenDrop() {
        //��ư �ʱ�ȭ
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
            print("���� ����");
            curAnimal.Attack(ref player);
            EncounterAnimal(true);
        }
    }
}
