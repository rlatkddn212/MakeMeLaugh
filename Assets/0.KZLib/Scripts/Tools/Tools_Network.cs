using UnityEngine;

public static partial class Tools
{
	public static bool CheckInternet()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}
}