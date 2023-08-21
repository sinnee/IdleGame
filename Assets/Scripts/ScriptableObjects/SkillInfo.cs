using UnityEngine;

[CreateAssetMenu(fileName = "Sample Skill Data", menuName = "Scriptable Object/Skill Info")]
public class SkillInfo : ScriptableObject
{
	public string skillName;
	public int skillAttackPower;
	public int healingParameter;
	public float attackDistance;
	public float attackAngle;
	public float animDuration;
	public bool isAttack;
	public bool isMultipleAttack;
	public bool isHealing;
}