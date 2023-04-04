using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeHeld;
    public float timeCounter;

    public bool holdForStart;
    public bool ableToStart;
    public bool solveActive;
    public bool plus2pressed;

    public Toggle showTimerToggle;
    public Text timerText;
    public Text timeList;
    public Text plus2Indicator;

    [SerializeField] public List<Solve> solves;

    public Scrambler scrambleHandler;
    public UIHandler UIhandler;

    public float PB;
    public float AVG5;
    public float AVG10;
    public float AVG;

    public Text PBText;
    public Text AVG5Text;
    public Text AVG10Text;
    public Text AVGText;

    // Start is called before the first frame update
    void Start()
    {
        holdForStart = false;
        timeHeld = 0;
        solves = new List<Solve>();
    }

    // Update is called once per frame
    void Update()
    {
        if (holdForStart){
            timerText.color = Color.red;
            timeHeld += Time.deltaTime;
            if (timeHeld >= 0.55f){
                timerText.color = Color.green;
                ableToStart = true;
            }
        }
        if (solveActive){
            timeCounter += Time.deltaTime;
            if (showTimerToggle.isOn){
                timerText.text = ConvertTimeToString(timeCounter);
            }
            else {
                timerText.text = "-";
            }
        }
        if (Input.GetMouseButtonDown(0)){
            Press();
        }
        if (Input.GetMouseButtonUp(0)){
            Release();
        }
    }

    void Press(){
        holdForStart = true;
        plus2Indicator.gameObject.SetActive(false);
        if (solveActive){
            StopTimer();
        }
    }

    void Release(){
        holdForStart = false;
        timeHeld = 0;
        timerText.color = Color.white;
        if (ableToStart){
            StartTimer();
        }
    }

    void StartTimer(){
        solveActive = true;
        ableToStart = false;
        plus2pressed = false;
        timeCounter = 0;
        UIhandler.HideScreen();
    }
    
    void StopTimer(){
        solveActive = false;

        timerText.text = ConvertTimeToString(timeCounter);

        if (timeCounter < PB){
            // PB Animation goes here
        }

        string scramble = scrambleHandler.scramble;
        solves.Add(new Solve(timeCounter, scramble));
        scrambleHandler.GenerateNewScramble();

        HandleTimeList();

        UIhandler.ShowScreen();
    }

    void HandleTimeList(){
        timeList.text = "";
        if (solves.Count() == 0){
            PBText.text = "-";
            AVGText.text = "-";
            return;
        }
        Solve best_solve = new Solve(2147483647, "");
        foreach(Solve solve in solves){
            if (solve.solveTime < best_solve.solveTime){
                best_solve.isBestSolve = false;
                best_solve = solve;
            }
        }

        best_solve.isBestSolve = true;
        PB = best_solve.solveTime;
        PBText.text = ConvertTimeToString(PB);

        int solve_count = Math.Max(0, solves.Count() - 18) + 1;
        foreach(Solve solve in solves.Skip(solve_count - 1)){
            if (solve.isBestSolve){
                timeList.text += $"<color=#00ff00>({solve_count}) {ConvertTimeToString(solve.solveTime)}</color>\n";
            }
            else if (solve.isBelowThreshold){
                timeList.text += $"<color=#ff0000>({solve_count}) {ConvertTimeToString(solve.solveTime)}</color>\n";
            }
            else {
                timeList.text += $"({solve_count}) {ConvertTimeToString(solve.solveTime)}\n";
            }
            solve_count++;
        }
        CalculateAverages();
    }

    public void DNF(){
        Solve last_solve = solves.LastOrDefault();
        solves.Remove(last_solve);
        timerText.text = "DNF";
        HandleTimeList();
    }

    public void Plus2(){
        if (plus2pressed){return;}
        plus2pressed = true;
        Solve last_solve = solves.LastOrDefault();
        solves.Remove(last_solve);
        
        Solve new_solve = new Solve(last_solve.solveTime + 2, last_solve.scramble);
        solves.Add(new_solve);

        timerText.text = ConvertTimeToString(new_solve.solveTime);
        
        HandleTimeList();

        plus2Indicator.gameObject.SetActive(true);
    }

    void CalculateAverages() 
    {
        float sum = 0;
        float avg5_sum = 0;
        float avg10_sum = 0;

        for (int i = 0; i < solves.Count(); i++) {
            Solve solve = solves[i];
            sum += solve.solveTime;
            
            if (i >= solves.Count() - 5) {
                avg5_sum += solve.solveTime;
            }

            if (i >= solves.Count() - 10) {
                avg10_sum += solve.solveTime;
            }
        }

        AVG = sum / solves.Count();
        AVGText.text = ConvertTimeToString(AVG);

        if (solves.Count() >= 5) {
            AVG5 = avg5_sum / 5;
            AVG5Text.text = ConvertTimeToString(AVG5);
        }

        if (solves.Count() >= 10) {
            AVG10 = avg10_sum / 10;
            AVG10Text.text = ConvertTimeToString(AVG10);
        }
    }
    public static string ConvertTimeToString(float time) //ChatGPT!
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)((time - (int)time) * 100);

        string format;

        if (minutes == 0)
        {
            format = "{0}.{1:00}";
            return string.Format(format, seconds, milliseconds);
        }
        else
        {
            format = "{0}:{1:00}.{2:00}";
            return string.Format(format, minutes, seconds, milliseconds);
        }
    }
}
[System.Serializable]
public class Solve
{
    [SerializeField] public float solveTime;
    [NonSerialized] public string scramble;
    [NonSerialized] public bool isBestSolve;
    [NonSerialized] public bool isBelowThreshold;
    [NonSerialized] public static float threshold = 20;
    public Solve(float solveTime, string scramble)
    {
        this.solveTime = solveTime;
        this.scramble = scramble;
        this.isBestSolve = false;
        if (this.solveTime < threshold){
            isBelowThreshold = true;
        }
    }
    public override string ToString(){
        return Timer.ConvertTimeToString(this.solveTime) + " " + scramble;
    }
}
