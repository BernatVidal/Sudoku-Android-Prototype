using UnityEngine;

public struct Cell_Info
{
	#region Fields
	int cell_row;
	int cell_column;
	int block;
    #endregion

    #region Properties
    public int Row => cell_row;
    public int Column => cell_column;
    public int Block => block;
    #endregion

    #region Public Methods
    
    /// Cell Info Constructor
    public Cell_Info (int row_id, int column_id, int block_id)
    {
        cell_row = row_id;
        cell_column = column_id;
        block = block_id;        
    }
    #endregion

}