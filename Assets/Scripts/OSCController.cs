using UnityEngine;
using System;
using UnityEngine.Events;
using UnityOSC;

public class OSCController : MonoBehaviour
{
    [Serializable, HideInInspector]
    public class DiceEvent : UnityEvent<Marker>
    {
    }
    public DiceEvent onNewOSCData;

    void Start()
    {
        //init UnityOSC (OSCHandler)
        OSCHandler.Instance.Init(); 
    }

    void Update()
    {
        OSCPacket packet;
        while (OSCHandler.Instance.InputBuffer.Count > 0)
        {
            packet = OSCHandler.Instance.InputBuffer.Dequeue();

            int eventType = Int32.Parse(packet.Data[0].ToString());
            int diceType = Int32.Parse(packet.Data[1].ToString());
            int diceId = Int32.Parse(packet.Data[2].ToString());
            int diceProjectedX = Int32.Parse(packet.Data[3].ToString());
            int diceProjectedY = Int32.Parse(packet.Data[4].ToString());

            Marker newDice = new Marker(eventType, diceType, diceId, diceProjectedX, diceProjectedY);

            //Event invoked when new dice dice data arrives
            onNewOSCData.Invoke(newDice);
        }
    }

}