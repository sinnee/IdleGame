using System;
using UnityEngine;
using UnityEngine.Events;

public class IdleManager : MonoBehaviour
{
	private const string ExitTimeKey = "ExitTime";
	public PlayerController playerController;
	public GameManager gameManager;
	public int minute;
	public int gainExp;
	public UnityEvent OnRewardPanel;
	public UnityEvent OnCalcLevel;
	private DateTime exitTime;

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			PlayerPrefs.SetString(ExitTimeKey, DateTime.Now.ToString());
			PlayerPrefs.Save();
		}
		else
		{
			if (PlayerPrefs.HasKey(ExitTimeKey))
			{
				var exitTimeString = PlayerPrefs.GetString(ExitTimeKey);
				DateTime.TryParse(exitTimeString, out exitTime);

				TimeSpan timeAway = DateTime.Now - exitTime;
				var minutesAway = timeAway.TotalMinutes;
				minute = (int)minutesAway;


				playerController.playerStatus.exp += CalcGainEp(gameManager.gainExpPerMin);
				OnCalcLevel.Invoke();
				OnRewardPanel.Invoke();

			}
		}
	}

	public int CalcGainEp(float KillMonstPerMin)
	{
		if (PlayerPrefs.HasKey(ExitTimeKey))
		{
			gainExp = (int)(minute * KillMonstPerMin);
			return gainExp;
		}
		return 0;
	}
}