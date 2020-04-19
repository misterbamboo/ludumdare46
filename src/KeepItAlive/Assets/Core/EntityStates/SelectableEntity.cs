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

    private float flashingPointer = 0;

    [SerializeField]
    private float flashingSpeed = 1f;

    private ToonScript toon;

    public IHudService HudService => DependencyInjection.Get<IHudService>();

    public IGameService GameService => DependencyInjection.Get<IGameService>();

    void Start()
    {
        ResetFlashingColor();
        toon = GetComponent<ToonScript>();
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
        var forceFlashing = ShouldForceFlashing();

        if (!isHover && !forceFlashing)
        {
            EnsureWriteColor();
            return;
        }

        if (HudService.MenuIsOpen() && !forceFlashing) return;

        if (toon == null && !GameService.HasSelectedToon()) return;

        for (int i = 0; i < highlightRenderers.Length; i++)
        {
            var renderer = highlightRenderers[i];
            var rendererColor = renderersColor[i];
            var rendererLightColor = renderersLightColor[i];

            flashingPointer += flashingSpeed * Time.deltaTime;
            if (flashingPointer > 2 * Mathf.PI)
            {
                flashingPointer -= 2 * Mathf.PI;
            }

            TranslateColorTo(renderer, rendererColor, rendererLightColor);
        }
    }

    private void EnsureWriteColor()
    {
        UnHover();
    }

    private bool ShouldForceFlashing()
    {
        if (toon == null) return false;
        return toon.IsSelected;
    }

    private void TranslateColorTo(Renderer renderer, Color initColor, Color flashingColor)
    {
        float value = (Mathf.Sin(flashingPointer) + 1f) / 2f;
        renderer.material.color = Color.Lerp(initColor, flashingColor, value);
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
            renderer.material.color = rendererColor;
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
