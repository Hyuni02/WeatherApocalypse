using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour {
    public Player player;
    public int elapse = 1;
    public Time time;

    [Header("Component")]
    public Button btn_return;

    private void Awake() {
        btn_return.onClick.AddListener(() => btn_Return());
    }

    private void Start() {
        print("레이드 진입");
        LoadData();
    }

    private void LoadData() {
        SaveData data = StaticFunctions.LoadData();
        elapse = data.elapse;
        time = data.time;
        player = data.player;
    }

    public void btn_Return() {
        time = Time.night;
        StaticFunctions.SaveData(new SaveData(elapse, player, time));
        StaticFunctions.ChangeScene(StaticFunctions.Scene_Hideout);
    }
}
