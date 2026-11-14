using UnityEngine;

public class GestionEffet : MonoBehaviour
{
    [SerializeField] private EffectEvent onEffect;
    [SerializeField] private EffectPayload effect;



    // Référence unique vers cette classe
    static GestionEffet _instance;

    // Moyen d'accès hors de la classe
    public static GestionEffet instance
    {
        get
        {
            return _instance;
        }
    }



    public void changeEffectToPlay()
    {
        effect.trigger = Trigger.Play;
    }

    public void changeEffectToAttack()
    {
        effect.trigger = Trigger.Attack;
    }

    public void changeEffectToDeath()
    {
        effect.trigger = Trigger.Death;
    }


    public void EffectTriggered()
    {
        onEffect.Invoke(effect);
    }

    public void TriggerEffect(EffectPayload effect)
    {
        switch (effect.trigger)
        {
            case Trigger.Play:
            Debug.Log("Carte jouée");
            break;
            case Trigger.Attack:
            Debug.Log("Carte attaque");
            break;
            case Trigger.Death:
            Debug.Log("Carte qui meurt");
            break;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
