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
    string[] moves;
    string[] appends;
    string[] lateral;
    string[] front_back;
    string[] up_down;
    #endregion
    [SerializeField] Visualizer visualizer;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "EskiKarıştırmalar") return;
        moves = new string[6] { "U", "D", "L", "R", "F", "B" };
        appends = new string[3] { "'", "2", "" };
        lateral = new string[2] { "L", "R" };
        front_back = new string[2] { "F", "B" };
        up_down = new string[2] { "U", "D" };

        scramble = "";
        scramble_list = new List<string>();
        _scrambleText.text = "";
        GenerateNewScramble();
    }

    Move GenerateSingleMove(int size)
    {
        string move = moves[RNG.Next(0, moves.Length)];
        string append = appends[RNG.Next(0, appends.Length)];
        return new Move(move, append); 
    }

    public void YeniKarıştırma(){
        _prevScrambles.RemoveAt(_prevScrambles.Count - 1);
        GenerateNewScramble();
    }

    public void GenerateNewScramble() 
    {
        Move last_move;
        int scramble_length;
        
        scramble_list.Clear();
        last_move = new Move("B", ""); //default move
        scramble_length = RNG.Next(18, 25);
        for (int i = 0; i < scramble_length; i++)
        {
            Move move = GenerateSingleMove(cube_size);
            if (last_move.move == move.move) { i--; continue; }
            if (lateral.Contains(move.move) && lateral.Contains(last_move.move)) { i--; continue; }
            if (front_back.Contains(move.move) && front_back.Contains(last_move.move)) { i--; continue; }
            if (up_down.Contains(move.move) && up_down.Contains(last_move.move)) { i--; continue; }

            last_move = move;
            scramble_list.Add(move.ToString());
            //scramble_list.Add(move.move + move.append);
        }
        scramble = string.Join(" ", scramble_list.ToArray());
        _scrambleText.text = scramble;
        _prevScrambles.Add(scramble);
        visualizer.InterpretAlgorithm(scramble);
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
