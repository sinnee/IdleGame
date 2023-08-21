using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Status Data", menuName = "Scriptable Object/Character Status")]
public class CharacterStatus : ScriptableObject
{
	public int health;
	public int attackPower;
	public float attackPerSec;

	public float attackCycle;
	public int exp;
	public List<SkillInfo> skillList;


	public void CalcAttackCycle()
	{
		attackCycle = 1.0f / attackPerSec;
	}

	public float CalcAttackSpeedMult(int index)
	{
		if (attackCycle < skillList[index].animDuration)
			return skillList[index].animDuration / attackCycle;
		return 1.0f;
	}
}