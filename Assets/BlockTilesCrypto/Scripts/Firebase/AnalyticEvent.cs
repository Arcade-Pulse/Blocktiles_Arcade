using Firebase.Analytics;
using UnityEngine;

public class AnalyticEvent : MonoBehaviour
{
    private void Start()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency);
    }
}
