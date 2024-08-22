using System;

public class MainState
{
	public enum State
	{
		Home,
		Ingame,
		GameOver,
		Pause,
		Waiting,
		Exit
	}

	public enum TypePlay
	{
		Normal,
		Tutorial
	}

	public static MainState.State state;

	public static MainState.TypePlay typePlay;

	public static MainState.State GetState
	{
		get
		{
			return MainState.state;
		}
	}

	public static void SetState(MainState.State newState)
	{
		MainState.state = newState;
	}
}
