using Random = System.Random;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Scrambler : MonoBehaviour
{
    [SerializeField] public string scramble;
    [SerializeField] Text _scrambleText;
    [SerializeField] Button scrambleSizeButton;

    Random RNG = new Random();
    
    public int cube_size;

    public List<string> scramble_list;
    #region Arrayler
    string[] moves;
    string[] appends;
    string[] lateral;
    string[] front_back;
    string[] up_down;
    string[] wide;
    string[] slice_moves;
    #endregion
    [SerializeField] Visualizer visualizer;

    void Start()
    {
        scramble_list = new List<string>();
        _scrambleText.text = "";
        scramble = GenerateNewScramble();
        visualizer.InterpretAlgorithm(scramble, cube_size);
    }

    Move GenerateSingleMove(int size)
    {
        string move;
        string append;
        string ifwide;
        bool ifslice;
        switch(size){
            case 2:
                moves = new string[3] {"R", "F", "U"};
                appends = new string[3] { "'", "2", "" };
                move = moves[RNG.Next(0, moves.Length)];
                append = appends[RNG.Next(0, appends.Length)];
                return new Move(move, append);
            case 3:
                moves = new string[6] { "U", "D", "L", "R", "F", "B" };
                appends = new string[3] { "'", "2", "" };
                
                move = moves[RNG.Next(0, moves.Length)];
                append = appends[RNG.Next(0, appends.Length)];
                return new Move(move, append);
            case 4:
                moves = new string[6] { "U", "D", "L", "R", "F", "B" };
                appends = new string[3] { "'", "2", "" };
                wide = new string[4] { "w", "", "", ""};
                
                move = moves[RNG.Next(0, moves.Length)];
                append = appends[RNG.Next(0, appends.Length)];
                ifwide = wide[RNG.Next(0, wide.Length)];
                return new Move(move+ifwide, append);
            case 5:
                moves = new string[6] { "U", "D", "L", "R", "F", "B" };
                appends = new string[3] { "'", "2", "" };
                wide = new string[4] { "w", "", "",""};
                slice_moves = new string[6] {"r", "l", "f", "b", "u", "d"};
                ifslice = RNG.Next(0, 2) == 1;
                if (ifslice)
                {
                    move = slice_moves[RNG.Next(0, slice_moves.Length)];
                    append = appends[RNG.Next(0, appends.Length)];
                    return new Move(move, append);
                }
                else
                {
                    move = moves[RNG.Next(0, moves.Length)];
                    append = appends[RNG.Next(0, appends.Length)];
                    ifwide = wide[RNG.Next(0, wide.Length)];
                    return new Move(move + ifwide, append);
                }
            default:
                return new Move("B", "");
        }
    }

    public void ScrambleButton()
    {
        scramble = GenerateNewScramble();
        visualizer.InterpretAlgorithm(scramble, cube_size);
    }

    public string GenerateNewScramble() 
    {
        Move last_move;
        int scramble_length;

        lateral = new string[4] { "L", "R", "l", "r"};
        front_back = new string[4] { "F", "B", "f", "b"};
        up_down = new string[4] { "U", "D", "u", "d"};

        scramble_list.Clear();
        last_move = new Move("B", ""); //default move
        scramble_length = RNG.Next(18, 25);
        switch (cube_size){
            case 2:
                scramble_length = RNG.Next(8, 12);
                break;
            case 3:
                scramble_length = RNG.Next(18, 25);
                break;
            case 4:
                scramble_length = RNG.Next(3,6);
                scramble_length = RNG.Next(35, 45);
                break;
            default:
                scramble_length = RNG.Next(18, 25);
                break;
        }
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
        visualizer.InterpretAlgorithm(scramble, cube_size);
        return scramble;
        // return "D";
        // return "D Bw2";
        // return "D Bw2 B";
        // return "D Bw2 B L2";

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
