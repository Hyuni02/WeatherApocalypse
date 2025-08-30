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

public class StaticFunctions
{
    public static readonly string Scene_Start = "Scene_Start";
    public static readonly string Scene_Hideout = "Scene_Hideout";
    public static readonly string Scene_Raid = "Scene_Raid";

    public static readonly string path = Application.persistentDataPath + "/SaveData.json";


    public static Dictionary<Item, int> dic_food;
    public static Dictionary<Item, int> dic_water;
    public static Dictionary<Item, int> dic_material;
    public static Dictionary<Item, int> dic_medical;
    public static Dictionary<Item, int> dic_equip;

    public static JsonSerializerSettings settings = new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.Auto
    };

    public static void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    //데이터 불러오기
    public static SaveData LoadData() {
        if (File.Exists(StaticFunctions.path)) {
            string json = File.ReadAllText(StaticFunctions.path);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json, settings);
            Debug.LogWarning("불러오기 완료");
            return data;
        }
        else {
            //초기 데이터 생성
            Debug.LogWarning("세이브 파일 없음, 새 데이터 생성");
            Player player = new Player();
            player.lst_equiped.Add("weapon", new SurvivalKnife());
            player.lst_belonging.Add(new Chocolate());
            //날씨 생성
            List<Weather> lst_weather = new List<Weather>();
            ExtendWeather(ref lst_weather);

            SaveData data = new SaveData(1, player, Time.day, lst_weather);
            SaveData(data);
            return data;
        }
    }
    //데이터 저장
    public static void SaveData(SaveData data) {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings); // true → 보기 좋게 들여쓰기
        File.WriteAllText(StaticFunctions.path, json);
        Debug.LogWarning("저장 완료: " + StaticFunctions.path);
    }

    //날씨 추가
    public static void ExtendWeather(ref List<Weather> lst_weather) {
        Weather[] allWeathers = (Weather[])Enum.GetValues(typeof(Weather));
        System.Random rand = new System.Random();
        int count = rand.Next(2, 6);
        Weather[] shuffled = allWeathers.OrderBy(x => rand.Next()).ToArray();
        lst_weather.AddRange(shuffled.Take(count));
    }

    //날씨 예보
    public static Weather WeatherReport(List<Weather> lst_weather) {
        if(lst_weather.Count > 0) {
            return lst_weather[0];
        }
        System.Random rand = new System.Random();
        Array values = Enum.GetValues(typeof(Weather));
        return (Weather)values.GetValue(rand.Next(values.Length));
    }

    //아이템 확률 선택 반환
    public static Item GetItem(string type) {
        int i = 0;
        System.Random random = new System.Random();
        Dictionary<Item, int> dic_selected = new Dictionary<Item, int>();
        switch(type) {
            case "food":
                if (dic_food == null) SetFood();
                i = random.Next(0, GetTotalWeight(ref dic_food));
                dic_selected = dic_food;
                break;
            case "water":
                if (dic_water == null) SetWater();
                i = random.Next(0, GetTotalWeight(ref dic_water));
                dic_selected = dic_water;
                break;
            case "material":
                if (dic_material == null) SetMaterial();
                i = random.Next(0, GetTotalWeight(ref dic_material));
                dic_selected = dic_material;
                break;
            case "medical":
                if (dic_medical == null) SetMedical();
                i = random.Next(0, GetTotalWeight(ref dic_medical));
                dic_selected = dic_medical;
                break;
            case "equip":
                if (dic_equip == null) SetEquip();
                i = random.Next(0, GetTotalWeight(ref dic_equip));
                dic_selected = dic_equip;
                break;
        }
        foreach (var item in dic_selected) {
            i -= item.Value;
            if (i < 0) {
                return item.Key;
            }
        }
        return null;
    }

    public static int GetTotalWeight(ref Dictionary<string, int> dic) {
        int totalWeight = 0;
        foreach(var weight in dic.Values) {
            totalWeight += weight;
        }
        return totalWeight;
    }

    public static int GetTotalWeight(ref Dictionary<Item, int> dic) {
        int totalWeight = 0;
        foreach (var weight in dic.Values) {
            totalWeight += weight;
        }
        return totalWeight;
    }

    private static void SetFood() {
        dic_food = new Dictionary<Item, int>();
        dic_food.Add(new Chocolate(), 1);
        dic_food.Add(new Tuna(), 1);
    }
    private static void SetWater() {
        dic_water = new Dictionary<Item, int>();
        dic_water.Add(new WaterBottle(), 1);
        dic_water.Add(new Monster(), 1);
    }
    private static void SetMaterial() {
        dic_material = new Dictionary<Item, int>();
        dic_material.Add(new Gasoline(), 1);
        dic_material.Add(new Fabric(), 1);
    }
    private static void SetMedical() {
        dic_medical = new Dictionary<Item, int>();
        dic_medical.Add(new Bandage(), 1);
        dic_medical.Add(new Disinfectant(), 1);
    }
    private static void SetEquip() {
        dic_equip = new Dictionary<Item, int>();
        dic_equip.Add(new SurvivalKnife(), 1);
        dic_equip.Add(new RainCoat(), 1);
    }
}
