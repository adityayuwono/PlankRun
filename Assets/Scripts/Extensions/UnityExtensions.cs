using UnityEngine;
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

        public static string Name(this RaycastHit hitInfo)
        {
            return hitInfo.collider.gameObject.name;
        }

        public static Transform LastDescendant(this Transform transform)
        {
            while(transform.childCount>0)
            {
                transform = transform.GetChild(0);
            }

            return transform;
        }
    }
}
