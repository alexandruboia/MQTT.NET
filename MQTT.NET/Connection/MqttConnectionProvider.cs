using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Mqtt.Net
{
    public class MqttConnectionProvider : IMqttConnectionProvider
    {
        private string _host = null;

        private int _port = 0;

        private Socket _socket = null;

        private bool _connected = false;

        private NetworkStream _stream = null;

        public void Connect(string host, int port)
        {
            IPAddress[] _addresses = null;
            IPAddress _selectedAddress = null;

            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");
            if (port < 0)
                throw new ArgumentException("Invalid port number", "port");
            
            _host = host;
            _port = port;

            _addresses = Dns.GetHostAddresses(host);
            foreach (IPAddress _addr in _addresses)
            {
                if (_addr.AddressFamily != AddressFamily.InterNetwork && _addr.AddressFamily != AddressFamily.InterNetworkV6)
                    continue;
                _selectedAddress = _addr;
                break;
            }
            if (_selectedAddress != null)
            {
                _socket = new Socket(_selectedAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_addresses, _port);
                _connected = _socket.Connected;
                if (_connected)
                    _stream = new NetworkStream(_socket, false);
            }
            if (!_connected)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        public void Disconnect()
        {
            _connected = false;
            if (_stream != null)
                _stream.Close();
            if (_socket != null)
                _socket.Close();
            _socket = null;
            _stream = null;
        }

        public Stream Stream
        {
            get
            {
                return _stream;
            }
        }

        public bool Connected
        {
            get 
            {
                return _connected;
            }
        }
    }
}