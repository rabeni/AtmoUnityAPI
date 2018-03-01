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

public class SerialController : MonoBehaviour {

    private Thread thread;
    private Queue outputQueue; 

    private const float periodSec = 0.02f;
    private byte[] buffer;
    private Strip strip;

    void Start()
    {
        strip = GetComponent<Strip>();

        StartThread();
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
        while (true)
        {
            if (outputQueue.Count != 0)
            {
                Serial.Write((byte[])outputQueue.Dequeue());
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
