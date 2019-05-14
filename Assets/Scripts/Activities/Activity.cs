using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity
{
    protected Actor owner;
    protected ActivityContainer activities;

    public Activity(Actor owner)
    {
        this.owner = owner;
        activities = owner.activities;
    }

    protected abstract bool Evaluate();

    protected abstract void DoActivity();

    public void Update()
    {
        if(Evaluate())
        {
            DoActivity();
        }
    }
}