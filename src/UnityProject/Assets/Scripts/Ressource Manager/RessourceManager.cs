using UnityEngine;
using System.Collections;

public class RessourceManager : MonoBehaviour {

    public GameManager game;

    public GameObject campfireObject;
    Campfire campfire;



    //tick interval in sec
    public float tickInterval;
    public bool gamePaused;

  
    //faith increase by Ritual
    public float faithIncreaseByKidsRitual = 0;
    public float faithIncreaseByRainRitual = 0;
    public float faithIncreaseByPraiseRitual = 5;
    public float faithIncreaseByWoodRitual = 0;


    //Amount of the materials stored
    public float foodAmount;

    public float woodAmount;
    public float faithAmount;
   // public float stoneAmount;


  //Base Incomes
    public float foodBaseIncrease;
    public float WoodBaseIncrease;
    public float faithBaseIncrease;
    //public float stoneBaseIncrease;


    //increse Rates per tick
    public float foodIncrease;
    public float woodIncrease;
    public float faithIncrese;
   // public float stoneIncrese;

    //decrease Rates

    public float foodDecrease;
    public float foodConsumePerUnit;

 

    //all Buildings in the Game

    public GameObject[] farms;
    public GameObject[] lumberjacks;
    public GameObject[] temples;
    public GameObject[] mines;

    //all villagers in the game;

    public GameObject[] villagers;
    public int villagersCount = 1;

    


    //RitualCosts
    bool ritualRunning = false;
    public int maxPraytime = 5;
    //kids
    public float kidsFoodCost = 1;
    public float kidsFoodCostMultiplyer = 2;

    //Rain
    public float rainWoodCost = 1;
    public float rainFaithCost = 1;

    //Praise 
    public float praiseFoodCost = 1;
    public float praiseWoodCost = 1;

    //Protect
    public float protectFaithCost = 1;
    public float protectFoodCost = 1;

    //gathering Points
    public GameObject[] gatheringPoints;
    public bool[] gatheringPointsFaceRight;

    bool updateRunning = false;



	// Use this for initialization
	void Start () {

        StartCoroutine(updateRessources());
        campfire =(Campfire) campfireObject.GetComponent<Campfire>();
      
        gatheringPointsFaceRight = new bool[gatheringPoints.Length];
        for (int i = 0; i < gatheringPointsFaceRight.Length; i++) {
            if (gatheringPoints[i].transform.position.x - campfireObject.transform.position.x > 0) {
                gatheringPointsFaceRight[i] = false;
            } else {
                gatheringPointsFaceRight[i] = true;
            }
        }



    }

    IEnumerator updateRessources()
    {
        updateRunning = true;
        while (!gamePaused)
        {
            CalculateIncome();
           
            yield return new WaitForSeconds(tickInterval);
        }
        updateRunning = false;
    }

    void Update() {
        if (!updateRunning) {
            StartCoroutine(updateRessources());
        }
    }

    public void CalculateIncome()
    {

        //farms
        farms = GameObject.FindGameObjectsWithTag("Farm");
        foodIncrease = 0;
        for(int i=0; i<farms.Length; i++)
        {
          foodIncrease += farms[i].GetComponent<Farm>().foodIncome;
        }

        foodAmount += foodBaseIncrease + foodIncrease;
        game.ChangeMenuVal("Food", foodAmount);
        //Lumberjacks

        lumberjacks = GameObject.FindGameObjectsWithTag("Lumberjack");
        woodIncrease = 0;
        for (int i = 0; i < lumberjacks.Length; i++)
        {
            woodIncrease += lumberjacks[i].GetComponent<Lumberjack>().woodIncome;
        }

        woodAmount += WoodBaseIncrease + woodIncrease;
        game.ChangeMenuVal("Wood", woodAmount);



        //faith

        temples = GameObject.FindGameObjectsWithTag("Temple");
        faithIncrese = 0;
        for (int i = 0; i < temples.Length; i++)
        {
            faithIncrese += temples[i].GetComponent<Temple>().faithIncome;
        }

        faithAmount += faithBaseIncrease + faithIncrese;
        game.ChangeMenuVal("Faith", faithAmount);


        /*mines

        mines = GameObject.FindGameObjectsWithTag("Mine");
        stoneIncrese = 0;
        for (int i = 0; i < mines.Length; i++)
        {
            stoneIncrese += mines[i].GetComponent<Mine>().stoneIncome;
        }

        stoneAmount += stoneBaseIncrease + stoneIncrese;
        game.ChangeMenuVal("Stone", stoneAmount);
        */

        //count villagers and calculate Food consume

        game.ChangeMenuVal("Human", villagersCount);

        villagers = GameObject.FindGameObjectsWithTag("Villager");
        foodDecrease= 0;
        for (int i = 0; i < villagersCount; i++)
        {
            foodDecrease += foodConsumePerUnit; 
        }

        if (foodAmount > foodDecrease) { 
        foodAmount = foodAmount - foodDecrease;
        game.ChangeMenuVal("food", foodAmount);
        }


    }


    //Perform Rituals

    public void PerformRitual(string typ) {
        if (!ritualRunning) {
            ritualRunning = true;
            gatherVillagersAroundCampfire(maxPraytime);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Character/RitualSoundÓneshot");
            if (typ == "kids") {
                Invoke("KidsRitual", maxPraytime +1);
            } else if (typ == "rain") {
                Invoke("RainRitual", maxPraytime + 1);
            } else if (typ == "praise") {
                Invoke("PraiseRitual", maxPraytime + 1);
            } else if (typ == "wood") {
                Invoke("WoodRitual", maxPraytime + 1);
            }
        }
    }

    void KidsRitual() {
        if (foodAmount >= kidsFoodCost) {
            foodAmount -= kidsFoodCost;
            //raise kidsFoodCost
            kidsFoodCost = kidsFoodCost * kidsFoodCostMultiplyer;
            campfire.createVillager();
            faithAmount += faithIncreaseByKidsRitual;
            villagersCount++;
        }
        ritualRunning = false;
    }

    void RainRitual() {
        if (faithAmount >= rainFaithCost && woodAmount >= rainWoodCost) {
            faithAmount -= rainFaithCost;
            woodAmount -= rainWoodCost;
            foodAmount += 20;
        }
        ritualRunning = false;
    }

    void PraiseRitual() {
          villagers = GameObject.FindGameObjectsWithTag("Villager");
        if (foodAmount >= praiseFoodCost && woodAmount >= praiseWoodCost && villagers.Length>1) {
            foodAmount -= praiseFoodCost;
            woodAmount -= praiseWoodCost;
            faithAmount += faithIncreaseByPraiseRitual;
            villagersCount--;
          
            Destroy(villagers[villagers.Length - 1].GetComponent<Unit_Standard>().grid.gameObject);
            Destroy(villagers[villagers.Length - 1]);

        }
        ritualRunning = false;
    }

    void WoodRitual() {
        if (faithAmount >= protectFaithCost && foodAmount >= protectFoodCost) {
            faithAmount -= protectFaithCost;
            foodAmount -= protectFoodCost;
            faithAmount += faithIncreaseByWoodRitual;
        }
        ritualRunning = false;
    }

    /*public void PerformKidsRitual()
    {
       
        gatherVillagersAroundCampfire();
        if (foodAmount >= kidsFoodCost)
        {
            foodAmount -= kidsFoodCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/RitualSoundÓneshot");
            campfire.createVillager();
        }
        faithAmount += faithIncreaseByKidsRitual;
    }

    public void PerformRainRitual() {
      
        gatherVillagersAroundCampfire();
        if (foodAmount >= rainFoodCost && woodAmount >=rainWoodCost) {
            foodAmount -= rainFoodCost;
            woodAmount -= rainWoodCost;
            //increse Food amount
            foodAmount += 5;
            FMODUnity.RuntimeManager.PlayOneShot("event:/RitualSoundÓneshot");
        }
        faithAmount += faithIncreaseByRainRitual;
    }

    public void PerformPraiseRitual() {
        
        gatherVillagersAroundCampfire();
        if (stoneAmount >= praiseStoneCost && woodAmount >= praiseWoodCost) {
            stoneAmount -= praiseStoneCost;
            woodAmount -= praiseStoneCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/RitualSoundÓneshot");
        }
        faithAmount += faithIncreaseByPraiseRitual;
    }

    public void PerformProtectionRitual() {
        
        gatherVillagersAroundCampfire();
        if (stoneAmount >= protectStoneCost && woodAmount >= protectWoodCost && foodAmount >= protectFoodCost) {
            stoneAmount -= protectStoneCost;
            woodAmount -= protectWoodCost;
            foodAmount -= protectFoodCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/RitualSoundÓneshot");
        }
        faithAmount += faithIncreaseByProtectionRitual;
    }*/

    public void gatherVillagersAroundCampfire(float t) {
        villagers = GameObject.FindGameObjectsWithTag("Villager");
        for (int i = 0; i < villagers.Length; i++) {

            if (gatheringPoints.Length > i) {
                villagers[i].GetComponent<Unit_Standard>().MoveTo(gatheringPoints[i]);
                villagers[i].GetComponent<Unit_Standard>().needsLookCorrection = true;
                villagers[i].GetComponent<Unit_Standard>().isPraying = true;
                villagers[i].GetComponent<Unit_Standard>().isLookingRightArrival = gatheringPointsFaceRight[i];
            }
        }
        Invoke("EndPray", t);

    }

    void EndPray() {
        for (int i = 0; i < villagers.Length; i++) {
            villagers[i].GetComponent<Unit_Standard>().isPraying = false;
        }
    }



}
