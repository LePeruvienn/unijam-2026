using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardVisual : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private TMP_Text label;
	[SerializeField] private TMP_Text desc;
	[SerializeField] private TMP_Text power;
	[SerializeField] private Image art;

	public void SetName(string value)
	{
		label.text = value;
	}

	public void SetDesc(string value)
	{
		desc.text = value;
	}

	public void SetPower(int value)
	{
		power.text = value.ToString();
	}

	public void SetSprite(Sprite value)
	{
		art.sprite = value;
	}

	// NOTE:There is no desc in Card class
	public void SetFromModel(Card model)
	{
		SetName(model.nom);
		SetSprite(model.image);
		SetPower(model.force);
	}
}

