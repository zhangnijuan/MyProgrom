using System;
using System.Text;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.Logging;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Ndtech.PortalModel.ViewModel;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceInterface;
using Ndtech.PortalService.DataModel;
using Ndtech.PortalService.SystemService;
using ServiceStack.FluentValidation.Results;

namespace Ndtech.PortalService.Auth
{
    /// <summary>
    /// 保存产品质量认证Service
    /// add by yangshuo 2015/01/05
    /// </summary>
    public class SaveItemCertificationService : Service, IPost<SaveItemCertificationRequest>
    {
        private ILog log = LogManager.GetLogger(typeof(SaveItemCertificationService));

        public IValidator<SaveItemCertificationRequest> ItemCertifiValidator { get; set; }

        public object Post(SaveItemCertificationRequest request)
        {
            ItemCertificationResponse response = new ItemCertificationResponse();

            if (request.AcceptCertificationFlag == "Y")
            {
                #region Modify

                if (request.ID > 0)
                {
                    //机构受理产品质量认证->modify
                    return ModifyMethod(request);
                }
                else
                {
                    response.ResponseStatus.ErrorCode = "Err_IDIsNull";
                    response.ResponseStatus.Message = Const.Err_IDIsNull;
                    return response;
                }

                #endregion
            }
            else
            {
                #region Add

                //申请产品质量认证->add
                //第一步:校验前端的数据合法性
                ValidationResult result = ItemCertifiValidator.Validate(request);
                if (!result.IsValid)
                {
                    response.ResponseStatus.ErrorCode = result.Errors[0].ErrorCode;
                    response.ResponseStatus.Message = result.Errors[0].ErrorMessage;
                    return response;
                }

                //第二步:new + response
                return AddMethod(request);

                #endregion
            }
        }

        private ItemCertificationResponse AddMethod(SaveItemCertificationRequest request)
        {
            ItemCertificationResponse response = new ItemCertificationResponse();

            //1.主档viewmodel转成datamodel
            EnterpriseItemsCertification certifiNew = request.TranslateTo<EnterpriseItemsCertification>();
            certifiNew.ID = RecordIDService.GetRecordID(1);
            certifiNew.CreateDate = DateTime.Now;
            certifiNew.State = 0;

            EnterpriseItemsCertificationDetail certifiDetailNew = null;
            List<EnterpriseItemsCertificationDetail> detailList = new List<EnterpriseItemsCertificationDetail>();
            foreach (var item in request.DetailData)
            {
                //2.明细档viewmodel转成datamodel
                certifiDetailNew = item.TranslateTo<EnterpriseItemsCertificationDetail>();
                certifiDetailNew.ID = RecordIDService.GetRecordID(1);
                certifiDetailNew.Mid = certifiNew.ID;

                //新增默认为-1
                certifiDetailNew.Results = -1;
                detailList.Add(certifiDetailNew);
            }

            //3.insert主档、明细档
            using (var trans = this.Db.BeginTransaction())
            {
                this.Db.Insert(certifiNew);
                foreach (var detail in detailList)
                {
                    this.Db.Insert(detail);
                }

                trans.Commit();
            }

            response.Success = true;
            return response;
        }

        private ItemCertificationResponse ModifyMethod(SaveItemCertificationRequest request)
        {
            ItemCertificationResponse response = new ItemCertificationResponse();

            //根据id查询主档
            var certifiOld = this.Db.FirstOrDefault<EnterpriseItemsCertification>(x => x.ID == request.ID);
            if (certifiOld != null)
            {
                //1.主档viewmodel转成datamodel
                var certifiNew = request.TranslateTo<EnterpriseItemsCertification>();
                certifiNew.CreateDate = certifiOld.CreateDate;
                DateTime dtNow = DateTime.Now;
                certifiNew.AcceptDate = dtNow;

                EnterpriseItemsCertificationDetail certifiDetailNew = null;
                List<EnterpriseItemsCertificationDetail> detailList = new List<EnterpriseItemsCertificationDetail>();
                foreach (var item in request.DetailData)
                {
                    //2.明细档viewmodel转成datamodel
                    certifiDetailNew = item.TranslateTo<EnterpriseItemsCertificationDetail>();
                    certifiDetailNew.AcceptDate = dtNow;
                    detailList.Add(certifiDetailNew);
                }

                //3.update主档、明细档和明细认证报告
                Resources resourceNew = null;
                List<Resources> resourcesList = null;
                List<Resources> resourceOldList = null;

                //未变化附件id
                StringBuilder sResIds = new StringBuilder();
                string resIds = string.Empty;

                using (var trans = this.Db.BeginTransaction())
                {
                    //更新主档,主要是状态更新
                    this.Db.Save(certifiNew);

                    for (int i = 0; i < detailList.Count; i++)
                    {
                        resourcesList = new List<Resources>();

                        //更新明细认证结果、认证说明
                        this.Db.Save(detailList[i]);

                        #region 更新附件资源

                        if (request.DetailData[i].PicResources != null && request.DetailData[i].PicResources.Count > 0)
                        {
                            foreach (var picView in request.DetailData[i].PicResources)
                            {
                                //先转化再判断,因为前台为新上传附件时是没有ID字段
                                resourceNew = picView.TranslateTo<Resources>();
                                if (resourceNew.Id <= 0)
                                {
                                    //新上传的附件累加待insert
                                    resourceNew.Id = RecordIDService.GetRecordID(1);
                                    resourceNew.DocumentID = detailList[i].ID;
                                    resourceNew.AccountID = certifiNew.CAccountID;
                                    resourcesList.Add(resourceNew);
                                }
                                else
                                {
                                    //未变化的附件id
                                    sResIds.Append(picView.Id + ",");
                                }
                            }

                            if (sResIds.Length > 0)
                            {
                                resIds = sResIds.ToString().Substring(0, sResIds.Length - 1);
                            }
                        }
                        else
                        {
                            //查询原附件资源
                            resourceOldList = this.Db.Where<Resources>(x => x.DocumentID == detailList[i].ID);
                        }

                        if (request.DetailData[i].PicResources != null && request.DetailData[i].PicResources.Count > 0)
                        {
                            //2.1.1 先del画面删除的附件
                            if (!string.IsNullOrEmpty(resIds))
                            {
                                this.Db.Delete<Resources>(string.Format("mid = {0} and id not in ({1})", detailList[i].ID, resIds));
                            }

                            //2.1.2 insert新上传的附件
                            foreach (var resourcesNew in resourcesList)
                            {
                                this.Db.Insert(resourcesNew);
                            }
                        }
                        else
                        {
                            //2.2 根据明细id删除附件资源档案
                            if (resourceOldList != null && resourceOldList.Count > 0)
                            {
                                this.Db.Delete<Resources>(string.Format("mid = {0}", detailList[i].ID));
                            }
                        }

                        #endregion

                        //更新产品档案是否认证状态
                        if (request.State == 1 && detailList[i].Results == 1)
                        {
                            this.Db.Update<EnterpriseItem>(string.Format("iscertification = 1 where id = {0}", detailList[i].ItemID));
                        }
                    }

                    trans.Commit();
                }
            }
            else
            {
                response.ResponseStatus.ErrorCode = "Err_NoInfoByID";
                response.ResponseStatus.Message = Const.Err_NoInfoByID;
                return response;
            }

            response.Success = true;
            return response;
        }
    }

    #region Validate

    public class ItemCertificationValidator : AbstractValidator<SaveItemCertificationRequest>
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override ValidationResult Validate(SaveItemCertificationRequest instance)
        {
            ValidationResult result = new ValidationResult();

            if (string.IsNullOrEmpty(instance.Whatsoever))
            {
                //申请方不能为空
                result.Errors.Add(new ValidationFailure("Whatsoever", "No Whatsoever Parameter", "Err_WhatsoeverIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Linkman))
            {
                //联系人不能为空
                result.Errors.Add(new ValidationFailure("Linkman", "No Linkman Parameter", "Err_LinkmanIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Mobile))
            {
                //手机不能为空
                result.Errors.Add(new ValidationFailure("Mobile", "No Mobile Parameter", "Err_MobileIsNull"));
                return result;
            }

            if (string.IsNullOrEmpty(instance.Address))
            {
                //联系地址不能为空
                result.Errors.Add(new ValidationFailure("Address", "No Address Parameter", "Err_AddressIsNull"));
                return result;
            }

            if (instance.DetailData == null)
            {
                //申请认证明细不能为空
                result.Errors.Add(new ValidationFailure("DetailData", "No DetailData Parameter", "Err_DetailDataIsNull"));
                return result;
            }
            return base.Validate(instance);
        }
    }

    #endregion
}
