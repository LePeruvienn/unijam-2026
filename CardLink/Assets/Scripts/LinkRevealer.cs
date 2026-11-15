using System.Collections;
using UnityEngine;

public class LinkRevealer : MonoBehaviour
{
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float width = 1.5f;
    [SerializeField] private float height = 1.5f;

    public void RevealLink(PlayedCard card, float seconds)
    {
        if (card == null) return;

        foreach (PlayedCard linked in card.GetLinkedCards() )
        {
            Flash(card, linked, seconds);
        }
    }

    public void Flash(PlayedCard a, PlayedCard b, float seconds)
    {
        if (a == null || b == null) return;
        StartCoroutine(FlashCoroutine(a, b, seconds));
    }

    private IEnumerator FlashCoroutine(PlayedCard a, PlayedCard b, float seconds)
    {
        GameObject lineObject = new GameObject("LinkLine");
        lineObject.transform.SetParent(transform);

        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        line.material = lineMaterial;
        line.startColor =  color;
        line.endColor = color;
        line.startWidth = width;
        line.endWidth = width;
        line.positionCount = 2;
        line.useWorldSpace = true;

        float time = 0f;

        while (time < seconds)
        {
            line.SetPosition(0,a.transform.position + Vector3.up*height);
            line.SetPosition(1,b.transform.position+Vector3.up*height);
            
            time += Time.deltaTime;
            yield return null;
        }
    }
}
