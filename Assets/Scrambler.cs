using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        scramble = "";
        scramble_list = new List<string>();
        _scrambleText.text = "";
        GenerateNewScramble();
    }
    public void GenerateNewScramble() 
    {
        scramble_list.Clear();
        string[] moves = new string[] { MOVE_U, MOVE_D, MOVE_L, MOVE_R, MOVE_F, MOVE_B };
        string[] appends = new string[] { APPEND_PRIME, APPEND_DOUBLE, APPEND_NONE };
        scramble = "";
        Move prev_move = new Move(MOVE_D, APPEND_PRIME); // default move
        for (int i = 0; i < 25; i++)
        {
            Move move = new Move(
                moves[RNG.Next(0, moves.Length)],
                appends[RNG.Next(0, appends.Length)]
            );
            if (prev_move.move == move.move) continue; // aynı hareketlerin ard arda gelmesini engelleme
            scramble_list.Add(move.ToString());
            prev_move = move;
        }
        scramble = string.Join(" ", scramble_list);
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
