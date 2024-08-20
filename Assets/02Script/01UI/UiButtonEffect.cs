using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButtonEffect : MonoBehaviour
{
    public Vector2 initalSize;

    public ParticleSystem particle;
    public RectTransform rectTransform;

    private Vector2 previousSize;
    private Vector2 intialScale;

    void Start()
    {
        previousSize = new Vector2(initalSize.x, initalSize.y);
        intialScale = transform.localScale;

        ResizeParticleSystem();
    }

    void Update()
    {
        if (GetWorldSize(rectTransform) != previousSize)
        {
            Logger.Log("Rect Transform Change!");
            ResizeParticleSystem();
            previousSize = GetWorldSize(rectTransform);
        }
    }

    private void ResizeParticleSystem()
    {
        Vector2 size = GetWorldSize(rectTransform);

        transform.localScale = new Vector3(intialScale.x * size.x / initalSize.x, intialScale.y * size.y / initalSize.y, 1f);
    }

    private Vector2 GetWorldSize(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        float width = Vector3.Distance(corners[0], corners[3]);
        float height = Vector3.Distance(corners[0], corners[1]);

        return new Vector2(width, height);
    }
}
