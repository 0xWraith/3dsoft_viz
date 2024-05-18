using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

public class InteractionTimer : MonoBehaviour
{
    enum InteractionEvent
    {
        graph = 0,
        menu  = 1,
        restriciton = 2,
        magnet = 3
    }

    public float graphInteractionTotal = 0;
    public float graphInteractionTimer = 0;

    public float menuInteractionTotal = 0;
    public float menuInteractionTimer = 0;

    public float restInteractionTotal = 0;
    public float restInteractionTimer = 0;

    public float magInteractionTotal = 0;
    public float magInteractionTimer = 0;

    public List<string> logs = new List<string>();

    //hotfix(hrumy)
    private bool firstTime = true; 

    public void LogEvent(int interactionEvent, bool start, int meta_id = -1) 
    {
        switch (interactionEvent)
        {
            case (int)InteractionEvent.graph:
            {
                logs.Add(DateTime.Now.TimeOfDay + ": Graph interaction" + (start ? " start" : " end"));
                break;
            }
            case (int)InteractionEvent.menu:
            {
                logs.Add(DateTime.Now.TimeOfDay + ": Menu interaction" + (start ? " start" : " end"));
                break;
            }
            case (int)InteractionEvent.restriciton:
            {
                logs.Add(DateTime.Now.TimeOfDay + ": Restriction (id = " + meta_id + ") interaction" + (start ? " start" : " end"));
                break;
            }
            case (int)InteractionEvent.magnet:
            {
                logs.Add(DateTime.Now.TimeOfDay + ": Magnet (id = " + meta_id + ") interaction" + (start ? " start" : " end"));
                break;
            }
            default:
                break;
        }
    }

    public void StartGraphTimer()
    {
        graphInteractionTimer = Time.realtimeSinceStartup;
        LogEvent((int)InteractionEvent.graph, true);
    }

    public void EndGraphTimer()
    {
        graphInteractionTotal += Time.realtimeSinceStartup - graphInteractionTimer;
        LogEvent((int)InteractionEvent.graph, false);
    }


    public void StartMenuTimer()
    {
        menuInteractionTimer = Time.realtimeSinceStartup;
        LogEvent((int)InteractionEvent.menu, true);
    }

    public void EndMenuTimer()
    {
        if (firstTime)
        {
            firstTime = false;
            return;
        }

        menuInteractionTotal += Time.realtimeSinceStartup - menuInteractionTimer;
        LogEvent((int)InteractionEvent.menu, false);
    }

    public void StartRestricitonTimer(int rest_id)
    {
        restInteractionTimer = Time.realtimeSinceStartup;
        LogEvent((int)InteractionEvent.restriciton, true, rest_id);
    }

    public void EndRestrictionTimer(int rest_id)
    {
        restInteractionTotal += Time.realtimeSinceStartup - restInteractionTimer;
        LogEvent((int)InteractionEvent.restriciton, false, rest_id);
    }

    public void StartMagnetTimer(int mag_id)
    {
        magInteractionTimer = Time.realtimeSinceStartup;
        LogEvent((int)InteractionEvent.magnet, true, mag_id);
    }

    public void EndMagnetTimer(int mag_id)
    {
        magInteractionTotal += Time.realtimeSinceStartup - magInteractionTimer;
        LogEvent((int)InteractionEvent.magnet, false, mag_id);
    }

    private void OnApplicationQuit() {
        var fs = new FileStream("E:\\Hromada\\logs\\" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".txt", FileMode.CreateNew, FileAccess.ReadWrite);
        using (var sw = new StreamWriter(fs))
        {
            sw.WriteLine("Total time:                     " + Time.realtimeSinceStartup + "s\n");
            sw.WriteLine("Graph Interaction total:        " + graphInteractionTotal + "s");
            sw.WriteLine("Menu interaction total:         " + menuInteractionTotal + "s");
            sw.WriteLine("Restrictions interaction total: " + restInteractionTotal + "s");
            sw.WriteLine("Magnets interaction total:      " + magInteractionTotal + "s\n");

            foreach (var log in logs)
            {
                sw.WriteLine(log);
            }
        }
    }
}   