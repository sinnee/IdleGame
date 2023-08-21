using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
	private GameManager _targetRef;

	private GUIStyle boldLargeLabelStyle;

	private int maxEnemyCount;

	private SerializedProperty maxEnemyCountProp;
	private float spawnCycle;
	private SerializedProperty spawnCycleProp;
	private float spawnRadius;
	private SerializedProperty spawnRadiusProp;


	private void OnEnable()
	{
		_targetRef = (GameManager)target;

		SceneView.duringSceneGui += OnSceneGUI;

		maxEnemyCountProp = serializedObject.FindProperty($"{nameof(GameManager.maxEnemyCount)}");
		spawnCycleProp = serializedObject.FindProperty($"{nameof(GameManager.spawnCycle)}");
		spawnRadiusProp = serializedObject.FindProperty($"{nameof(GameManager.spawnRadius)}");

		boldLargeLabelStyle = new GUIStyle(EditorStyles.label)
		{
		fontSize = 18,
		fontStyle = FontStyle.Bold
		};
	}

	private void OnDisable()
	{
		SceneView.duringSceneGui -= OnSceneGUI;


	}

	private void OnSceneGUI(SceneView obj)
	{
		Handles.Disc(_targetRef.player.transform.rotation, _targetRef.player.transform.position, new Vector3(0, 1, 0), spawnRadius, false,
		1);
		Handles.Label(_targetRef.player.transform.position + Vector3.right * spawnRadius, "몬스터의 생성 반경");
	}

	public override void OnInspectorGUI()
	{


		base.OnInspectorGUI();

		serializedObject.Update();

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


		EditorGUILayout.LabelField("몬스터 스포너 설정", boldLargeLabelStyle);
		EditorGUILayout.Space();

		maxEnemyCount = EditorGUILayout.IntField("몬스터의 동시 생성 가능한 최대 수", maxEnemyCountProp.intValue);
		spawnCycle = EditorGUILayout.FloatField("몬스터의 생성 주기", spawnCycleProp.floatValue);
		spawnRadius = EditorGUILayout.FloatField("몬스터의 생성 반경", spawnRadiusProp.floatValue);

		maxEnemyCount = Mathf.Max(maxEnemyCount, 1);
		spawnCycle = Mathf.Max(spawnCycle, 1f);
		spawnRadius = Mathf.Max(spawnRadius, 1f);

		maxEnemyCountProp.intValue = maxEnemyCount;
		spawnCycleProp.floatValue = spawnCycle;
		spawnRadiusProp.floatValue = spawnRadius;

		serializedObject.ApplyModifiedProperties();
	}
}