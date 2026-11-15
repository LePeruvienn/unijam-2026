using UnityEngine;
using System.Collections.Generic;

public class PlayedCard : MonoBehaviour
{
	public enum State
	{
		Idle,
		Hovered,
		Selected,
		Attacking
	}

	[Header("Card Position Offsets")]
	[SerializeField] private Vector3 hoveredPosition = new Vector3(0f, 0.5f, 0f);
	[SerializeField] private Vector3 idlePosition    = new Vector3(0f,   0f, 0f);
	[SerializeField] private Vector3 selectPosition  = new Vector3(0f, 0f, 1f);

	[Header("Animations")]
	[SerializeField] private float animationSpeed = 2f;

	private bool _isRotated = false;

	private State _state = State.Idle;

	[SerializeField]
	private Vector3 _targetPosition = Vector3.zero;

	private Card _model;
	
	protected void Update()
	{
		UpdatePosition();
		UpdateRotation();
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

	// TODO : Animation de mort
	public void Kill()
	{
		Destroy(gameObject);
	}
}
