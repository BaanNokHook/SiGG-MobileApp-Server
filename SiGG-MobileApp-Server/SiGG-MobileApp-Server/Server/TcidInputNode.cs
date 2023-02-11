using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    public class TcidInputNode
    {
        public int m_instanceNo;
        public string m_inputNodeName;

        public bool m_hasDtfInput;
        public InputsConfiguration.INPUT_TYPE m_dtfInputType;
        public Input m_dtfInput = null;

        public bool m_hasTofInput;
        public InputsConfiguration.INPUT_TYPE m_tofInputType;
        public Input m_tofInput = null;

        private List<SettingData> m_allSettingData;

        public TcidInputNode()
        {
            m_allSettingData = new List<SettingData>();
        }

        public void GetSplitTcidAndProtocolType(string key, ref string tcid, ref string s_protocolType)
        {
            int index = key.IndexOf("_");
            tcid = key.Substring(0, index);
            s_protocolType = key.Substring(index + 1, key.Length - index - 1);
        }

        public void ReadInput(int instanceNo
                              , iniFileManager iniFileMgr
                              , int inputIndex
                              , ref List<TcidInputNode> m_listTcidInputsNode
                              , string tcid
                              , string key
                              , InputsConfiguration.INPUT_TYPE inputType)
        {
            if (inputType == InputsConfiguration.INPUT_TYPE.INPUT_TYPE_SOCKET_DTF)
            {
                m_listTcidInputsNode[inputIndex].ReadDtfInput(instanceNo, iniFileMgr, tcid, true, key, inputType);
            }
            else if (inputType == InputsConfiguration.INPUT_TYPE.INPUT_TYPE_SOCKET_TOF)
            {
                m_listTcidInputsNode[inputIndex].ReadTofInput(instanceNo, iniFileMgr, tcid, true, key, inputType);
            }
        }

        public void ReadDtfInput(int instanceNo
                                 , iniFileManager iniFileMgr
                                 , string inputNodeName
                                 , bool hasDtfInput
                                 , string inputKey
                                 , InputsConfiguration.INPUT_TYPE dtfInputType)
        {
            m_instanceNo = instanceNo;
            m_inputNodeName = inputNodeName;
            m_hasDtfInput = hasDtfInput;
            m_dtfInputType = dtfInputType;

            m_dtfInput = new InputDtf(instanceNo, iniFileMgr, inputKey, m_dtfInputType);
        }

        public void ReadTofInput(int instanceNo
                                 , iniFileManager iniFileMgr
                                 , string inputNodeName
                                 , bool hasTofInput
                                 , string inputKey
                                 , InputsConfiguration.INPUT_TYPE tofInputType)
        {
            m_instanceNo = instanceNo;
            m_inputNodeName = inputNodeName;
            m_hasTofInput = hasTofInput;
            m_tofInputType = tofInputType;

            m_tofInput = new InputTof(instanceNo, iniFileMgr, inputKey, m_tofInputType);
        }

        public void LoadAllSettingData()
        {
            string key = "";
            string value = "";

            if (m_hasDtfInput)
            {
                #region DTF Node
                key = ConfigManager.Instance.NODE_NAME_DTF;

                ConfigManager.Instance.AddSettingData(true
                                                       , false
                                                       , key
                                                       , value
                                                       , key
                                                       , ""
                                                       , ""
                                                       , FieldStaus.Inactive_InputDtf
                                                       , FieldShow.Both
                                                       , true
                                                       , false
                                                       , GetServiceName()
                                                       , ref m_allSettingData);
                #endregion
            }

            if (m_hasTofInput)
            {
                #region TOF Node
                key = ConfigManager.Instance.NODE_NAME_TOF;

                ConfigManager.Instance.AddSettingData(true
                                                       , false
                                                       , key
                                                       , value
                                                       , key
                                                       , ""
                                                       , ""
                                                       , FieldStaus.Inactive_InputTof
                                                       , FieldShow.Both
                                                       , true
                                                       , false
                                                       , GetServiceName()
                                                       , ref m_allSettingData);
                #endregion
            }
        }

        public List<SettingData> GetAllSettingData()
        {
            m_allSettingData.Clear();

            LoadAllSettingData();

            return m_allSettingData;
        }

        public int GetInstanceNo()
        {
            return m_instanceNo;
        }

        public string GetServiceName()
        {
            return string.Format("{0}{1}", ConfigManager.Instance.GetPrefixServiceName(), m_instanceNo.ToString("00"));
        }
    }
}
