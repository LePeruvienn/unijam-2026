using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private InputScript inputScript;
	[SerializeField] private UIManager uiManager;
	[SerializeField] private HeadMovement headMovement;
	[SerializeField] private CardEffectManager cardEffectManager;

	[Header("Board Terrains")]
	[SerializeField] private BoardTerrain p1BoardTerrain;
	[SerializeField] private BoardTerrain p2BoardTerrain;

	[Header("Players Data")]
	[SerializeField] private DeckData deckData;
	[SerializeField] private int startPV = 3;
	[SerializeField] private int maxPV = 3;

	[SerializeField] HealthBar healthBar1;
	[SerializeField] HealthBar healthBar2;

	// The ID of the player who is playing this turn
	private PlayerId _currentPlayer = PlayerId.P1;

	// Pioches
	private Deck _p1Deck = null;
	private Deck _p2Deck = null;

	// Mains (Gerees ici dans le GameManager)
	private List<Card> _p1Hand = new List<Card>();
	private List<Card> _p2Hand = new List<Card>();

	// PVs
	private int _p1PV = 0;
	private int _p2PV = 0;

	// Unity Functions

	protected void Awake()
	{
		_p1Deck = new Deck(deckData);
		_p2Deck = new Deck(deckData);

		_p1PV = startPV;
		_p2PV = startPV;
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
			return _p1Deck;
		else
			return _p2Deck;
	}

	// Recuperer la main du joueur actuel
	private List<Card> GetCurrentPlayerHand()
	{
		return (_currentPlayer == PlayerId.P1) ? _p1Hand : _p2Hand;
	}

	private void DrawCardToHand(Deck deck)
	{
		List<Card> currentHand = GetCurrentPlayerHand();

		while (currentHand.Count < 3)
		{
			Card card = deck.DrawCard();

			if (card == null)
			{
				return;
			}

			currentHand.Add(card);
		}
	}

	private void PlayerTakeDamage(PlayerId player, int amount = 1)
	{
		Debug.Log($"Player {player} take {amount} damage!");

		if(player == PlayerId.P1)
			_p1PV -= amount;
		else
			_p2PV -= amount;

		Debug.Log($"P1 Health : {_p1PV} || P2 Health : {_p2PV}");

		if(_p1PV <= 0)
		{
			Debug.Log("P1 DEAD !!");
			uiManager.ShowGameOver(PlayerId.P2);
			_p1PV = 0;
		}
		else if(_p2PV <= 0)
		{
			Debug.Log("P2 DEAD !!");
			uiManager.ShowGameOver(PlayerId.P1);
			_p2PV = 0;
		}
		healthBar1.currentHealth = _p1PV;
		healthBar2.currentHealth = _p2PV;

	}

	private void PlayerHeal(PlayerId player, int amount = 1)
	{
		if(player == PlayerId.P1)
			_p1PV += amount;
		else
			_p2PV += amount;

		if(_p1PV > maxPV) _p1PV = maxPV;
		if(_p2PV > maxPV) _p2PV = maxPV;
	}

	// Probleme si egalite ?
	private void KillWithLinks(PlayedCard origin)
	{
		HashSet<PlayedCard> toKill = new HashSet<PlayedCard>();
		Stack<PlayedCard> stack = new Stack<PlayedCard>();
		stack.Push(origin);

		while (stack.Count > 0)
		{
			PlayedCard card = stack.Pop();

			if (card == null || !toKill.Add(card))
				continue;

			List<PlayedCard> links = card.GetLinkedCards();

			for (int i = 0; i < links.Count; ++i)
			{
				stack.Push(links[i]);
			}
		}

		foreach (PlayedCard card in toKill)
		{
			if (p1BoardTerrain.HaveCard(card))
				p1BoardTerrain.KillCard(card);
			else if (p2BoardTerrain.HaveCard(card))
				p2BoardTerrain.KillCard(card);
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
 	
		uiManager.ChangingTo(_currentPlayer);

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

		// Transmettre la main complete du joueur actuel a la vue (InputScript)
		inputScript.SetCards(GetCurrentPlayerHand());

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

		uiManager.HideEverything();

		ShowHand(false);
		
		Vector3 cardPosition = inputScript.GetCardPosition(cardIndex);
		Card cardModel = inputScript.GetCardModel(cardIndex);

		// Remove card from GameManager hand data
		List<Card> currentHand = GetCurrentPlayerHand();
		if (cardIndex >= 0 && cardIndex < currentHand.Count)
		{
			currentHand.RemoveAt(cardIndex);
		}

		// Remove card from visual InputScript
		inputScript.RemoveCard(cardIndex);

		// Adding card to terrain
		BoardTerrain terrain = GetPlayerBoardTerrain(_currentPlayer);

		PlayedCard playedCard = terrain.AddNewCard(cardModel, cardPosition);

		// Gestion des effet onPlay
		cardEffectManager.HandleOnPlayEffects(playedCard, cardModel, EndTurn);
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

		uiManager.ChangingTo(oppenent);

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
			PlayerTakeDamage(oppenent);
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
			uiManager.HideEverything();
			MakeResolution();
		}
	}

	public void MakeResolution()
	{
		PlayerId attacker = _currentPlayer;
		PlayerId defender = GetNextPlayer();

		BoardTerrain attackerTerrain = GetPlayerBoardTerrain(attacker);
		BoardTerrain defenderTerrain = GetPlayerBoardTerrain(defender);

		PlayedCard attackerPlayedCard = attackerTerrain.GetAttackingCard();
		PlayedCard defenderPlayedCard = defenderTerrain.GetAttackingCard();

		if (attackerPlayedCard == null || defenderPlayedCard == null)
			Debug.LogError("Failed to set make resolution, attacking PlayedCard null");

		Card attackerCard = attackerPlayedCard.GetModel();
		Card defenderCard =  defenderPlayedCard.GetModel();

		if (attackerCard == null || defenderCard == null)
			Debug.LogError("Failed to set make resolution, attacking Card model null");

		Debug.Log($"Fight Start : {attackerCard.nom} vs {defenderCard.nom}");

		headMovement.Shake();

		// Probleme si egalite + LINK?
		// => il m'as pas l'air d'avoir de bug
		if (attackerCard.force >= defenderCard.force)
		{
			KillWithLinks(defenderPlayedCard);
			Debug.Log($"DIED {defenderCard.nom} !");
		}
		if(attackerCard.force <= defenderCard.force)
		{
			KillWithLinks(attackerPlayedCard);
			Debug.Log($"DIED {attackerCard.nom} !");
		}

		EndTurn();
	}

	public void EndTurn()
	{
		ShowHand(false);

		DisableAllBoardInteractions();

		// Afficher la resolution ...
		// Animation de bagarre ...

		// UI
		uiManager.HideEverything();
		uiManager.ShowStartNewTurnUI();
	}

	// Link function
	void StartLink()
	{
		
	}

	void EndLink()
	{

	}
}
