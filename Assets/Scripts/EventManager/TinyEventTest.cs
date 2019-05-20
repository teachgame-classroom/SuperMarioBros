using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyEventTest : MonoBehaviour
{
    public bool sendTest1;

    // Start is called before the first frame update
    void Start()
    {
    }

    void OnEnable()
    {
        if (sendTest1)
        {
            TinyEventManager.RegisterEvent("TinyTest2", OnTest2);
            TinyEventManager.RegisterEvent<int>("Int TinyTest2", OnTest2);
            TinyEventManager.RegisterEvent<float>("Float TinyTest2", OnTest2);
            TinyEventManager.RegisterEvent<TinyEventTest>("Tiny TinyTest2", OnTest2);
            TinyEventManager.RegisterEvent<int, float, bool, string>("All TinyTest2", OnTest2);
        }
        else
        {
            TinyEventManager.RegisterEvent("TinyTest1", OnTest1);
            TinyEventManager.RegisterEvent<int>("Int TinyTest1", OnTest1);
            TinyEventManager.RegisterEvent<float>("Float TinyTest1", OnTest1);
            TinyEventManager.RegisterEvent<TinyEventTest>("Tiny TinyTest1", OnTest1);
            TinyEventManager.RegisterEvent<int, float, bool, string>("All TinyTest1", OnTest1);
        }
    }

    void OnDisable()
    {
        if (sendTest1)
        {
            TinyEventManager.UnregisterEvent("TinyTest2", OnTest2);
            TinyEventManager.UnregisterEvent<int>("Int TinyTest2", OnTest2);
            TinyEventManager.UnregisterEvent<float>("Float TinyTest2", OnTest2);
            TinyEventManager.UnregisterEvent<TinyEventTest>("Tiny TinyTest2", OnTest2);
            TinyEventManager.UnregisterEvent<int, float, bool, string>("All TinyTest2", OnTest2);
        }
        else
        {
            TinyEventManager.UnregisterEvent("TinyTest1", OnTest1);
            TinyEventManager.UnregisterEvent<int>("Int TinyTest1", OnTest1);
            TinyEventManager.UnregisterEvent<float>("Float TinyTest1", OnTest1);
            TinyEventManager.UnregisterEvent<TinyEventTest>("Tiny TinyTest1", OnTest1);
            TinyEventManager.UnregisterEvent<int, float, bool, string>("All TinyTest1", OnTest1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            if(sendTest1)
            {
                TinyEventManager.ExecuteEvent("TinyTest1");
                TinyEventManager.ExecuteEvent<int>("Int TinyTest1", 1);
                TinyEventManager.ExecuteEvent<float>("Float TinyTest1", 1.0f);
                TinyEventManager.ExecuteEvent<TinyEventTest>("Tiny TinyTest1", this);
                TinyEventManager.ExecuteEvent<int, float, bool, string>("All TinyTest1", 1, 1.0f, true, "test1");
            }
            else
            {
                TinyEventManager.ExecuteEvent("TinyTest2");
                TinyEventManager.ExecuteEvent<int>("Int TinyTest2", 2);
                TinyEventManager.ExecuteEvent<float>("Float TinyTest2", 2.0f);
                TinyEventManager.ExecuteEvent<TinyEventTest>("Tiny TinyTest2", this);
                TinyEventManager.ExecuteEvent<int, float, bool, string>("All TinyTest2", 2, 2.0f, false, "test2");
            }
        }
    }


    void OnTest1()
    {
        Debug.Log(gameObject.name + " received Test 1");
    }

    void OnTest1(int a)
    {
        Debug.Log(gameObject.name + " received int test1:" + a);
    }

    void OnTest1(float a)
    {
        Debug.Log(gameObject.name + " received float test1:" + a);
    }

    void OnTest1(TinyEventTest a)
    {
        Debug.Log(gameObject.name + " received TinyEventTest test1:" + a.sendTest1);
    }

    void OnTest1(int i, float f, bool b, string s)
    {
        Debug.Log(gameObject.name + string.Format(" test1, i:{0}, f:{1}, b:{2}, s:{3}", i, f, b, s));
    }

    void OnTest2()
    {
        Debug.Log(gameObject.name + " received Test 2");
    }

    void OnTest2(int a)
    {
        Debug.Log(gameObject.name + " received int test2:" + a);
    }

    void OnTest2(float a)
    {
        Debug.Log(gameObject.name + " received float test2:" + a);
    }

    void OnTest2(TinyEventTest a)
    {
        Debug.Log(gameObject.name + " received TinyEventTest test2:" + a.sendTest1);
    }

    void OnTest2(int i, float f, bool b, string s)
    {
        Debug.Log(gameObject.name + string.Format(" test2, i:{0}, f:{1}, b:{2}, s:{3}", i, f, b, s));
    }
}
