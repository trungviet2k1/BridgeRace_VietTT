using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Bot : Character
{
    [Header("Navmesh Agent")]
    public NavMeshAgent navMeshAgent;

    private IState<Bot> currentState;
    private Stage currentStage;
    private Vector3 destination;

    [HideInInspector] public List<GameObject> brickPositions = new();

    public override void OnInit()
    {
        base.OnInit();
        if (navMeshAgent == null) return;

        navMeshAgent.speed = GetMoveSpeed();
        ChangeState(new PatrolState());
        brickPositions.Clear();
        ClearBricks();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        currentState = null;
    }

    public void SetDestination(Vector3 position)
    {
        navMeshAgent.enabled = true;
        destination = position;
        destination.y = 0;
        navMeshAgent.SetDestination(position);
    }

    private void Update()
    {
        if (currentState == null) return;
        currentState.OnExecute(this);
        CanMove(TF.position);
        UpdateCurrentStage();
    }

    public void ChangeState(IState<Bot> state)
    {
        currentState?.OnExit(this);
        currentState = state;
        currentState.OnEnter(this);
    }

    public void SeekBrick()
    {
        if (currentStage == null || brickPositions == null) return;

        List<Brick> bricks = currentStage.FindBricksWithColor(color);
        foreach (Brick brick in bricks)
        {
            brickPositions.Add(brick.gameObject);
        }

        if (currentState == null || currentState is not PatrolState)
        {
            ChangeState(new PatrolState());
        }
    }

    public void BotMovement()
    {
        if (brickPositions.Count == 0 || navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return;

        int randomIndex = Random.Range(0, brickPositions.Count);
        GameObject targetBrick = brickPositions[randomIndex];
        if (targetBrick == null) return;

        Vector3 targetPosition = targetBrick.transform.position;
        if (CanMove(targetPosition))
        {
            navMeshAgent.isStopped = false;
            SetDestination(targetPosition);
        }
    }

    protected override void HandleMovement() { }

    private void UpdateCurrentStage()
    {
        Stage[] stages = FindObjectsOfType<Stage>();
        foreach (Stage stage in stages)
        {
            if (IsOnStage(stage))
            {
                currentStage = stage;
                return;
            }
        }
        currentStage = null;
    }

    private bool IsOnStage(Stage stage)
    {
        Vector3 characterPosition = TF.position;
        Ray ray = new(characterPosition + Vector3.up * 1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, LayerMask.GetMask("Ground")))
        {
            if (hit.collider.transform.IsChildOf(stage.transform))
            {
                return true;
            }
        }
        return false;
    }

    internal void StopMove()
    {
        ChangeAnim(Constants.ANIM_IDLE);
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
    }
}