using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject main;
    public GameObject options;
    public GameObject ingmenu;
    public GameObject ingoptions;
    public string clicked = "main";
    public RessourceManager res;
    public GameManager gam;

    public UnityEngine.UI.Text AudioSliderText;

    public UnityEngine.UI.Text Wood, Food, Human, Faith;

    public void onClickOptions() {
        clicked = "options";
    }

    public void onClickBack() {
        clicked = "main";
        if(Camera.main.GetComponent<MouseRts>() !=null)
            Camera.main.GetComponent<MouseRts>().enabled = true;
        if (res != null) res.gamePaused = false;
        if (gam != null) gam.gamePaused = false;
    }

    public void onClickIngMenu() {
        clicked = "ingmenu";
        Camera.main.GetComponent<MouseRts>().enabled = false;
        res.gamePaused = true;
        gam.gamePaused = true;
    }

    public void onClickIngOptions() {
        clicked = "ingoptions";
    }

    //Level Laden, unter Menü?
    public void onClickStart() {
        Application.LoadLevel(1);
    }

    public void onClickBackMain() {
        Application.LoadLevel(0);
    }

    public void onClickExit() {
        Application.Quit();
    }

    public void onVolumeChanged(float val) {
        AudioListener.volume = val;
        AudioSliderText.text = val.ToString();
    }

    public void onFoodChanged(float val) {
        Food.text = ((int)val)+"";
    }

    public void onWoodChanged(float val) {
        Wood.text = ((int)val) + "";
    }

    public void onHumanChanged(float val) {
        Human.text = ((int)val) + "";
    }

    public void onFaithChanged(float val) {
        Faith.text = ((int)val) + "";
    }

    void Start() {
        if (main == null) {
            clicked = null;
        }
    }



    void Update() {
        if (clicked != null) {
            if (clicked == "main") {
                if(main != null)
                main.SetActive(true);
                if (options != null)
                options.SetActive(false);
                if (ingmenu != null)
                ingmenu.SetActive(false);
                if (ingoptions != null)
                ingoptions.SetActive(false);
            } else if (clicked == "options") {
                if (main != null)
                main.SetActive(false);
                if (options != null)
                options.SetActive(true);
                if (ingmenu != null)
                ingmenu.SetActive(false);
                if (ingoptions != null)
                ingoptions.SetActive(false);
            } else if (clicked == "ingmenu") {
                if (main != null)
                main.SetActive(false);
                if (options != null)
                options.SetActive(false);
                if (ingmenu != null)
                ingmenu.SetActive(true);
                if (ingoptions != null)
                ingoptions.SetActive(false);
            } else if (clicked == "ingoptions") {
                if (main != null)
                main.SetActive(false);
                if (options != null)
                options.SetActive(false);
                if (ingmenu != null)
                ingmenu.SetActive(false);
                if (ingoptions != null)
                ingoptions.SetActive(true);
            }
        }
    }
}
