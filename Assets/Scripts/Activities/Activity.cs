using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity
{
    protected Actor owner;

    public Activity(Actor owner)
    {
        this.owner = owner;
    }

    public abstract void Update();
}