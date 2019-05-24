using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ActivityContainer
{
    Actor owner;
    List<Activity> actList;

    public ActivityContainer(Actor owner, string name)
    {
        actList = new List<Activity>();
        this.owner = owner;

        string fileName = "Activity_" + name + ".txt";

        string[] jsons = File.ReadAllLines(Application.streamingAssetsPath + "/" + fileName);

        foreach(string json in jsons)
        {
            Add(ActivityDictionary.CreateFromJson(json));
        }
    }

    public void Update()
    {
        foreach( Activity activity in actList )
        {
            activity.Update();
        }
    }

    public void Add(Activity newActivity)
    {
        newActivity.SetContainer(this);
        newActivity.SetOwner(this.owner);

        actList.Add(newActivity);
    }

    public void Create<T>() where T : Activity, new()
    {
        T newActivity = new T();

        newActivity.SetContainer(this);
        newActivity.SetOwner(this.owner);

        actList.Add(newActivity);
    }

    public T Get<T>() where T : Activity
    {
        foreach(Activity activity in actList)
        {
            if(activity.GetType() == typeof(T))
            {
                return activity as T;
            }
        }

        return null;
    }
}
