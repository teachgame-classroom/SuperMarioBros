using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity_Input : Activity
{
    public Activity_Input(Actor owner) : base(owner)
    {

    }

    public float h { get; private set; }
    public float v { get; private set; }
    public bool jumpButton { get; private set; }
    public bool fireButton { get; private set; }

    public bool test1 { get; private set; }
    public bool test2 { get; private set; }
    public bool test3 { get; private set; }
    public bool test4 { get; private set; }


    public override void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        jumpButton = Input.GetKeyDown(KeyCode.K);
        fireButton = Input.GetKeyDown(KeyCode.J);

        test1 = Input.GetKeyDown(KeyCode.Alpha1);
        test2 = Input.GetKeyDown(KeyCode.Alpha2);
        test3 = Input.GetKeyDown(KeyCode.Alpha3);
        test4 = Input.GetKeyDown(KeyCode.Alpha4);
    }
}
