using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Core.Services
{
    public class HudService : MonoBehaviour, IHudService
    {
        private IGameService GameService => DependencyInjection.Get<IGameService>();

        [SerializeField]
        private GameObject hudObject;

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateActions(IEnumerable<GameAction> actions)
        {
            var buttons = hudObject.GetComponentsInChildren<Button>();
            buttons = buttons.OrderBy(b => b.transform.position.y).ToArray();

            int i = 0;
            foreach (var action in actions)
            {
                buttons[i].gameObject.SetActive(false);

                var button = buttons[i];
                var text = button.GetComponentInChildren<Text>();
                text.text = action.Text;

                var buttonScript = button.GetComponent<GameActionButtonScript>();
                buttonScript.SetGameAction(action);

                buttons[i].gameObject.SetActive(true);

                i++;
            }

            while (i < buttons.Length)
            {
                buttons[i].gameObject.SetActive(false);
                i++;
            }

        }

        public void OpenHud()
        {
            hudObject.SetActive(true);
        }

        public void CloseHud()
        {
            hudObject.SetActive(false);
        }

        public bool HudIsOpen()
        {
            return hudObject.activeSelf;
        }
    }
}