using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private GameObject StartNewTurnUI;

	[SerializeField] private GameObject ChooseActionUI;

	[SerializeField] private GameObject PlaySelectionUI;

	[SerializeField] private GameObject AttackSelectionUI;
	[SerializeField] private GameObject DefendSelectionUI;

	[SerializeField] private GameObject LinkSelectionUI;
	
	[SerializeField] private GameObject GameOverUIP1;
	[SerializeField] private GameObject GameOverUIP2;

	[SerializeField] private GameObject PV1Panel;
	[SerializeField] private GameObject PV2Panel;
	[SerializeField] private GameObject[] P1PV;
	[SerializeField] private GameObject[] P2PV;
	[SerializeField] private GameObject P1Turn;
	[SerializeField] private GameObject P2Turn;

	[SerializeField] private GameObject ChangeToP1;
	[SerializeField] private GameObject ChangeToP2;

	[SerializeField] private GameObject[] Buttons;
	
	[SerializeField] private GameObject CardSelectionUI;
	[SerializeField] private GameObject CardSelectionSelect;
	[SerializeField] private GameObject CardSelectionPass;

	// Unity Functions

	protected void Awake()
	{
		if (StartNewTurnUI == null ||
		    ChooseActionUI == null ||
		    PlaySelectionUI == null ||
		    AttackSelectionUI == null ||
		    LinkSelectionUI == null ||
		    DefendSelectionUI == null ||
		    GameOverUIP1 == null ||
		    GameOverUIP2 == null ||
		    //PV1Panel == null ||
		    //PV2Panel  == null ||
		    //P1PV == null ||
		    //P2PV == null ||
		    P1Turn == null ||
		    P2Turn == null ||
		    ChangeToP1 == null ||
		    ChangeToP2 == null
		    )
		{
			Debug.LogError("UIManager: One or more UI references are missing!", this);
		}

		//PV1Panel = P1PV[2];
		//PV1Panel.SetActive(true);
		//PV2Panel = P2PV[2];
		//PV2Panel.SetActive(true);
		P1Turn.SetActive(true);
		SetTurn(PlayerId.P1);

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
		GameOverUIP1.SetActive(false);
		GameOverUIP2.SetActive(false);
		ChangeToP1.SetActive(false);
		ChangeToP2.SetActive(false);
	}

	public void SetPV(int p1, int p2)
	{
		PV1Panel = P1PV[p1 - 1];
		PV2Panel = P2PV[p2 - 1];
	}

	private void SetTurn(PlayerId player)
	{
		if (player == PlayerId.P1)
		{
			P1Turn.SetActive(true);
			P2Turn.SetActive(false);
			foreach (GameObject button in Buttons)
			{
				var skin =  button.GetComponent<ButtonSkin>();
				Debug.Log("button changé");
				
				skin.P1Turn();
			}
		}
		else
		{
			P1Turn.SetActive(false);
			P2Turn.SetActive(true);
			foreach (GameObject button in Buttons)
			{
				var skin = button.GetComponent<ButtonSkin>();
				skin.P2Turn();
			}
		}
	}

	public void ChangingTo(PlayerId player)
	{
		StartCoroutine(Changement(player));
		SetTurn(player);
	}

	public void ShowGameOver(PlayerId winner)
	{
		HideEverything();
		PV1Panel.SetActive(false);
		PV2Panel.SetActive(false);
		P1Turn.SetActive(false);
		P2Turn.SetActive(false);
		if (winner == PlayerId.P1)
		{
			GameOverUIP1.SetActive(true);
		}
		else
		{
			GameOverUIP2.SetActive(true);
		}
	}

	private IEnumerator Changement(PlayerId player)
	{
		if (player == PlayerId.P1)
		{
			ChangeToP1.SetActive(true);
			yield return new WaitForSeconds(2f);
			ChangeToP1.SetActive(false);
		}
		else
		{
			ChangeToP2.SetActive(true);
			yield return new WaitForSeconds(2f);
			ChangeToP2.SetActive(false);
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
	
	public void ShowCardSelectionUI(bool value = true, bool canPass = true)
	{
		CardSelectionUI.SetActive(value);
		CardSelectionSelect.SetActive(value);

		if (canPass && value)
		{
			CardSelectionPass.SetActive(true);
		}
		else
		{
			CardSelectionPass.SetActive(false);
		}
	}

}
