using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : CharacterController
{
	public List<GameObject> multipleTargetList;

	[HideInInspector] public UnityEvent onRequsetMutipleTarget;
	public UnityEvent onChangedHealth;
	public UnityEvent onChangedLevel;
	public PlayerStatus playerStatus;
	public int[] amountLevelUpExpArray;

	private int amountLvUpIndex;


	// Start is called before the first frame update
	private void Awake()
	{

		playerStatus = (PlayerStatus)status;
		GetData();

		var maxIndex = amountLevelUpExpArray.Length - 1;
		amountLvUpIndex = Mathf.Clamp(playerStatus.level - 1, 0, maxIndex);
		playerStatus.expMax = amountLevelUpExpArray[amountLvUpIndex];
		CalcLevel();

		currentHealth = playerStatus.health;
		playerStatus.CurrentHealth = currentHealth;
		DefaultInit();
		isCoolTime = true;


	}

	private void Update()
	{
		DefaultUpdate();
		playerStatus.CurrentHealth = currentHealth;
	}

	public void CalcLevel()
	{
		var maxIndex = amountLevelUpExpArray.Length - 1;
		amountLvUpIndex = Mathf.Clamp(playerStatus.level - 1, 0, maxIndex);

		while (playerStatus.exp >= amountLevelUpExpArray[amountLvUpIndex])
		{

			playerStatus.level++;
			playerStatus.exp -= amountLevelUpExpArray[amountLvUpIndex];

			amountLvUpIndex = Mathf.Clamp(playerStatus.level - 1, 0, maxIndex);
			playerStatus.expMax = amountLevelUpExpArray[amountLvUpIndex];
			onChangedLevel.Invoke();


		}
	}

	public void SetTarget(GameObject target)
	{
		targetObject = target;
	}

	public void OnAttackMultipleTrigger()
	{
		var damage = status.attackPower + status.skillList[attackCycleIndex].skillAttackPower;
		onRequsetMutipleTarget.Invoke();

		if (status.skillList[attackCycleIndex].isAttack)
			foreach (GameObject VARIABLE in multipleTargetList)
				VARIABLE.GetComponent<CharacterController>().ChangeHealth(-damage);
		if (status.skillList[attackCycleIndex].isHealing)
			ChangeHealth(damage * status.skillList[attackCycleIndex].healingParameter);

		multipleTargetList.Clear();
	}

	public override void ChangeHealth(int val)
	{
		base.ChangeHealth(val);

		if (currentHealth < 1)
			currentHealth = 1;
		else if (playerStatus.health < currentHealth)
			currentHealth = playerStatus.health;

		playerStatus.CurrentHealth = currentHealth;
		onChangedHealth.Invoke();
	}

	private void GetData()
	{
		if (PlayerPrefs.HasKey("Level"))
			playerStatus.level = PlayerPrefs.GetInt("Level");

		if (PlayerPrefs.HasKey("Level"))
			playerStatus.exp = PlayerPrefs.GetInt("Exp");

	}
}