using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData {
    public int elapse;
    public Time time;
    public Player player;

    public SaveData(int e, Player p, Time t) {
        elapse = e;
        player = p;
        time = t;
    }
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
            SaveData data = new SaveData(1, player, Time.day);
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
}
