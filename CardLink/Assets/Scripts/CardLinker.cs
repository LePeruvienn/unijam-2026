using System;
using System.Collections.Generic;
using UnityEngine;

public class CardLinker : MonoBehaviour
{
	[Header("Terrains")]
	[SerializeField] private BoardTerrain p1BoardTerrain;
	[SerializeField] private BoardTerrain p2BoardTerrain;

	[Header("UI")]
	[SerializeField] private UIManager uiManager;

	private bool _isSelecting = false;
	private int _targetsNeeded = 0;

	private PlayedCard _sourceCard = null;

	[Header("Debug")]
	[SerializeField] private List<PlayedCard> _selectedCards = new List<PlayedCard>();

	private Action _onCompleteCallback = null;

	private void Start()
	{
		if (uiManager == null)
		{
			Debug.LogError("UI Manager is NULL");
		}

		uiManager.ShowLinkSelectionUI(false);
	}

	private void Update()
	{
		if (_isSelecting == false) return;

		CheckTerrainSelection(p1BoardTerrain);
		CheckTerrainSelection(p2BoardTerrain);
	}

	private void CheckTerrainSelection(BoardTerrain terrain)
	{
		if (terrain == null || !_isSelecting) return;

		PlayedCard selected = terrain.GetSelectedCard();
		
		if (selected != null && selected != _sourceCard && !_selectedCards.Contains(selected))
		{
			_selectedCards.Add(selected);
			
			selected.SetState(PlayedCard.State.Idle);
			selected.SetLinkSelected(true);


			if (_selectedCards.Count >= _targetsNeeded)
			{
				_isSelecting = false;
				p1BoardTerrain.SetIsInteractible(false);
				p2BoardTerrain.SetIsInteractible(false);

				uiManager.ShowLinkSelectionUI(true);
			}
		}
	}

	// UI

	public void ConfirmLink()
	{
		if (_sourceCard != null && _selectedCards.Count == 1)
		{
			_sourceCard.AddLink(_selectedCards[0]);
		}
		else if (_selectedCards.Count == 2)
		{
			_selectedCards[0].AddLink(_selectedCards[1]);
		}

		EndLinkProcess();
	}

	public void CancelLink()
	{
		uiManager.ShowLinkSelectionUI(false);

		for (int i = 0; i < _selectedCards.Count; ++i)
		{
			_selectedCards[i].SetLinkSelected(false);
		}

		_selectedCards.Clear();

		p1BoardTerrain.SetAllCardsToIdle();
		p2BoardTerrain.SetAllCardsToIdle();

		if (_sourceCard != null)
		{
			StartSingleTargetLink(_sourceCard, true, true, _onCompleteCallback);
		}
		else
		{
			StartDoubleTargetLink(true, true, _onCompleteCallback);
		}
	}

	private void EndLinkProcess()
	{
		if (_sourceCard != null)
			_sourceCard.SetLinkSelected(false);

		for (int i = 0; i < _selectedCards.Count; ++i)
		{
			_selectedCards[i].SetLinkSelected(false);
		}

		uiManager.ShowLinkSelectionUI(false);

		p1BoardTerrain.SetAllCardsToIdle();
		p2BoardTerrain.SetAllCardsToIdle();

		Action callback = _onCompleteCallback;
		_onCompleteCallback = null;
		callback?.Invoke();
	}

	// GameManager

	public void StartSingleTargetLink(PlayedCard sourceCard, bool allowP1Terrain, bool allowP2Terrain, Action onComplete)
	{
		// Check si il y a assez de cartes
		int available = (allowP1Terrain ? p1BoardTerrain.GetCardCount() : 0)
		                + (allowP2Terrain ? p2BoardTerrain.GetCardCount() : 0) - 1;

		if (available < 1)
		{
			Debug.LogWarning($"Impossible de Link pas assez de cartes sur le plateau : ${available}.");
			onComplete?.Invoke(); return;
		}

		uiManager.ShowLinkSelectionUI(false);

		sourceCard.SetLinkSelected(true);

		_sourceCard = sourceCard;

		_targetsNeeded = 1;
		SetupSelection(allowP1Terrain, allowP2Terrain, onComplete);
	}

	public void StartDoubleTargetLink(bool allowP1Terrain, bool allowP2Terrain, Action onComplete)
	{
		int available = (allowP1Terrain ? p1BoardTerrain.GetCardCount() : 0)
		                + (allowP2Terrain ? p2BoardTerrain.GetCardCount() : 0);

		if (available < 2)
		{
			Debug.LogWarning($"Impossible de Link pas assez de cartes sur le plateau : ${available}.");
			onComplete?.Invoke(); return;
		}

		uiManager.ShowLinkSelectionUI(false);

		_sourceCard = null;
		_targetsNeeded = 2;
		SetupSelection(allowP1Terrain, allowP2Terrain, onComplete);
	}

	private void SetupSelection(bool allowP1Terrain, bool allowP2Terrain, Action onComplete)
	{
		_selectedCards.Clear();
		_onCompleteCallback = onComplete;
		_isSelecting = true;

		p1BoardTerrain.SetAllCardsToIdle();
		p2BoardTerrain.SetAllCardsToIdle();

		p1BoardTerrain.SetIsInteractible(allowP1Terrain);
		p2BoardTerrain.SetIsInteractible(allowP2Terrain);
	}
}
