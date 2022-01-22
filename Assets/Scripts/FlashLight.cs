using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum LightEventType { Enter, Stay, Exit }

public class LightEventData
{
    public string name;
    public Action<RaycastHit>[] methods;

    public LightEventData(string na, Action<RaycastHit>[] meths)
    {
        name = na;
        methods = meths;
    }
}

public class RayData
{
    public Vector3 position;
    public Vector3 direction;
    public float distance;

    public RayData(Vector3 dir,Light light)
    {
        position = light.transform.position + direction * distance;
        direction = dir;
        distance = light.range;
    }
    public RayData(float x,float y,Light light)
    {
        float radius = Mathf.Tan(Mathf.Deg2Rad * light.spotAngle / 2) * light.range;

        position =light.transform.position +light.transform.forward 
            * light.range +light.transform.rotation 
            * new Vector3(x,y) * radius;

        direction = Vector3.Normalize(position -light.transform.position);
        distance=Vector3.Distance(position,light.transform.position);
    }
}

public class HitInfoList:List<RaycastHit>
{
    public HitInfoList() { }
    public HitInfoList(HitInfoList copy)
    {
        foreach (var elem in copy)
            this.Add(elem);
    }
    public bool AddElement(RaycastHit info)
    {
        RaycastHit result;
        if (!ExistElement(info,out result))
        {
            this.Add(info);
            return true;
        }
        return false;
    }

    public bool RemoveElement(RaycastHit info)
    {
        RaycastHit result;
        if (ExistElement(info, out result))
        {
            this.Remove(result);
            return true;
        }
        return false;
    }

    public HitInfoList GetSubList(HitInfoList another)
    {
        HitInfoList list = new HitInfoList(this);
        foreach(var elem in another)
        {
            RaycastHit result;
            if (ExistElement(elem, out result))
                list.Remove(result);
        }
        return list;
    }

    public bool ExistElement(RaycastHit info,out RaycastHit result)
    {
        foreach (var elem in this)
        {
            string iName = info.collider.name;
            string eName = elem.collider.name;
            if (iName.Equals(eName))
            {
                result = elem;
                return true;
            }
        }
        result = new RaycastHit();
        return false;
    }
}

public class FlashLight : MonoBehaviour {


    public float dampSpeed=0.5f;
    public float rangeOffset=1f;
    public bool useMultipleRay=true;

    [Range(0.05f,1f)]
    public float sectionOfMultipleRay=0.05f;

    Light light;
    float dampTmp;
    float maxLightDistance;

    static HitInfoList InfoList = new HitInfoList();

    void Start()
    {
        light = GetComponent<Light>();
        maxLightDistance = light.range;
    }

    void Update()
    {
        ControlLightRange();
        DetectObject();
    }

    public void Switch(bool switchOn) { light.enabled = switchOn; }

    void ControlLightRange()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit info;
        float distance;

        if (Physics.Raycast(ray, out info))
        {
            distance = Mathf.Clamp(info.distance + rangeOffset, 0, maxLightDistance);
            Debug.DrawLine(transform.position, info.point);
        }
        else
            distance = maxLightDistance;

        light.range = Mathf.SmoothDamp(light.range, distance, ref dampTmp, dampSpeed);
    }

    void DetectObject()
    {
        HitInfoList list = new HitInfoList();

        foreach (var data in GetRayData())
        {         
            Ray ray = new Ray(transform.position, data.direction);
            RaycastHit info;

            if (Physics.Raycast(ray, out info,data.distance))
                list.AddElement(info);

            Debug.DrawLine(transform.position, data.position, Color.red);
        }

        int operate = (list.Count == InfoList.Count) ? 0
            : ((list.Count > InfoList.Count) ? 1 : -1);

        HandleEvent(list, operate);
    }

    void HandleEvent(HitInfoList current,int operate)
    {

        foreach (var elem in current)
        {
            if (operate != -1 && InfoList.AddElement(elem))
                LightEventListener.EventHandler(elem, LightEventType.Enter);
        }

        foreach(var elem in InfoList.GetSubList(current))
        {
            if (operate != 1 && InfoList.RemoveElement(elem))
                LightEventListener.EventHandler(elem, LightEventType.Exit);
        }

        foreach (var elem in InfoList)
            LightEventListener.EventHandler(elem, LightEventType.Stay);
    }

    List<RayData> GetRayData()
    {
        List<RayData> list = new List<RayData>();

        if (useMultipleRay)
            list.AddRange(GetFullCircleRay(new int[] { 1, -1 },
                new int[] { 1, -1 },sectionOfMultipleRay,light));
        else
            list.Add(new RayData(transform.forward,light));

        return list;
    }

    List<RayData> GetFullCircleRay(int[] xParams, int[] yParams, float section, Light light)
    {
        List<RayData> list = new List<RayData>();

        foreach (var x in xParams)
            foreach (var y in yParams)
                list.AddRange(GetQuarterCircleRay(x, y, section,light));

        return list;
    }

    List<RayData> GetQuarterCircleRay(int xParam, int yParam,float section,Light light)
    {
        float x = 0, y = 0;    
        List<RayData> list = new List<RayData>();

        while ((yParam > 0) ? y <= 1 : y >= -1)
        {
            while (x * x + y * y <= 1)
            {            
                list.Add(new RayData(x,y,light));
                x += xParam*section;
            }
            x = 0;
            y += yParam * section;
        }

        return list;
    }

}

public abstract class LightEventListener : MonoBehaviour
{

    private static List<LightEventData> ListenerList = new List<LightEventData>();

    public static void EventHandler(RaycastHit info, LightEventType type)
    {
        foreach (var listener in ListenerList)
        {
            string lName = listener.name;
            string iName = info.collider.name;
            Action<RaycastHit> Method = listener.methods[(int)type];

            if (lName.Equals(iName) && Method != null)
            {
                Method(info);
            }
        }
    }

    protected abstract void OnLightEnter(RaycastHit info);
    protected abstract void OnLightStay(RaycastHit info);
    protected abstract void OnLightExit(RaycastHit info);

    void OnEnable()
    {
        LightEventData data = new LightEventData(name,
            new Action<RaycastHit>[] { OnLightEnter, OnLightStay, OnLightExit });

        ListenerList.Add(data);
    }

}
