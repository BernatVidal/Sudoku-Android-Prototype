using UnityEngine;
using UnityEngine.UI;

public class InteractiveCell : MonoBehaviour
{
	#region Fields

	[SerializeField] Color highlightColor;
	[SerializeField] Color selectedColor;
	[SerializeField] Color blockedColor;

	Button btn;
	Image img;
	Text txt;
	Animation anim;
	Cell_Info cellinfo;

	int settedValue;
	bool isBlocked;
	bool isSelected;
	#endregion

	#region Properties
	public Cell_Info CellInfo => cellinfo;
	public int SettedValue => settedValue;
	public bool IsSelected => isSelected;
	#endregion
	
	#region Unity Methods

	void Awake()
	{
		btn = GetComponentInChildren<Button>();
		img = GetComponentInChildren<Image>();
		txt = GetComponentInChildren<Text>();
		anim = GetComponentInChildren<Animation>();
	}
	void Start()
	{
		btn.onClick.AddListener(OnBtnClicked);
	}


    #endregion

    #region Public Methods
	/// <summary>
	/// Sets Cell info struct according to info passed
	/// </summary>
	public void SetCellInfo(int row, int column, int block)
    {
		cellinfo = new Cell_Info(row, column, block);
	}

	/// <summary>
	/// Sets its value if isBlocked, and change its properties according to this
	/// </summary>
	public void SetCellValues(int value, bool isBlocked)
    {
        this.isBlocked = isBlocked;
		btn.interactable = !isBlocked;
		if (isBlocked)
        {
			settedValue = value;

			txt.fontStyle = FontStyle.Bold;
			img.color = blockedColor;
        }

		UpdateText();
    }

	/// <summary>
	/// Sets the number cell will have (according to user input)
	/// </summary>
	public void SetCellNumber(int i)
    {
		settedValue = i;
		UpdateText();
    }

	/// <summary>
	/// If it isn't blocked, it will highlight and play an animation, or unhighlight according to bool var
	/// </summary>
    public void Highlight(bool isHighlight)
    {
        if (isHighlight)
        {
            img.color = highlightColor;
			if (!isBlocked)
				anim.Play();
        }
        else
        {
            img.color = Color.white;
			if (isBlocked)
				img.color = blockedColor;
        }
    }

	/// <summary>
	/// Selects the cell according to user input
	/// </summary>
    public void Select(bool isSelected)
    {
		this.isSelected = isSelected;
		if (this.isSelected)
            img.color = selectedColor;
    }
	#endregion

	#region Private Methods

	void OnBtnClicked()
    {
		GameEvents.current.OnCellButtonPressed(cellinfo, settedValue > 0);
		Select(true);
        //Debug.Log($"Row: {cellinfo.Row} | Col: {cellinfo.Column} | Block: {cellinfo.Block}");
    }


    void UpdateText()
    {
		if (settedValue > 0)
			txt.text = settedValue.ToString();
		else
			txt.text = " ";
	}

	#endregion

}