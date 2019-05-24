using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity
{
    protected ActivityContainer container;
    protected Actor owner;

    public Activity()
    {

    }

    public Activity(Actor owner)
    {
        SetOwner(owner);
    }

    public virtual void SetOwner(Actor owner)
    {
        this.owner = owner;
    }

    public void SetContainer(ActivityContainer activityContainer)
    {
        this.container = activityContainer;
    }

    public virtual void SetInfo(IActivityInfo info)
    {

    }

    public virtual void SetJson(string json)
    {

    }

    public abstract void Update();
}