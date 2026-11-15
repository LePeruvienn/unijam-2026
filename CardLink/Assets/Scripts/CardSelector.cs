using UnityEngine;
using System.Collections.Generic;
using System;

public class CardSelector : MonoBehaviour
{
	[Header("Terrains")]
	[SerializeField] private BoardTerrain p1BoardTerrain;
	[SerializeField] private BoardTerrain p2BoardTerrain;

	[Header("UI")]
	[SerializeField] private UIManager uiManager;

	private bool _isSelecting = false;
	private Action<PlayedCard> _currentAction;

	// Helper
	
	private int GetTotalCardAmount()
	{
		return p1BoardTerrain.GetCardCount() + p2BoardTerrain.GetCardCount();
	}

	// Public

	public void StartSelection(Action<PlayedCard> callback, int minCardAmount = 1)
	{
		if(_isSelecting)
		{
			Debug.LogError("Already seleting card!");
			return;
		}

		if (GetTotalCardAmount() < minCardAmount)
		{
			Debug.LogWarning("Not enought card to apply selector effect");
			callback.Invoke(null);
			return;
		}

		uiManager.HideEverything();
		uiManager.ShowCardSelectionUI();

		p1BoardTerrain.SetIsInteractible(true);
		p2BoardTerrain.SetIsInteractible(true);

		_currentAction = callback;

		_isSelecting = true;
	}

	public void Validate()
	{
		if(_isSelecting == false)
		{
			Debug.LogError("Card Selector, not in sleecting pahse");
			return;
		}

		PlayedCard select1 = p1BoardTerrain.GetSelectedCard();
		PlayedCard select2 = p2BoardTerrain.GetSelectedCard();

		PlayedCard selectedCard = (select1 == null) ? select2 : select1;

		if (selectedCard == null)
		{
			return;
		}

		_isSelecting = false;

		uiManager.ShowCardSelectionUI(false);

		_currentAction.Invoke(selectedCard);
	}
}
