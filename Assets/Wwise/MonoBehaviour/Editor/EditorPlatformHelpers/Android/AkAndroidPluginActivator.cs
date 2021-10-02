#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
public class AkAndroidPluginActivator
{
	static AkAndroidPluginActivator()
	{
		AkPluginActivator.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.Android, "Android");
		AkBuildPreprocessor.BuildTargetToPlatformName.Add(UnityEditor.BuildTarget.Android, "Android");
	}
}
#endif