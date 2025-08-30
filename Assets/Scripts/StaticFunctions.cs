using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData {
    public int elapse;
    public Time time;
    public Player player;
    public List<Weather> lst_weather;

    public SaveData(int e, Player p, Time t, List<Weather> lw) {
        elapse = e;
        player = p;
        time = t;
        lst_weather = lw;
    }
}

public enum Weather {
    sunny, 
    heatwave, 
    residualrain, 
    heavyrain, 
    fog, 
    strongwind, 
    heavysnow
}

public class StaticFunctions
{
    public static readonly string Scene_Start = "Scene_Start";
    public static readonly string Scene_Hideout = "Scene_Hideout";
    public static readonly string Scene_Raid = "Scene_Raid";

    public static readonly string path = Application.persistentDataPath + "/SaveData.json";


    public static JsonSerializerSettings settings = new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.Auto
    };

    public static void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    //������ �ҷ�����
    public static SaveData LoadData() {
        if (File.Exists(StaticFunctions.path)) {
            string json = File.ReadAllText(StaticFunctions.path);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json, settings);
            Debug.LogWarning("�ҷ����� �Ϸ�");
            return data;
        }
        else {
            //�ʱ� ������ ����
            Debug.LogWarning("���̺� ���� ����, �� ������ ����");
            Player player = new Player();
            player.lst_equiped.Add("weapon", new SurvivalKnife());
            player.lst_belonging.Add(new Chocolate());
            //���� ����
            List<Weather> lst_weather = new List<Weather>();
            ExtendWeather(ref lst_weather);

            SaveData data = new SaveData(1, player, Time.day, lst_weather);
            SaveData(data);
            return data;
        }
    }
    //������ ����
    public static void SaveData(SaveData data) {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings); // true �� ���� ���� �鿩����
        File.WriteAllText(StaticFunctions.path, json);
        Debug.LogWarning("���� �Ϸ�: " + StaticFunctions.path);
    }

    public static void ExtendWeather(ref List<Weather> lst_weather) {
        Weather[] allWeathers = (Weather[])Enum.GetValues(typeof(Weather));
        System.Random rand = new System.Random();
        int count = rand.Next(2, 6);
        Weather[] shuffled = allWeathers.OrderBy(x => rand.Next()).ToArray();
        lst_weather.AddRange(shuffled.Take(count));
    }
}
