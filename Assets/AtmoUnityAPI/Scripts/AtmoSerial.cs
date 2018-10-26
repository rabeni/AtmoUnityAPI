/*
  AtmoSerial.cs - This script modifies the behaviour of Serial.cs for Atmo.
  It makes sure that we connect to the right port.
  Created by Atmo, February 2, 2018.
*/
using System.Collections;
using Boo.Lang;
using UnityEngine;
using System.IO.Ports;

public class AtmoSerial : Serial
{
    private string _serialId = "";
    private static bool _isValidatingSerial = false;

    public static bool IsValidatingSerial
    {
        get { return _isValidatingSerial; }
    }

    /// <summary>
    /// Verify if the serial port is opened and opens it if necessary.
    /// In addition it makes sure to connect to Atmo Teensy's port.
    /// </summary>
    /// <returns><c>true</c>, if the right port is opened, <c>false</c> otherwise.</returns>
    /// <param name="portSpeed">Port speed.</param>
    protected override bool checkOpen(int portSpeed = BAUD)
    {
        // if no serial has been opened and validation is not ongoing
        if (s_serial == null && !_isValidatingSerial)
        {

            List<string> portNames = GetPortName();

            if (portNames.Count == 0)
            {
                Debug.Log("Cannot find any open serial ports. Check if USB is connected.");
            }
            else
            {
                StartCoroutine(ValidateSerial(portNames, portSpeed));
            }

            return false;

        }
        
        // only return s_serial.IsOpen if there's no ongoing validation
        if (_isValidatingSerial) return false;
        return s_serial.IsOpen;
    }

    IEnumerator ValidateSerial(List<string> portNames, int portSpeed)
    {
        // start listening to incoming serial messages
        NotifyLines = true;
        _isValidatingSerial = true;
        string found = "";

        foreach (string portName in portNames)
        {
            _serialId = "";
            
            // close port if it's open, before opening another. Otherwise 
            if (s_serial != null) s_serial.Close();
            
            s_serial = new SerialPort(portName, portSpeed);

            s_serial.Open();
            //s_serial.WriteTimeout = -1 in default, which means infinite
            
            if (s_debug)
                Debug.Log("Looking for Atmo on port: " + portName + Time.realtimeSinceStartup);

            if (!s_serial.IsOpen)
            {
                Debug.Log("Post " + portName + " is not open. Trying next.");
                continue;
            }

             //clear input buffer from previous garbage
            s_serial.DiscardInBuffer();

            float t = Time.realtimeSinceStartup;
            
            // wait until something arrives from a serial device or 2 sec elapses 
            yield return new WaitUntil(() => !_serialId.Equals("") || Time.realtimeSinceStartup-t > 2f);

            // this never happens. teensy is sending atmo continously.
            if (_serialId.Equals("Atmo\r"))
            {
                found = portName;
                break;
            }

        }
        
        if (!found.Equals("")) Debug.Log("Atmo is connected on port " + found);
        else Debug.Log("Did not receive the right serial id on either of the possible ports. Check if USB is connected.");
        
        // stop listening to incoming serial messages
        NotifyLines = false;
        _isValidatingSerial = false;
    }
    
    private new List<string> GetPortName()
    {

        string[] portNames;
        List<string> portNameList = new List<string>();

        switch (Application.platform)
        {
            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.LinuxEditor:

                portNames = System.IO.Directory.GetFiles("/dev/");

                foreach (string portName in portNames)
                {
                    if (portName.StartsWith("/dev/ttyACM"))
                        portNameList.Add(portName);
                }

                break;
            
            default:
                Debug.LogWarning("GetPortname is not prepared for this platform. Only works with Linux.");
                break;
        }
        return portNameList;
    }

    void OnSerialLine(string line)
    {
        _serialId = line;
    }

}
