using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ndtech.WarningEngine
{
    public abstract class AbstractListener
    {
        private bool m_Stop;

        public virtual string ListenedChannel
        {
            get
            {
                return string.Empty;
            }
        }
        public virtual int Timeout
        {
            get
            {
                return 3;
            }
        }

        private void Run()
        {
            Info info = null;
            while (!m_Stop)
            {
                Thread.Sleep(TimeSpan.FromSeconds(Timeout));
                info = WarningPipeManager.Pipe.Get(ListenedChannel);
                if (info != null)
                {
                    try
                    {
                        Process(info);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        public void Start()
        {
            m_Stop = false;
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }
        public void Stop()
        {
            m_Stop = true;
        }
        protected virtual void Process(Info p_Info)
        {

        }
    }
    public class Info
    {
        public string Name;
        public Stream Data;

        public Info(string p_Name,  Stream p_Data)
        {
            Name = p_Name;
            Data = p_Data;
        }
    }
    internal class InfoPipe
    {
        private string _name;
        private Dictionary<string, Queue> _channels;

        public string Name
        {
            get { return _name; }
        }

        public InfoPipe(string p_Name)
        {
            _name = p_Name;
            _channels = new Dictionary<string, Queue>();
        }

        public void CreateChanel(string p_ChanelID)
        {
            if (_channels.ContainsKey(p_ChanelID))
                throw new Exception("Channel exist");
            Queue queue = new Queue(10);
            _channels.Add(p_ChanelID, Queue.Synchronized(queue));
        }

        public void RemoveChanel(string p_ChanelID)
        {
            if (_channels.ContainsKey(p_ChanelID))
            {
                _channels[p_ChanelID].Clear();
            }
        }

        public Info Get(string p_ChanelID)
        {
            if (_channels.ContainsKey(p_ChanelID))
            {
                if (_channels[p_ChanelID] != null && _channels[p_ChanelID].Count > 0)
                    return _channels[p_ChanelID].Dequeue() as Info;
            }
            return null;
        }

        public void Send(string p_ChanelID, Info p_Info)
        {
            if (p_Info == null)
                return;

            if (!_channels.ContainsKey(p_ChanelID) || _channels[p_ChanelID] == null)
                throw new Exception("Channel not exist");

            _channels[p_ChanelID].Enqueue(p_Info);
        }

        public void Clear()
        {
            foreach (string channel in _channels.Keys)
                _channels[channel].Clear();

            _channels.Clear();
        }
    }
    static class WarningPipeManager
    {
        private static InfoPipe m_Pipe;

        public static InfoPipe Pipe
        {
            get { return m_Pipe; }
        }

        public static void Initialize()
        {
            m_Pipe = new InfoPipe("WarningPipe");
            m_Pipe.CreateChanel("InquiryOffDate");
        }

        public static void Dispose()
        {
            m_Pipe.Clear();
        }
    }
}
