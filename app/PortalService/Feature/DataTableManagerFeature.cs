using Ndtech.PortalService.DataModel;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Feature
{
    public class DataTableManagerFeature : IPlugin
    {
        public void Register(IAppHost appHost)
        {
            IDbConnectionFactory db = appHost.GetContainer().Resolve<IDbConnectionFactory>();

            using (var conn = db.OpenDbConnection())
            {
                try
                {
                    if (!conn.TableExists(typeof(NdtechStaffInfo).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechStaffInfo>();

                        //conn.ExecuteSql("ALTER TABLE Entity ALTER COLUMN Column VARCHAR(MAX)");
                    }
                    if (!conn.TableExists(typeof(NdtechTelCode).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechTelCode>();
                    }
                    if (!conn.TableExists(typeof(NdtechAcntSystem).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechAcntSystem>();
                    }
                    if (!conn.TableExists(typeof(NdtechCompany).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechCompany>();
                    }
                    if (!conn.TableExists(typeof(AppData).Name))
                    {
                        conn.CreateTableIfNotExists<AppData>();
                    }
                    if (!conn.TableExists(typeof(NdtechLoginLog).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechLoginLog>();
                    }
                    if (!conn.TableExists(typeof(DocumentNumber).Name))
                    {
                        conn.CreateTableIfNotExists<DocumentNumber>();
                    }
                    if (!conn.TableExists(typeof(NdtechItem).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechItem>();
                    }
                    if (!conn.TableExists(typeof(NdtechItemCategory).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechItemCategory>();
                    }
                    if (!conn.TableExists(typeof(NdtechAttribute).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechAttribute>();
                    }
                    if (!conn.TableExists(typeof(NdtechItemAttribute).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechItemAttribute>();
                    }
                    if (!conn.TableExists(typeof(PurInquiry).Name))
                    {
                        conn.CreateTableIfNotExists<PurInquiry>();
                    }
                    if (!conn.TableExists(typeof(PurInquiryDetail).Name))
                    {
                        conn.CreateTableIfNotExists<PurInquiryDetail>();
                    }
                    if (!conn.TableExists(typeof(LowerEmployee).Name))
                    {
                        conn.CreateTableIfNotExists<LowerEmployee>();
                    }
                    if (!conn.TableExists(typeof(Resources).Name))
                    {
                        conn.CreateTableIfNotExists<Resources>();
                    }
                    if (!conn.TableExists(typeof(SuperiorEmployee).Name))
                    {
                        conn.CreateTableIfNotExists<SuperiorEmployee>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItem).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItem>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItemCategory).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItemCategory>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItemAttribute).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItemAttribute>();
                    }
                    if (!conn.TableExists(typeof(CompanyCertification).Name))
                    {
                        conn.CreateTableIfNotExists<CompanyCertification>();
                    }

                    if (!conn.TableExists(typeof(CompanyMainProduct).Name))
                    {
                        conn.CreateTableIfNotExists<CompanyMainProduct>();
                    }

                    if (!conn.TableExists(typeof(SalQuotation).Name))
                    {
                        conn.CreateTableIfNotExists<SalQuotation>();
                    }
                    if (!conn.TableExists(typeof(SalQuotationDetail).Name))
                    {
                        conn.CreateTableIfNotExists<SalQuotationDetail>();
                    }
                    if (!conn.TableExists(typeof(NdtechDealLog).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechDealLog>();
                    }
                    if (!conn.TableExists(typeof(UdocCustomer).Name))
                    {
                        conn.CreateTableIfNotExists<UdocCustomer>();
                    }
                    if (!conn.TableExists(typeof(UdocSupplier).Name))
                    {
                        conn.CreateTableIfNotExists<UdocSupplier>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseAddress).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseAddress>();
                    }
                    if (!conn.TableExists(typeof(PurSelect).Name))
                    {
                        conn.CreateTableIfNotExists<PurSelect>();
                    }
                    if (!conn.TableExists(typeof(PurSelectResults).Name))
                    {
                        conn.CreateTableIfNotExists<PurSelectResults>();
                    }
                    if (!conn.TableExists(typeof(ArapReceiving).Name))
                    {
                        conn.CreateTableIfNotExists<ArapReceiving>();
                    }
                    if (!conn.TableExists(typeof(SalOutNoticeDetail).Name))
                    {
                        conn.CreateTableIfNotExists<SalOutNoticeDetail>();
                    }
                    if (!conn.TableExists(typeof(SubscribeFilter).Name))
                    {
                        conn.CreateTableIfNotExists<SubscribeFilter>();
                    }
                    if (!conn.TableExists(typeof(SubscribeContact).Name))
                    {
                        conn.CreateTableIfNotExists<SubscribeContact>();
                    }
                    if (!conn.TableExists(typeof(SubscribeReceive).Name))
                    {
                        conn.CreateTableIfNotExists<SubscribeReceive>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItemLog).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItemLog>();
                    }
                    if (!conn.TableExists(typeof(PurShoppingCart).Name))
                    {
                        conn.CreateTableIfNotExists<PurShoppingCart>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItemLog).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItemLog>();
                    }
                    if (!conn.TableExists(typeof(NdtechItemCertification).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechItemCertification>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItemsCertification).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItemsCertification>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseItemsCertificationDetail).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseItemsCertificationDetail>();
                    }
                    if (!conn.TableExists(typeof(SubscribeFilterDetail).Name))
                    {
                        conn.CreateTableIfNotExists<SubscribeFilterDetail>();
                    }
                    if (!conn.TableExists(typeof(StateLog).Name))
                    {
                        conn.CreateTableIfNotExists<StateLog>();
                    }
                    if (!conn.TableExists(typeof(UdocProject).Name))
                    {
                        conn.CreateTableIfNotExists<UdocProject>();
                    }
                    if (!conn.TableExists(typeof(UdocProOptions).Name))
                    {
                        conn.CreateTableIfNotExists<UdocProOptions>();
                    }
                    if (!conn.TableExists(typeof(PurEvaluation).Name))
                    {
                        conn.CreateTableIfNotExists<PurEvaluation>();
                    }
                    if (!conn.TableExists(typeof(PurEvaluationDetail).Name))
                    {
                        conn.CreateTableIfNotExists<PurEvaluationDetail>();
                    }
                    if (!conn.TableExists(typeof(NdtechItemPrice).Name))
                    {
                        conn.CreateTableIfNotExists<NdtechItemPrice>();
                    }
                    if (!conn.TableExists(typeof(PurPlan).Name))
                    {
                        conn.CreateTableIfNotExists<PurPlan>();
                    }
                    if (!conn.TableExists(typeof(PurPlanDetail).Name))
                    {
                        conn.CreateTableIfNotExists<PurPlanDetail>();
                    }
                    if (!conn.TableExists(typeof(EnterpriseDefAddress).Name))
                    {
                        conn.CreateTableIfNotExists<EnterpriseDefAddress>();
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
