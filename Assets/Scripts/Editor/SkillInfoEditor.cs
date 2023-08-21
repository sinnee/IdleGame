using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillInfo))]
public class SkillInfoEditor : Editor
{
	private SerializedProperty attackAngleProp;
	private SerializedProperty attackDistanceProp;
	private SerializedProperty healingParameterProp;
	private SerializedProperty isAttackProp;
	private SerializedProperty isHealingProp;
	private SerializedProperty isMultipleAttackProp;

	private Camera lastActiveCamera;
	private GameObject player;
	private float previousFOV;
	private SceneView sceneView;

	private SerializedProperty skillAttackPowerProp;
	private SkillInfo targetRef;
	private bool wasPerspective;

	private void OnEnable()
	{
		sceneView = SceneView.lastActiveSceneView;
		player = GameObject.FindGameObjectWithTag("Player");
		SceneView.duringSceneGui += OnSceneGUI;


		targetRef = (SkillInfo)target;


		skillAttackPowerProp = serializedObject.FindProperty($"{nameof(SkillInfo.skillAttackPower)}");
		healingParameterProp = serializedObject.FindProperty($"{nameof(SkillInfo.healingParameter)}");
		attackDistanceProp = serializedObject.FindProperty($"{nameof(SkillInfo.attackDistance)}");
		attackAngleProp = serializedObject.FindProperty($"{nameof(SkillInfo.attackAngle)}");
		isAttackProp = serializedObject.FindProperty($"{nameof(SkillInfo.isAttack)}");
		isMultipleAttackProp = serializedObject.FindProperty($"{nameof(SkillInfo.isMultipleAttack)}");
		isHealingProp = serializedObject.FindProperty($"{nameof(SkillInfo.isHealing)}");


	}

	private void OnDisable()
	{
		SceneView.duringSceneGui -= OnSceneGUI;


	}

	private void OnSceneGUI(SceneView obj)
	{


		if (targetRef.isMultipleAttack)
		{
			Handles.Label(player.transform.position + Vector3.right * 1, targetRef.name);
			Handles.Disc(player.transform.rotation, player.transform.position, new Vector3(0, 1, 0), targetRef.attackDistance, false, 1);

			var angleInRadians = (90 - targetRef.attackAngle / 2) * Mathf.Deg2Rad;
			var tangentValue = Mathf.Tan(angleInRadians);
			Vector3 test = new Vector3(Mathf.Cos(angleInRadians) * targetRef.attackDistance, 0,
			Mathf.Sin(angleInRadians) * targetRef.attackDistance);
			Handles.DrawLine(player.transform.position, test + player.transform.position);

			Vector3 test2 = new Vector3(Mathf.Cos(angleInRadians) * targetRef.attackDistance * -1, 0,
			Mathf.Sin(angleInRadians) * targetRef.attackDistance);
			Handles.DrawLine(player.transform.position, test2 + player.transform.position);

			DrawAttackDisc();

		}
	}

	public override void OnInspectorGUI()
	{


		base.OnInspectorGUI();

		serializedObject.Update();

		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);


		EditorGUILayout.LabelField("스킬 정보 설정", EditorStyles.boldLabel);

		EditorGUILayout.PropertyField(skillAttackPowerProp, new GUIContent("스킬 공격력"));


		EditorGUILayout.PropertyField(isAttackProp, new GUIContent("공격 스킬 여부"));
		if (isAttackProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(attackDistanceProp, new GUIContent("스킬 사거리"));
			EditorGUILayout.PropertyField(isMultipleAttackProp, new GUIContent("다중 공격여부"));
			if (isMultipleAttackProp.boolValue)
			{
				targetRef.skillName = "Attack2";
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(attackAngleProp, new GUIContent("스킬 각도"));
				EditorGUI.indentLevel--;
			}
			else
			{
				targetRef.skillName = "Attack1";
			}
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.PropertyField(isHealingProp, new GUIContent("회복 스킬 여부"));
		if (isHealingProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(healingParameterProp, new GUIContent("체력 회복 비율"));
			EditorGUI.indentLevel--;
		}


		skillAttackPowerProp.intValue = Mathf.Max(skillAttackPowerProp.intValue, 0);
		healingParameterProp.intValue = Mathf.Max(healingParameterProp.intValue, 0);
		attackDistanceProp.floatValue = Mathf.Max(attackDistanceProp.floatValue, 0);
		attackAngleProp.floatValue = Mathf.Clamp(attackAngleProp.floatValue, 0, 360);


		serializedObject.ApplyModifiedProperties();
	}

	private void DrawAttackDisc()
	{
		Vector3 discCenter = player.transform.position;
		Quaternion discRotation = player.transform.rotation;
		Vector3 discNormal = new Vector3(0, 1, 0);
		Color discColor = new Color(1f, 0.1f, 0.1f, 0.2f); // 내부를 채울 색상 설정 (파란색과 약간의 투명도)

		Handles.color = discColor;
		Handles.DrawSolidArc(discCenter, discNormal, Vector3.forward, targetRef.attackAngle / 2, targetRef.attackDistance);
		Handles.DrawSolidArc(discCenter, discNormal, Vector3.forward, targetRef.attackAngle / -2, targetRef.attackDistance);

		sceneView.LookAt(player.transform.position, Quaternion.Euler(90f, 0f, 0f));
		sceneView.orthographic = true;
	}
}