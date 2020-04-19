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

    public IMapService MapService => DependencyInjection.Get<IMapService>();

    public bool IsSelected => GameService.IsToonSelected(this);

    private Queue<MovingStep> ScheduledMovingSteps { get; set; }

    private Vector3 PreviousPosition { get; set; }

    private Vector3 PreviousFacing { get; set; }

    private MovingStep CurrentMovingStep { get; set; }

    private float CurrentMovingStepProgression { get; set; }

    private float Speed { get; set; } = 2;

    private ToonLifeGoals ToonLifeGoal { get; set; }

    private int LifeGoalTargetX { get; set; }

    private int LifeGoalTargetZ { get; set; }

    private float Progress { get; set; }

    private float MiningRockSpeed { get; set; } = 2;

    void Start()
    {
        ScheduledMovingSteps = new Queue<MovingStep>();
    }

    void Update()
    {
        ScheduleNextStep();
        Move();

        DoLifeGoal();
        MoveBody();
    }

    private void MoveBody()
    {
        var angle = transform.localEulerAngles;
        if (CurrentMovingStep == null)
        {
            angle.x = Mathf.Sin(Progress) * 16 - 8;
        }
        else
        {
            angle.x = 0;
        }
        transform.localEulerAngles = angle;
    }

    private void DoLifeGoal()
    {
        if (CurrentMovingStep != null) return;
        switch (ToonLifeGoal)
        {
            case ToonLifeGoals.Wait:
                break;
            case ToonLifeGoals.MineRock:
                MineRock();
                LootAtLifeGoal();
                break;
            default:
                break;
        }
    }

    private void LootAtLifeGoal()
    {
        Vector3 target;
        target.y = transform.position.y;
        target.x = LifeGoalTargetX;
        target.z = LifeGoalTargetZ;

        var direction = transform.position - target;
        transform.forward = direction;
    }

    private void MineRock()
    {
        var cubeType = MapService.GetCubeType(LifeGoalTargetX, LifeGoalTargetZ);
        if (cubeType == CubeTypes.Rock)
        {
            int count = MapService.GetRessourceCount(LifeGoalTargetX, LifeGoalTargetZ);
            if (count > 0)
            {
                Progress += Time.deltaTime * MiningRockSpeed;
                if (Progress > 1)
                {
                    Progress -= 1f;
                    MapService.RemoveRessource(LifeGoalTargetX, LifeGoalTargetZ);
                    GameService.RockCount++;
                }
            }
        }
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
            Progress = 0;
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

    public void SetLifeGoal(ToonLifeGoals lifeGoal, int x, int z)
    {
        ToonLifeGoal = lifeGoal;
        LifeGoalTargetX = x;
        LifeGoalTargetZ = z;
    }
}
