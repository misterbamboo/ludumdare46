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
        private GameObject menuObject;

        [SerializeField]
        private GameObject hudObject;

        [SerializeField]
        private Text rockTextObject;

        [SerializeField]
        private Text wheatTextObject;

        [SerializeField]
        private Text treeTextObject;

        void Start()
        {
        }

        void Update()
        {
            rockTextObject.text = GameService.RockCount.ToString();
            wheatTextObject.text = GameService.WheatCount.ToString();
            treeTextObject.text = GameService.TreeCount.ToString();
        }

        public void UpdateMenuActions(IEnumerable<GameAction> actions)
        {
            var buttons = menuObject.GetComponentsInChildren<Button>();
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

        public void OpenMenu()
        {
            menuObject.SetActive(true);
        }

        public void CloseMenu()
        {
            menuObject.SetActive(false);
        }

        public bool MenuIsOpen()
        {
            return menuObject.activeSelf;
        }
    }
}