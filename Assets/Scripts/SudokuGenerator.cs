using System;
using UnityEngine;

public static class SudokuGenerator
{
	#region Public Methods

	public static int[,] GenerateSudokuGrid(int GRID_SIZE, int showAmount)
    {
		/// Create grid and random seed
		int[,] _grid = new int[GRID_SIZE,GRID_SIZE];
		System.Random _rnd = new System.Random();

		/// Generate the main seed number array {1,2,3,...}
		int[] _seedArray = new int[GRID_SIZE];
		for(int i = 0; i < GRID_SIZE; i++)
			_seedArray[i] = i + 1;
        /// Randomize it
		Shuffle(_seedArray, _rnd);

		/// Set the basic working grid
		_grid = GenerateBasicGrid(_seedArray);

		/// Shuffle the array
        /// Shuffle Rows and Columns
		for (int i = 0; i < 3; i++)
        {
			Shuffle2DArray(_grid, 3, true, i * (GRID_SIZE / 3), GRID_SIZE / 3, _rnd);  // Rows
			Shuffle2DArray(_grid, 3, false, i * (GRID_SIZE / 3), GRID_SIZE / 3, _rnd); // Columns
        }
		/// Shuffle Row Blocks
		Shuffle2DArray(_grid, 3, true, 0, 9, _rnd);
		/// Shuffle Col Blocks
		Shuffle2DArray(_grid, 3, false, 0, 9, _rnd);

		/// Mask
		/// randomly hide some numbers by setting them to 0 in the mask
		while(showAmount < GRID_SIZE*GRID_SIZE)
		{
			int x = _rnd.Next(GRID_SIZE);
			int y = _rnd.Next(GRID_SIZE);
			if (_grid[x, y] != 0)
			{
				_grid[x, y] = 0;
				++showAmount;
			}
		}

		/// Debug
		PrintGrid(_grid);

		return _grid;
    }

	#endregion

	#region Private Methods

	/// <summary>
	/// Shuffles an array (Fisher-Yates shuffle)
	/// </summary>
	static void Shuffle(int[] array, System.Random rnd)
	{
		int _n = array.Length;
		while (_n > 1)
		{
			int k = rnd.Next(_n--);
			int temp = array[_n];
			array[_n] = array[k];
			array[k] = temp;
		}
	}

	/// <summary>
	/// Pass a 2D array (or a portion of original grid) to shuffle its positions according to the setted num of ranges inside and if you want rows or cols
	/// </summary>
	/// <param name="arr">The 2D array</param>
	/// <param name="ranges">How many partitions of the array you want *(should be multiple of arr.Length)  (always 3 in sudoku grid case)</param>
	/// <param name="doRows">Bool to chose wether to shuffle Rows or Cols</param>
	static void Shuffle2DArray(int[,] arr, int ranges, bool doRows, int startIndex, int length, System.Random _rnd)
    {
		/// Generate Positions array to Shuffle the array positions
		int[] _positions = new int[ranges];
		for (int i = 0; i < ranges; i++)
        {
			_positions[i] = i;  // Generate positions array ({0,1,2,..})
        }
		Shuffle(_positions, _rnd);

		/// Set the array with the new shuffled positions :
		/// Deep Copy original array
		int[,] _tempArr = DeepCopy2DArray(arr);
		/// Get Range length
		int _rangeLength = length / ranges;
		int _range = 0;
		int _rangeIterations = 0;
		/// Rows or Cols (doRows || !doRows)
		for(int i = startIndex; i < startIndex + length; i++)
		{
			/// Cols or Rows (doRows || !doRows)
			for (int j = 0; j < arr.GetLength(doRows? 1 : 0); j++)
            {
				if (doRows)
					arr[_positions[_range] * _rangeLength + _rangeIterations + startIndex , j] = _tempArr[i, j];
				else
					arr[j , _positions[_range] * _rangeLength + _rangeIterations + startIndex] = _tempArr[j, i];
			}

            /// Ranges
            if (++_rangeIterations >= _rangeLength)
            {
                _rangeIterations = 0;
                ++_range;
            }
        }	
	}

	/// <summary>
	/// Builds the basic sudoku-ready grid composition:
	/// 
	/// 123-456-789
	/// 456-789-123
	/// 789-123-456
	/// -----------
	/// 234-567-891
	/// ...
	/// 
	/// </summary>
	/// <param name="seedArray"> The array seed used to build the basic grid</param>
	/// <returns></returns>
	static int[,] GenerateBasicGrid(int[] seedArray)
    {
		int[,] _sudokuGrid = new int[seedArray.Length, seedArray.Length];		

		for(int row = 0; row < _sudokuGrid.GetLength(0); row++)
        {
            /// shift if not 1st row
            if (row != 0)
            {
                /// each row +3, and +4 if changing block
                int _offset = row % (seedArray.Length / 3) == 0 ? 1 : 0;
                ShiftLeft(seedArray, 3 + _offset);
            }

            /// Set numbers to the array
            for (int col = 0; col < _sudokuGrid.GetLength(0); col++)
            {
                _sudokuGrid[row, col] = seedArray[col];
            }
        }

        return _sudokuGrid;
    }

	/// <summary>
	/// Used to shift the presented array val times to the left
	/// </summary>
	// Using Array.Copy for a better performance
	static void ShiftLeft(int[] array, int val)
    {
		while(val > 0)
		{
			int _temp = array[0];
			Array.Copy(array, 1, array, 0, array.Length - 1);
			array[array.Length - 1] = _temp;
			--val;
		}
    }

	/// <summary>
	/// Used to deep copy 2D arrays
	/// </summary>
	// Using T so this can be used in any context if setted to Public
	static T[,] DeepCopy2DArray<T>(T[,] arr)
	{
		T[,] _deepCopy = new T[arr.GetLength(0), arr.GetLength(1)];
		for (int i = 0; i < arr.GetLength(0); i++)
		{
			for (int j = 0; j < arr.GetLength(1); j++)
			{
				_deepCopy[i, j] = arr[i, j];
			}
		}
		return _deepCopy;
	}

    #endregion


    #region Debug Methods
    static void PrintGrid(int[,] array)
	{
		string _line = "";
		for (int i = 0; i < array.GetLength(0); i++)
		{
			for (int j = 0; j < array.GetLength(1); j++)
			{
				_line += array[i, j] + " ";
			}
			_line += "\n";
		}
		Debug.Log(_line);
	}

	public static void PrintArray(int[] array)
    {
		string _line = "";
		for (int i = 0; i < array.GetLength(0); i++)
		{
			_line += array[i];
		}
		Debug.Log(_line);
	}

	#endregion


}