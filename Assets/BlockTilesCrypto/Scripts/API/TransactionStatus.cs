public class TransactionStatus
{
    public bool error;
    public string message;
    public bool found;
    public bool pending;
    public bool completed;
    public bool failed;


    public TransactionStatus()
    {
        found = true;
        pending = false;
        completed = false;
        failed = false;
        error = false;
        message = "";
    }
}
