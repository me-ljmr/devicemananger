﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZkemConnector.NET
{
    public class DeviceManipulator
    {
        private static DeviceManipulator _deviceManipulator;
        public static DeviceManipulator Instance
        {
            get
            {
                if (_deviceManipulator == null) _deviceManipulator = new DeviceManipulator();
                return _deviceManipulator;
            }

        }
        public ICollection<UserInfo> GetAllUserInfo(ZKTecoDevice objZkeeper, int machineNumber)
        {
            string sdwEnrollNumber = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex;
            bool bEnabled = false;

            ICollection<UserInfo> lstFPTemplates = new List<UserInfo>();

            objZkeeper.ReadAllUserID(machineNumber);
            objZkeeper.ReadAllTemplate(machineNumber);

            while (objZkeeper.SSR_GetAllUserInfo(machineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (objZkeeper.GetUserTmpExStr(machineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))
                    {
                        UserInfo fpInfo = new UserInfo();
                        fpInfo.MachineNumber = machineNumber;
                        fpInfo.EnrollNumber = sdwEnrollNumber;
                        fpInfo.Name = sName;
                        fpInfo.FingerIndex = idwFingerIndex;
                        fpInfo.TmpData = sTmpData;
                        fpInfo.Privelege = iPrivilege;
                        fpInfo.Password = sPassword;
                        fpInfo.Enabled = bEnabled;
                        fpInfo.iFlag = iFlag.ToString();

                        lstFPTemplates.Add(fpInfo);
                    }
                }

            }
            return lstFPTemplates;
        }

        public ICollection<MachineInfo> GetLogData(ZKTecoDevice objZkeeper, int machineNumber)
        {
            string dwEnrollNumber1 = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<MachineInfo> lstEnrollData = new List<MachineInfo>();

            objZkeeper.ReadAllGLogData(machineNumber);

            while (objZkeeper.SSR_GetGeneralLogData(machineNumber, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))


            {
                string inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();

                MachineInfo objInfo = new MachineInfo();
                objInfo.MachineNumber = machineNumber;
                objInfo.IndRegID = int.Parse(dwEnrollNumber1);
                objInfo.DateTimeRecord = inputDate;

                lstEnrollData.Add(objInfo);
            }

            return lstEnrollData;
        }

        public ICollection<UserIDInfo> GetAllUserID(ZKTecoDevice objZkeeper, int machineNumber)
        {
            int dwEnrollNumber = 0;
            int dwEMachineNumber = 0;
            int dwBackUpNumber = 0;
            int dwMachinePrivelage = 0;
            int dwEnabled = 0;

            ICollection<UserIDInfo> lstUserIDInfo = new List<UserIDInfo>();

            while (objZkeeper.GetAllUserID(machineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwBackUpNumber, ref dwMachinePrivelage, ref dwEnabled))
            {
                UserIDInfo userID = new UserIDInfo();
                userID.BackUpNumber = dwBackUpNumber;
                userID.Enabled = dwEnabled;
                userID.EnrollNumber = dwEnrollNumber;
                userID.MachineNumber = dwEMachineNumber;
                userID.Privelage = dwMachinePrivelage;
                lstUserIDInfo.Add(userID);
            }
            return lstUserIDInfo;
        }

        public void GetGeneratLog(ZKTecoDevice objZkeeper, int machineNumber, string enrollNo)
        {
            string name = null;
            string password = null;
            int previlage = 0;
            bool enabled = false;
            byte[] byTmpData = new byte[2000];
            int tempLength = 0;

            int idwFingerIndex = 0;// [ <--- Enter your fingerprint index here ]
            int iFlag = 0;

            objZkeeper.ReadAllTemplate(machineNumber);

            while (objZkeeper.SSR_GetUserInfo(machineNumber, enrollNo, out name, out password, out previlage, out enabled))
            {
                if (objZkeeper.GetUserTmpEx(machineNumber, enrollNo, idwFingerIndex, out iFlag, out byTmpData[0], out tempLength))
                {
                    break;
                }
            }
        }


        public bool PushUserDataToDevice(ZKTecoDevice objZkeeper, int machineNumber, string enrollNo)
        {
            string userName = string.Empty;
            string password = string.Empty;
            int privelage = 1;
            return objZkeeper.SSR_SetUserInfo(machineNumber, enrollNo, userName, password, privelage, true);
        }

        public bool UploadFTPTemplate(ZKTecoDevice objZkeeper, int machineNumber, List<UserInfo> lstUserInfo)
        {
            string sdwEnrollNumber = string.Empty, sName = string.Empty, sTmpData = string.Empty;
            int idwFingerIndex = 0, iPrivilege = 0, iFlag = 1, iUpdateFlag = 1;
            string sPassword = "";
            string sEnabled = "";
            bool bEnabled = false;

            if (objZkeeper.BeginBatchUpdate(machineNumber, iUpdateFlag))
            {
                string sLastEnrollNumber = "";

                for (int i = 0; i < lstUserInfo.Count; i++)
                {
                    sdwEnrollNumber = lstUserInfo[i].EnrollNumber;
                    sName = lstUserInfo[i].Name;
                    idwFingerIndex = lstUserInfo[i].FingerIndex;
                    sTmpData = lstUserInfo[i].TmpData;
                    iPrivilege = lstUserInfo[i].Privelege;
                    sPassword = lstUserInfo[i].Password;
                    sEnabled = lstUserInfo[i].Enabled.ToString();
                    iFlag = Convert.ToInt32(lstUserInfo[i].iFlag);
                    bEnabled = true;

                    /* [ Identify whether the user 
                         information(except fingerprint templates) has been uploaded */

                    if (sdwEnrollNumber != sLastEnrollNumber)
                    {
                        if (objZkeeper.SSR_SetUserInfo(machineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the memory
                            objZkeeper.SetUserTmpExStr(machineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the memory
                        else return false;
                    }
                    else
                    {
                        /* [ The current fingerprint and the former one belongs the same user,
                        i.e one user has more than one template ] */
                        objZkeeper.SetUserTmpExStr(machineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);
                    }

                    sLastEnrollNumber = sdwEnrollNumber;
                }

                return true;
            }
            else
                return false;
        }

        public object ClearData(ZKTecoDevice objZkeeper, int machineNumber, ClearFlag clearFlag)
        {
            int iDataFlag = (int)clearFlag;

            if (objZkeeper.ClearData(machineNumber, iDataFlag))
                return objZkeeper.RefreshData(machineNumber);
            else
            {
                int idwErrorCode = 0;
                objZkeeper.GetLastError(ref idwErrorCode);
                return idwErrorCode;
            }
        }

        public bool ClearGLog(ZKTecoDevice objZkeeper, int machineNumber)
        {
            return objZkeeper.ClearGLog(machineNumber);
        }


        public string FetchDeviceInfo(ZKTecoDevice objZkeeper, int machineNumber)
        {
            StringBuilder sb = new StringBuilder();

            string returnValue = string.Empty;


            objZkeeper.GetFirmwareVersion(machineNumber, ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Firmware V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }


            returnValue = string.Empty;
            objZkeeper.GetVendor(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Vendor: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            string sWiegandFmt = string.Empty;
            objZkeeper.GetWiegandFmt(machineNumber, ref sWiegandFmt);

            returnValue = string.Empty;
            objZkeeper.GetSDKVersion(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("SDK V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            objZkeeper.GetSerialNumber(machineNumber, out returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Serial No: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            objZkeeper.GetDeviceMAC(machineNumber, ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Device MAC: ");
                sb.Append(returnValue);
            }

            return sb.ToString();
        }



    }
}
