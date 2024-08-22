using System.Text.RegularExpressions;
using UnityEngine;

public static class GeneralFunctions
{
    public static bool ValidateEmail(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);

        if (match.Success)
            return true;
        else
            return false;
    }

    public static bool IsEmpty(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string RandomDigitCodeGeneration(int numberOfCharacters)
    {
        const string glyphs = "0123456789"; //add the characters you want
        int charAmount = numberOfCharacters;
        var myString = "";
        for (int i = 0; i < charAmount; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }

        //Debug.Log("room number is: " + myString);
        return myString;
    }

    public static void GetDeviceID(out string android_id, out string ios_id, out string custom_id)
    {
        android_id = string.Empty;
        ios_id = string.Empty;
        custom_id = string.Empty;

#if UNITY_ANDROID && !UNITY_EDITOR
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
            android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");
        }

#elif UNITY_IOS
        {
            ios_id = UnityEngine.iOS.Device.vendorIdentifier;
        }
#else
        {
            custom_id = SystemInfo.deviceUniqueIdentifier;
        }
#endif
    }
}
