using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void hp(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.hp))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.hp);

                SendUDPDataToAll(_packet);
            }
        }



        public static void PlayerAttacked(Player _attacker, Player _target)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerAttacked))
            {
                {
                    _packet.Write(_attacker.id);
                    _packet.Write(_target.id);

                    SendTCPDataToAll(_packet);
                }
            }
        }

        public static void PlayerDown(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerDown))
            {
                {
                    _packet.Write(_player.id);

                    SendTCPDataToAll(_packet);
                }
            }
        }

        public static void EndGame(Player winner)
        {
            using (Packet _packet = new Packet((int)ServerPackets.endGame))
            {
                {
                    _packet.Write(winner.id);
                    SendTCPDataToAll(_packet);
                }
            }
        }


        public static void PlayerMissed(Player _attacker)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerMissed))
            {
                {
                    _packet.Write(_attacker.id);

                    SendTCPDataToAll(_packet);
                }
            }
        }

        public static void SpawnPlayer(int _toClient, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.position);
                _packet.Write(_player.rotation);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void PlayerPosition(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.position);

                SendUDPDataToAll(_packet);
            }
        }

        public static void RemovePlayer(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.removePlayer))
            {
                _packet.Write(_player.id);

                SendTCPDataToAll(_packet);
            }
        }


        public static void PlayerRotation(Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.rotation);

                SendUDPDataToAll(_player.id, _packet);
            }
        }
        #endregion
    }
}
