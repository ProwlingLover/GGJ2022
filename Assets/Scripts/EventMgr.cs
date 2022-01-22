using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum EventNameEnum
{
    开始战斗 = 10,
    继续战斗 = 20,
    游戏结束 = 30,
}
public class EventName
{
    public static string StartBattle = "开始战斗";
}
public class Example : MonoBehaviour
{
    private void Start()
    {
        //这些别处几乎不会去调用的函数 ，也可以考虑直接用中文函数名，但是需要注意选择支持中文的编码格式 比如UTF-8
        //这里可以直接在参数里写一个函数名，然后选中函数名按住alt+enter 可以选择快速创建一个函数模板
        EventMgr.RegisterEvent("开始战斗", 开始战斗);
        //开发中比较忌讳直接使用一个字符串 这里建议是采取这种字符串变量的形式，1是方便后续修改，2是调用的时候不容易写错，注册的时候也是
        //因为字符串出错了不会提示  你写开始战斗 与战斗开始他都不会报错
        EventMgr.RegisterEvent(EventName.StartBattle, 开始战斗);
        EventMgr.RegisterEvent(EventNameEnum.开始战斗.ToString(), 开始战斗);
        //如果这个游戏物体或者对象并不会被销毁，那么也可以直接偷懒用lamda表达式  
        //lamda表达式注册的事件是无法单独注销的，因为他没有名字， 但是可以通过移除这个事件名来移除这个函数
        EventMgr.RegisterEvent(EventName.StartBattle,a=> { return null; });
    }

    private object 开始战斗(object[] arg)
    {
        throw new NotImplementedException();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventMgr.ExecuteEvent("开始战斗");
            EventMgr.ExecuteEvent(EventName.StartBattle);
            EventMgr.ExecuteEvent(EventNameEnum.开始战斗.ToString());
        }
    }
    private void OnDestroy()
    {
        //把上面注册的代码复制一下，然后在函数明前加一个Un就行
        //如果这个游戏物体或者对象是可能在游戏过程中被销毁的，那么就一定需要在销毁前注销事件，否则执行事件的时候会报对象为空等bug
        //但是如果这个游戏物体或者对象在游戏结束之前一直都存在，那么注销与否影响不大
        EventMgr.UnRegisterEvent("开始战斗", 开始战斗);
        EventMgr.UnRegisterEvent(EventName.StartBattle, 开始战斗);
        EventMgr.UnRegisterEvent(EventNameEnum.开始战斗.ToString(), 开始战斗);

    }
}
public class EventMgr 
{

    private static Dictionary<string, Func<object[], object>> events = new Dictionary<string, Func<object[], object>>();
    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public static void RegisterEvent(string eventName, Func<object[],object> func)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] += func;
        }
        else
        {
            events.Add(eventName, func);
        }          
    }    
    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="o"></param>
    /// <returns></returns>
    public static object ExecuteEvent(string eventName, object[] o = null)
    {
        if (events.ContainsKey(eventName))
        {
            return events[eventName](o);
        }                
        else
        {  
            Debug.LogError("不存在名为 "+eventName+" 的事件，请先添加再执行");
            return null;
        }
    }
    /// <summary>
    /// 注销事件，仅注销这个事件名下的具体某个事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    public static void UnRegisterEvent(string eventName, Func<object[], object> func)
    {
        if (events.ContainsKey(eventName))
        {
            events[eventName] -= func;
        }           
    }
    /// <summary>
    /// 删除某个事件名，这个事件名下的所有事件也就都失效了
    /// </summary>
    /// <param name="eventName"></param>
    public static void DelEvent(string eventName)
    {
        if (events.ContainsKey(eventName))
        {
            events.Remove(eventName);          
        }        
    }
    /// <summary>
    /// 删除全部事件名
    /// </summary>
    public static void DelEventAll()
    {
        events.Clear();
    }
}
