using UnityEngine;

public class BoardInteraction : MonoBehaviour
{
	[Header("Raycast Interaction")]
	[SerializeField] private float maxRaycastDistance = 100f;
	[SerializeField] private string raycastLayerName = "PlayedCard";

	private Vector2 _mousePosition = Vector2.zero;

	private PlayedCard _clickedPlayedCard = null;
	private PlayedCard _hoveredPlayedCard = null;

	private Camera _mainCamera;
	private LayerMask _playedCardLayerMask;

	// Unity

	protected void Awake()
	{
		_mainCamera = Camera.main;
		_playedCardLayerMask = LayerMask.GetMask(raycastLayerName);
	}

	// Input Events

	public void OnMouseMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		Vector2 newMousePosition = context.ReadValue<Vector2>();

		if (newMousePosition != _mousePosition)
		{
			_mousePosition = newMousePosition;
			_hoveredPlayedCard = GetPlayedCardUnderCursor();
		}
	}

	public void OnMouseClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (context.performed == false)
		{
			return;
		}

		float click = context.ReadValue<float>();

		if (click > 0f)
		{
			PlayedCard newClickedCard = GetPlayedCardUnderCursor();

			if (newClickedCard == _clickedPlayedCard)
			{
				_clickedPlayedCard = null;
				Debug.Log("UnSetAttack");
			}
			else
			{
				Debug.Log("Set New Attack!");
				_clickedPlayedCard = newClickedCard;
			}
		}
	}

	// Implementation

	private PlayedCard GetPlayedCardUnderCursor()
	{
		RaycastHit hit;
		Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);

		if (Physics.Raycast(ray, out hit, maxRaycastDistance, _playedCardLayerMask))
		{
			Transform objectHit = hit.transform;

			PlayedCard cardHit = objectHit.GetComponent<PlayedCard>();

			return cardHit;
		}

		return null;
	}
	
	// Public methods
	public PlayedCard GetHoveredPlayedCard()
	{
		return _hoveredPlayedCard;
	}

	public PlayedCard GetClickedPlayedCard()
	{
		return _clickedPlayedCard;
	}
}
