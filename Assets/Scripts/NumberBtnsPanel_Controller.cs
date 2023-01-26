using UnityEngine;
using UnityEngine.UI;

public class NumberBtnsPanel_Controller : MonoBehaviour
{
	#region Fields

	[SerializeField] Transform numbersParent;
	Button[] btns;
	[SerializeField] Button BackBtn;
	[SerializeField] Button ValidateBtn;

	#endregion

	
	#region Unity Methods

	void Awake()
	{
		btns = numbersParent.GetComponentsInChildren<Button>(true);
	}
	void Start()
	{
		/// Set Listeners to buttons
		foreach (Button _btn in btns)
			_btn.onClick.AddListener(() => OnNumberBtnClick(_btn));

		GameEvents.current.onSelectedCellPossibilites += ShowNumberBtns;

		ShowNumberBtns(null, false);
	}

	#endregion
	


	#region Public Methods
	
	void OnNumberBtnClick(Button btn)
    {
		int _num = int.TryParse(btn.GetComponentInChildren<Text>().text, out _num) ? _num : 0;
		GameEvents.current.OnNumberButtonPressed(num: _num, isSetted: _num > 0);
    }

	#endregion



	#region Private Methods
	/// <summary>
	/// Shows the desired number buttons and decides wether or not should show RemoveBtn
	/// </summary>
	/// <param name="nums">List of unused nums to show</param>
	/// <param name="isValueSetted">Used to know if Remove Btn is required</param>
	void ShowNumberBtns(int[] nums, bool isValueSetted)
    {
		/// Set parents active/unactive
		numbersParent.gameObject.SetActive(nums != null);

		if (nums != null)
		{
			/// Activate buttons according to nums
			int _count = 0;
			for (int i = 0; i < btns.Length; i++)
            {
				/// If number is same than button, activate it and add 1 to count, else deactivate it
				btns[i].gameObject.SetActive(i == nums[_count]-1);
				if (i == nums[_count] - 1)
				{
					_count = Mathf.Clamp(++_count, 0, nums.Length - 1); // Clamp to array length
				}
			}
			/// Delete Btn active if required
			btns[btns.Length - 1].gameObject.SetActive(isValueSetted);
		}
	}

	#endregion


}