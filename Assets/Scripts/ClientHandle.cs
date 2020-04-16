using System.Collections;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}"+"you are player no."+_myId);
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        Debug.Log("Packet to spawn Player " + _username + " recieved");

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);

    }

    public static void EndGame(Packet _packet)
    {
        Debug.Log("EndGame");

        int _id = _packet.ReadInt();
        Debug.Log("EndGame2");


        GameManager.instance.GameOver(_id);

    }




     public static void PlayerDown(Packet _packet)
    {

        Debug.Log("plYER DWB");



        int _id = _packet.ReadInt();

        Debug.Log("PLAYERDWB 2");

        GameManager.instance.PlayAudioFile(GameManager.instance.ShaveKO , GameManager.players[_id] );
        Debug.Log("PLAYERDWB 2");

        GameManager.instance.PlayerDowned(_id);
    }

    public static void RemovePlayer (Packet _packet)
    {
        int _id = _packet.ReadInt();

        if (GameManager.players.ContainsKey(_id))
        {
            Debug.Log("Player " + GameManager.players[_id].username + " left");



            Destroy(GameManager.players[_id].gameObject);
            GameManager.players.Remove(_id);
            GameManager.instance.StatusDEBUG();
        }

    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        try
        {

            GameManager.players[_id].transform.position = _position;
        }
        catch (Exception _ex)
        {
            // Debug.Log("Exception: " + _ex);
            Debug.Log(".");
        }
    }

    public static void Hp(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _hp = _packet.ReadInt();
        try
        {
            GameManager.players[_id].hp = _hp;
        }
        catch (Exception _ex)
        {
            // Debug.Log("Exception: " + _ex);
            Debug.Log(".");
        }

    }

    public static void PlayerAttacked(Packet _packet)
    {
        int _attackerId = _packet.ReadInt();
        int _targetId = _packet.ReadInt();

        {
            if (_attackerId == Client.instance.myId)
            {
                GameManager.instance.PlayAudioFile(GameManager.instance.ShavePunch, GameManager.players[_attackerId]);
            }
            else
            {
                GameManager.instance.PlayAudioFile(GameManager.instance.ShaveOuch, GameManager.players[_attackerId]);

            }
            UIManager.instance.HitAnOpp();

            GameManager.instance.PlayAudioFile(GameManager.instance.ShaveHit, GameManager.players[_targetId]);

            GameManager.instance.PlayAudioFile(GameManager.instance.ShaveUgh, GameManager.players[_targetId]);                                                                                                

        }
     
        Debug.Log(GameManager.players[_attackerId].gameObject.GetComponent<PlayerManager>().username + " hit " + GameManager.players[_targetId].gameObject.GetComponent<PlayerManager>().username);
        
    }

    public static void PlayerMissed(Packet _packet)
    {
        int _attackerId = _packet.ReadInt();
        if (_attackerId == Client.instance.myId)
        {
            GameManager.instance.PlayAudioFile(GameManager.instance.ShavePunch, GameManager.players[_attackerId]);
        }
        else
        {
            GameManager.instance.PlayAudioFile(GameManager.instance.ShaveOuch, GameManager.players[_attackerId]);

        }
        Debug.Log(GameManager.players[_attackerId].gameObject.GetComponent<PlayerManager>().username + " missed.");
        
    }




    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        try
        {
            GameManager.players[_id].transform.rotation = _rotation;
        }
        catch(Exception _ex)
        {
           // Debug.Log("Exception: " + _ex);
            Debug.Log(".");
        }
    }
}
