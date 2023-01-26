using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class iniFileManager
    {
        public string m_iniFilePath { get; private set; }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        static extern uint GetPrivateProfileSectionNames(IntPtr pszReturnBuffer, uint nSize, string lpFileName);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
        (
            [In][MarshalAs(UnmanagedType.LPStr)] string strSectionName,
            // Note that because the key/value pars are returned as null-terminated
            // strings with the last string followed by 2 null-characters, we cannot
            // use StringBuilder.
            [In] IntPtr pReturnedString,
            [In] UInt32 nSize,
            [In][MarshalAs(UnmanagedType.LPStr)] string strFileName
        );

        public iniFileManager(string iniFilePath)
        {
            m_iniFilePath = iniFilePath;
        }

        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, m_iniFilePath);
        }

        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.m_iniFilePath);
            return temp.ToString();
        }

        public string[] GetAllKeysInSection(string section)
        {
            // Allocate in unmanaged memory a buffer of suitable size.
            // I have specified here the max size of 32767 as documentation
            // in MSDN.
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            // Start with an array of 1 string only.
            // Will embellish as we go along.
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(section, pBuffer, 32767, m_iniFilePath);

            // iStartAddress will point to the first character of the buffer,
            int iStartAddress = pBuffer.ToInt32();
            // iEndAddress will point to the last null char in the buffer.
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;

            // Navigate through pBuffer.
            while (iStartAddress < iEndAddress)
            {
                // Determine the current size of the array.
                int iArrayCurrentSize = strArray.Length;
                // Increment the size of the string array by 1.
                Array.Resize(ref strArray, iArrayCurrentSize + 1);
                // Get the current string which starts at "iStartAddress".
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                // Insert "strCurrent" into the string array.
                strArray[iArrayCurrentSize] = strCurrent;
                // Make "iStartAddress" point to the next string.
                iStartAddress += (strCurrent.Length + 1);
            }

            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;

            return strArray;
        }

        public string[] GetAllSectionNames()
        {
            string path = System.IO.Path.GetFullPath(m_iniFilePath);
            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
                return null;

            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            return local.Substring(0, local.Length - 1).Split('\0');
        }

        public void SplitKeyAndValue(string textLine, ref string key, ref string value)
        {
            int iEqualSignIndx = textLine.IndexOf("=");
            key = textLine.Substring(0, iEqualSignIndx);
            value = textLine.Substring((iEqualSignIndx + 1), textLine.Length - (iEqualSignIndx + 1));
        }
    }
}
