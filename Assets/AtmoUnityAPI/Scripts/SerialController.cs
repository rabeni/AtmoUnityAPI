/*
  SerialController.cs - This script handles the serial communication 
  between Unity and Atmo lights. It periodically fills the outputQueue with
  led color data that is periodically sent in a separate thread via a serial 
  port.
  Created by Atmo, February 2, 2018.
*/

using System.Collections;
using UnityEngine;
using System.Threading;
using System.Linq;
using System.Runtime.Remoting.Messaging;

public class SerialController : MonoBehaviour
{

    private static SerialController instance;

    private Thread thread;
    private Queue outputQueue;

    private const float periodSec = 1f / 30f;
    private byte[] buffer;
    private Strip strip;

    void Start()
    {
        // needs to be singleton otherwise it starts a new thread on every new scene
        if (instance == null)
        {
            instance = this;

            strip = GetComponent<Strip>();
            StartThread();
        }
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());

        StartCoroutine(FillOuputQueuePeriodically());

        // Creates and starts the thread
        thread = new Thread(ThreadLoop);
        thread.Start();
    }

    private void ThreadLoop()
    {
        int ms = (int)(periodSec * 1000);
        while (true)
        {
            Thread.Sleep(ms);
            if (outputQueue.Count != 0)
            {
                AtmoSerial.Write((byte[])outputQueue.Dequeue());
            }
        }
    }

    IEnumerator FillOuputQueuePeriodically()
    {
        //delaying the start of serial communication
        yield return new WaitForSeconds(0.1f);

        while (true)
        {
            outputQueue.Enqueue(ProcessBytesForSending());
            yield return new WaitForSeconds(periodSec);
        }
    }

    private byte[] ProcessBytesForSending()
    {
        // header, brightness, color bytes
        byte[] header = new byte[] { 0xff };
        buffer = header.Concat(strip.GetColorBytes()).ToArray();
        return buffer;
    }
}
