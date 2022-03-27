using Singleton;

namespace Audio.Managers
{
    public class UIAudioPerdurableValues : Singleton<UIAudioPerdurableValues>
    {
        private static float m_MasterFaderValue = 1;
        public static float MasterFaderValue
        {
            get { return m_MasterFaderValue; }
            set { m_MasterFaderValue = value; }
        }

        private static float m_MxFaderValue = 1;
        public static float MxFaderValue
        {
            get { return m_MxFaderValue; }
            set { m_MxFaderValue = value; }
        }

        private static float m_SfxFaderValue = 1;
        public static float SfxFaderValue
        {
            get { return m_SfxFaderValue; }
            set { m_SfxFaderValue = value; }
        }

        private static float m_DxFaderValue = 1;
        public static float DxFaderValue
        {
            get { return m_DxFaderValue; }
            set { m_DxFaderValue = value; }
        }
    }
}