public class TransactionError
{
    public bool error;
    public string message;

    public TransactionError(bool err, string msg)
    {
        error = err;
        message = msg;
    }
}
