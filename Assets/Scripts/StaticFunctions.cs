using UnityEngine.SceneManagement;

public class StaticFunctions
{
    public static readonly string Scene_Start = "Scene_Start";
    public static readonly string Scene_Hideout = "Scene_Hideout";
    public static readonly string Scene_Raid = "Scene_Raid";

    public static void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
