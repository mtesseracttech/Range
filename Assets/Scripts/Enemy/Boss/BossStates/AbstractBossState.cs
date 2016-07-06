using UnityEngine;

public abstract class AbstractBossState
{
    protected BossAgent _agent;

    protected AbstractBossState(BossAgent agent)
    {
        _agent = agent;
    }

    public abstract void Update();
}