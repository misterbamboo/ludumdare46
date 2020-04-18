using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.SelectionManagement
{
    public class MousePointer : MonoBehaviour
    {
        private Renderer lastRenderer = null;
        private Color lastRendererInitialColor;
        private Color lastRendererLightColor;
        private bool becomesLighter = true;

        private float lastScrollValue = 0;
        private float zoom = 0;

        void Start()
        {
            lastScrollValue = Input.mouseScrollDelta.y;
        }

        void Update()
        {
            UpdatePointer();
            UpdateZoom();
        }

        private void UpdatePointer()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var renderer = hit.transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    PointedRenderer(renderer);
                }
            }
        }

        private void UpdateZoom()
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom, 0.1f);

            if (Input.mouseScrollDelta.y == lastScrollValue) return;

            float diffToApply = Input.mouseScrollDelta.y - lastScrollValue;
            zoom += diffToApply;
            zoom = Mathf.Clamp(zoom, 4, 10);
        }

        void FixedUpdate()
        {
            if (lastRenderer == null) return;

            if (becomesLighter)
            {
                lastRenderer.material.color = Color.Lerp(lastRenderer.material.color, lastRendererLightColor, 0.1f);
                if (lastRenderer.material.color == lastRendererLightColor)
                {
                    becomesLighter = false;
                }
            }
            else
            {
                lastRenderer.material.color = Color.Lerp(lastRendererLightColor, lastRenderer.material.color, 0.1f);
                if (lastRenderer.material.color == lastRendererInitialColor)
                {
                    becomesLighter = true;
                }
            }
        }

        private void PointedRenderer(Renderer renderer)
        {
            if (lastRenderer != renderer)
            {
                ResetLastRenderer();
                SetNewRenderer(renderer);
            }
        }

        private void ResetLastRenderer()
        {
            if (lastRenderer == null) return;
            lastRenderer.material.color = lastRendererInitialColor;
        }

        private void SetNewRenderer(Renderer renderer)
        {
            lastRenderer = renderer;
            lastRendererInitialColor = lastRenderer.material.color;
            lastRendererLightColor = Color.Lerp(lastRenderer.material.color, Color.white, 0.5f);
            becomesLighter = true;
        }
    }
}
