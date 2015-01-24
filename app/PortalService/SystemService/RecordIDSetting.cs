using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Ndtech.PortalService.SystemService
{
    internal static class RecordIDSetting
    {
        internal static Dictionary<string, long> RecordIDMax = new Dictionary<string, long>();
        internal static Dictionary<string, long> RecordIDCurrent = new Dictionary<string, long>();
        private static string BaseDir = "";
        //private static FileStream ConfigFile=null;

        internal static void Load(string p_BaseDir)
        {
            BaseDir = p_BaseDir;
            string FileFullName = string.Format("{0}Configuration{1}RecordID.config", p_BaseDir, Path.DirectorySeparatorChar);
            StreamReader reader = new StreamReader(FileFullName);
            try
            {
                string maxRecordID = reader.ReadLine();
                RecordIDMax["Public"] = Convert.ToInt64(maxRecordID);
                RecordIDCurrent["Public"] = RecordIDMax["Public"];
            }
            finally
            {
                reader.Close();
            }
        }
        internal static void Save()
        {
            string FileFullName = string.Format("{0}{1}Configuration{1}RecordID.config", BaseDir, Path.DirectorySeparatorChar);
            StreamWriter write = new StreamWriter(FileFullName);
            write.BaseStream.Seek(0, SeekOrigin.Begin);
            try
            {
                write.Write(RecordIDMax["Public"]);
            }
            finally
            {
                write.Close();
            }
        }
        internal static void Clear()
        {
            //ConfigFile.Close();
            //ConfigFile = null;
            RecordIDMax.Clear();
            RecordIDMax = null;
        }
    }
}
