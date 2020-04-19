using Assets.Core.DI;
using Assets.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableEntity : MonoBehaviour
{
    public Renderer[] highlightRenderers;
    private Color[] renderersColor;
    private Color[] renderersLightColor;
    private bool isHover = false;

    private bool becomesLighter = true;
    private float flashingPointer = 0;
    private float flashingSpeed = 2;

    public IHudService HudService => DependencyInjection.Get<IHudService>();

    void Start()
    {
        ResetFlashingColor();
    }

    public void ResetFlashingColor()
    {
        if (highlightRenderers == null || highlightRenderers.Length == 0)
        {
            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                highlightRenderers = new Renderer[0];
            }
            else
            {
                highlightRenderers = new Renderer[]
                {
                    renderer,
                };
            }
        }

        renderersColor = new Color[highlightRenderers.Length];
        renderersLightColor = new Color[highlightRenderers.Length];

        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            renderersColor[i] = highlightRenderers[i].material.color;
            renderersLightColor[i] = Color.Lerp(renderersColor[i], Color.white, 0.5f);
        }
    }

    void FixedUpdate()
    {
        if (!isHover) return;
        if (HudService.HudIsOpen()) return;

        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var renderer = highlightRenderers[i];
            var rendererColor = renderersColor[i];
            var rendererLightColor = renderersLightColor[i];

            if (becomesLighter)
            {
                flashingPointer += flashingSpeed * Time.deltaTime;
                if (flashingPointer > 1)
                {
                    flashingPointer = 1;
                    becomesLighter = false;
                }
            }
            else
            {
                flashingPointer -= flashingSpeed * Time.deltaTime;
                if (flashingPointer < 0)
                {
                    flashingPointer = 0;
                    becomesLighter = true;
                }
            }

            TranslateColorTo(renderer, rendererColor, rendererLightColor);
        }
    }

    private void TranslateColorTo(Renderer renderer, Color initColor, Color flashingColor)
    {
        renderer.material.color = Color.Lerp(initColor, flashingColor, flashingPointer);
    }

    public void Hover()
    {
        isHover = true;
    }

    public void UnHover()
    {
        isHover = false;
        flashingPointer = 0;

        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var renderer = highlightRenderers[i];
            var rendererColor = renderersColor[i];
            var rendererLightColor = renderersLightColor[i];
            TranslateColorTo(renderer, rendererColor, rendererLightColor);
        }
    }

    public void Click()
    {
        var position = transform.position;
        int x = (int)position.x;
        int z = (int)position.z;

        DependencyInjection.Instance.GetService<IGameService>().SelectPosition(x, z);
    }
}
