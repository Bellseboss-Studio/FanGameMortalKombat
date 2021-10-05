using System;

[Serializable]
public class SystemInfoCustom
{
  public string model;
  public string name;
  public string os;
  public string processor;
  public string graphicsDeviceName;
}
/*
Debug.Log($"S{SystemInfo.deviceModel}");
Debug.Log($"S{SystemInfo.deviceName}");
Debug.Log($"S{SystemInfo.operatingSystem}");
Debug.Log($"S{SystemInfo.processorType}");
Debug.Log($"S{SystemInfo.graphicsDeviceName}");
*/