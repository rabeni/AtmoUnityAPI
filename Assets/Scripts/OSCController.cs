using UnityEngine;
using System;
using UnityEngine.Events;
using UnityOSC;
using DisruptorUnity3d;

public class OSCController : MonoBehaviour
{
    [Serializable, HideInInspector]
    public class DiceEvent : UnityEvent<Marker>
    {
    }
    public DiceEvent onNewOSCData;

    private RingBuffer<OSCPacket> _inputBuffer = new RingBuffer<OSCPacket>(15);

    void Start()
    {
        //init UnityOSC (OSCHandler)
        OSCHandler.Instance.Init(); 
        OSCHandler.Instance.CreateServer("AtmoTracking", 7771);
        OSCHandler.Instance.Servers["AtmoTracking"].server.PacketReceivedEvent += OnPacketReceived;
    }

    void Update()
    {
        OSCPacket packet;
        while (_inputBuffer.Count > 0)
        {
            packet = _inputBuffer.Dequeue();

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

    //Puts incoming packets into a buffer that is read by OSCController
    void OnPacketReceived(OSCServer server, OSCPacket packet)
    {
        if (packet.Address == "/tracking")
        {

            _inputBuffer.Enqueue(packet);

        }
    }

}