using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using rnd = UnityEngine.Random;
using UnityEngine;
using KModkit;

public class Cookie_Clicker : MonoBehaviour {

    //Main Display
    public TextMesh CookieCount;
    public KMSelectable CookieButton;
    public KMSelectable Buildings;
    public KMSelectable Rebirth;
    public KMSelectable Buildings_Display;
    public TextMesh Buildings_Text;
    public KMSelectable[] MultiplierButtons;
    public TextMesh[] MultiText;
    public GameObject Pay_Amount;
    public TextMesh Pay_Amount_text;
    public GameObject Cookie_Amount;
    public TextMesh Cookie_Amount_text;
    public GameObject Display;
    public KMSelectable Return_Button;
    public TextMesh Return_Button_text;
    public Sprite[] Totalcookies;
    public Sprite[] Grandmas;
    public Sprite[] Farms;
    public Sprite[] Mines;
    public Sprite[] Factories;
    public Sprite[] Banks;
    public Sprite[] Temples;
    public Sprite[] Wizard_Towers;
    public Sprite[] Shipments;
    public Sprite[] Alchemy_Labs;
    public Sprite[] Portals;
    public Sprite[] Time_Machines;
    public Sprite[] Antimater_Condensors;
    public Sprite[] Prisms;
    public GameObject Achi_template;
    public GameObject Achi_Template;
    public TextMesh Achi_text;
    public SpriteRenderer Achi_Sprite;
    public KMBombInfo Bomb;
    public KMAudio audio;
    public bool _modsolved = false;
    public int _modIDCount = 1;
    public int _modID;
    public bool holding = false;
    public bool IsAnimating = false;
    public long CookiestobeAdded = 0;
    public long TotalCookies;
    public long Desired_Cookies = 0;
    public int Desired_Achievement_type = 0;
    public int Desired_Achievement_num = 0;
    public float InitialTime = 0;

    //Building

    public double[] Buildings_Multiplier = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    long[] Buildings_CPS = new long[] { 100, 220, 500, 2000, 8000, 25000, 50000, 5000000, 2000000, 16000000, 80000000, 550000000, 3500000000 };
    public int[] Building_Quantity = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    long[] Buildings_BaseCost = new long[] {50,550, 8000, 13000, 140000, 2000000, 330000000, 510000000, 7500000000, 10000000000, 140000000000, 170000000000, 2100000000000 };

    string[] Buildings_Names = new string[] { "Grandma", "Farm", "Mine", "Factory", "Bank", "Temple", "Wizard Tower", "Shipment", "Alchemy Lab", "Portal", "Time Machine", "Antimater\n Condensor", "Prism" };
    bool[][] isAchieved = new bool[14][]
    {
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false},
        new bool[]{ false,false,false,false,false,false,false,false,false,false,false,false,false}
    };

    public int Building_Counter;
    public bool[] Multiplier_index = new bool[4];
    public bool isMultiON = false;

    //Scripts

    Calculation_Script calcscript;

    //Rebirth

    public int Rebirths = 0;


	// Use this for initialization
	void Start () {
        StartCoroutine(UpdateHighlights());
        InitialTime = Bomb.GetTime();
        calcscript = new Calculation_Script(this);
        CalculateDesiredStuff();
        CookieButton.OnInteract += delegate { ClicksaidButton(); return false; };
        Buildings.OnInteract += delegate { SetBuildingsScene(); return false; };
        Buildings_Display.OnInteract += delegate { OnButtonPress(); return false; };
        Buildings_Display.OnInteractEnded += delegate { OnButtonRelease(); return; };
        Return_Button.OnInteract += delegate { ReturnToMainCookie(); return false; };
        Rebirth.OnInteract += delegate { RebirthButton(); return false; };
        foreach(KMSelectable MultiplierButton in MultiplierButtons){
            MultiplierButton.OnInteract += delegate { SetMultiplier(MultiplierButton); if (_activeMultiplier != -1) { SetPriceforBuilding(); } return false; };
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(TotalCookies >= Desired_Cookies && isAchieved[Desired_Achievement_type-1][Desired_Achievement_num-1] && !_modsolved){
            _modsolved = true;
            GetComponent<KMBombModule>().HandlePass();
            StopAllCoroutines();
            Debug.LogFormat("[Cookie Clicker #1] Yay you did it you managed to get finish the module",_modID);

        }
	}

    void Awake(){
        _modID = _modIDCount++;
    }

    void ClicksaidButton(){
        audio.PlaySoundAtTransform("eat"+rnd.Range(1,4),this.transform);
        TotalCookies++;
        CookieCount.text = calcscript.ConvertToScientific(TotalCookies);
        if (TotalCookies >= 1 && !isAchieved[0][0])
        {
            calcscript.SetAchievement(0, 0);
            isAchieved[0][0] = true;
            StartCoroutine(AchievementAnimation(0.001f));
        }

    }
    
    void SetBuildingsScene(){
        Display.SetActive(false);
        CookieButton.gameObject.SetActive(false);
        Buildings.gameObject.SetActive(false);
        Rebirth.gameObject.SetActive(false);

        Buildings_Display.GetComponent<Renderer>().enabled = true;
        Buildings_Display.Highlight.gameObject.SetActive(true);
        Buildings_Text.GetComponent<Renderer>().enabled = true;

        foreach(KMSelectable multiplier in MultiplierButtons){
            multiplier.GetComponent<Renderer>().enabled = true;
            multiplier.Highlight.gameObject.SetActive(true);
        }

        foreach(TextMesh multitext in MultiText){
            multitext.GetComponent<Renderer>().enabled = true;
        }

        Pay_Amount.GetComponent<Renderer>().enabled = true;
        Pay_Amount_text.GetComponent<Renderer>().enabled = true;
        Cookie_Amount.GetComponent<Renderer>().enabled = true;
        Cookie_Amount_text.GetComponent<Renderer>().enabled = true;
        Return_Button.GetComponent<Renderer>().enabled = true;
        Return_Button.Highlight.gameObject.SetActive(true);
        Return_Button_text.GetComponent<Renderer>().enabled = true;
        Cookie_Amount_text.text = calcscript.ConvertToScientific(Convert.ToDouble(CookieCount.text));

        Buildings_Text.text = "Grandma";
    }

    void SetPriceforBuilding(){

        switch (_activeMultiplier){
            case 0:
                int index = Array.IndexOf(Buildings_Names, Buildings_Text.text);
                double cost = calcscript.CalculateBasePrice(1,Buildings_BaseCost[index]);
                Pay_Amount_text.text = calcscript.ConvertToScientific(cost);
                break;
            case 1:
                int index1 = Array.IndexOf(Buildings_Names, Buildings_Text.text);
                double cost1 = calcscript.CalculateBasePrice(10, Buildings_BaseCost[index1]);
                Pay_Amount_text.text = calcscript.ConvertToScientific(cost1);
                break;
            case 2:
                int index2 = Array.IndexOf(Buildings_Names, Buildings_Text.text);
                double cost2 = calcscript.CalculateBasePrice(100, Buildings_BaseCost[index2]);
                Pay_Amount_text.text = calcscript.ConvertToScientific(cost2);
                break;
            case 3:
                int index3 = Array.IndexOf(Buildings_Names, Buildings_Text.text);
                double cost3 = calcscript.CalculateBasePrice(1000, Buildings_BaseCost[index3]);
                Pay_Amount_text.text = calcscript.ConvertToScientific(cost3);
                break;
            default: Debug.LogFormat("[Cookie Clicker #{0}] Cannot generate price due to an incorrect Multiplier!",_modID);
                return;

        }
    }

    void RebirthButton(){
        if (Bomb.GetModuleNames().EqualsAny("Cookie Jars") && Bomb.GetBatteryCount() == 4 && Bomb.GetBatteryHolderCount() == 2)
        {
            Rebirths++;
            Debug.LogFormat("[Cookie Clicker #{0}] Hey you did it, you got the special Unicorn and solved the module, good job you ;)", _modID);
            GetComponent<KMBombModule>().HandlePass();
        }
        else
        {
            Debug.LogFormat("[Cookie Clicker #{0}] You thought you could cheat, but cheaters are not allowed to be cheating here :)",_modID);
            GetComponent<KMBombModule>().HandleStrike();
        }
    }

    public int _activeMultiplier = 0;
    void SetMultiplier(KMSelectable Multi){
        var index = Array.IndexOf(MultiplierButtons, Multi);
        if (Multiplier_index.All(x => !x)){
            MultiText[index].color = new Color32(0, 255, 0, 255);
            Multiplier_index[index] = true;
            _activeMultiplier = index;
            return;
        }
        if (_activeMultiplier == index){
            _activeMultiplier = -1;
            MultiText[index].color = new Color32(0, 0, 0, 255);
            Multiplier_index[index] = false;
            return;
        }
        else{
            MultiText[_activeMultiplier].color = new Color32(0, 0, 0, 255);
            Multiplier_index[_activeMultiplier] = false;
            MultiText[index].color = new Color32(0, 255, 0, 255);
            Multiplier_index[index] = true;
            _activeMultiplier = index;
            return;
        }

    }

    public void OnButtonPress(){
        StartCoroutine(HoldButtonTime());
    }

    public void OnButtonRelease(){
        if (IsAnimating) { return; }
        StopAllCoroutines();
        if (holding){
            if (TotalCookies < Convert.ToDouble(Pay_Amount_text.text)) { GetComponent<KMBombModule>().HandleStrike();Debug.LogFormat("[Cookie Clicker #{0}]Not enough cookies to buy the desired building, please gather more and try again!", _modID); holding = false; audio.PlaySoundAtTransform("cookieBreak", this.transform); return; }
            TotalCookies -= long.Parse(Convert.ToDouble(Pay_Amount_text.text).ToString());
            Cookie_Amount_text.text = calcscript.ConvertToScientific(TotalCookies);
            int index = Array.IndexOf(Buildings_Names, Buildings_Text.text);
            int multi = 0;
            switch (_activeMultiplier){
                case 0:
                    multi = 1;
                    break;
                case 1:
                    multi = 10;
                    break;
                case 2:
                    multi = 100;
                    break;
                case 3:
                    multi = 1000;
                    break;
            }
            Building_Quantity[index] += multi;
            StartCoroutine(UpdateCookieCount());
            holding = false;
            audio.PlaySoundAtTransform("buy" + rnd.Range(1, 5), this.transform);
            return;
        }
        Building_Counter = Array.IndexOf(Buildings_Names, Buildings_Text.text);
        Building_Counter++;
        if (Building_Counter > Buildings_Names.Length - 1)
        {
            Building_Counter = 0;
        }
        Buildings_Text.text = Buildings_Names[Building_Counter];
        SetPriceforBuilding();
        holding = false;
        StartCoroutine(UpdateCookieCount());
    }
    public void ReturnToMainCookie(){
        Buildings_Display.GetComponent<Renderer>().enabled = false;
        Buildings_Display.Highlight.gameObject.SetActive(false);
        Buildings_Text.GetComponent<Renderer>().enabled = false;

        foreach (KMSelectable multiplier in MultiplierButtons)
        {
            multiplier.GetComponent<Renderer>().enabled = false;
            multiplier.Highlight.gameObject.SetActive(false);
        }

        foreach (TextMesh multitext in MultiText)
        {
            multitext.GetComponent<Renderer>().enabled = false;
        }

        Pay_Amount.GetComponent<Renderer>().enabled = false;
        Pay_Amount_text.GetComponent<Renderer>().enabled = false;
        Cookie_Amount.GetComponent<Renderer>().enabled = false;
        Cookie_Amount_text.GetComponent<Renderer>().enabled = false;
        Return_Button.GetComponent<Renderer>().enabled = false;
        Return_Button.Highlight.gameObject.SetActive(false);
        Return_Button_text.GetComponent<Renderer>().enabled = false;
        Display.SetActive(true);
        CookieButton.gameObject.SetActive(true);
        Buildings.gameObject.SetActive(true);
        Rebirth.gameObject.SetActive(true);
    }

    IEnumerator UpdateHighlights(){
        yield return new WaitForSeconds(0.7f);
        Buildings_Display.Highlight.gameObject.SetActive(false);
        Return_Button.Highlight.gameObject.SetActive(false);
        foreach(KMSelectable mult in MultiplierButtons){
            mult.Highlight.gameObject.SetActive(false);
        }
        yield break; 
    }

    IEnumerator UpdateCookieCount(){
        while (true){
            foreach (var Building in Buildings_Names)
            {
                int index = Array.IndexOf(Buildings_Names, Building);
                int count = Building_Quantity[index];
                double multi = Buildings_Multiplier[index];
                long baseCPS = Buildings_CPS[index];
                CookiestobeAdded += (long)((count * baseCPS)*multi);

                if (!IsAnimating)
                {
                    if (count >= 1 && count <= 500)
                    {
                        if (count >= 1 && !isAchieved[index + 1][0])
                        {
                            calcscript.SetAchievement(index + 1, 0);
                            isAchieved[index + 1][0] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 1.1;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 50 && !isAchieved[index + 1][1])
                        {
                            calcscript.SetAchievement(index + 1, 1);
                            isAchieved[index + 1][1] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 1.5;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 100 && !isAchieved[index + 1][2])
                        {
                            calcscript.SetAchievement(index + 1, 2);
                            isAchieved[index + 1][2] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 2.1;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 150 && !isAchieved[index + 1][3])
                        {
                            calcscript.SetAchievement(index + 1, 3);
                            isAchieved[index + 1][3] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 2.8;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 200 && !isAchieved[index + 1][4])
                        {
                            calcscript.SetAchievement(index + 1, 4);
                            isAchieved[index + 1][4] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 3.5;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 250 && !isAchieved[index + 1][5])
                        {
                            calcscript.SetAchievement(index + 1, 5);
                            isAchieved[index + 1][5] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 4.1;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 300 && !isAchieved[index + 1][6])
                        {
                            calcscript.SetAchievement(index + 1, 6);
                            isAchieved[index + 1][6] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 4.9;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 350 && !isAchieved[index + 1][7])
                        {
                            calcscript.SetAchievement(index + 1, 7);
                            isAchieved[index + 1][7] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 5.5;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 400 && !isAchieved[index + 1][8])
                        {
                            calcscript.SetAchievement(index + 1, 8);
                            isAchieved[index + 1][8] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 6.3;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 450 && !isAchieved[index + 1][9])
                        {
                            calcscript.SetAchievement(index + 1, 9);
                            isAchieved[index + 1][9] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 7.5;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                        else if (count >= 500 && !isAchieved[index + 1][10])
                        {
                            calcscript.SetAchievement(index + 1, 10);
                            isAchieved[index + 1][10] = true;
                            var currentmulti = Buildings_Multiplier[index];
                            currentmulti += 10;
                            Buildings_Multiplier[index] = currentmulti;
                            StartCoroutine(AchievementAnimation(0.001f));
                        }
                    }

                    if (TotalCookies >= 1000 && !isAchieved[0][1])
                    {
                        calcscript.SetAchievement(0, 1);
                        isAchieved[0][1] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 100000 && !isAchieved[0][2])
                    {
                        calcscript.SetAchievement(0, 2);
                        isAchieved[0][2] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 1000000 && !isAchieved[0][3])
                    {
                        calcscript.SetAchievement(0, 3);
                        isAchieved[0][3] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 100000000 && !isAchieved[0][4])
                    {
                        calcscript.SetAchievement(0, 4);
                        isAchieved[0][4] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 100000000000 && !isAchieved[0][5])
                    {
                        calcscript.SetAchievement(0, 5);
                        isAchieved[0][5] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 1000000000000 && !isAchieved[0][6])
                    {
                        calcscript.SetAchievement(0, 6);
                        isAchieved[0][6] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 1000000000000000 && !isAchieved[0][7])
                    {
                        calcscript.SetAchievement(0, 7);
                        isAchieved[0][7] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 10000000000000000 && !isAchieved[0][8])
                    {
                        calcscript.SetAchievement(0, 8);
                        isAchieved[0][8] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                    else if (TotalCookies >= 1000000000000000000 && !isAchieved[0][9])
                    {
                        calcscript.SetAchievement(0, 9);
                        isAchieved[0][9] = true;
                        StartCoroutine(AchievementAnimation(0.001f));
                    }
                }
            }
            TotalCookies += CookiestobeAdded;
            CookieCount.text = calcscript.ConvertToScientific(TotalCookies);
            Cookie_Amount_text.text = calcscript.ConvertToScientific(TotalCookies);
            CookiestobeAdded = 0;
            yield return new WaitForSeconds(1f);
        }
    }

    void CalculateDesiredStuff(){
        Desired_Cookies = (Bomb.GetModuleNames().Count + (long)InitialTime) * 100000;
        foreach(var indicators in Bomb.GetIndicators()){
            switch (indicators){
                case "BOB":
                    Desired_Achievement_type += 1;
                    break;
                case "SND":
                    Desired_Achievement_type += 2;
                    break;
                case "CLR":
                    Desired_Achievement_type += 3;
                    break;
                case "CAR":
                    Desired_Achievement_type += 4;
                    break;
                case "IND":
                    Desired_Achievement_type += 5;
                    break;
                case "FRQ":
                    Desired_Achievement_type += 6;
                    break;
                case "SIG":
                    Desired_Achievement_type += 7;
                    break;
                case "NSA":
                    Desired_Achievement_type += 8;
                    break;
                case "MSA":
                    Desired_Achievement_type += 9;
                    break;
                case "TRN":
                    Desired_Achievement_type += 10;
                    break;
                case "FRK":
                    Desired_Achievement_type += 11;
                    break;
                default:
                    Desired_Achievement_type += 15;
                    break;
            }
        }
        Desired_Achievement_type = Desired_Achievement_type % 14 == 0 ? 1 : Desired_Achievement_type % 14;

        foreach(var ports in Bomb.GetPorts()){
            switch (ports){
                case "DVI":
                    Desired_Achievement_num += 2;
                    break;                   
                case "Parallel":
                    Desired_Achievement_num += 4;
                    break;
                case "PS2":
                    Desired_Achievement_num += 6;
                    break;
                case "RJ45":
                    Desired_Achievement_num += 8;
                    break;
                case "Serial":
                    Desired_Achievement_num += 10;
                    break;
                case "StereoRCA":
                    Desired_Achievement_num += 12;
                    break;
                default:
                    Desired_Achievement_num += 20;
                    break;
            }
        }

        Desired_Achievement_num = Desired_Achievement_num % calcscript.Achievements[Desired_Achievement_type-1].Length == 0 ? 1 : Desired_Achievement_num % calcscript.Achievements[Desired_Achievement_type-1].Length;
        Debug.LogFormat("[Cookie Clicker #{0}] Welcome to Cookie Clicker, Today you are required to get this type of achievement {1} with this name {2} and this much cookies {3}!!!",_modID,Desired_Achievement_type == 1 ? "Total Cookies":Buildings_Names[Desired_Achievement_type-2],calcscript.GetAchievement(Desired_Achievement_type-1,Desired_Achievement_num-1),calcscript.ConvertToScientific(Desired_Cookies));
    }

    IEnumerator HoldButtonTime(){
        yield return new WaitForSeconds(1f);
        holding = true;
        yield break;
    }

    IEnumerator AchievementAnimation(float delay){
        IsAnimating = true;
        audio.PlaySoundAtTransform("upgrade", this.transform);
        Transform baseTrans = Achi_Template.transform;
        Vector3 Ori = baseTrans.localPosition;
        Vector3 pos = new Vector3(0f, 0f, -0.026f);
        while (true)
        {
            float x = (float)Math.Round(baseTrans.localPosition.x, 4);
            float y = (float)Math.Round(baseTrans.localPosition.y, 4);
            float z = (float)Math.Round(baseTrans.localPosition.z, 4);
            if ((z <= pos.z + 0.0003f && z >= pos.z - 0.0003f)) { break; }
            if (z < pos.z) { baseTrans.localPosition = new Vector3(x, y, z + 0.00025f); }
            else if (z > pos.z) { baseTrans.localPosition = new Vector3(x, y, z - 0.00025f); }
            yield return new WaitForSeconds(delay);
        }
        baseTrans.localPosition = Ori;
        Achi_template.GetComponent<Renderer>().enabled = false;
        Achi_text.GetComponent<Renderer>().enabled = false;
        Achi_Sprite.enabled = false;
        IsAnimating = false;
        yield break;
    }
}
