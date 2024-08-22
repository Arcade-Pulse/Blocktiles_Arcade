using System;
using System.Collections;
using System.Collections.Generic;
using Backtrace.Unity;
using Backtrace.Unity.Model;
using UnityEngine;

public class BackTraceTest : MonoBehaviour
{
    private void OnEnable()
    {
        //Read from manager BacktraceClient instance
        var backtraceClient = GameObject.Find("BackTrace").GetComponent<BacktraceClient>();

        //Set custom client attribute
        backtraceClient["attribute"] = "attribute value";

        //Read from manager BacktraceClient instance
        var database = GameObject.Find("BackTrace").GetComponent<BacktraceDatabase>();


        try{
            //throw exception here
        }
        catch(Exception exception){
            var report = new BacktraceReport(exception);
            backtraceClient.Send(report);
        }
    }
}
