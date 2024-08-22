using System;
using UnityEngine;

public class Settings
{
	public static int GetBest
	{
		get
		{
			if (!Settings.HasKey("best"))
			{
				Settings.SetBest(0);
			}
			return PlayerPrefs.GetInt("best");
		}
	}

	public static int GetLevel
	{
		get
		{
			if (!Settings.HasKey("level"))
			{
				Settings.SetLevel(1);
			}
			return PlayerPrefs.GetInt("level");
		}
	}

	public static int GetMaxLevel
	{
		get
		{
			if (!Settings.HasKey("maxlevel"))
			{
				Settings.SetMaxLevel(1);
			}
			return PlayerPrefs.GetInt("maxlevel");
		}
	}

	public static int GetTypeMove
	{
		get
		{
			return PlayerPrefs.GetInt("typemove");
		}
	}

	public static int GetSound
	{
		get
		{
			if (!Settings.HasKey("sound"))
			{
				Settings.SetSound(1);
			}
			return PlayerPrefs.GetInt("sound");
		}
	}

	public static int GetBGSound
	{
		get
		{
			if (!Settings.HasKey("soundbg"))
			{
				Settings.SetBGSound(1);
			}
			return PlayerPrefs.GetInt("soundbg");
		}
	}

	public static int GetMusic
	{
		get
		{
			if (!Settings.HasKey("music"))
			{
				Settings.SetMusic(1);
			}
			return PlayerPrefs.GetInt("music");
		}
	}

	public static int GetTime
	{
		get
		{
			return PlayerPrefs.GetInt("time");
		}
	}

	public static int GetTimePlay
	{
		get
		{
			if (!Settings.HasKey("timeplay"))
			{
				Settings.SetTimePlay(0);
			}
			return PlayerPrefs.GetInt("timeplay");
		}
	}

	public static int GetContinue
	{
		get
		{
			if (!Settings.HasKey("continue"))
			{
				Settings.SetContinue(0);
			}
			return PlayerPrefs.GetInt("continue");
		}
	}

	public static string GetContinueData
	{
		get
		{
			if (!Settings.HasKey("continuedata"))
			{
				Settings.SetContinueData(string.Empty);
			}
			return PlayerPrefs.GetString("continuedata");
		}
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void SetBest(int value)
	{
		PlayerPrefs.SetInt("best", value);
	}

	public static void SetLevel(int value)
	{
		PlayerPrefs.SetInt("level", value);
	}

	public static void SetMaxLevel(int value)
	{
		PlayerPrefs.SetInt("maxlevel", value);
	}

	public static void SetTypeMove(int value)
	{
		PlayerPrefs.SetInt("typemove", value);
	}

	public static void SetSound(int value)
	{
		PlayerPrefs.SetInt("sound", value);
	}

	public static void SetBGSound(int value)
	{
		PlayerPrefs.SetInt("soundbg", value);
	}

	public static void SetMusic(int value)
	{
		PlayerPrefs.SetInt("music", value);
	}

	public static void SetTime(int value)
	{
		PlayerPrefs.SetInt("time", value);
	}

	public static void SetTimePlay(int value)
	{
		PlayerPrefs.SetInt("timeplay", value);
	}

	public static void SetStar(int value, int index)
	{
		PlayerPrefs.SetInt("star" + index, value);
	}

	public static int GetStar(int index)
	{
		if (!Settings.HasKey("star" + index))
		{
			Settings.SetStar(1, index);
		}
		return PlayerPrefs.GetInt("star" + index);
	}

	public static void SetContinue(int value)
	{
		PlayerPrefs.SetInt("continue", value);
	}

	public static void SetContinueData(string value)
	{
		PlayerPrefs.SetString("continuedata", value);
	}
}
