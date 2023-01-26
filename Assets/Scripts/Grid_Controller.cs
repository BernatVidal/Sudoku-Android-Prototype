using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Controller : MonoBehaviour
{
    #region Fields
    public const int GRID_SIZE = 9;

    InteractiveCell[] cells;
	[SerializeField] GameObject endPanel;
	#endregion


	#region Unity Methods

	void Start()
	{
		GetCellGrids();
		GameEvents.current.onCellButtonPressed += OnCellClicked;
        GameEvents.current.onNumberButtonPressed += OnNumberButtonPressed;
		CheckForGameCompleted();
	}

    #endregion

    #region Public Methods
	/// <summary>
	/// Sets Up the sudoku grid cells according to the sended grid.
	/// The grid may be masked with 0's where the user will need to fill
	/// </summary>
	/// <param name="grid"></param>
    public void SetupGrid(int[,]grid)
    {
		for (int i = 0; i < GRID_SIZE; i++)
        {
			InteractiveCell[] _row = GetRow(i);
			for (int j = 0; j < GRID_SIZE; j++)
            {
				_row[j].SetCellValues (grid[i, j], grid[i,j] != 0);
            }
        }
    }

	#endregion
	
	#region Private Methods

	/// <summary>
	/// Get all the cells and assigns them to the corresponding row, column and block
	/// </summary>
	void GetCellGrids()
    {
		/// Get the cells
		cells = GetComponentsInChildren<InteractiveCell>();

		/// Assign them
		int _rowCount = 0;
		int _columnCount = 0;
		int _blockCount = 0;

		int _rowOffset = 0;
		int _colOffset = 0;

		foreach(InteractiveCell _cell in cells)
        {
			_cell.SetCellInfo(_rowCount, _columnCount, _blockCount);			

			/// Change Row
			if (++_columnCount >= 3 + _colOffset)
            {
				_columnCount = _colOffset;
				_rowCount++;
            }
			/// Change Block
			if (_rowCount >= 3 + _rowOffset)
			{
				_rowCount = _rowOffset;
				_blockCount++;

				/// Modify Offsets (Due to UI Layouts system setup)
				_colOffset = (_blockCount % 3) * 3;
				_columnCount = _colOffset;
                _rowOffset = _blockCount % 3 == 0? _blockCount : _rowOffset;
				_rowCount = _rowOffset;
            }			
        }

    }
	
	/// <summary>
	/// Checks if the game is completed, if so launches OnGameCompelte()
	/// </summary>
	void CheckForGameCompleted()
    {
		bool _isCompleted = true;
		foreach(InteractiveCell _cell in cells)
        {
			if (_cell.SettedValue <= 0)
			{
				_isCompleted = false;
				break;
			}
        }

		OnGameCompleted(_isCompleted);
    }

	#endregion


	#region InteractiveCells Methods
	/// <summary>
	/// Called from event manager action : onCellButtonPressed
	/// </summary>
	/// <param name="cellinfo">Cell_info from the clicked cell</param>
	/// <param name="isSetted">Wether or not the cell have a value already assigned</param>
	void OnCellClicked(Cell_Info cellinfo, bool isSetted)
    {
		foreach (InteractiveCell _cell in cells)
			_cell.Select(false);
		HighlightCells(cellinfo);

		GetCellNumberPossibilities(cellinfo, isSetted);
		//Debug.Log($"Row: {cellinfo.Row} | Col: {cellinfo.Column} | Block: {cellinfo.Block}");
	}

	/// <summary>
	/// Used to higlight cells that shares same row, column or block than the selected one
	/// </summary>
	/// <param name="cellinfo">Cell_info from the clicked cell</param>
	void HighlightCells(Cell_Info cellinfo)
    {
		/// Unhighlight All
		foreach (InteractiveCell _cell in cells)
			_cell.Highlight(false);

		/// Highlight Row
		foreach (InteractiveCell _cell in GetRow(cellinfo.Row))
			_cell.Highlight(true);

		/// Highlight Column
		foreach (InteractiveCell _cell in GetColumn(cellinfo.Column))
			_cell.Highlight(true);

		/// Highlight Block
		foreach (InteractiveCell _cell in GetBlock(cellinfo.Block))
			_cell.Highlight(true);

	}

	/// <summary>
	/// Returns a row of interactive cells
	/// </summary>
	/// <param name="rowId">Row Id to return</param>
	/// <returns></returns>
	InteractiveCell[] GetRow(int rowId)
    {
		InteractiveCell[] _selectedCells = new InteractiveCell[9];
		int _count = 0;
		foreach (InteractiveCell _cell in cells)
		{
			if (_cell.CellInfo.Row == rowId)
            {
				_selectedCells[_count++] = _cell;
            }
        }

		return _selectedCells;
    }

	/// <summary>
	/// Returns a Column of interactive cells
	/// </summary>
	/// <param name="columnId">Column Id to return</param>
	/// <returns></returns>
	InteractiveCell[] GetColumn(int columnId)
	{
		InteractiveCell[] _selectedCells = new InteractiveCell[9];
		int _count = 0;
		foreach (InteractiveCell _cell in cells)
		{
			if (_cell.CellInfo.Column == columnId)
			{
				_selectedCells[_count++] = _cell;
			}
		}

		return _selectedCells;
	}

	/// <summary>
	/// Returns a Block of interactive cells
	/// </summary>
	/// <param name="blockId">Block Id to return</param>
	/// <returns></returns>
	InteractiveCell[] GetBlock(int blockId)
	{
		InteractiveCell[] _selectedCells = new InteractiveCell[9];
		int _count = 0;
		foreach (InteractiveCell _cell in cells)
		{
			if (_cell.CellInfo.Block == blockId)
			{
				_selectedCells[_count++] = _cell;
			}
		}

		return _selectedCells;
	}

	/// <summary>
	/// Get the cell numbers used
	/// </summary>
	/// <param name="cellinfo"></param>
	/// <param name="isSetted"></param>
	void GetCellNumberPossibilities(Cell_Info cellinfo, bool isSetted)
    {
		/// Get Highlighted numbers
		List<int> _usedNums = new List<int>();

		/// Highlight Row
		foreach (InteractiveCell _cell in GetRow(cellinfo.Row))
			if (_cell.SettedValue > 0)
				_usedNums.Add(_cell.SettedValue);

		/// Highlight Column
		foreach (InteractiveCell _cell in GetColumn(cellinfo.Column))
			if (_cell.SettedValue > 0)
				_usedNums.Add(_cell.SettedValue);

		/// Highlight Block
		foreach (InteractiveCell _cell in GetBlock(cellinfo.Block))
			if (_cell.SettedValue > 0)
				_usedNums.Add(_cell.SettedValue);

		/// Get the unused nums sorted (avoiding List.Sort and LinQ methods like .distinct)
		int[] _unusedNums = GetUnusedNumsSorted(_usedNums.ToArray());

		GameEvents.current.OnSelectedCellPossibilites(_unusedNums, isSetted);

    }

	/// <summary>
	/// Returns an int[] with the unused numbers sorted
	/// </summary>
	int[] GetUnusedNumsSorted(int[] arr)
    {
		/// Sort
		Array.Sort(arr);

		/// Get Unused vals
		List<int> _unused = new List<int>();
		int _value = 1;
		for (int i = 0; i < arr.Length; i++)
        {
			/// Get values velow each arr component, and add them if not contained on _unused
			while(_value < arr[i])
            {
				if (!_unused.Contains(_value))
					_unused.Add(_value);
				++_value;
            }
			if (_value == arr[i])
				++_value;
        }
		/// Get rest of possibilities above arr max value
		while (_value <= 9)
			_unused.Add(_value++);

		return _unused.ToArray();
    }
    #endregion



    #region UI Methods
	
	/// Triggered when a Number Button is Pressed
	void OnNumberButtonPressed(int num, bool var)
    {
		foreach (InteractiveCell _cell in cells)
		{
			if (_cell.IsSelected)
			{
				_cell.SetCellNumber(num);
				///Update Buttons
				GetCellNumberPossibilities(_cell.CellInfo, var);
				break;
			}
		}

		CheckForGameCompleted();
    }

	/// Triggered when the game is completed
	void OnGameCompleted(bool isGameCompleted)
    {
		endPanel.SetActive(isGameCompleted);
    }


    #endregion


}