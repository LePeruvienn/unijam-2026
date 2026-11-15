using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkin : MonoBehaviour
{
    [SerializeField] private Sprite p1Turn;
    [SerializeField] private Sprite p2Turn;
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
    }

    public void P1Turn()
    {
        _image.sprite = p1Turn;
    }

    public void P2Turn()
    {
        _image.sprite = p2Turn;
    }
}
