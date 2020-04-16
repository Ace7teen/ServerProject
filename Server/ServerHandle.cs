using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient} called {_username}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }
            Quaternion _rotation = _packet.ReadQuaternion();
            if (Server.clients[_fromClient].player !=null)
            {
                Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
            }
        }

        public static void PlayerAttack(int _fromClient, Packet _packet)
        {
            int _id = _packet.ReadInt();
            if (Server.clients[_fromClient].player != null)
            {
                ServerSend.PlayerAttacked(Server.clients[_fromClient].player, Server.clients[_id].player);

                Server.clients[_id].player.hp -= 1;

                if(Server.clients[_id].player.hp <= 0)
                {
                    ServerSend.PlayerDown(Server.clients[_id].player);

                    int i = 0;
                    Player winner = null;
                    foreach (Client _client in Server.clients.Values)
                    {
                        if (_client.player != null && _client.player.hp > 0)
                        {
                            i++;
                            winner = _client.player;
                        }
                    }
                    if (i == 1) {
                        ServerSend.EndGame(winner);
                        // ELIMINATE ALL PLAYER DATA, RESET GAME, fix camera on victory.
                        // lesch olle jusa!
                        foreach (Client _client in Server.clients.Values)
                        {
                            _client.Disconnect();
                                
                        }
                        Server.clients = new Dictionary<int, Client>();
                        Server.InitializeServerData();

                    }


                }


            }
        }


        public static void PlayerMiss(int _fromClient, Packet _packet)
        {
            if (Server.clients[_fromClient].player != null)
            {
                ServerSend.PlayerMissed(Server.clients[_fromClient].player);
            }
        }
    }
}
