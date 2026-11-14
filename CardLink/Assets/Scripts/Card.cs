using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card")]
public class Card : ScriptableObject
{
    [Header("Information")]
    public string nom;
    public int force;
    public Sprite image;


    [Header("Entrée en jeu")]
    // Voyance révèle les liens d'une carte en entrant en jeu
    public bool aVoyance;
    // Cupidon lie 2 carte en entrant en jeu
    public bool aCupidon;
    // Amoureux se lie à une carte en entrant en jeu
    public bool aAmoureux;
    // Vol transfert tous les liens d'une carte sur celle-ci en entrant en jeu
    public bool aVol;



    [Header("Attaque")]
    // Lien de sang crée un lien entre 1 carte de chaque camp en attaquant
    public bool aLienDeSang;
    // Sacage bris un lien aléatoire d'une carte en attaquant
    public bool aSacage;


    [Header("Mort")]
    // Post Mortem ajoute un lien entre deux carte en mourant
    public bool aPostMortem;
    // Brise Lien détruit tous les liens d'une carte en mourant
    public bool aBriseLien;

}
