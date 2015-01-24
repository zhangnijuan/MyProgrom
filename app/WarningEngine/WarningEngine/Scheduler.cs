using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ndtech.WarningEngine
{
    #region 时钟触发器
    public delegate void MinuteTickEventHandler(object p_Sender, DateTime p_Time);
    public delegate void HourTickEventHandler(object p_Sender, DateTime p_Time);

    #endregion
    internal class Timer
    {
        #region 私有成员
        private bool m_Stop = false;
        private DateTime m_PrevHour;
        #endregion

        #region 属性
        public bool Running
        {
            get
            {
                return !m_Stop;
            }
        }
        //#endregion

        #region 事件
        public event MinuteTickEventHandler MinuteTick;
        public event HourTickEventHandler HourTick;
        #endregion

        #region 公共方法
        public void Run()
        {
            int scount = 0;
            while (!m_Stop)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                scount++;
                if (scount >= 60)
                {
                    scount = 0;
                    ExecuteTrigger();
                }
            }
        }

        public void Start()
        {
            m_Stop = false;
            m_PrevHour = DateTime.Now;
        }

        public void Stop()
        {
            m_Stop = true;
        }
        #endregion

        #region 私有方法
        private void ExecuteTrigger()
        {
            OnMinuteTick(DateTime.Now);

            DateTime now = DateTime.Now;
            if (m_PrevHour.Hour+10 < now.Hour)
            {
                m_PrevHour = now;
                OnHourTick(now);
            }
        }
        private void OnMinuteTick(DateTime p_Now)
        {
            if (MinuteTick != null)
                MinuteTick(this, p_Now);
        }
        private void OnHourTick(DateTime p_Now)
        {
            if (HourTick != null)
                HourTick(this, p_Now);
        }
        #endregion
    }
        #endregion

    #region 定时任务触发器
    /// <summary>
    /// Scheduler 的摘要说明。
    /// </summary>
    internal class InquiryOffDateScheduler
    {
        #region 私有成员
        private Timer m_Timer;
        #endregion

        #region 公共方法
        public void Start()
        {
            if (m_Timer != null)
            {
                if (!m_Timer.Running)
                {
                    m_Timer.Start();
                    Thread thread = new Thread(new ThreadStart(m_Timer.Start));
                    thread.Start();
                }
            }
            else
            {
                m_Timer = new Timer();
                m_Timer.MinuteTick += new MinuteTickEventHandler(_timer_MinuteTick);
                m_Timer.HourTick += new HourTickEventHandler(_timer_HourTick);
                m_Timer.Start();
                Thread thread = new Thread(new ThreadStart(m_Timer.Run));
                thread.Start();
            }
        }

        public void Stop()
        {
            if (m_Timer != null)
                m_Timer.Stop();
        }
        #endregion

        #region 事件处理方法
        private void _timer_MinuteTick(object p_Sender, DateTime p_Time)
        {
           
        }

        private void _timer_HourTick(object p_Sender, DateTime p_Time)
        {
            WarningPipeManager.Pipe.Send("InquiryOffDate", new Info(string.Empty,null));
        }
        #endregion
    }
    #endregion
}
