using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkin : MonoBehaviour
{
    [SerializeField] private Sprite p1Turn;
    [SerializeField] private Sprite p2Turn;
    private Image image;
    
    public void P1Turn()
    {
        image = GetComponent<Image>();
        image.sprite = p1Turn;
    }

    public void P2Turn()
    {
        image = GetComponent<Image>();
        image.sprite = p2Turn;
    }
}
