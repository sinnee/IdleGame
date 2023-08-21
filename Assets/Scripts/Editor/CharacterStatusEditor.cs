using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterStatus))]
public class CharacterStatusEditor : Editor
{
	private SerializedProperty attackPerSecProp;
	private SerializedProperty attackPowerProp;

	private SerializedProperty healthProp;

	private void OnEnable()
	{
		healthProp = serializedObject.FindProperty($"{nameof(CharacterStatus.health)}");
		attackPowerProp = serializedObject.FindProperty($"{nameof(CharacterStatus.attackPower)}");
		attackPerSecProp = serializedObject.FindProperty($"{nameof(CharacterStatus.attackPerSec)}");


	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		CharacterStatus characterStatus = (CharacterStatus)target;
		characterStatus.CalcAttackCycle();

		serializedObject.Update();
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

		EditorGUILayout.PropertyField(healthProp, new GUIContent("체력"));
		EditorGUILayout.PropertyField(attackPowerProp, new GUIContent("공격력"));
		EditorGUILayout.PropertyField(attackPerSecProp, new GUIContent("초당 공격 횟수"));

		serializedObject.ApplyModifiedProperties();
	}
}