using System.Collections.Generic;
using UnityEngine;

public class Deck
{
	private DeckData _source;
	private List<Card> _cards = new List<Card>();

	public Deck(DeckData source)
	{
		_source = source;
		Reset();
	}

	public void Reset()
	{
		_cards.Clear();

		if (_source == null)
		{
			Debug.LogError("Deck source is null");
			return;
		}

		for (int i = 0; i < _source.cards.Count; ++i)
		{
			CardEntry entry = _source.cards[i];

			if (entry == null || entry.card == null || entry.quantity <= 0)
			{
				continue;
			}

			for (int j = 0; j < entry.quantity; j++)
			{
				_cards.Add(entry.card);
			}
		}
	}

	public Card DrawCard()
	{
		if (_cards.Count == 0)
		{
			Debug.LogWarning("Deck is empty.");
			return null;
		}

		int index = Random.Range(0, _cards.Count);
		Card drawnCard = _cards[index];

		_cards.RemoveAt(index);

		return drawnCard;
	}

	public int GetRemainingCards()
	{
		return _cards.Count;
	}
}

