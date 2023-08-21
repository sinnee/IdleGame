using UnityEngine;
using UnityEngine.Events;

public class EnemyController : CharacterController
{
	public bool isDead;
	public UnityEvent OnDead;
	public UnityEvent OnDeadFinish;


	// Start is called before the first frame update
	private void Awake()
	{
		DefaultInit();
		targetObject = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		if (!isDead)
			DefaultUpdate();
	}


	private void OnEnable()
	{
		isCoolTime = true;
		isDead = false;

		//status init
		currentHealth = status.health;
		_navMeshAgent.enabled = true;
	}

	public override void ChangeHealth(int val)
	{
		base.ChangeHealth(val);

		if (currentHealth < 1)
			Die();
	}

	private void Die()
	{
		isDead = true;
		_animator.SetTrigger(Dead);
		_navMeshAgent.enabled = false;
		OnDead.Invoke();
	}

	public void OnDeadTrigger()
	{
		OnDeadFinish.Invoke();
	}
}