using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyEventManager
{
    public static Dictionary<string, Delegate> s_GlobalEventTable = new Dictionary<string, Delegate>();

    public static void RegisterEvent(string eventName, Action handler)
    {
        RegisiterEvent(eventName, (Delegate)handler);
    }

    public static void RegisterEvent<T>(string eventName, Action<T> handler)
    {
        RegisiterEvent(eventName, (Delegate)handler);
    }

    public static void RegisterEvent<T1, T2>(string eventName, Action<T1, T2> handler)
    {
        RegisiterEvent(eventName, (Delegate)handler);
    }

    public static void RegisterEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> handler)
    {
        RegisiterEvent(eventName, (Delegate)handler);
    }

    public static void RegisterEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> handler)
    {
        RegisiterEvent(eventName, (Delegate)handler);
    }

    private static void RegisiterEvent(string eventName, Delegate handler)
    {
        Delegate prevHandler;

        if (s_GlobalEventTable.TryGetValue(eventName, out prevHandler))  // 如果之前已经有方法在监听此事件
        {
            Delegate combineHandler = Delegate.Combine(prevHandler, handler);
            s_GlobalEventTable[eventName] = combineHandler;
        }
        else    // 如果之前没有方法在监听此事件
        {
            s_GlobalEventTable.Add(eventName, (Delegate)handler);
        }
    }

    public static void ExecuteEvent(string eventName)
    {
        Delegate handler;

        if (s_GlobalEventTable.TryGetValue(eventName, out handler))  // 根据事件名称从字典查找已经登录的监听方法
        {
            if(handler != null)
            {
                Action action = handler as Action;
                action();
            }
        }
    }

    public static void ExecuteEvent<T>(string eventName, T arg1)
    {
        Delegate handler;

        if (s_GlobalEventTable.TryGetValue(eventName, out handler))  // 根据事件名称从字典查找已经登录的监听方法
        {
            if (handler != null)
            {
                Action<T> action = handler as Action<T>;
                action(arg1);
            }
        }
    }

    public static void ExecuteEvent<T1, T2>(string eventName, T1 arg1, T2 arg2)
    {
        Delegate handler;

        if (s_GlobalEventTable.TryGetValue(eventName, out handler))  // 根据事件名称从字典查找已经登录的监听方法
        {
            if (handler != null)
            {
                Action<T1,T2> action = handler as Action<T1,T2>;
                action(arg1,arg2);
            }
        }
    }

    public static void ExecuteEvent<T1, T2, T3>(string eventName, T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate handler;

        if (s_GlobalEventTable.TryGetValue(eventName, out handler))  // 根据事件名称从字典查找已经登录的监听方法
        {
            if (handler != null)
            {
                Action<T1,T2,T3> action = handler as Action<T1,T2,T3>;
                action(arg1,arg2,arg3);
            }
        }
    }

    public static void ExecuteEvent<T1, T2, T3, T4>(string eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate handler;

        if (s_GlobalEventTable.TryGetValue(eventName, out handler))  // 根据事件名称从字典查找已经登录的监听方法
        {
            if (handler != null)
            {
                Action<T1, T2, T3, T4> action = handler as Action<T1, T2, T3, T4>;
                action(arg1, arg2, arg3, arg4);
            }
        }
    }


    public static void UnregisterEvent(string eventName, Action handler)
    {
        UnregisiterEvent(eventName, (Delegate)handler);
    }


    public static void UnregisterEvent<T>(string eventName, Action<T> handler)
    {
        UnregisiterEvent(eventName, (Delegate)handler);
    }

    public static void UnregisterEvent<T1,T2>(string eventName, Action<T1,T2> handler)
    {
        UnregisiterEvent(eventName, (Delegate)handler);
    }

    public static void UnregisterEvent<T1,T2,T3>(string eventName, Action<T1,T2,T3> handler)
    {
        UnregisiterEvent(eventName, (Delegate)handler);
    }

    public static void UnregisterEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> handler)
    {
        UnregisiterEvent(eventName, (Delegate)handler);
    }

    private static void UnregisiterEvent(string eventName, Delegate handler)
    {
        Delegate prevHandler;

        if (s_GlobalEventTable.TryGetValue(eventName, out prevHandler))
        {
            Delegate removedHandler = Delegate.Remove(prevHandler, handler);
            s_GlobalEventTable[eventName] = removedHandler;
        }
    }
}
