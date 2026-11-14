using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    [Header("Information")]
    public string nom;
    public int force;
    public Sprite image;


    [Header("Entrée en jeu")]
    public bool aVoyance;
    public bool aCupidon;
    public bool aAmoureux;



    [Header("Attaque")]
    public bool aLienDeSang;
    public bool aSacage;



    [Header("Mort")]
    public bool aPostMortem;
    public bool aBriseLien;

}
