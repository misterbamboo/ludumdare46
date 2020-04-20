using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameActionButtonScript : MonoBehaviour
{
    public IGameService GameService => DependencyInjection.Get<IGameService>();
    
    private GameAction CurrentAction { get; set; }

    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        if (CurrentAction == null) return;
        GameService.ExecuteGameAction(CurrentAction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameAction(GameAction action)
    {
        CurrentAction = action;
    }
}
