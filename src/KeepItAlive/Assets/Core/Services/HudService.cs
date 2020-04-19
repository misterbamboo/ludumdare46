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
        [SerializeField]
        private GameObject hudObject;

        [SerializeField]
        private GameObject gameActionsPannel;

        [SerializeField]
        private GameObject gameActionsButtonPrefab;

        private GameObject[] gameActionsButtons;

        void Start()
        {
            gameActionsButtons = new GameObject[0];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateActions(IEnumerable<GameAction> actions)
        {
            foreach (var gameActionsButton in gameActionsButtons)
            {
                Destroy(gameActionsButton);
            }

            gameActionsButtons = new GameObject[actions.Count()];
            int i = 0;
            foreach (var action in actions)
            {
                gameActionsButtons[i] = Instantiate(gameActionsButtonPrefab, gameActionsPannel.transform);
                var text = gameActionsButtons[i].GetComponentInChildren<Text>();
                text.text = action.Text;

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
    }
}