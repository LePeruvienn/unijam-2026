using System.Collections.Generic;
using UnityEngine;

public class BoardTerrain : MonoBehaviour
{
	[Header("Played Cards")]
	[SerializeField] private List<PlayedCard> playedCards = new List<PlayedCard>();
	[SerializeField] private Transform attackSlot;
	
	[Header("Size")]
	[SerializeField] private float cardWidth = 2f;

	[Header("Interaction")]
	[SerializeField] private BoardInteraction boardInteraction;

	[Header("Prefab")]
	[SerializeField] private PlayedCard playedCardPrefab;

	private bool _rotateCards = false;
	private bool _isInteractible = true;

	// Unity Functions

	protected void Update()
	{
		if (_isInteractible)
		{
			HandleInteraction();
		}
		else
		{
			HandleIdle();
		}
	}

	private void HandleIdle()
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard card = playedCards[i];

			if (card.GetState() == PlayedCard.State.Attacking)
				continue;

			card.SetState(PlayedCard.State.Idle);

			float x = GetCardPosition(i);

			Vector3 position =  new Vector3(x, 0f, 0f);

			card.SetTargetPosition(position);
		}
	}

	private void HandleInteraction()
	{
		PlayedCard hoveredCard = boardInteraction.GetHoveredPlayedCard();
		PlayedCard clickedCard = boardInteraction.GetClickedPlayedCard();

		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard card = playedCards[i];
			
			if (card.GetState() == PlayedCard.State.Attacking)
			{
				continue;
			}

			float x = GetCardPosition(i);

			Vector3 position =  new Vector3(x, 0f, 0f);

			card.SetTargetPosition(position);

			if (card.IsLinkSelected())
				continue;
			
			if(clickedCard == card)
			{
				card.SetState(PlayedCard.State.Selected);
			}
			else if (hoveredCard == card)
			{
				card.SetState(PlayedCard.State.Hovered);
			}
			else
			{
				card.SetState(PlayedCard.State.Idle);
			}
		}
	}
	
	// Implementation

	private float GetCardPosition(int index)
	{
		if (index < 0)
		{
			Debug.LogError("Bas va te faire foutre en fait");
			return 0f;
		}

		float totalWidth = (playedCards.Count - 1) * cardWidth;
		float offset = (index * cardWidth) - (totalWidth / 2);

		return offset;
	}

	// Public Functions

	public PlayedCard AddNewCard(Card card, Vector3 spawnPos)
	{
		if(card == null)
		{
			Debug.LogWarning("Added a card with a null model");
		}

		PlayedCard played = Instantiate(playedCardPrefab, spawnPos, Quaternion.identity, transform);

		played.SetModel(card);
		played.SetIsRotated(_rotateCards);

		playedCards.Add(played);

		return played;
	}

	public bool DeleteFromModel(Card card)
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];

			if (played.GetModel() == card)
			{
				playedCards.Remove(played);
				return true;
			}
		}

		Debug.LogError("Failed to delete card from board terrain");
		return false;
	}

	public bool HaveCard(PlayedCard target)
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];

			if (played == target)
			{
				return true;
			}
		}

		return false;
	}

	public bool DeletePlayedCard(PlayedCard target)
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];

			if (played == target)
			{
				playedCards.Remove(played);
				return true;
			}
		}

		Debug.LogError("Failed to delete card from board terrain");
		return false;
	}

	public bool GetRotateCards()
	{
		return _rotateCards;
	}

	public void SetRotate(bool value)
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];

			played.SetIsRotated(value);
		}

		_rotateCards = value;
	}

	public void SetIsInteractible(bool value)
	{
		_isInteractible = value;
	}

	public PlayedCard GetSelectedCard()
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];

			if (played.GetState() == PlayedCard.State.Selected)
			{
				return played;
			}
		}

		return null;
	}

	public PlayedCard GetAttackingCard()
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];

			if (played.GetState() == PlayedCard.State.Attacking)
			{
				return played;
			}
		}

		return null;
	}

	public bool ApplySelection()
	{
		PlayedCard selected = GetSelectedCard();

		if (selected != null)
		{
			Vector3 targetPosition = attackSlot.localPosition;

			selected.SetTargetPosition(targetPosition);
			selected.SetState(PlayedCard.State.Attacking);
			return true;
		}
		else
		{
			Debug.LogWarning("Couldnt apply selection");
			return false;
		}
	}

	public void SetAllCardsToIdle()
	{
		for (int i = 0; i < playedCards.Count; ++i)
		{
			PlayedCard played = playedCards[i];
			played.SetState(PlayedCard.State.Idle);
		}

		boardInteraction.ResetHoveredClicked();
	}

	public bool KillCard(PlayedCard target)
	{
 		bool sucess = DeletePlayedCard(target);

		if (sucess)
		{
			target.Kill();
			return true;
		}
		else
		{
			Debug.LogError("Failed to kill card");
			return false;
		}
	}

	public bool HaveCards()
	{
		return (playedCards.Count > 0);
	}

	public int GetCardCount()
	{
		return playedCards.Count;
	}
}
