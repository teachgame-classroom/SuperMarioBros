using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IActivityOwner
{

    public Dictionary<Type, Activity> activities { get { return _activities; } }

    protected Dictionary<Type, Activity> _activities = new Dictionary<Type, Activity>();
}
