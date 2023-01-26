using UnityEngine;

public class SettingsManager : MonoBehaviour
{
	#region Fields
	public static SettingsManager Instance { get; private set; }

	int difficultyLevel;
	int[] difficultyValues = { 40, 30, 20 };
	#endregion

	#region Properties
	public int DifficultyValues => difficultyValues[difficultyLevel];

    #endregion

    #region Unity Methods

    void Awake()
	{
		// Singleton
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	#endregion
	
	#region Public Methods

	public void SetDifficulty(int val)
    {
		difficultyLevel = val;
		Debug.Log($"Difficulty:{difficultyLevel}");
    }
	#endregion


}