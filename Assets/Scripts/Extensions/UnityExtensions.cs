using UnityEngine.AI;

namespace Assets.Scripts.Extensions
{
    public static class UnityExtensions
    {
        public static bool ReachedDestination(this NavMeshAgent agent)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
