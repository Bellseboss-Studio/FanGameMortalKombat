#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class AkWSAPluginActivator
{
	static AkWSAPluginActivator()
	{
		AkPluginActivator.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.WSAPlayer, "WSA");
		AkBuildPreprocessor.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.WSAPlayer, "Windows");
	}
}
#endif