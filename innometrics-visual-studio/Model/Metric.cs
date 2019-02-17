﻿using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using ApiClient;

namespace innometrics_visual_studio.Model.Metrics
{
    public class Metric:IMetric
    {

        public string ExecutableName { get; }

        public string ActivityType { get; }

        public string Ip4Address
        {
            get
            {

                string ip4Address;

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    ip4Address = socket.RemoteEndPoint is IPEndPoint endPoint ? endPoint.Address.ToString() : "127.0.0.1";
                }

                return ip4Address;
            }
        }

        public string MacAddress => NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();


        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public string Value { get; private set; }

        public Metric(string executableName, string activityType)
        {
            ExecutableName = executableName;
            ActivityType = activityType;
        }

        public void UpdateMetric(int value, DateTime start, DateTime end)
        {
            //todo add checks
            Value = value.ToString();
            StartTime = start;
            EndTime = end;
        }    
    }
}