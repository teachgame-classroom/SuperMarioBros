using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    public ActivityContainer activities;

    protected void InitContainer()
    {
        activities = new ActivityContainer(this);
    }
}
