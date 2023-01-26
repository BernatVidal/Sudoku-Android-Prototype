using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	#region Fields

	public static GameEvents current;
	public event Action<Cell_Info, bool> onCellButtonPressed;
	public event Action<int[], bool> onSelectedCellPossibilites;
	public event Action<int, bool> onNumberButtonPressed;
	#endregion
	

	#region Unity Methods
	/// Singleton pattern
	void Awake()
	{
		current = this;
	}

	#endregion


	#region Public Methods

	/// Event called when a cell from the grid is pressed
	public void OnCellButtonPressed(Cell_Info info, bool isSetted)
    {
		if (onCellButtonPressed != null)
        {
			onCellButtonPressed(info, isSetted);
		}
    }

	/// Event called when a cell from the grid selected has its values calculated (triggered automatically after OnCellButtonPressed)
	public void OnSelectedCellPossibilites(int[] nums, bool isSetted)
    {
		if (onSelectedCellPossibilites != null)
        {
			onSelectedCellPossibilites(nums, isSetted);
        }
    }

	/// Event called when a number button from the UI is pressed to replace a cell value
	public void OnNumberButtonPressed(int num, bool isSetted)
    {
		if (onNumberButtonPressed != null)
        {
			onNumberButtonPressed(num, isSetted);
        }
    }

	#endregion
	
}