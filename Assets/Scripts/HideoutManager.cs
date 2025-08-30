using UnityEngine;
using UnityEngine.UI;

public enum Time {
    day, night
}

public class HideoutManager : MonoBehaviour {
    public Player player;
    public int elapse = 1;
    public Time time;

    [Header("Interaction")]
    public Button btn_raid;
    public Button btn_sleep;


    public void Awake() {
        btn_raid.onClick.AddListener(() => btn_Raid());
        btn_sleep.onClick.AddListener(() => btn_Sleep());
    }

    public void Start() {
        print("하이드 아웃 진입");
        LoadData();

        Update_All();
    }

    private void LoadData() {
        SaveData data = StaticFunctions.LoadData();
        elapse = data.elapse;
        time = data.time;
        player = data.player;
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

    }

    public void Update_GeneratorState() {

    }

    public void Update_Interation() {
        btn_raid.gameObject.SetActive(time == Time.day);
        btn_sleep.gameObject.SetActive(time == Time.night);
    }

    public void btn_Raid() {
        StaticFunctions.SaveData(new SaveData(elapse, player, time));
        StaticFunctions.ChangeScene(StaticFunctions.Scene_Raid);
    }

    public void btn_Sleep() {
        StaticFunctions.SaveData(new SaveData(elapse, player, time));
        print("수면");
        elapse++;
        time = Time.day;
        //todo 밤사이 이벤트 발생 구현
        StaticFunctions.SaveData(new SaveData(elapse, player, time));

        Update_All();
    }
}
