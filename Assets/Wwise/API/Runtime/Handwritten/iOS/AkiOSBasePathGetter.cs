#if (UNITY_IOS || UNITY_TVOS) && !UNITY_EDITOR
public partial class AkBasePathGetter
{
	static string DefaultPlatformName = "iOS";
}
#endif