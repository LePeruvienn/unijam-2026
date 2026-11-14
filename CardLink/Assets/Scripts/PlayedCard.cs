using UnityEngine;

public class PlayedCard : MonoBehaviour
{
	public enum State
	{
		Idle,
		Hovered,
		Attacking
	}

	[Header("Card Position Offsets")]
	[SerializeField] private Vector3 hoveredPosition = new Vector3(0f, 0.5f, 0f);
	[SerializeField] private Vector3 idlePosition    = new Vector3(0f,   0f, 0f);
	[SerializeField] private Vector3 attackPosition  = new Vector3(0.5f, 0f, 0f);

	[Header("Animations")]
	[SerializeField] private float animationSpeed = 2f;

	private State _state = State.Idle;

	private Vector3 _targetPosition = Vector3.zero;

	private Card _model;

	protected void Update()
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

			case State.Attacking:
				position += attackPosition;
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

	public void ToggleAttackState()
	{
		if(_state == State.Attacking)
		{
			_state = State.Idle;
		}
		else
		{
			_state = State.Attacking;
		}
	}

	public void SetTargetPosition(Vector3 position)
	{
		_targetPosition = position;
	}
}
