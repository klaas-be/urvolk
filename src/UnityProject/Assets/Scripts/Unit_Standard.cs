using UnityEngine;
using System.Collections;

public class Unit_Standard : MonoBehaviour {

    GameObject pathfinder;
    Pathfinding pathfinding;
    public Animator anim_beine;
    public Animator anim_body;
    bool isSelected = false;
    bool isInJob = false;
    public bool isMoving = false;
    public bool isInGoal = false;
    public bool inHouse = false;
    public bool flipped = false;
    public bool isPraying = false;
    public bool isLookingRight = true;
    public bool isLookingRightArrival = true;
    public bool needsLookCorrection = false;
    public bool lookCorrected = false;
    bool brokeOut;
    int Hunger;
    int Health;
    public bool newPathFound = false;
    public bool routineRunning = false;
    public bool ableToStart = true;
    Vector2 lastPos;
    public Grid grid;
    public float foodConsume = 1;
    public string walkingCommand;
    GameObject[] villagers;
    Building targetBuilding;

    //fmod things
   // FMOD.Studio.EventInstance WalkingSound;
    //FMOD.Studio.ParameterInstance walkingValue;


	// Use this for initialization
	void Start () {
        pathfinder = Instantiate(GameObject.FindGameObjectWithTag("Pathfinder"));  
        pathfinding = pathfinder.GetComponent<Pathfinding>();
        grid = pathfinder.GetComponent<Grid>();
        lastPos = gameObject.transform.position;
        routineRunning = false;
        if (Random.value < 0.49) {
            walkingCommand = "event:/Character/WalkingCommand1";
        } else {
            walkingCommand = "event:/Character/WalkingCommand2";
        }

        //fmod
      //  WalkingSound = FMOD_StudioSystem.instance.GetEvent("event:/WalkingFootstaps");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(lastPos.x- gameObject.transform.position.x> 0 && !lookCorrected) {
            isLookingRight = false;
        } else if (lastPos.x - gameObject.transform.position.x < 0 && !lookCorrected) {
            isLookingRight = true;
        }
        if (isLookingRightArrival && !isMoving && needsLookCorrection && isInGoal) {
            isLookingRight = true;
            needsLookCorrection = false;
            lookCorrected = true;
        } else if (!isLookingRightArrival && !isMoving && needsLookCorrection && isInGoal) {
            isLookingRight = false;
            needsLookCorrection = false;
            lookCorrected = true;
        }
        if (isLookingRight) {
            anim_body.SetBool("lookright", true);
        } else {
            anim_body.SetBool("lookright", false);
        }

        if (isPraying && !isMoving) {
            anim_body.SetBool("praying", true);
            if (isLookingRight && !flipped) {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                flipped = true;
            }
        } else {
            anim_body.SetBool("praying", false);
            if (flipped) {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                flipped = false;
            }
        }

        

        if (isMoving) {
            isInGoal = false;
            anim_beine.SetBool("moving", true);
            anim_body.SetBool("moving", true);
        } else {
            anim_beine.SetBool("moving", false);
            anim_body.SetBool("moving", false);
        }

       /* FMOD
        if (isMoving) {
            gameObject.GetComponent<FMOD_StudioEventEmitter>().
        } else {
            walkingValue.setValue(0);
        }
        **/
        if ((inHouse == true) && (gameObject.transform.position-targetBuilding.transform.position).magnitude < 0.5f) {
            gameObject.SetActive(false);
            isInJob = true;
            gameObject.tag = "Worker";
            Destroy(grid.gameObject, 0f);
            if(targetBuilding != null) {
                if(targetBuilding.gameObject.name =="Farm") {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Buildings/FarmSelect");
                } else if(targetBuilding.gameObject.name =="Tempel") {
                     FMODUnity.RuntimeManager.PlayOneShot("event:/Buildings/TempleSelect");
                } else if(targetBuilding.gameObject.name =="LumberjackCamp") {
                     FMODUnity.RuntimeManager.PlayOneShot("event:/Buildings/LumberjackSelect");

                } 
            }
          
        }

            if (isInGoal) isInGoal = false;
	}

    public void MoveToBuilding(GameObject target) {
        villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (villagers.Length < 2) return;
        targetBuilding = target.GetComponent<Building>();
        target.GetComponent<Collider2D>().enabled = false;
        MoveTo(target);
        target.GetComponent<Collider2D>().enabled = true;
        if(targetBuilding !=null)
        targetBuilding.increaseIncome(2);
        inHouse = true;
    }

    public void MoveTo(GameObject target) {
        if (isPraying) return;
        if (isInJob) return;
        newPathFound = true;
        isInGoal = false;
        lookCorrected = false;
        if (gameObject.Equals(target.gameObject)) {
            newPathFound = false;
            ableToStart = true;
            routineRunning = false;
            return;
        }
        
        pathfinding.seeker = gameObject.transform;
        pathfinding.target = target.transform;

        grid.Create();
        grid.player = gameObject.transform;
        if (!grid.NodeFromWorldPoint(new Vector3(target.transform.position.x, target.transform.position.y, 0)).walkable) {
            return;
        }
        pathfinding.FindPath(pathfinding.seeker.position, pathfinding.target.position);
        ableToStart = true;
        StartCoroutine(TryMoving()); 
        FMODUnity.RuntimeManager.PlayOneShot(walkingCommand);

    }

    IEnumerator TryMoving() {
        while (ableToStart) {

            if (!routineRunning) {

                ableToStart = false;
                isMoving = true;
                newPathFound = false;
                StartCoroutine(Moving());
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Moving() {
        routineRunning = true;

        if (grid.path == null || grid.path.Count == 0) {
            routineRunning = false;
            isMoving = false;
            yield break; ;
        }
        for (int i = 0; i < grid.path.Count; i++) {

            Vector3 pos = grid.path[i].worldPosition;
                while ((pos - gameObject.transform.position).magnitude >= 0.05f && !newPathFound) {

                    
                    lastPos = gameObject.transform.position;
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, grid.path[i].worldPosition, 0.15f);

                    yield return new WaitForSeconds(0.01f);

                }
                if (newPathFound) {
                    routineRunning = false;
                    isMoving = false;
                    isInGoal = false;
                    brokeOut = true;
                    yield break; ;
                }

        }
        routineRunning = false;
        if (brokeOut) { isInGoal = true; } 
        brokeOut = false;
        isMoving = false;
        yield break;
        
    }
}
