using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	#region Fields
	[SerializeField] GameObject buttonsPanel;
	[SerializeField] Button[] gameModeBtns;
	[SerializeField] Button closeBtn;
	#endregion


	#region Unity Methods

	void Start()
	{		
		/// Set Onclicks
		for(int i = 0; i < gameModeBtns.Length; i++)
		{
			int _i = i;
			gameModeBtns[i].onClick.AddListener(() => OnDifficultySet(_i));			
        }
        closeBtn.onClick.AddListener(OnClose);

	}
	#endregion
	
	
	#region Private Methods

	void OnDifficultySet(int id)
    {
		SettingsManager.Instance.SetDifficulty(id);
		SceneManager.LoadScene(1);
    }

	void OnClose()
    {
		Debug.Log("Close App");
		Application.Quit();
    }

	
	#endregion


}