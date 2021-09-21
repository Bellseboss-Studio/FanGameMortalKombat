#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class AkiOSPluginActivator
{
	static AkiOSPluginActivator()
	{
		AkPluginActivator.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.iOS, "iOS");
		AkPluginActivator.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.tvOS, "tvOS");
		AkBuildPreprocessor.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.iOS, "iOS");
		AkBuildPreprocessor.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.tvOS, "iOS");
	}
}
#endif