using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [Header("Components")]
    public Button btn_start;
    public Button btn_quit;

    public void Start() {
        btn_start.onClick.AddListener(() => btn_Start());
        btn_quit.onClick.AddListener(() => btn_Quit());
    }

    public void btn_Start() {
        StaticFunctions.ChangeScene(StaticFunctions.Scene_Hideout);
    }

    public void btn_Quit() {
        if (File.Exists(StaticFunctions.path)) {
            File.Delete(StaticFunctions.path);
        }
        return;

        Application.Quit();
    }
}
