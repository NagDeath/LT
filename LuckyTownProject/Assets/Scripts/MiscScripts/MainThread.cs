/* Attach this to any object in your scene, to make it work */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainThread : MonoBehaviour
{

    private class CallInfo
    {
        private Action action;

        public CallInfo(Action action)
        {
            this.action = action;
        }

        public void Execute()
        {
            action();
        }
    }

    private static List<CallInfo> calls = new List<CallInfo>();

    private static object callsLock = new object();

    private void Start()
    {
        calls = new List<CallInfo>();

        StartCoroutine(Executer());
    }

    /// <summary>
    /// если метод не имеет параметров
    /// MainThread.Call(Method);
    /// если имеет параметры
    /// MainThread.Call(() => Method(parameter, parameter, etc...));
    /// </summary>
    public static void Call(Action action)
    {
        lock (callsLock)
        {
            calls.Add(new CallInfo(action));
        }
    } 

    private IEnumerator Executer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            while (calls.Count > 0)
            {
                try
                {
                    calls[0].Execute();
                }
                catch(Exception e)
                {
                    print(calls[0].ToString() + "  " + e);
                }
                lock (callsLock)                
                {
                    calls.RemoveAt(0);
                }
            }
        }
    }
}
