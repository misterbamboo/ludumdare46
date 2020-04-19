using Assets.Core.DI;
using Assets.Core.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonScript : MonoBehaviour
{
    public IGameService GameService => DependencyInjection.Get<IGameService>();

    public bool IsSelected => GameService.IsToonSelected(this);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
