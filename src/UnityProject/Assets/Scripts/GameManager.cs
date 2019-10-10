using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    RaycastHit2D hit;
    float rayCastLength = 100;
    public MenuManager menu;
    public LayerMask layerBuilding;
    public LayerMask layerVillager;
    public LayerMask layerVoid;
    public GameObject clickedGameObject;
    public GameObject clickedGameObject_right;
    public GameObject selectedUnitMarker;
    public bool gamePaused;
    GameObject hitpos_right;

    void Start() {
        hitpos_right = new GameObject("HitPosition");
        selectedUnitMarker.SetActive(false);
    }

    void Update()
    {
        if (!gamePaused) {
            //LINKSCLICK
            //detect which object is clicked
            if (Input.GetMouseButtonDown(0)) {
                hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);

                clickedGameObject = hit.collider.gameObject;


            }

            //RECHTSCLICK
            if (Input.GetMouseButtonDown(1)) {

                hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
                hitpos_right.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickedGameObject_right = hit.collider.gameObject;

                if (clickedGameObject != null) {
                    if (clickedGameObject.layer == Mathf.RoundToInt(Mathf.Log10(layerVillager.value) / Mathf.Log10(2))) {

                        if (clickedGameObject_right.layer == Mathf.RoundToInt(Mathf.Log10(layerBuilding.value) / Mathf.Log10(2))) {
                            //CHECKE OB GEBÄUDE PLATZ HAT UND SO!
                            // TODOOOO
                            clickedGameObject.GetComponent<Unit_Standard>().MoveToBuilding(clickedGameObject_right);
                        } else {
                            clickedGameObject.GetComponent<Unit_Standard>().MoveTo(hitpos_right);
                            //VILLAGER GEHE HIER HIN
                        }
                    }
                    if (clickedGameObject.layer == layerBuilding) {
                        //ÖFFNE GUI
                    }
                }
            }
            if (clickedGameObject != null) {
                //select villager
                if (clickedGameObject.layer == Mathf.RoundToInt(Mathf.Log10(layerVillager.value) / Mathf.Log10(2))) {
                    selectedUnitMarker.SetActive(true);
                    selectedUnitMarker.transform.position = clickedGameObject.transform.position - new Vector3(0, 1.1f, 0);
                    //mach sachen  
                } else {
                    selectedUnitMarker.SetActive(false);
                }
            }
        }
        }

    public void ChangeMenuVal(string typ, float val) {
        if (typ == "Food") {
            menu.onFoodChanged(val);
        } else if (typ == "Wood") {
            menu.onWoodChanged(val);
        } else if (typ == "Human") {
            menu.onHumanChanged(val);
        } else if (typ == "Faith") {
            menu.onFaithChanged(val);
        } 
    }
}
