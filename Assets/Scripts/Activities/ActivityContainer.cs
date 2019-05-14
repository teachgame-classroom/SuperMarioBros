using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActivityContainer
{
    private Actor owner;
    private Dictionary<Type, Activity> activityDict = new Dictionary<Type, Activity>();
    private Type[] type = new Type[] { typeof(Actor) };
    private object[] parameter;

    public ActivityContainer(Actor owner)
    {
        this.owner = owner;
        parameter = new object[] { owner };
    }

    public bool Add<T>() where T : Activity
    {
        Type t = typeof(T);

        if (activityDict.ContainsKey(t))
        {
            return false;
        }
        else
        {
            activityDict.Add(t, (Activity)t.MakeGenericType(t).GetConstructor(type).Invoke(parameter));
            return true;
        }
    }

    public T Get<T>() where T : Activity
    {
        Type t = typeof(T);

        if (activityDict.ContainsKey(t))
        {
            return activityDict[t] as T;
        }
        else
        {
            return null;
        }
    }

    public void Update()
    {
        foreach(Activity act in activityDict.Values)
        {
            act.Update();
        }
    }
}
