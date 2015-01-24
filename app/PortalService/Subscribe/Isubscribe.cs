using Ndtech.PortalService.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Subscribe
{
   public interface Isubscribe
    {
       /// <summary>
       /// 添加订阅者
       /// </summary>
       /// <param name="p_SubscribeContact"></param>
       /// <returns></returns>
       SubscribeContact addSubscriber(SubscribeContact p_SubscribeContact);
       /// <summary>
       /// 添加过滤器
       /// </summary>
       /// <param name="p_SubscribeFilter"></param>
       /// <returns></returns>
       SubscribeFilter addSubscribeFilter(SubscribeFilter p_SubscribeFilter); //添加过滤器

       /// <summary>
       /// 接受订阅信息
       /// </summary>
       /// <param name="p_SubscribeContact"></param>
       /// <param name="p_SubscribeFilter"></param>
       /// <returns></returns>
       bool ReceiveSubscribe(long p_FromDataID, int p_Substate, int p_SubType,int p_ToAccountID, long p_Subsciber, string p_SubscriberCode, string p_SubscriberName); //接受订阅信息
     //addSubscribeTask(任务对象) // 添加发送任务
     //getSubscribeFilter(订阅者对象) //获取过滤器
     //getSubscriber(fromAccountID) // 被订阅企业获取订阅人列表
     //getSubscribeTask(state) //获取发送任务列表

    }
}
