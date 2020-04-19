using Assets.Core.DI;
using Assets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Services
{
    public class MouseService : MonoBehaviour, IMouseService
    {
        private SelectableEntity lastSelectable = null;

        private float lastScrollValue = 0;
        private float zoom = 0;
        private bool mouseWasDown = false;

        private IHudService HudService => DependencyInjection.Get<IHudService>();

        void Start()
        {
            lastScrollValue = Input.mouseScrollDelta.y;
            zoom = Camera.main.orthographicSize;
        }

        void Update()
        {
            UpdateMouseClick();
            UpdatePointer();
            UpdateZoom();
        }

        private void UpdateMouseClick()
        {
            if (HudService.HudIsOpen()) return;

            var mouseDown = Input.GetMouseButtonDown(0);
            if (mouseDown && !mouseWasDown)
            {
                ClickOnSelectable();
            }
            mouseWasDown = mouseDown;
        }

        private void ClickOnSelectable()
        {
            if (lastSelectable == null)
            {
                HudService.CloseHud();
            }
            else
            {
                lastSelectable.Click();
                lastSelectable = null;
            }
        }

        private void UpdatePointer()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var selectable = hit.transform.GetComponent<SelectableEntity>();
                if (selectable != null)
                {
                    HoverSelection(selectable);
                }
                else
                {
                    UnHoverLastSelectable();
                }
            }
        }

        private void UpdateZoom()
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoom, 0.1f);

            if (Input.mouseScrollDelta.y == lastScrollValue) return;

            float diffToApply = Input.mouseScrollDelta.y - lastScrollValue;
            zoom -= diffToApply;
            zoom = Mathf.Clamp(zoom, 4, 10);
        }

        private void HoverSelection(SelectableEntity selectable)
        {
            if (lastSelectable != selectable)
            {
                UnHoverLastSelectable();
                lastSelectable = selectable;
                selectable.Hover();
            }
        }

        private void UnHoverLastSelectable()
        {
            if (lastSelectable == null) return;
            lastSelectable.UnHover();
            lastSelectable = null;
        }
    }
}
