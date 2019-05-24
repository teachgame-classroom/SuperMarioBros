using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ActivityDictionary
{
    public static Dictionary<string, Type> dict = new Dictionary<string, Type>();
    public static Dictionary<string, IActivityInfo> info_dict = new Dictionary<string, IActivityInfo>(); 


    private static Type[] types = new Type[0];
    private static object[] paras = new object[0];

    public static void Init()
    {
        dict.Add("UpdateAnim", typeof(Activity_UpdateAnim));
        dict.Add("Input", typeof(Activity_Input));
        dict.Add("Raycast", typeof(Activity_Raycast));
        dict.Add("Move", typeof(Activity_Move));
        dict.Add("Direction", typeof(Activity_Direction));
        dict.Add("StateManagement", typeof(Activity_StateManagement));
        dict.Add("Health", typeof(Activity_Health));
        dict.Add("Jump", typeof(Activity_Jump));
        dict.Add("Shoot", typeof(Activity_Shoot));
        dict.Add("Goal", typeof(Activity_Goal));
        dict.Add("SimpleMove", typeof(Activity_SimpleMove));

        info_dict.Add("Move", ActivityInfo.CreateInfoFromJson<ActivityInfo_Move>());
    }

    public static Activity Create(string activityName)
    {
        Type type = dict[activityName];
        Activity act = type.GetConstructor(types).Invoke(paras) as Activity;

        if(info_dict.ContainsKey(activityName))
        {
            Debug.Log("有" + activityName + "的默认参数信息");
            IActivityInfo info = info_dict[activityName];
            act.SetInfo(info);
        }

        return act;
    }

    public static Activity CreateFromJson(string json)
    {
        ActivityInfo info = JsonUtility.FromJson<ActivityInfo>(json);

        Type type = dict[info.name];
        Activity act = type.GetConstructor(types).Invoke(paras) as Activity;
        act.SetJson(json);

        return act;
    }

}
