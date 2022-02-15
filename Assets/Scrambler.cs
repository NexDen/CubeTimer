using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Scrambler : MonoBehaviour
{
    string scramble;
    [SerializeField] Text _scrambleText;
    string MOVE_U = "U";
    string MOVE_D = "D";
    string MOVE_L = "L";
    string MOVE_R = "R";
    string MOVE_F = "F";
    string MOVE_B = "B";
    string APPEND_PRIME = "'";
    string APPEND_DOUBLE = "2";
    string APPEND_NONE = "";
    Random RNG = new Random();
    
    List<string> scramble_list;
    string[] moves;
    string[] appends;
    string[] lateral;
    string[] front_back;
    string[] up_down;

    void Start()
    {
        moves = new string[6] { MOVE_U, MOVE_D, MOVE_L, MOVE_R, MOVE_F, MOVE_B };
        appends = new string[3] { APPEND_PRIME, APPEND_DOUBLE, APPEND_NONE };
        lateral = new string[2] { MOVE_L, MOVE_R };
        front_back = new string[2] { MOVE_F, MOVE_B };
        up_down = new string[2] { MOVE_U, MOVE_D };
        scramble = "";
        scramble_list = new List<string>();
        _scrambleText.text = "";
        GenerateNewScramble();
    }

    Move GenerateSingleMove()
    {
        string move = moves[RNG.Next(0, moves.Length)];
        string append = appends[RNG.Next(0, appends.Length)];
        Move m = new Move(move, append);
        return m;
    }

    public void GenerateNewScramble() 
    {
        scramble_list.Clear();
        Move last_move = new Move(MOVE_B, APPEND_NONE); //default move
        int scramble_length = RNG.Next(18, 22);
        for (int i = 0; i < scramble_length; i++)
        {
            Move move = GenerateSingleMove();
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
