using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;

namespace Ndtech.PortalService.SystemService
{
    public class RecordIDService
    {
       static WorkerGenerator worker = new WorkerGenerator();

        public static long GetRecordID(int count)
        {
           
            worker.setWorkerID(count);
            return worker.nextId();
        }

    }
    internal class WorkerGenerator
    {
        private long workerID;

        private long sequence = 0;
        private long mark = 0;

        private const int MAXSEQUENCE = 128;
        private const int MAXWORKERID = 128;

        private long lastTimeGen;

        // private final int workerIDBitWidth = 7;
        private const int timeStampBitWidth = 40;
        private const int sequenceBitWidth = 7;

        private int sequenceLeftShift = 0;
        private int timeStampLeftShift = sequenceBitWidth;
        private int workerIDLeftShift = sequenceBitWidth + timeStampBitWidth;

        private const long TIMEBASELINE = 1288834974657L;
        internal long nextId()
        {
            lock (this)
            {
                long timeGen = CurrentTimeMillis();
                mark++;
                sequence = (++sequence) % MAXSEQUENCE;
                if (timeGen == lastTimeGen)
                {
                    if (mark >= MAXSEQUENCE)
                    {
                        timeGen = tillNextMill();
                        mark = 0;
                    }
                }
                else
                {
                    mark = 0;
                }
                lastTimeGen = timeGen;

                return sequence << sequenceLeftShift | (timeGen - TIMEBASELINE) << timeStampLeftShift
                        | workerID << workerIDLeftShift;
                //return sequence << sequenceLeftShift;
            }

        }

        internal void setWorkerID(long id)
        {
            if (validateWorkerID(id))
            {
                workerID = id;
            }
            else
            {
                throw new System.InvalidOperationException("invalid id:" + id);
            }
        }

        internal bool validateWorkerID(long id)
        {
            if (id > MAXWORKERID || id < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private long tillNextMill()
        {
            long timeGen = CurrentTimeMillis();
            while (timeGen <= lastTimeGen)
            {
                timeGen = CurrentTimeMillis();
            }
            // System.out.println("i'm in tillNextMill");
            return timeGen;
        }
        private readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        //internal  WorkerGenerator getIdWorker()
        //{
        //    WorkerGenerator work = new WorkerGenerator();
        //    work.setWorkerID(1);
        //    return work;

        //}
    }



    public class RecordSnumService
    {
        static object obj = new object();
        static int[] sizeTable = { 9, 99, 999, 9999};
        static string[] sizeFormat = { "0000", "000", "00", "0" };
        static int sizeOfInt(int x)
        {
            for (int i = 0; ; i++)
                if (x <= sizeTable[i])
                    return i + 1;
        }
        public static string GetSnum(Service p_Service,int p_AccountID,SnumType p_Type)
        {
            string snum = string.Empty;
            string type = string.Empty;
            switch (p_Type)
            {
                case SnumType.Inquiry:
                    type = "XJ";
                    break;
                case SnumType.InquiryRelease:
                    type = "XJFB";
                    break;
                case SnumType.Offer:
                    type = "BJ";
                    break;
                case SnumType.OfferRelease:
                    type = "BJFB";
                    break;
                case SnumType.PurSelect:
                    type = "CGYX";
                    break;
                case SnumType.PurOrder:
                    type = "CGDD";
                    break;
                case SnumType.PurEvaluation:
                    type = "CGPJ";
                    break;
                case SnumType.ItemCertification:
                    type = "ZLRZ";
                    break;
                default:
                    break;
            }
            lock (obj)
            {
                DocumentNumber number = p_Service.Db.QuerySingle<DocumentNumber>(string.Format("select * from udoc_snum where snumbertype = '{0}' and a = {1}", type,p_AccountID));
                if (number == null)
                {
                    number = new DocumentNumber();
                    number.SnumType = type;
                    number.AccountID = p_AccountID;
                    number.Number = 1;
                    p_Service.Db.Insert<DocumentNumber>(number);
                }
                else
                {
                    int count =number.Number+1;
                    number.SnumType = type;
                    number.AccountID = p_AccountID;
                    number.Number = count;
                    p_Service.Db.Save<DocumentNumber>(number);
                }
       
                
                string numfromt = string.Empty;
                int size = sizeOfInt(number.Number);
                if (size < 5 && number.Number < 9999)
                {
                    numfromt = string.Format("{0}{1}", sizeFormat[size], number.Number);
                }
                else
                {
                    numfromt = number.Number.ToString();
                }
                snum = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMdd"), numfromt);

            }
  
            return snum;
        }
    }
    public enum SnumType
    {
        /// <summary>
        /// 询价单号
        /// </summary>
        Inquiry,
        /// <summary>
        /// 询价发布单号
        /// </summary>
        InquiryRelease,
        /// <summary>
        /// 报价单号
        /// </summary>
        Offer,
        /// <summary>
        /// 报价发布单号
        /// </summary>
        OfferRelease,

        /// <summary>
        /// 优选单单号
        /// </summary>
        PurSelect,

         /// <summary>
        /// 订单
        /// </summary>
        PurOrder,

         /// <summary>
        /// 评价
        /// </summary>
        PurEvaluation,

         /// <summary>
        /// 产品质量认证单号
        /// </summary>
        ItemCertification,
        /// <summary>
        /// 采购申请单号
        /// </summary>
        PurRequest
    }
}
