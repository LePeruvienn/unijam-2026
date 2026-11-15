using UnityEngine;
using System.Collections.Generic;

public class PlayedCard : MonoBehaviour
{
	public enum State
	{
		Idle,
		Hovered,
		Selected,
		Attacking,
		Linked,
	}

	[Header("Visual")]
	[SerializeField] private CardVisual visual;
	[SerializeField] private ParticleSystem particuleSystem;


	[Header("Card Position Offsets")]
	[SerializeField] private Vector3 hoveredPosition = new Vector3(0f, 0.5f, 0f);
	[SerializeField] private Vector3 idlePosition    = new Vector3(0f,   0f, 0f);
	[SerializeField] private Vector3 selectPosition  = new Vector3(0f, 0f, 1f);

	[Header("Animations")]
	[SerializeField] private float animationSpeed = 2f;

	private bool _isRotated = false;

	private State _state = State.Idle;

	private Vector3 _targetPosition = Vector3.zero;

	private Card _model;

	private bool _isLinkSelected = false;

	// LIAISONNNNS!!
	private List<PlayedCard> _linkedCards = new List<PlayedCard>();
	
	protected void Update()
	{
		UpdateParticles();
		UpdatePosition();
		UpdateRotation();
	}

	
	private void UpdateParticles()
	{
		if (particuleSystem == null) return;

		if (_isLinkSelected && !particuleSystem.isPlaying)
			particuleSystem.Play();
		else if (!_isLinkSelected && particuleSystem.isPlaying)
			particuleSystem.Stop();
	}

	private void UpdateRotation()
	{
		Quaternion targetRotation = Quaternion.Euler(
			0f,
			_isRotated ? 180f : 0f,
			0f
		);

		transform.localRotation = Quaternion.Lerp(
			transform.localRotation,
			targetRotation,
			Time.deltaTime * animationSpeed
		);
	}
	private void UpdatePosition()
	{
		Vector3 position = _targetPosition;

		switch (_state)
		{
			case State.Idle:
				position += idlePosition;
				break;

			case State.Hovered:
				position += hoveredPosition;
				break;

			case State.Selected:
				position += transform.localRotation * selectPosition;
				break;
		}

		transform.localPosition = Vector3.Lerp(transform.localPosition, position,
		                                       Time.deltaTime * animationSpeed);
	}

	public void SetModel(Card card)
	{
		_model = card;

		visual.SetFromModel(card);
	}

	public Card GetModel()
	{
		return _model;
	}

	public void SetState(State state)
	{
		_state = state;
	}

	public State GetState()
	{
		return _state;
	}

	public void ToggleAttackState()
	{
		if(_state == State.Selected)
		{
			_state = State.Idle;
		}
		else
		{
			_state = State.Selected;
		}
	}

	public void SetTargetPosition(Vector3 position)
	{
		_targetPosition = position;
	}

	public void SetIsRotated(bool value)
	{
		_isRotated = value;
	}

	public void AddLink(PlayedCard targetCard)
	{
		if (targetCard != null && targetCard != this && !_linkedCards.Contains(targetCard))
		{
			_linkedCards.Add(targetCard);
			// Optionnel : lier aussi dans l'autre sens si la liaison est bidirectionnelle
			if (!targetCard._linkedCards.Contains(this))
			{
				targetCard._linkedCards.Add(this);
			}

			Debug.Log($"{_model.nom} est lie a {targetCard.GetModel().nom}");
		}
	}

	public void RemoveLink(PlayedCard targetCard)
	{
		if (targetCard != null)
		{
			_linkedCards.Remove(targetCard);
			targetCard._linkedCards.Remove(this);
		}
	}

	public List<PlayedCard> GetLinkedCards()
	{
		return _linkedCards;
	}

	public void SetLinkSelected(bool value)
	{
		_isLinkSelected = value;
	}

	public bool IsLinkSelected()
	{
		return _isLinkSelected;
	}

	// TODO : Animation de mort
	public void Kill()
	{
		// MODIFIE LINKEDCARDS PENDANT LA BOUCLE
		for (int i = _linkedCards.Count - 1; i >= 0; --i)
		{
			RemoveLink(_linkedCards[i]);
		}

		Destroy(gameObject);
	}
}
