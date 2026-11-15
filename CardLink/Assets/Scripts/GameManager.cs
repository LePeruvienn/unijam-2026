using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private InputScript inputScript;
	[SerializeField] private UIManager uiManager;
	[SerializeField] private HeadMovement headMovement;

	[Header("Board Terrains")]
	[SerializeField] private BoardTerrain p1BoardTerrain;
	[SerializeField] private BoardTerrain p2BoardTerrain;

	[Header("Deck")]
	[SerializeField] private DeckData deckData;

	// The ID of the player who is playing this turn
	private PlayerId _currentPlayer = PlayerId.P1;

	// Pioches
	private Deck p1Deck = null;
	private Deck p2Deck = null;

	// When resolving duels
	private Card _attackerCard = null;
	private Card _defenderCard = null;

	// Unity Functions

	protected void Awake()
	{
		p1Deck = new Deck(deckData);
		p2Deck = new Deck(deckData);
	}

	protected void Start()
	{
		if (inputScript == null)
			Debug.LogError("InputScript Reference is null.");
		if (uiManager == null)
			Debug.LogError("UIManager Reference is null.");
		if(headMovement == null)
			Debug.LogError("HeadMovement Reference is null");
		if(p1BoardTerrain == null)
			Debug.LogError("p1BoardTerrain Reference is null");
		if(p2BoardTerrain == null)
			Debug.LogError("p2BoardTerrain Reference is null");

		// Init We need to start turn before
		InitGame();
	}

	private void InitGame()
	{
		DisableAllBoardInteractions();

		// UI
		uiManager.HideEverything();
		uiManager.ShowStartNewTurnUI();

		// Disable Hand Visibility 
		ShowHand(false);

		Debug.Log("GAME INIT");
	}

	private PlayerId GetNextPlayer()
	{
		return (_currentPlayer == PlayerId.P1) ?
			PlayerId.P2 : PlayerId.P1;
	}

	private void ShowHand(bool value = true)
	{
		inputScript.persoPlay = value;
	}

	private void SetCameraTo(PlayerId player)
	{
		if (player == PlayerId.P1)
		{
			headMovement.ChangeHeadPos(false);

			// Rotate Cards in the right way
			p1BoardTerrain.SetRotate(false);
			p2BoardTerrain.SetRotate(true);
		}
		else
		{
			headMovement.ChangeHeadPos(true);

			// Rotate Cards in the right way
			p1BoardTerrain.SetRotate(true);
			p2BoardTerrain.SetRotate(false);
		}
	}

	private BoardTerrain GetPlayerBoardTerrain(PlayerId player)
	{
		if (player == PlayerId.P1)
			return p1BoardTerrain;
		else
			return p2BoardTerrain;
	}

	private void DisableAllBoardInteractions()
	{
		p1BoardTerrain.SetIsInteractible(false);
		p2BoardTerrain.SetIsInteractible(false);
	}

	private void EnableBoardInteraction(PlayerId player)
	{
		DisableAllBoardInteractions();

		BoardTerrain terrain = GetPlayerBoardTerrain(player);
		terrain.SetIsInteractible(true);
	}

	private Deck GetPlayerDeck(PlayerId player)
	{
		if (player == PlayerId.P1)
			return p1Deck;
		else
			return p2Deck;
	}

	private void DrawCardToHand(Deck deck)
	{
		while (inputScript.GetCardCount() < 3)
		{
			Card card = deck.DrawCard();

			if (card == null)
			{
				return;
			}

			inputScript.AddCard(card);
		}
	}

	// Public mehods

	public void StartNewTurn()
	{
		DisableAllBoardInteractions();

		// Reset cards from attack / defend status
		p1BoardTerrain.SetAllCardsToIdle();
		p2BoardTerrain.SetAllCardsToIdle();

		_currentPlayer = GetNextPlayer();

		SetCameraTo(_currentPlayer);

		Debug.Log($"START {_currentPlayer} TURN");

		// UI Show
		uiManager.HideEverything();
		uiManager.ShowChooseActionUI();

		// Deck
		Deck deck = GetPlayerDeck(_currentPlayer);
	}

	public void StartPlayCard()
	{
		DisableAllBoardInteractions();
		
		Deck deck = GetPlayerDeck(_currentPlayer);

		DrawCardToHand(deck);

		if(inputScript.GetCardCount() <= 0)
		{
			Debug.LogWarning("Cannot Play a Card no card in hand !!");
			return;
		}

		ShowHand(true);

		// UI
		uiManager.HideEverything();
		uiManager.ShowPlaySelectionUI();
	}

	public void EndPlayCardSelection()
	{
		int cardIndex = inputScript.GetSelectedCard();

		if (cardIndex == -1)
		{
			return;
		}

		ShowHand(false);
		
		Vector3 cardPosition = inputScript.GetCardPosition(cardIndex);

		// Remove card from hand
		inputScript.RemoveCard(cardIndex);

		// Adding card to terrain
		BoardTerrain terrain = GetPlayerBoardTerrain(_currentPlayer);

		terrain.AddNewCard(null, cardPosition);

		EndTurn();
	}

	public void StartAttack()
	{
		DisableAllBoardInteractions();

		BoardTerrain terrain = GetPlayerBoardTerrain(_currentPlayer);

		if (terrain.HaveCards())
		{
			EnableBoardInteraction(_currentPlayer);

			// UI
			uiManager.HideEverything();
			uiManager.ShowAttackSelectionUI();
		}
		else
		{
			Debug.LogWarning("Cannot Choose Play Card. No Cards in terrain");
		}
	}

	public void EndAttackSelection()
	{
		BoardTerrain terrain = GetPlayerBoardTerrain(_currentPlayer);

		bool success = terrain.ApplySelection();

		if (success)
		{
			DisableAllBoardInteractions();
			StartDefend();
		}
	}

	public void StartDefend()
	{
		DisableAllBoardInteractions();

		PlayerId oppenent = GetNextPlayer();

		BoardTerrain terrain = GetPlayerBoardTerrain(oppenent);

		if (terrain.HaveCards())
		{
			EnableBoardInteraction(oppenent);

			// UI
			uiManager.HideEverything();
			uiManager.ShowDefendSelectionUI();

			SetCameraTo(oppenent);
		}
		else
		{
			// TODO: Apply Damage
			EndTurn();
		}
	}

	public void EndDefendSelection()
	{
		PlayerId oppenent = GetNextPlayer();

		BoardTerrain terrain = GetPlayerBoardTerrain(oppenent);

		bool success = terrain.ApplySelection();

		if (success)
		{
			DisableAllBoardInteractions();
			EndTurn();
		}
	}

	public void EndTurn()
	{
		ShowHand(false);

		DisableAllBoardInteractions();

		// Afficher la résolution ...
		// Animation de bagarre ...

		// UI
		uiManager.HideEverything();
		uiManager.ShowStartNewTurnUI();
	}
}
