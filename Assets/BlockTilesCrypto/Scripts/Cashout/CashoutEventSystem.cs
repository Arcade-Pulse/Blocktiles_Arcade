using System;

public static class CashoutEventSystem
{
    public static event Action OnCashoutButtonClicked;

    public static void InvokeCashoutButtonClicked()
    {
        OnCashoutButtonClicked?.Invoke();
    }
}
