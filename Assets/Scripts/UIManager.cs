using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public TextMeshProUGUI expText;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI Level;

	public TextMeshProUGUI IdleMText;

	public PlayerStatus playerStatus;

	public Slider HealthSlider;
	public Slider ExpSlider;

	public IdleManager idleManager;

	public GameObject rewardPanel;

	// Start is called before the first frame update
	private void Start()
	{
		HealthSlider.maxValue = playerStatus.health;
		OnChangExpValue();
		OnChangeHealth();
		OnChangLevel();
		OnRewardPanel();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		rewardPanel.SetActive(true);
	}


	public void OnChangExpValue()
	{
		expText.text = "Exp " + playerStatus.exp;
		ExpSlider.value = playerStatus.exp;
		PlayerPrefs.SetInt("Exp", playerStatus.exp);
		PlayerPrefs.Save();
	}

	public void OnChangLevel()
	{
		ExpSlider.maxValue = playerStatus.expMax;
		Level.text = "Lv " + playerStatus.level;
		PlayerPrefs.SetInt("Level", playerStatus.level);
		PlayerPrefs.Save();
	}

	public void OnChangeHealth()
	{
		healthText.text = "Health " + playerStatus.CurrentHealth;
		HealthSlider.value = playerStatus.CurrentHealth;
	}

	public void OnRewardPanel()
	{
		IdleMText.text = $"{idleManager.minute} minute\nExp : {idleManager.gainExp}";
	}
}