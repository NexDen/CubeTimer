using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using System.Linq;
using System.ComponentModel;

/*
BIRAKTIĞIN YER: 4x4 İÇİN D, B', Lw2 VEYA F' HAREKETİNDE HATA VAR.
*/


public class Visualizer : MonoBehaviour
{
    public Cube cube;

    [SerializeField] public Scrambler scrambler;

    public Button cubeSizeButton;
    public int cube_size;

    [SerializeField] public GameObject twobytwo;
    [SerializeField] public GameObject threebythree;
    [SerializeField] public GameObject fourbyfour;


    [SerializeField] public List<GameObject> right2;
    [SerializeField] public List<GameObject> left2;
    [SerializeField] public List<GameObject> up2;
    [SerializeField] public List<GameObject> down2;
    [SerializeField] public List<GameObject> front2;
    [SerializeField] public List<GameObject> back2;

    [SerializeField] public List<GameObject> right3;
    [SerializeField] public List<GameObject> left3;
    [SerializeField] public List<GameObject> up3;
    [SerializeField] public List<GameObject> down3;
    [SerializeField] public List<GameObject> front3;
    [SerializeField] public List<GameObject> back3;

    [SerializeField] public List<GameObject> right4;
    [SerializeField] public List<GameObject> left4;
    [SerializeField] public List<GameObject> up4;
    [SerializeField] public List<GameObject> down4;
    [SerializeField] public List<GameObject> front4;
    [SerializeField] public List<GameObject> back4;

    private void Start() {
        
    }

    public void cubeSizeChange() {
        cube_size++;
        if (cube_size > 4) { cube_size = 2;}
        switch (cube_size){
            case 2:
                twobytwo.SetActive(true);
                threebythree.SetActive(false);
                fourbyfour.SetActive(false);
                break;
            case 3:
                twobytwo.SetActive(false);
                threebythree.SetActive(true);
                fourbyfour.SetActive(false);
                break;
            case 4:
                twobytwo.SetActive(false);
                threebythree.SetActive(false);
                fourbyfour.SetActive(true);
                break;
        }
        cubeSizeButton.GetComponentInChildren<Text>().text = cube_size.ToString();
        scrambler.cube_size = cube_size;
        string scramble = scrambler.GenerateNewScramble();
        InterpretAlgorithm(scramble, cube_size);
        print(cube);
    }

    public Dictionary<int,Color> colors = new Dictionary<int, Color>()
    {
        /*
        0 = Siyah (Boş) <- bunu görürsen bişeyler yanlış gitti
        1 = Beyaz (U)
        2 = Yeşil (F)
        3 = Turuncu (L)
        4 = Mavi (B)
        5 = Kırmızı (R)
        6 = Sarı (D)
        */
        {0, Color.black},
        {1, Color.white},
        {2, Color.green},
        {3, new Color32(252,169,3,255)},
        {4, Color.blue},
        {5, new Color32(194,2,2,255)},
        {6, new Color32(223,252,3,255)}
    };
    public void Visualize(){
        int index = 0;
        if (cube_size == 2){
            foreach (GameObject g in right2){
                g.GetComponent<Renderer>().material.color = colors[cube.R[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in left2){
                g.GetComponent<Renderer>().material.color = colors[cube.L[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in up2){
                g.GetComponent<Renderer>().material.color = colors[cube.U[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in down2){
                g.GetComponent<Renderer>().material.color = colors[cube.D[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in front2){
                g.GetComponent<Renderer>().material.color = colors[cube.F[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in back2){
                g.GetComponent<Renderer>().material.color = colors[cube.B[index]];
                index++;
            }
        }
        else if (cube_size == 3){
            foreach (GameObject g in right3){
                g.GetComponent<Renderer>().material.color = colors[cube.R[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in left3){
                g.GetComponent<Renderer>().material.color = colors[cube.L[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in up3){
                g.GetComponent<Renderer>().material.color = colors[cube.U[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in down3){
                g.GetComponent<Renderer>().material.color = colors[cube.D[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in front3){
                g.GetComponent<Renderer>().material.color = colors[cube.F[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in back3){
                g.GetComponent<Renderer>().material.color = colors[cube.B[index]];
                index++;
            }
        }
        else if (cube_size == 4){
            foreach (GameObject g in right4){
                g.GetComponent<Renderer>().material.color = colors[cube.R[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in left4){
                g.GetComponent<Renderer>().material.color = colors[cube.L[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in up4){
                g.GetComponent<Renderer>().material.color = colors[cube.U[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in down4){
                g.GetComponent<Renderer>().material.color = colors[cube.D[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in front4){
                g.GetComponent<Renderer>().material.color = colors[cube.F[index]];
                index++;
            }
            index = 0;
            foreach (GameObject g in back4){
                g.GetComponent<Renderer>().material.color = colors[cube.B[index]];
                index++;
            }
        }
    }
    public void InterpretAlgorithm(string MoveSet, int cube_size){
        cube = new Cube(cube_size);
        foreach (string c in MoveSet.Split(' ')){
            switch (c){
                case "U":
                    cube.MOVE_U();
                    break;
                case "U\'":
                    cube.MOVE_U_PRIME();
                    break;
                case "U2":
                    cube.MOVE_U();
                    cube.MOVE_U();
                    break;
                case "D":
                    cube.MOVE_D();
                    break;
                case "D\'":
                    cube.MOVE_D_PRIME();
                    break;
                case "D2":
                    cube.MOVE_D();
                    cube.MOVE_D();
                    break;
                case "F":
                    cube.MOVE_F();
                    break;
                case "F\'":
                    cube.MOVE_F_PRIME();
                    break;
                case "F2":
                    cube.MOVE_F();
                    cube.MOVE_F();
                    break;
                case "B":
                    cube.MOVE_B();
                    break;
                case "B\'":
                    cube.MOVE_B_PRIME();
                    break;
                case "B2":
                    cube.MOVE_B();
                    cube.MOVE_B();
                    break;
                case "R":
                    cube.MOVE_R();
                    break;
                case "R\'":
                    cube.MOVE_R_PRIME();
                    break;
                case "R2":
                    cube.MOVE_R();
                    cube.MOVE_R();
                    break;
                case "L":
                    cube.MOVE_L();
                    break;
                case "L\'":
                    cube.MOVE_L_PRIME();
                    break;
                case "L2":
                    cube.MOVE_L();
                    cube.MOVE_L();
                    break;
                case "Uw":
                    cube.MOVE_U_WIDE();
                    break;
                case "Uw\'":    
                    cube.MOVE_U_WIDE_PRIME();
                    break;
                case "Uw2":
                    cube.MOVE_U_WIDE();
                    cube.MOVE_U_WIDE();
                    break;
                case "Dw":
                    cube.MOVE_D_WIDE();
                    break;
                case "Dw\'":
                    cube.MOVE_D_WIDE_PRIME();
                    break;
                case "Dw2":
                    cube.MOVE_D_WIDE();
                    cube.MOVE_D_WIDE();
                    break;
                case "Fw":
                    cube.MOVE_F_WIDE();
                    break;
                case "Fw\'":    
                    cube.MOVE_F_WIDE_PRIME();
                    break;
                case "Fw2": 
                    cube.MOVE_F_WIDE();
                    cube.MOVE_F_WIDE();
                    break;
                case "Bw":
                    cube.MOVE_B_WIDE();
                    break;
                case "Bw\'":
                    cube.MOVE_B_WIDE_PRIME();
                    break;
                case "Bw2":
                    cube.MOVE_B_WIDE();
                    cube.MOVE_B_WIDE();
                    break;
                case "Rw":
                    cube.MOVE_R_WIDE();
                    break;
                case "Rw\'":
                    cube.MOVE_R_WIDE_PRIME();
                    break;
                case "Rw2":
                    cube.MOVE_R_WIDE();
                    cube.MOVE_R_WIDE();
                    break;
                case "Lw":
                    cube.MOVE_L_WIDE();
                    break;
                case "Lw\'":
                    cube.MOVE_L_WIDE_PRIME();
                    break;
                case "Lw2":
                    cube.MOVE_L_WIDE();
                    cube.MOVE_L_WIDE();
                    break;
                default:
                    break;
            }
        }
        Visualize();
    }

}

public class Cube
{
    public int cube_size;

    public int[] U;
    public int[] F;
    public int[] L;
    public int[] B;
    public int[] R;
    public int[] D;

    public Cube(int size)
    {
        cube_size = size;
        U = Enumerable.Repeat(1, cube_size*cube_size).ToArray();
        F = Enumerable.Repeat(2, cube_size*cube_size).ToArray();
        L = Enumerable.Repeat(3, cube_size*cube_size).ToArray();
        B = Enumerable.Repeat(4, cube_size*cube_size).ToArray();
        R = Enumerable.Repeat(5, cube_size*cube_size).ToArray();
        D = Enumerable.Repeat(6, cube_size*cube_size).ToArray();
    }

    #region Moves
    public void MOVE_U(){
        switch (cube_size){
            case 2:
                int[] temp2 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp2[0] = F[0];
                temp2[1] = F[1];

                F[0] = R[0];
                F[1] = R[1];

                R[0] = B[0];
                R[1] = B[1];

                B[0] = L[0];
                B[1] = L[1];

                L[0] = temp2[0];
                L[1] = temp2[1];

                Array.Copy(U, temp2, 4);
                U[0] = temp2[2];
                U[1] = temp2[0];
                U[2] = temp2[3];
                U[3] = temp2[1];
                break;
            case 3:
                int[] temp3 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp3[0] = F[0];
                temp3[1] = F[1];
                temp3[2] = F[2];

                F[0] = R[0];
                F[1] = R[1];
                F[2] = R[2];

                R[0] = B[0];
                R[1] = B[1];
                R[2] = B[2];

                B[0] = L[0];
                B[1] = L[1];
                B[2] = L[2];

                L[0] = temp3[0];
                L[1] = temp3[1];
                L[2] = temp3[2];

                Array.Copy(U, temp3, 9);
                U[0] = temp3[6];
                U[1] = temp3[3];
                U[2] = temp3[0];
                U[3] = temp3[7];
                U[4] = temp3[4];
                U[5] = temp3[1];
                U[6] = temp3[8];
                U[7] = temp3[5];
                U[8] = temp3[2];
                break;
            case 4:
                int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp4[0] = F[0];
                temp4[1] = F[1];
                temp4[2] = F[2];
                temp4[3] = F[3];

                F[0] = R[0];
                F[1] = R[1];
                F[2] = R[2];
                F[3] = R[3];

                R[0] = B[0];
                R[1] = B[1];
                R[2] = B[2];
                R[3] = B[3];

                B[0] = L[0];
                B[1] = L[1];
                B[2] = L[2];
                B[3] = L[3];

                L[0] = temp4[0];
                L[1] = temp4[1];
                L[2] = temp4[2];
                L[3] = temp4[3];

                Array.Copy(U, temp4, 16);
                U[0] = temp4[12];
                U[1] = temp4[8];
                U[2] = temp4[4];
                U[3] = temp4[0];
                U[4] = temp4[13];
                U[5] = temp4[9];
                U[6] = temp4[5];
                U[7] = temp4[1];
                U[8] = temp4[14];
                U[9] = temp4[10];
                U[10] = temp4[6];
                U[11] = temp4[2];
                U[12] = temp4[15];
                U[13] = temp4[11];
                U[14] = temp4[7];
                U[15] = temp4[3];
                break;
        }
    }
    public void MOVE_U_PRIME(){
        MOVE_U();
        MOVE_U();
        MOVE_U();
    }

    public void MOVE_U_WIDE(){
        int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

        temp4[0] = F[0];
        temp4[1] = F[1];
        temp4[2] = F[2];
        temp4[3] = F[3];
        temp4[4] = F[4];
        temp4[5] = F[5];
        temp4[6] = F[6];
        temp4[7] = F[7];


        F[0] = R[0];
        F[1] = R[1];
        F[2] = R[2];
        F[3] = R[3];
        F[4] = R[4];
        F[5] = R[5];
        F[6] = R[6];
        F[7] = R[7];


        R[0] = B[0];
        R[1] = B[1];
        R[2] = B[2];
        R[3] = B[3];
        R[4] = B[4];
        R[5] = B[5];
        R[6] = B[6];
        R[7] = B[7];



        B[0] = L[0];
        B[1] = L[1];
        B[2] = L[2];
        B[3] = L[3];
        B[4] = L[4];
        B[5] = L[5];
        B[6] = L[6];
        B[7] = L[7];

        L[0] = temp4[0];
        L[1] = temp4[1];
        L[2] = temp4[2];
        L[3] = temp4[3];
        L[4] = temp4[4];
        L[5] = temp4[5];
        L[6] = temp4[6];
        L[7] = temp4[7];

        Array.Copy(U, temp4, 16);
        U[0] = temp4[12];
        U[1] = temp4[8];
        U[2] = temp4[4];
        U[3] = temp4[0];
        U[4] = temp4[13];
        U[5] = temp4[9];
        U[6] = temp4[5];
        U[7] = temp4[1];
        U[8] = temp4[14];
        U[9] = temp4[10];
        U[10] = temp4[6];
        U[11] = temp4[2];
        U[12] = temp4[15];
        U[13] = temp4[11];
        U[14] = temp4[7];
        U[15] = temp4[3];
    }
    public void MOVE_U_WIDE_PRIME(){
        MOVE_U_WIDE();
        MOVE_U_WIDE();
        MOVE_U_WIDE();
    }

    public void MOVE_D(){
        switch (cube_size){
            case 3:
                int[] temp = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp[6] = F[6];
                temp[7] = F[7];
                temp[8] = F[8];

                F[6] = L[6];
                F[7] = L[7];
                F[8] = L[8];

                L[6] = B[6];
                L[7] = B[7];
                L[8] = B[8];

                B[6] = R[6];
                B[7] = R[7];
                B[8] = R[8];

                R[6] = temp[6];
                R[7] = temp[7];
                R[8] = temp[8];

                Array.Copy(D, temp, 9);
                D[0] = temp[6];
                D[1] = temp[3];
                D[2] = temp[0];
                D[3] = temp[7];
                D[4] = temp[4];
                D[5] = temp[1];
                D[6] = temp[8];
                D[7] = temp[5];
                D[8] = temp[2];
                break;
            case 4:
                int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp4[0] = F[12];
                temp4[1] = F[13];
                temp4[2] = F[14];
                temp4[3] = F[15];

                F[12] = L[12];
                F[13] = L[13];
                F[14] = L[14];
                F[15] = L[15];

                L[12] = B[12];
                L[13] = B[13];
                L[14] = B[14];
                L[15] = B[15];

                B[12] = R[12];
                B[13] = R[13];
                B[14] = R[14];
                B[15] = R[15];

                R[12] = temp4[0];
                R[13] = temp4[1];
                R[14] = temp4[2];
                R[15] = temp4[3];

                Array.Copy(D, temp4, 16);
                D[0] = temp4[12];
                D[1] = temp4[8];
                D[2] = temp4[4];
                D[3] = temp4[0];
                D[4] = temp4[13];
                D[5] = temp4[9];
                D[6] = temp4[5];
                D[7] = temp4[1];
                D[8] = temp4[14];
                D[9] = temp4[10];
                D[10] = temp4[6];
                D[11] = temp4[2];
                D[12] = temp4[15];
                D[13] = temp4[11];
                D[14] = temp4[7];
                D[15] = temp4[3];
                break;
        }
    }
    public void MOVE_D_PRIME(){
        MOVE_D();
        MOVE_D();
        MOVE_D();
    }

    public void MOVE_D_WIDE(){
        int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

        temp4[8] = F[8];
        temp4[9] = F[9];
        temp4[10] = F[10];
        temp4[11] = F[11];
        temp4[12] = F[12];
        temp4[13] = F[13];
        temp4[14] = F[14];
        temp4[15] = F[15];

        F[8] = L[8];
        F[9] = L[9];
        F[10] = L[10];
        F[11] = L[11];
        F[12] = L[12];
        F[13] = L[13];
        F[14] = L[14];
        F[15] = L[15];

        L[8] = B[8];
        L[9] = B[9];
        L[10] = B[10];
        L[11] = B[11];
        L[12] = B[12];
        L[13] = B[13];
        L[14] = B[14];
        L[15] = B[15];

        B[8] = R[8];
        B[9] = R[9];
        B[10] = R[10];
        B[11] = R[11];
        B[12] = R[12];
        B[13] = R[13];
        B[14] = R[14];
        B[15] = R[15];

        R[8] = temp4[8];
        R[9] = temp4[9];
        R[10] = temp4[10];
        R[11] = temp4[11];
        R[12] = temp4[12];
        R[13] = temp4[13];
        R[14] = temp4[14];
        R[15] = temp4[15];

        Array.Copy(D, temp4, 16);
        D[0] = temp4[12];
        D[1] = temp4[8];
        D[2] = temp4[4];
        D[3] = temp4[0];
        D[4] = temp4[13];
        D[5] = temp4[9];
        D[6] = temp4[5];
        D[7] = temp4[1];
        D[8] = temp4[14];
        D[9] = temp4[10];
        D[10] = temp4[6];
        D[11] = temp4[2];
        D[12] = temp4[15];
        D[13] = temp4[11];
        D[14] = temp4[7];
        D[15] = temp4[3];
    }
    public void MOVE_D_WIDE_PRIME(){
        MOVE_D_WIDE();
        MOVE_D_WIDE();
        MOVE_D_WIDE();
    }
    public void MOVE_F(){
        switch (cube_size){
            case 2:
                int[] temp2 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp2[2] = U[2];
                temp2[3] = U[3];

                U[2] = L[3];
                U[3] = L[1];

                L[1] = D[0];
                L[3] = D[1];

                D[0] = R[2];
                D[1] = R[0];

                R[0] = temp2[2];
                R[2] = temp2[3];

                Array.Copy(F, temp2, 4);
                F[0] = temp2[2];
                F[1] = temp2[0];
                F[2] = temp2[3];
                F[3] = temp2[1];
                break;
            case 3:
                int[] temp = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp[6] = U[6];
                temp[7] = U[7];
                temp[8] = U[8];

                U[6] = L[8];
                U[7] = L[5];
                U[8] = L[2];

                L[2] = D[0];
                L[5] = D[1];
                L[8] = D[2];

                D[0] = R[6];
                D[1] = R[3];
                D[2] = R[0];

                R[0] = temp[6];
                R[3] = temp[7];
                R[6] = temp[8];

                Array.Copy(F, temp, 9);
                F[0] = temp[6];
                F[1] = temp[3];
                F[2] = temp[0];
                F[3] = temp[7];
                F[4] = temp[4];
                F[5] = temp[1];
                F[6] = temp[8];
                F[7] = temp[5];
                F[8] = temp[2];
                break;
            case 4:
                int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp4[12] = U[12];
                temp4[13] = U[13];
                temp4[14] = U[14];
                temp4[15] = U[15];

                U[12] = L[15];
                U[13] = L[11];
                U[14] = L[7];
                U[15] = L[3];

                L[3] = D[0];
                L[7] = D[1];
                L[11] = D[2];
                L[15] = D[3];

                D[0] = R[12];
                D[1] = R[8];
                D[2] = R[4];
                D[3] = R[0];

                R[0] = temp4[12];
                R[4] = temp4[13];
                R[8] = temp4[14];
                R[12] = temp4[15];

                Array.Copy(F, temp4, 16);
                F[0] = temp4[12];
                F[1] = temp4[8];
                F[2] = temp4[4];
                F[3] = temp4[0];
                F[4] = temp4[13];
                F[5] = temp4[9];
                F[6] = temp4[5];
                F[7] = temp4[1];
                F[8] = temp4[14];
                F[9] = temp4[10];
                F[10] = temp4[6];
                F[11] = temp4[2];
                F[12] = temp4[15];
                F[13] = temp4[11];
                F[14] = temp4[7];
                F[15] = temp4[3];
                break;
        }
    }
    public void MOVE_F_PRIME(){
        MOVE_F();
        MOVE_F();
        MOVE_F();
    }

    public void MOVE_F_WIDE(){
        int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

        temp4[8] = U[8];
        temp4[9] = U[9];
        temp4[10] = U[10];
        temp4[11] = U[11];
        temp4[12] = U[12];
        temp4[13] = U[13];
        temp4[14] = U[14];
        temp4[15] = U[15];

        U[8] = L[14];
        U[9] = L[10];
        U[10] = L[6];
        U[11] = L[2];
        U[12] = L[15];
        U[13] = L[11];
        U[14] = L[7];
        U[15] = L[3];

        L[2] = D[4];
        L[3] = D[0];
        L[6] = D[5];
        L[7] = D[1];
        L[10] = D[6];
        L[11] = D[2];
        L[14] = D[7];
        L[15] = D[3];

        D[0] = R[12];
        D[1] = R[8];
        D[2] = R[4];
        D[3] = R[0];
        D[4] = R[13];
        D[5] = R[9];
        D[6] = R[5];
        D[7] = R[1];

        R[0] = temp4[12];
        R[4] = temp4[13];
        R[8] = temp4[14];
        R[12] = temp4[15];
        R[1] = temp4[8];
        R[5] = temp4[9];
        R[9] = temp4[10];
        R[13] = temp4[11];

        Array.Copy(F, temp4, 16);
        F[0] = temp4[12];
        F[1] = temp4[8];
        F[2] = temp4[4];
        F[3] = temp4[0];
        F[4] = temp4[13];
        F[5] = temp4[9];
        F[6] = temp4[5];
        F[7] = temp4[1];
        F[8] = temp4[14];
        F[9] = temp4[10];
        F[10] = temp4[6];
        F[11] = temp4[2];
        F[12] = temp4[15];
        F[13] = temp4[11];
        F[14] = temp4[7];
        F[15] = temp4[3];
    }
    public void MOVE_F_WIDE_PRIME(){
        MOVE_F_WIDE();
        MOVE_F_WIDE();
        MOVE_F_WIDE();
    }
    public void MOVE_B(){
        switch (cube_size){
            case 3:
                int[] temp = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp[0] = U[0];
                temp[1] = U[1];
                temp[2] = U[2];

                U[0] = R[2];
                U[1] = R[5];
                U[2] = R[8];

                R[2] = D[8];
                R[5] = D[7];
                R[8] = D[6];

                D[6] = L[0];
                D[7] = L[3];
                D[8] = L[6];

                L[0] = temp[2];
                L[3] = temp[1];
                L[6] = temp[0];

                Array.Copy(B, temp, 9);
                B[0] = temp[6];
                B[1] = temp[3];
                B[2] = temp[0];
                B[3] = temp[7];
                B[4] = temp[4];
                B[5] = temp[1];
                B[6] = temp[8];
                B[7] = temp[5];
                B[8] = temp[2];
                break;
            case 4:
                int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp4[0] = U[0];
                temp4[1] = U[1];
                temp4[2] = U[2];
                temp4[3] = U[3];

                U[0] = R[3];
                U[1] = R[7];
                U[2] = R[11];
                U[3] = R[15];

                R[3] = D[15];
                R[7] = D[14];
                R[11] = D[13];
                R[15] = D[12];

                D[12] = L[0];
                D[13] = L[4];
                D[14] = L[8];
                D[15] = L[12];

                L[0] = temp4[3];
                L[4] = temp4[2];
                L[8] = temp4[1];
                L[12] = temp4[0];

                Array.Copy(B, temp4, 16);
                B[0] = temp4[12];
                B[1] = temp4[8];
                B[2] = temp4[4];
                B[3] = temp4[0];
                B[4] = temp4[13];
                B[5] = temp4[9];
                B[6] = temp4[5];
                B[7] = temp4[1];
                B[8] = temp4[14];
                B[9] = temp4[10];
                B[10] = temp4[6];
                B[11] = temp4[2];
                B[12] = temp4[15];
                B[13] = temp4[11];
                B[14] = temp4[7];
                B[15] = temp4[3];
                break;
        }
    }

    public void MOVE_B_PRIME(){
        MOVE_B();
        MOVE_B();
        MOVE_B();
    }

    public void MOVE_B_WIDE(){
        int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

        temp4[0] = U[0];
        temp4[1] = U[1];
        temp4[2] = U[2];
        temp4[3] = U[3];
        temp4[4] = U[4];
        temp4[5] = U[5];
        temp4[6] = U[6];
        temp4[7] = U[7];

        U[0] = R[3];
        U[1] = R[7];
        U[2] = R[11];
        U[3] = R[15];
        U[4] = R[2];
        U[5] = R[6];
        U[6] = R[10];
        U[7] = R[14];

        R[3] = D[15];
        R[7] = D[14];
        R[11] = D[13];
        R[15] = D[12];
        R[2] = D[11];
        R[6] = D[10];
        R[10] = D[9];
        R[14] = D[8];

        D[12] = L[0];
        D[13] = L[4];
        D[14] = L[8];
        D[15] = L[12];
        D[8] = L[1];
        D[9] = L[5];
        D[10] = L[9];
        D[11] = L[13];

        L[0] = temp4[3];
        L[4] = temp4[2];
        L[8] = temp4[1];
        L[12] = temp4[0];
        L[1] = temp4[7];
        L[5] = temp4[6];
        L[9] = temp4[5];
        L[13] = temp4[4];

        Array.Copy(B, temp4, 16);
        B[0] = temp4[12];
        B[1] = temp4[8];
        B[2] = temp4[4];
        B[3] = temp4[0];
        B[4] = temp4[13];
        B[5] = temp4[9];
        B[6] = temp4[5];
        B[7] = temp4[1];
        B[8] = temp4[14];
        B[9] = temp4[10];
        B[10] = temp4[6];
        B[11] = temp4[2];
        B[12] = temp4[15];
        B[13] = temp4[11];
        B[14] = temp4[7];
        B[15] = temp4[3];

    }
    public void MOVE_B_WIDE_PRIME(){
        MOVE_B_WIDE();
        MOVE_B_WIDE();
        MOVE_B_WIDE();
    }
    public void MOVE_R(){
        switch (cube_size){
            case 2:
                int[] temp2 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp2[1] = U[1];
                temp2[3] = U[3];

                U[1] = F[1];
                U[3] = F[3];

                F[1] = D[1];
                F[3] = D[3];

                D[1] = B[2];
                D[3] = B[0];

                B[2] = temp2[3];
                B[0] = temp2[1];

                Array.Copy(R, temp2, 4);
                R[0] = temp2[2];
                R[1] = temp2[0];
                R[2] = temp2[3];
                R[3] = temp2[1];
                break;
            case 3:
                int[] temp = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp[2] = U[2];
                temp[5] = U[5];
                temp[8] = U[8];

                U[2] = F[2];
                U[5] = F[5];
                U[8] = F[8];

                F[2] = D[2];
                F[5] = D[5];
                F[8] = D[8];

                D[2] = B[6];
                D[5] = B[3];
                D[8] = B[0];

                B[0] = temp[8];
                B[3] = temp[5];
                B[6] = temp[2];

                Array.Copy(R, temp, 9);
                R[0] = temp[6];
                R[1] = temp[3];
                R[2] = temp[0];
                R[3] = temp[7];
                R[4] = temp[4];
                R[5] = temp[1];
                R[6] = temp[8];
                R[7] = temp[5];
                R[8] = temp[2];
                break;
            case 4:
                int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp4[3] = U[3];
                temp4[7] = U[7];
                temp4[11] = U[11];
                temp4[15] = U[15];

                U[3] = F[3];
                U[7] = F[7];
                U[11] =F[11];
                U[15] =F[15];

                F[3] = D[3];
                F[7] = D[7];
                F[11] = D[11];
                F[15] = D[15];

                D[3] = B[12];
                D[7] = B[8];
                D[11] = B[4];
                D[15] = B[0];

                B[0] = temp4[15];
                B[4] = temp4[11];
                B[8] = temp4[7];
                B[12] = temp4[3];

                Array.Copy(R, temp4, 16);
                R[0] = temp4[12];
                R[1] = temp4[8];
                R[2] = temp4[4];
                R[3] = temp4[0];
                R[4] = temp4[13];
                R[5] = temp4[9];
                R[6] = temp4[5];
                R[7] = temp4[1];
                R[8] = temp4[14];
                R[9] = temp4[10];
                R[10] = temp4[6];
                R[11] = temp4[2];
                R[12] = temp4[15];
                R[13] = temp4[11];
                R[14] = temp4[7];
                R[15] = temp4[3];
                break;
        }
    }
    public void MOVE_R_PRIME(){
        MOVE_R();
        MOVE_R();
        MOVE_R();
    }

    public void MOVE_R_WIDE(){
        int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

        temp4[2] = U[2];
        temp4[3] = U[3];
        temp4[6] = U[6];
        temp4[7] = U[7];
        temp4[10] = U[10];
        temp4[11] = U[11];
        temp4[14] = U[14];
        temp4[15] = U[15];

        U[2] = F[2];
        U[3] = F[3];
        U[6] = F[6];
        U[7] = F[7];
        U[10] = F[10];
        U[11] = F[11];
        U[14] = F[14];
        U[15] = F[15];

        F[2] = D[2];
        F[3] = D[3];
        F[6] = D[6];
        F[7] = D[7];
        F[10] = D[10];
        F[11] = D[11];
        F[14] = D[14];
        F[15] = D[15];

        D[2] = B[13];
        D[3] = B[12];
        D[6] = B[9];
        D[7] = B[8];
        D[10] = B[5];
        D[11] = B[4];
        D[14] = B[1];
        D[15] = B[0];

        B[0] = temp4[15];
        B[1] = temp4[14];
        B[4] = temp4[11];
        B[5] = temp4[10];
        B[8] = temp4[7];
        B[9] = temp4[6];
        B[12] = temp4[3];
        B[13] = temp4[2];

        Array.Copy(R, temp4, 16);
        R[0] = temp4[12];
        R[1] = temp4[8];
        R[2] = temp4[4];
        R[3] = temp4[0];
        R[4] = temp4[13];
        R[5] = temp4[9];
        R[6] = temp4[5];
        R[7] = temp4[1];
        R[8] = temp4[14];
        R[9] = temp4[10];
        R[10] = temp4[6];
        R[11] = temp4[2];
        R[12] = temp4[15];
        R[13] = temp4[11];
        R[14] = temp4[7];
        R[15] = temp4[3];
    }
    public void MOVE_R_WIDE_PRIME(){
        MOVE_R_WIDE();
        MOVE_R_WIDE();
        MOVE_R_WIDE();
    }
    public void MOVE_L(){
        switch (cube_size){
            case 3:
                int[] temp = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp[0] = U[0];
                temp[3] = U[3];
                temp[6] = U[6];

                U[0] = B[8];
                U[3] = B[5];
                U[6] = B[2];

                B[2] = D[6];
                B[5] = D[3];
                B[8] = D[0];

                D[0] = F[0];
                D[3] = F[3];
                D[6] = F[6];

                F[0] = temp[0];
                F[3] = temp[3];
                F[6] = temp[6];

                Array.Copy(L, temp, 9);
                L[0] = temp[6];
                L[1] = temp[3];
                L[2] = temp[0];
                L[3] = temp[7];
                L[4] = temp[4];
                L[5] = temp[1];
                L[6] = temp[8];
                L[7] = temp[5];
                L[8] = temp[2];
                break;
            case 4:
                int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

                temp4[0] = U[0];
                temp4[4] = U[4];
                temp4[8] = U[8];
                temp4[12] = U[12];

                U[0] = B[15];
                U[4] = B[11];
                U[8] = B[7];
                U[12] = B[3];

                B[3] = D[12];
                B[7] = D[8];
                B[11] = D[4];
                B[15] = D[0];

                D[0] = F[0];
                D[4] = F[4];
                D[8] = F[8];
                D[12] = F[12];

                F[0] = temp4[0];
                F[4] = temp4[4];
                F[8] = temp4[8];
                F[12] = temp4[12];

                Array.Copy(L, temp4, 16);
                L[0] = temp4[12];
                L[1] = temp4[8];
                L[2] = temp4[4];
                L[3] = temp4[0];
                L[4] = temp4[13];
                L[5] = temp4[9];
                L[6] = temp4[5];
                L[7] = temp4[1];
                L[8] = temp4[14];
                L[9] = temp4[10];
                L[10] = temp4[6];
                L[11] = temp4[2];
                L[12] = temp4[15];
                L[13] = temp4[11];
                L[14] = temp4[7];
                L[15] = temp4[3];
                break;
        }
    }
    public void MOVE_L_PRIME(){
        MOVE_L();
        MOVE_L();
        MOVE_L();
    }

    public void MOVE_L_WIDE(){
        int[] temp4 = Enumerable.Repeat(0, cube_size * cube_size).ToArray();

        temp4[0] = U[0];
        temp4[1] = U[1];
        temp4[4] = U[4];
        temp4[5] = U[5];
        temp4[8] = U[8];
        temp4[9] = U[9];
        temp4[12] = U[12];
        temp4[13] = U[13];

        U[0] = B[15];
        U[4] = B[11];
        U[8] = B[7];
        U[12] = B[3];
        U[1] = B[14];
        U[5] = B[10];
        U[9] = B[6];
        U[13] = B[2];

        B[2] = D[13];
        B[6] = D[9];
        B[10] = D[5];
        B[14] = D[1];
        B[3] = D[12];
        B[7] = D[8];
        B[11] = D[4];
        B[15] = D[0];

        D[0] = F[0];
        D[4] = F[4];
        D[8] = F[8];
        D[12] = F[12];
        D[1] = F[1];
        D[5] = F[5];
        D[9] = F[9];
        D[13] = F[13];

        F[0] = temp4[0];
        F[4] = temp4[4];
        F[8] = temp4[8];
        F[12] = temp4[12];
        F[1] = temp4[1];
        F[5] = temp4[5];
        F[9] = temp4[9];
        F[13] = temp4[13];

        Array.Copy(L, temp4, 16);
        L[0] = temp4[12];
        L[1] = temp4[8];
        L[2] = temp4[4];
        L[3] = temp4[0];
        L[4] = temp4[13];
        L[5] = temp4[9];
        L[6] = temp4[5];
        L[7] = temp4[1];
        L[8] = temp4[14];
        L[9] = temp4[10];
        L[10] = temp4[6];
        L[11] = temp4[2];
        L[12] = temp4[15];
        L[13] = temp4[11];
        L[14] = temp4[7];
        L[15] = temp4[3];
    }
    public void MOVE_L_WIDE_PRIME(){
        MOVE_L_WIDE();
        MOVE_L_WIDE();
        MOVE_L_WIDE();
    }

    #endregion Moves
    public override string ToString(){
        string s = "";
        s+= "U:" + String.Join(" ", U) + "\n";
        s+= "F:" + String.Join(" ", F) + "\n";
        s+= "L:" + String.Join(" ", L) + "\n";
        s+= "B:" + String.Join(" ", B) + "\n";
        s+= "R:" + String.Join(" ", R) + "\n";
        s+= "D:" + String.Join(" ", D) + "\n";
        return s;
    }
}