using UnityEngine;
using UnityEngine.SceneManagement;

public class SudokuGameManager : MonoBehaviour
{

	#region Fields
	[SerializeField] Grid_Controller grid;
	#endregion
		

	#region Unity Methods

	void Start()
    {
		int[,] _sudokugrid = SudokuGenerator.GenerateSudokuGrid(Grid_Controller.GRID_SIZE, SettingsManager.Instance.DifficultyValues);
        grid.SetupGrid(_sudokugrid);
    }

	#endregion

	#region Public Methods

	public void GoToMenu()
    {
		SceneManager.LoadScene(0);
    }


	public void NextGame()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	#endregion

}