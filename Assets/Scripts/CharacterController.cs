using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
	protected static readonly int Speed = Animator.StringToHash("Speed");
	protected static readonly int Dead = Animator.StringToHash("Dead");
	public GameObject targetObject;
	public CharacterStatus status;
	public float rotationSpeed = 10;
	public int currentHealth;

	public int attackCycleIndex;
	protected Animator _animator;

	private ParticleSystem _bloodParticle;
	private ParticleSystem _healingParticle;

	protected NavMeshAgent _navMeshAgent;

	protected bool isCoolTime;

	protected void Attack()
	{
		attackCycleIndex = (attackCycleIndex + 1) % status.skillList.Count;

		_animator.SetTrigger(status.skillList[attackCycleIndex].skillName);
		StartCoroutine(StartCooldown(status.attackCycle));
	}


	public void OnAttackSingleTrigger()
	{
		var damage = status.attackPower + status.skillList[attackCycleIndex].skillAttackPower;
		if (status.skillList[attackCycleIndex].isAttack)
			targetObject.GetComponent<CharacterController>().ChangeHealth(-damage);
		if (status.skillList[attackCycleIndex].isHealing)
			ChangeHealth(damage * status.skillList[attackCycleIndex].healingParameter);
	}

	public virtual void ChangeHealth(int val)
	{
		if (val < 0)
		{
			_bloodParticle.Play();
		}
		else
		{
			_healingParticle?.Play();
		}
		currentHealth += val;
	}

	protected IEnumerator StartCooldown(float time)
	{
		isCoolTime = false;
		yield return new WaitForSeconds(time);
		isCoolTime = true;
	}

	protected void DefaultInit()
	{
		_bloodParticle = transform.Find("BloodParticle")?.GetComponent<ParticleSystem>();
		_healingParticle = transform.Find("HealingParticle")?.GetComponent<ParticleSystem>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		attackCycleIndex = 0;

		for (var i = 0; i < status.skillList.Count; i++)
			_animator.SetFloat(status.skillList[i].skillName + "Speed", status.CalcAttackSpeedMult(i));
	}

	protected void DefaultUpdate()
	{
		_animator.SetFloat(Speed, _navMeshAgent.velocity.magnitude);

		if (targetObject != null)
		{
			_navMeshAgent.SetDestination(targetObject.transform.position);

			Vector3 directionToTarget = targetObject.transform.position - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

			//공격 판정
			if (Vector3.Distance(transform.position, targetObject.transform.position) <=
				status.skillList[attackCycleIndex].attackDistance && isCoolTime)
				Attack();


		}
	}
}