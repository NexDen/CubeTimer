using Random = System.Random;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Scrambler : MonoBehaviour
{
    string scramble;
    [SerializeField] Text _scrambleText;
    [SerializeField] public List<string> _prevScrambles = new List<string>();
    [SerializeField] Text _prevScramblesText;
    Random RNG = new Random();
    
    public int cube_size = 3;

    public List<string> scramble_list;
    #region Arrayler
    string[] moves3;
    string[] appends3;
    string[] lateral3;
    string[] front_back3;
    string[] up_down3;

    string[] moves4;
    string[] appends4;
    string[] lateral4;
    string[] front_back4;
    string[] up_down4;
    string[] lateral_wide4;
    string[] front_back_wide4;
    string[] up_down_wide4;
    #endregion
    [SerializeField] Visualizer visualizer;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "EskiKarıştırmalar") return;
        moves3 = new string[6] { "U", "D", "L", "R", "F", "B" };
        appends3 = new string[3] { "'", "2", "" };
        lateral3 = new string[2] { "L", "R" };
        front_back3 = new string[2] { "F", "B" };
        up_down3 = new string[2] { "U", "D" };

        moves4 = new string[] { "U", "D", "L", "R", "F", "B", "Uw", "Dw", "Lw", "Rw", "Fw", "Bw"};
        appends4 = new string[] { "'", "2", "" };
        lateral4 = new string[] { "L", "R" };
        lateral_wide4 = new string[] { "Lw", "Rw" };
        front_back4 = new string[] { "F", "B" };
        front_back_wide4 = new string[] { "Fw", "Bw" };
        up_down4 = new string[] { "U", "D" };
        up_down_wide4 = new string[] { "Uw", "Dw" };


        scramble = "";
        scramble_list = new List<string>();
        _scrambleText.text = "";
        GenerateNewScramble(cube_size);
    }

    Move GenerateSingleMove(int size)
    {
        string move = moves3[RNG.Next(0, moves3.Length)];
        string append = appends3[RNG.Next(0, appends3.Length)];
        return new Move(move, append); 
    }

    public void YeniKarıştırma(){
        _prevScrambles.RemoveAt(_prevScrambles.Count - 1);
        GenerateNewScramble(cube_size);
    }

    public void GenerateNewScramble(int size) 
    {
        Move last_move;
        int scramble_length;
        
        scramble_list.Clear();
        last_move = new Move("B", ""); //default move
        scramble_length = RNG.Next(18, 19);
        for (int i = 0; i < scramble_length; i++)
        {
            Move move = GenerateSingleMove(cube_size);
            if (last_move.move == move.move) { i--; continue; }
            if (lateral3.Contains(move.move) && lateral3.Contains(last_move.move)) { i--; continue; }
            if (front_back3.Contains(move.move) && front_back3.Contains(last_move.move)) { i--; continue; }
            if (up_down3.Contains(move.move) && up_down3.Contains(last_move.move)) { i--; continue; }
            if (lateral_wide4.Contains(move.move) && lateral_wide4.Contains(last_move.move)) { i--; continue; }
            if (front_back_wide4.Contains(move.move) && front_back_wide4.Contains(last_move.move)) { i--; continue; }
            if (up_down_wide4.Contains(move.move) && up_down_wide4.Contains(last_move.move)) { i--; continue; }

            last_move = move;
            scramble_list.Add(move.ToString());
            //scramble_list.Add(move.move + move.append);
        }
        scramble = string.Join(" ", scramble_list.ToArray());
        _scrambleText.text = scramble;
        _prevScrambles.Add(scramble);
        if (cube_size == 3) visualizer.InterpretAlgorithm(scramble);
        else visualizer.setAllZero();
        }

    List<string> SonElemanlarıAl(List<string> liste,int index){
        List<string> yeniListe = new List<string>();
        for (int i = Math.Max(0,liste.Count() - index); i < liste.Count(); i++){
            yeniListe.Add(liste[i]);
        }
        return yeniListe;
    }

    public void ReturnPrevScrambles(){
        _prevScramblesText.text = string.Join("\n", SonElemanlarıAl(_prevScrambles, 16).ToArray());
    }
}


public class Move
{
    public string move;
    public string append;
    public Move(string move, string append)
    {
        this.move = move;
        this.append = append;
    }
    public override string ToString()
    {
        return this.move + this.append;
    }
}
