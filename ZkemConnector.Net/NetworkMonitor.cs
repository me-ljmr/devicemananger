using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZkemConnector.NET
{
    public class NetworkMonitor
    {
        private Object _obj = new Object();
        static readonly NetworkMonitor m_instance = new NetworkMonitor();
        static List<DeviceDTO> m_devices = new List<DeviceDTO>();
        Thread _thread;
        bool m_IsAlive;
        public static NetworkMonitor Instance
        {
            get { return m_instance; }
        }
        public List<DeviceDTO> DeviceList
        {
            get { return m_devices; }
            set { m_devices = value; }
        }

        static NetworkMonitor()
        {


        }
        public void StartMonitor()
        {
            m_IsAlive = true;
            _thread = new Thread(new ThreadStart(MonitorAll));
            _thread.Start();
        }

        ~NetworkMonitor()
        {
            m_IsAlive = false;
            _thread.Abort();
        }

        public void Destroy()
        {
            m_IsAlive = false;
            try
            {
                Monitor.Exit(_obj);
                _thread.Abort();
                _thread = null;
            }
            catch { }
        }

        void MonitorAll()
        {
            Monitor.Enter(_obj);
            while (m_IsAlive)
            {

                try
                {
                    Update();
                }
                catch (Exception ex)
                {
                    Monitor.Exit(_obj);
                }
                Thread.Sleep(100);
            }
        }
        void Update()
        {
            Task.Run(() =>
            {
                foreach (var dev in m_devices)
                {

                    if (DeviceUtility.PingTheDevice(dev.IPAddress))
                    {
                        dev.Status = Constants.DeviceStatus.Active;
                    }
                    else
                    { 
                        dev.Status = Constants.DeviceStatus.NotInNetwork;
                    }

                }
            });


        }
    }
}
