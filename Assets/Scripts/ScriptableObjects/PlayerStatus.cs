using UnityEngine;

[CreateAssetMenu(fileName = "SampleEnemyStatusData", menuName = "Scriptable Object/Player Status")]
public class PlayerStatus : CharacterStatus
{
	public int level;

	public int expMax;
	private int currentHealth;

	public int CurrentHealth
	{
		get => currentHealth;
		set
		{
			if (value < 1)
				currentHealth = 1;
			else if (value > health)
				currentHealth = health;
			else
				currentHealth = value;
		}
	}
}