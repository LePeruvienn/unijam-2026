using UnityEngine;

[CreateAssetMenu(fileName = "Effect event", menuName = "Events/Effect event")]
public class EffectEvent : AbstractEvent<EffectPayload>
{
    
}
public enum Trigger {Play, Attack, Death};

public struct EffectPayload
{
    public Trigger trigger;
    public Card carte;

    public EffectPayload(Trigger _trigger, Card _carte)
    {
        trigger = _trigger;
        carte = _carte;
    }
}