using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityContainer
{
    Actor owner;
    List<Activity> actList;

    public ActivityContainer(Actor owner)
    {
        actList = new List<Activity>();
        this.owner = owner;
    }

    public void Update()
    {
        foreach( Activity activity in actList )
        {
            activity.Update();
        }
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
