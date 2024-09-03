using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    public NavMeshAgent navMeshAgent;

    private IState<Bot> currentState;
    [HideInInspector] public List<GameObject> brickPositions = new();

    private void Start()
    {
        base.OnInit();
        if (navMeshAgent == null) return;
        ChangeState(new PatrolState());
    }

    private void Update()
    {
        if (currentState == null) return;
        currentState.OnExecute(this);
    }

    public void ChangeState(IState<Bot> state)
    {
        currentState?.OnExit(this);
        currentState = state;
        currentState.OnEnter(this);
    }

    public void SeekBrick()
    {
        if (brickPositions.Count > 0) return;

        Stage currentStage = FindObjectOfType<Stage>();
        if (currentStage != null)
        {
            List<Brick> bricks = currentStage.FindBricksWithColor(color);
            foreach (Brick brick in bricks)
            {
                brickPositions.Add(brick.gameObject);
            }
        }

        if (currentState == null || currentState is not PatrolState)
        {
            ChangeState(new PatrolState());
        }
    }

    public void MoveToNextBrick()
    {
        HandleMovement();
    }

    protected override void HandleMovement()
    {
        if (brickPositions.Count == 0) return;
        if (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return;

        int randomIndex = Random.Range(0, brickPositions.Count);
        GameObject targetBrick = brickPositions[randomIndex];
        if (targetBrick == null) return;

        Vector3 targetPosition = targetBrick.transform.position;
        navMeshAgent.SetDestination(targetPosition);
    }
}