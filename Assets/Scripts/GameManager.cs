using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
	public IdleManager idleManager;
	public GameObject player;

	[HideInInspector] public List<GameObject> monsterPool;

	public List<GameObject> monsterPrefabList;

	public int poolSize;


	public int maxEnemyCount;
	public float spawnCycle;
	[FormerlySerializedAs("radius")] public float spawnRadius = 5.0f;

	public UnityEvent OnExpValueChange;
	public float gainExpPerMin;

	private PlayerController _playerController;
	private int spawnedMonster;
	private bool isCoolTime;

	private void Awake()
	{
		spawnedMonster = 0;
		_playerController = player.GetComponent<PlayerController>();

		#region CaluGainExp

		EnemyController monsterController = monsterPrefabList[0].GetComponent<EnemyController>();
		var powerPerSec = _playerController.playerStatus.attackPerSec * _playerController.playerStatus.attackPower;
		var killingMPerSec = 1 / (monsterController.status.health / powerPerSec);
		var gainExpPerSec = killingMPerSec * monsterController.status.exp;
		gainExpPerMin = gainExpPerSec * 60;

		#endregion

	}

	private void Start()
	{

		monsterPool = new List<GameObject>();
		for (var i = 0; i < poolSize; i++)
		{
			GameObject monsterGameObject = Instantiate(monsterPrefabList[0], transform, true);
			EnemyController enemyController = monsterGameObject.GetComponent<EnemyController>();
			enemyController.OnDead.AddListener(() => SendEnemyDead(monsterGameObject));
			enemyController.OnDeadFinish.AddListener(() => ReturnBeatBarToPool(monsterGameObject));
			monsterGameObject.name = $"Monster {i}";
			monsterGameObject.SetActive(false);
			monsterPool.Add(monsterGameObject);
		}

		isCoolTime = true;
	}

	private void Update()
	{
		if (isCoolTime)
		{
			StartCoroutine(StartCycle(spawnCycle));
			SpawnMonster(maxEnemyCount);
		}
	}


	private void SpawnMonster(int count)
	{
		var numberMonsterCount = count - spawnedMonster;
		if (numberMonsterCount < 1)
			return;

		for (var i = 0; i < numberMonsterCount; i++)
			GetMonsterFromPool(CalculateCircularPosition(Random.Range(0f, 360f)));
	}

	public GameObject GetMonsterFromPool(Vector3 position)
	{
		foreach (GameObject monster in monsterPool)
			if (!monster.activeInHierarchy)
			{
				monster.transform.position = position;
				monster.SetActive(true);
				CalcTarget();
				spawnedMonster++;

				return monster;
			}

		GameObject newMonster = Instantiate(monsterPrefabList[0], position, new Quaternion());
		newMonster.GetComponent<EnemyController>().OnDead.AddListener(() =>
		SendEnemyDead(newMonster));
		newMonster.transform.SetParent(transform);
		monsterPool.Add(newMonster);
		CalcTarget();
		spawnedMonster++;

		return newMonster;
	}

	public void ReturnBeatBarToPool(GameObject monsterGameObject)
	{
		monsterGameObject.transform.position = new Vector3(100f, 100f, 100f);
		monsterGameObject.SetActive(false);
		spawnedMonster--;
	}

	private IEnumerator StartCycle(float time)
	{
		isCoolTime = false;
		yield return new WaitForSeconds(time);
		isCoolTime = true;
	}

	private void CalcTarget()
	{
		var shortestDistance = float.MaxValue;
		var shortestIndex = 0;
		float distance;

		for (var i = 0; i < monsterPool.Count; i++)
		{
			GameObject monster = monsterPool[i];
			EnemyController enemyController = monster.GetComponent<EnemyController>();

			if (monster.activeInHierarchy && !enemyController.isDead)
			{
				distance = Vector3.Distance(player.transform.position, monsterPool[i].transform.position);
				if (distance < shortestDistance)
				{
					shortestDistance = distance;
					shortestIndex = i;
				}

			}
		}

		player.GetComponent<PlayerController>().SetTarget(monsterPool[shortestIndex]);


	}

	public void CalcMutipleTarget()
	{
		var attackDistance = _playerController.status.skillList[_playerController.attackCycleIndex].attackDistance;
		var attackAngleHalf = _playerController.status.skillList[_playerController.attackCycleIndex].attackAngle / 2;

		for (var i = 0; i < monsterPool.Count; i++)
		{
			GameObject monster = monsterPool[i];
			EnemyController enemyController = monster.GetComponent<EnemyController>();

			if (monster.activeInHierarchy && !enemyController.isDead)
			{
				Vector3 monsterPosition = monster.transform.position;
				var distanceToMonster = Vector3.Distance(player.transform.position, monsterPosition);
				var angleToMonster = GetAngle(player.transform.position, monsterPosition) - player.transform.rotation.eulerAngles.y;
				angleToMonster = (angleToMonster + 720) % 360;

				if (distanceToMonster <= attackDistance &&
					(0 <= angleToMonster && angleToMonster <= attackAngleHalf ||
					360 - attackAngleHalf <= angleToMonster && angleToMonster <= 360))
				{
					_playerController.multipleTargetList.Add(monster);
				}
			}
		}
	}

	public void SendEnemyDead(GameObject enemy)
	{
		_playerController.playerStatus.exp += enemy.GetComponent<EnemyController>().status.exp;
		_playerController.CalcLevel();
		OnExpValueChange.Invoke();
		CalcTarget();
	}


	public Vector3 CalculateCircularPosition(float angleDegrees)
	{
		var angleRadians = angleDegrees * Mathf.Deg2Rad;

		Vector3 position = player.transform.position;
		var x = position.x + spawnRadius * Mathf.Cos(angleRadians);
		var z = position.z + spawnRadius * Mathf.Sin(angleRadians);

		return new Vector3(x, position.y, z);
	}

	public static float GetAngle(Vector3 vStart, Vector3 vEnd)
	{
		Vector3 v = vEnd - vStart;
		var angle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;

		if (angle < 0)
			angle += 360;

		return angle;
	}
}