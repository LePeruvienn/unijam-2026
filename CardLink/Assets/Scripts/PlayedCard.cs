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
	[SerializeField] private Vector3 attackPosition  = new Vector3(0f, 0.5f, 0f);

	[Header("Animations")]
	[SerializeField] private float animationSpeed = 2f;

	private State _state = State.Idle;

	protected void Update()
	{
		Vector3 targetPosition = Vector3.zero;

		switch (_state)
		{
			case State.Idle:
				targetPosition = idlePosition;
				break;

			case State.Hovered:
				targetPosition = hoveredPosition;
				break;

			case State.Attacking:
				targetPosition = attackPosition;
				break;
		}

		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition,
		                                  Time.deltaTime * animationSpeed);
	}

	public void SetState(State state)
	{
		_state = state;
	}
}
