using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Time {
    day, night
}

public class HideoutManager : MonoBehaviour {
    public Player player;
    public int elapse = 1;
    public Time time;
    public List<Weather> lst_weather;

    [Header("Interaction")]
    public Button btn_raid;
    public Button btn_stay;
    public Button btn_sleep;


    public void Awake() {
        btn_raid.onClick.AddListener(() => btn_Raid());
        btn_stay.onClick.AddListener(() => btn_Stay());
        btn_sleep.onClick.AddListener(() => btn_Sleep());
    }

    public void Start() {
        print("하이드 아웃 진입");
        LoadData();

        Update_All();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ShowWeather();
            ShowElapse();
        }
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

    private void ShowWeather() {
        print($"Weather : {lst_weather[0]}");
    }

    private void ShowElapse() {
        print($"Elapse : {elapse}");
    }
}
