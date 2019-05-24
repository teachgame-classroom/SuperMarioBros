using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ActorInfo
{
    public string name;
    public ActivityInfo_Move[] activityInfos;

    public ActorInfo(string name, ActivityInfo_Move[] infos)
    {
        this.name = name;
        this.activityInfos = infos;
    }

    public static void CreateJson(ActorInfo info)
    {
        string json = JsonUtility.ToJson(info);
        File.WriteAllText(Application.streamingAssetsPath + "/" + info.name + ".txt", json);
    }

    public static ActorInfo CreateInfoFromJson(string name)
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + "/" + name + ".txt");
        ActorInfo info = JsonUtility.FromJson<ActorInfo>(json);

        return info;
    }
}
