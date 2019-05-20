using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTools;

public class Activity_Input : Activity
{
    public System.Action<float, float> onAxis;

    public System.Action onButtonDown_Fire;
    public System.Action onButtonDown_Jump;

    public System.Action onButtonDown_Test1;
    public System.Action onButtonDown_Test2;
    public System.Action onButtonDown_Test3;
    public System.Action onButtonDown_Test4;

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
        if (onButtonDown_Jump != null && Input.GetKeyDown(KeyCode.K)) onButtonDown_Jump();
        if (onButtonDown_Fire != null && Input.GetKeyDown(KeyCode.J)) onButtonDown_Fire();
        if (onButtonDown_Test1 != null && Input.GetKeyDown(KeyCode.Alpha1)) onButtonDown_Test1();
        if (onButtonDown_Test2 != null && Input.GetKeyDown(KeyCode.Alpha2)) onButtonDown_Test2();
        if (onButtonDown_Test3 != null && Input.GetKeyDown(KeyCode.Alpha3)) onButtonDown_Test3();
        if (onButtonDown_Test4 != null && Input.GetKeyDown(KeyCode.Alpha4)) onButtonDown_Test4();

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (onAxis != null) onAxis(h,v);


        //fireButton = Input.GetKeyDown(KeyCode.J);

        //test1 = Input.GetKeyDown(KeyCode.Alpha1);
        //test2 = Input.GetKeyDown(KeyCode.Alpha2);
        //test3 = Input.GetKeyDown(KeyCode.Alpha3);
        //test4 = Input.GetKeyDown(KeyCode.Alpha4);
    }
}
