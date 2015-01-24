using Ndtech.PortalService.DataModel;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ndtech.PortalService.Auth
{
    public class NdtechOrmLiteAuthRepository : INdtechUserAuthRepository, IClearable
    {
        private readonly IDbConnectionFactory dbFactory;
        private readonly IHashProvider passwordHasher;

        public NdtechOrmLiteAuthRepository(IDbConnectionFactory dbFactory)
            : this(dbFactory, new SaltedHash())
        { }

        public NdtechOrmLiteAuthRepository(IDbConnectionFactory dbFactory, IHashProvider passwordHasher)
        {
            this.dbFactory = dbFactory;
            this.passwordHasher = passwordHasher;
            
        }

       
        public DataModel.NdtechStaffInfo CreateUserAuth(DataModel.NdtechStaffInfo newUser, string password)
        {
            using (var conn = dbFactory.OpenDbConnection())
            {
                try
                {
                    string salt;
                    string hash;
                    passwordHasher.GetHashAndSaltString(password, out hash, out salt);
                    newUser.PassWord = hash;
                    newUser.Salt = salt;
                    conn.Insert<NdtechStaffInfo>();
                    newUser = conn.GetById<NdtechStaffInfo>(conn.GetLastInsertId());
                    return newUser;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public DataModel.NdtechStaffInfo UpdateUserAuth(DataModel.NdtechStaffInfo existingUser, DataModel.NdtechStaffInfo newUser, string password)
        {
            using (var conn = dbFactory.OpenDbConnection())
            {
                string salt = existingUser.Salt;
                string hash = existingUser.PassWord;
                if (string.IsNullOrEmpty(password))
                {
                    passwordHasher.GetHashAndSaltString(password, out hash, out salt);
                }

                newUser.PassWord = hash;
                newUser.Salt = salt;
                newUser.ID = existingUser.ID;
                newUser.SysCode = existingUser.SysCode;
                newUser.UserName = existingUser.UserName;
                try
                {
                    conn.Save<NdtechStaffInfo>(newUser);
                    return newUser;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
              
            }
        }

        public DataModel.NdtechStaffInfo GetUserAuthByUserName(string userNameOrEmail)
        {
            using (var conn = dbFactory.OpenDbConnection())
            {
                try
                {
                    return conn.Select<NdtechStaffInfo>(q => q.UserName == userNameOrEmail).FirstOrDefault();
                }
                catch (Exception ex)
                { throw ex; }
                finally {
                    conn.Close();
                }
            }
        }

        public bool TryAuthenticate(string userName, string password, out DataModel.NdtechStaffInfo userAuth)
        {
            userAuth = GetUserAuthByUserName(userName);
            if (null == userAuth)
                return false;
            if (passwordHasher.VerifyHashString(password, userAuth.PassWord, userAuth.Salt))
            {
                //userId = userAuth.Id.ToString(CultureInfo.InvariantCulture);
                return true;
            }

            userAuth = null;
            return false;


        }

        public bool TryAuthenticate(Dictionary<string, string> digestHeaders, string PrivateKey, int NonceTimeOut, string sequence, out DataModel.NdtechStaffInfo userAuth)
        {
            throw new NotImplementedException();
        }

        public void LoadUserAuth(INdtechAuthSession session)
        {
            throw new NotImplementedException();
        }

        public DataModel.NdtechStaffInfo GetUserAuth(string userAuthId)
        {
            using (var conn = dbFactory.OpenDbConnection())
            {
                try
                {
                    return conn.Select<NdtechStaffInfo>("ID={0}", userAuthId).FirstOrDefault();
                }
                catch (Exception ex)
                { throw ex; }
                finally
                {
                    conn.Close();
                }
            }
        }

        public void SaveUserAuth(INdtechAuthSession authSession)
        {
            throw new NotImplementedException();
        }

        public void SaveUserAuth(DataModel.NdtechStaffInfo userAuth)
        {
            throw new NotImplementedException();
        }

        public DataModel.NdtechStaffInfo GetUserAuth(INdtechAuthSession authSession)
        {
            //if (!string.IsNullOrEmpty(authSession.UserID))
            //{
            //    return 
            //}
            return null;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
        public void CreateTable()
        {
            dbFactory.Run(db =>
            {
                db.CreateTableIfNotExists<NdtechStaffInfo>();
               
            });
        }


        public string CreateOrMergeAuthSession(INdtechAuthSession authSession)
        {
            throw new NotImplementedException();
        }
    }
}
