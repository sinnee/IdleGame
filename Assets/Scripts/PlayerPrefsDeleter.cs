using UnityEngine;

public class PlayerPrefsDeleter : MonoBehaviour
{
	private void Start()
	{
		// "Score" 키에 저장된 값을 삭제
		PlayerPrefs.DeleteKey("Level");
		PlayerPrefs.DeleteKey("Exp");

		// "Score" 키에 대한 값이 삭제되었는지 확인
		var hasScore = PlayerPrefs.HasKey("Score");

		if (hasScore)
		{
			Debug.Log("Score still exists.");
		}
		else
		{
			Debug.Log("Score has been deleted.");
		}
	}
}