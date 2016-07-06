using UnityEngine;
public class BossReturnState : AbstractBossState
{
    private float _returnSpeed = 0.01f;
    private float _slerpSpeed = 0.1f;
    private readonly Vector3 _originalPoint;
    private readonly Quaternion _originalRotation;
    private Vector2 _targetXZ;
    private Vector2 _parentXZ;

    public BossReturnState(BossAgent agent, Vector3 originalPoint, Quaternion originalRotation) : base(agent)
    {
        _originalPoint = originalPoint;
        _originalRotation = originalRotation;
        _targetXZ = new Vector2(_originalPoint.x, _originalPoint.z);
        _parentXZ = new Vector2(_agent.Parent.transform.position.x, _agent.Parent.transform.position.z);
    }

    public override void Update()
    {
        if (_agent.EnteredNewState)
        {
            Debug.Log("Assigned original patrol point");
            _agent.NavAgent.destination = _originalPoint;
            _agent.NavAgent.Resume();
            _agent.EnteredNewState = false;
        }

        if (_agent.SeesTarget)
        {
            _agent.SetState(typeof (BossAttackState));
        }
        else
        {
            _parentXZ.Set(_agent.Parent.transform.position.x, _agent.Parent.transform.position.z);
            _targetXZ.Set(_originalPoint.x, _originalPoint.z);
            if (Vector2.Distance(_parentXZ, _targetXZ) < 0.1f)
            {
                _agent.Parent.position = _originalPoint;
                _agent.NavAgent.Stop();
                _agent.SetState(typeof(BossPatrolState));
            }
        }
    }
}
