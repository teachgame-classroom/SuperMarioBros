using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public interface IActivityInfo
{

}

[System.Serializable]
public class ActivityInfo : IActivityInfo
{
    public string name;

    public static void ToJsonFile(ActivityInfo info)
    {
        string JsonPath = Application.streamingAssetsPath + "/" + info.GetType().ToString() + ".txt";

        string json = JsonUtility.ToJson(info);
        File.WriteAllText(JsonPath, json);
    }

    public static void ToJsonFile(ActivityInfo[] info, string fileName)
    {
        string JsonPath = Application.streamingAssetsPath + "/" + fileName + ".txt";

        string[] jsons = new string[info.Length];

        for(int i = 0; i < info.Length; i++)
        {
            jsons[i] = JsonUtility.ToJson(info[i]);
        }

        File.WriteAllLines(JsonPath, jsons);
    }


    public static T CreateInfoFromJson<T>() where T : ActivityInfo
    {
        string JsonPath = Application.streamingAssetsPath + "/" + typeof(T).ToString() + ".txt";

        string json = File.ReadAllText(JsonPath);
        T instance = JsonUtility.FromJson<T>(json);

        return instance;
    }

    public static T CreateActivityFromInfo<T>(ActivityInfo info) where T : Activity, new()
    {
        T act = new T();
        act.SetInfo(info);

        return act;
    }

    [MenuItem("Activity Info/Generate Json For All Infos")]
    static void GenerateJsons()
    {
        ActivityInfo_Move info_move = new ActivityInfo_Move();
        info_move.movePower = 20;

        ActivityInfo_Jump info_jump = new ActivityInfo_Jump();
        info_jump.jumpMaxPower = 15;
        info_jump.jumpClipName = "smb_jump-small";

        ActivityInfo[] infos = new ActivityInfo[] { info_move, info_jump };

        ToJsonFile(info_move);
        ToJsonFile(info_jump);

        ToJsonFile(infos, "Mario");

        //ActorInfo actorInfo = new ActorInfo("Mario", new ActivityInfo_Move[] { info_move });
        //ActorInfo.CreateJson(actorInfo);

        //Debug.Log(info_move.movePower);
    }

}