using Assets.Core.DI;
using Assets.Core.Entities;
using Assets.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonScript : MonoBehaviour
{
    public IGameService GameService => DependencyInjection.Get<IGameService>();

    public bool IsSelected => GameService.IsToonSelected(this);

    private Queue<MovingStep> ScheduledMovingSteps { get; set; }

    private Vector3 PreviousPosition { get; set; }
    private Vector3 PreviousFacing { get; set; }

    private MovingStep CurrentMovingStep { get; set; }

    private float CurrentMovingStepProgression { get; set; }

    private float Speed { get; set; } = 2;

    private ToonLiveGoals ToonLiveGoal { get; set; }

    void Start()
    {
        ScheduledMovingSteps = new Queue<MovingStep>();
    }

    void Update()
    {
        ScheduleNextStep();
        Move();
    }

    private void Move()
    {
        if (CurrentMovingStep == null) return;

        var movement = Speed * Time.deltaTime;
        CurrentMovingStepProgression += movement;

        var destination = new Vector3(CurrentMovingStep.X, transform.position.y, CurrentMovingStep.Z);
        transform.position = Vector3.Lerp(PreviousPosition, destination, CurrentMovingStepProgression);

        var facing = PreviousPosition - destination;
        transform.forward = Vector3.Lerp(PreviousFacing, facing, CurrentMovingStepProgression * 2);

        if (CurrentMovingStepProgression >= 1)
        {
            CurrentMovingStep = null;
            ScheduleNextStep();
        }
    }

    private void ScheduleNextStep()
    {
        if (CurrentMovingStep == null && ScheduledMovingSteps.Count > 0)
        {
            PreviousFacing = transform.forward;
            PreviousPosition = transform.position;
            CurrentMovingStep = ScheduledMovingSteps.Dequeue();
            CurrentMovingStepProgression = 0;
        }
    }

    public void ScheduleMovingSteps(IEnumerable<MovingStep> movingSteps)
    {
        ScheduledMovingSteps.Clear();
        foreach (var movingStep in movingSteps)
        {
            ScheduledMovingSteps.Enqueue(movingStep);
        }
    }

    public void SetLiveGoal(ToonLiveGoals liveGoal)
    {
        ToonLiveGoal = liveGoal;
    }
}
