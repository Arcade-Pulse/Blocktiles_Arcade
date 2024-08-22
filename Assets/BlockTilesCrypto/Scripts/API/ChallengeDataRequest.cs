using System;

public class ChallengeDataRequest
{
    public string question { get; set; }
    public int answer { get; set; }
    public DateTime initiatedAt { get; set; }
    public int chances { get; set; }
    public bool isCompleted { get; set; }
}
