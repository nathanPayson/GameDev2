using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCSearch : MonoBehaviour
{
    private NavMeshAgent navAgent;
    [SerializeField]
    private NPCMode npcMode = NPCMode.wander;

    private Vector3 nextLocation;
    [SerializeField]
    private float wanderDistance = 10f;

    public static List<GameObject> pickUps = new List<GameObject>();
    public static List<GameObject> critters = new List<GameObject>();
    private GameObject pickUpTarget;
    private GameObject critterTarget;
    [SerializeField]
    private float pickUpDistance = 25f;

    // Start is called before the first frame update
    void Start()
    {
        nextLocation = this.transform.position;
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (npcMode)
        {
            // ENTER THE STATE
            case NPCMode.wander:
                // DO WHATEVER THE STATE IS SUPPOSED TO DO
                DoWander(); // each state runs an action to be performed while in that state,
                            // but the action is state-independent (can be run by any state)
                // SPECIFY THE CONDITION ON EXITING THE STATE HERE AND WHICH STATE TO EXIT TO
                if (CanSeeCritter()) // if condition to exit the wander state and enter the attack state is met...
                    npcMode = NPCMode.attack; // simply change the state. Do not perform any state-related action
                else if (CanSeePickUp()) // ... if conditional transition to going to attack has not been met, maybe go to collect a pikcup?
                    npcMode = NPCMode.collect; // ... if so, only chnage the satate to collect, but do not perform any action
                break;
            case NPCMode.collect:
                DoCollect();
                if (CanSeeCritter())
                    npcMode = NPCMode.attack;
                else if (!pickUpTarget.activeSelf)
                    npcMode = NPCMode.wander;
                break;
            case NPCMode.attack:
                MoveToCritter();
                if (!critterTarget.activeSelf)
                    npcMode = NPCMode.wander;
                break;

            default:
                break;
        }
        
        
    }

    private void DoWander()
    {
        //if close choose next location
        if(navAgent.remainingDistance < 1f)
        {
            Vector3 random = Random.insideUnitSphere * wanderDistance;
            random.y = 0f;
            nextLocation = this.transform.position + random;

            if(NavMesh.SamplePosition(nextLocation, out NavMeshHit hit, 5f , NavMesh.AllAreas))
            {
                nextLocation = hit.position;
                navAgent.SetDestination(nextLocation);
            }            
        }
    }

    private bool CanSeePickUp()
    {
        foreach (GameObject pickup in pickUps)
        {
            float distance = (pickup.transform.position - this.transform.position).magnitude;
            if(distance < pickUpDistance)
            {
                Vector3 direction = pickup.transform.position - (this.transform.position + Vector3.up);
                Ray ray = new Ray(this.transform.position + Vector3.up, direction);
                Debug.DrawRay(this.transform.position + Vector3.up, direction, Color.blue);

                if(Physics.Raycast(ray,out RaycastHit hit, pickUpDistance))
                {
                    if (hit.collider.gameObject == pickup)
                    {
                        pickUpTarget = pickup;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void DoCollect()
    {
        if (navAgent.destination != pickUpTarget.transform.position)
            navAgent.SetDestination(pickUpTarget.transform.position);
    }

    private void MoveToCritter()
    {
        if (navAgent.destination != critterTarget.transform.position)
            navAgent.SetDestination(critterTarget.transform.position);

        if(navAgent.remainingDistance < 4f)
        {
            critterTarget.GetComponent<CritterWander>().DoDestroy();
        }
    }

    private bool CanSeeCritter()
    {
        foreach (GameObject critter in critters)
        {
            float distance = (critter.transform.position - this.transform.position).magnitude;
            if (distance < pickUpDistance)
            {
                Vector3 direction = critter.transform.position - (this.transform.position + Vector3.up);
                Ray ray = new Ray(this.transform.position + Vector3.up, direction);
                Debug.DrawRay(this.transform.position + Vector3.up, direction, Color.red);

                if (Physics.Raycast(ray, out RaycastHit hit, pickUpDistance))
                {
                    Debug.Log("I hit" + hit.collider.name);

                    if (hit.collider.gameObject == critter)
                    {
                        critterTarget = critter;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public enum NPCMode
    {
        wander,
        collect,
        attack
    }
}
