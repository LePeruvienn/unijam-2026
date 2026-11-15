using UnityEngine;

public class UIManager : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private GameObject StartNewTurnUI;

	[SerializeField] private GameObject ChooseActionUI;

	[SerializeField] private GameObject PlaySelectionUI;

	[SerializeField] private GameObject AttackSelectionUI;
	[SerializeField] private GameObject DefendSelectionUI;

	[SerializeField] private GameObject LinkSelectionUI;

	[SerializeField] private GameObject CardSelectionUI;

	// Unity Functions

	protected void Awake()
	{
		if (StartNewTurnUI   == null ||
		    ChooseActionUI   == null ||
		    PlaySelectionUI   == null ||
		    AttackSelectionUI == null ||
		    LinkSelectionUI == null ||
		    CardSelectionUI == null ||
		    DefendSelectionUI == null)
		{
			Debug.LogError("UIManager: One or more UI references are missing!", this);
		}

		HideEverything();
	}

	// Public Methods

	public void HideEverything()
	{
		StartNewTurnUI.SetActive(false);
		ChooseActionUI.SetActive(false);
		PlaySelectionUI.SetActive(false);
		AttackSelectionUI.SetActive(false);
		DefendSelectionUI.SetActive(false);
		LinkSelectionUI.SetActive(false);
	}

	public void ShowStartNewTurnUI(bool value = true)
	{
		StartNewTurnUI.SetActive(value);
	}

	public void ShowChooseActionUI(bool value = true)
	{
		ChooseActionUI.SetActive(value);
	}

	public void ShowPlaySelectionUI(bool value = true)
	{
		PlaySelectionUI.SetActive(value);
	}

	public void ShowAttackSelectionUI(bool value = true)
	{
		AttackSelectionUI.SetActive(value);
	}

	public void ShowDefendSelectionUI(bool value = true)
	{
		DefendSelectionUI.SetActive(value);
	}

	public void ShowLinkSelectionUI(bool value = true)
	{
		LinkSelectionUI.SetActive(value);
	}

	public void ShowCardSelectionUI(bool value = true)
	{
		CardSelectionUI.SetActive(value);
	}
}
