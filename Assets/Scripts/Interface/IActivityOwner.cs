using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IActivityOwner
{
    Dictionary<Type, Activity> activities { get; }
}
