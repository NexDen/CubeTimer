using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Visualizer : MonoBehaviour
{
    public Cube cube;
    [SerializeField] public List<GameObject> right;
    [SerializeField] public List<GameObject> left;
    [SerializeField] public List<GameObject> up;
    [SerializeField] public List<GameObject> down;
    [SerializeField] public List<GameObject> front;
    [SerializeField] public List<GameObject> back;
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
        foreach (GameObject g in right){
            g.GetComponent<Renderer>().material.color = colors[cube.R[index]];
            index++;
        }
        index = 0;
        foreach (GameObject g in left){
            g.GetComponent<Renderer>().material.color = colors[cube.L[index]];
            index++;
        }
        index = 0;
        foreach (GameObject g in up){
            g.GetComponent<Renderer>().material.color = colors[cube.U[index]];
            index++;
        }
        index = 0;
        foreach (GameObject g in down){
            g.GetComponent<Renderer>().material.color = colors[cube.D[index]];
            index++;
        }
        index = 0;
        foreach (GameObject g in front){
            g.GetComponent<Renderer>().material.color = colors[cube.F[index]];
            index++;
        }
        index = 0;
        foreach (GameObject g in back){
            g.GetComponent<Renderer>().material.color = colors[cube.B[index]];
            index++;
        }
    }

    public void setAllZero(){
        int index = 0;
        foreach (GameObject g in right){
            cube.R[index] = 0;
            index++;
        }
        index = 0;
        foreach (GameObject g in left){
            cube.L[index] = 0;
            index++;
        }
        index = 0;
        foreach (GameObject g in up){
            cube.U[index] = 0;
            index++;
        }
        index = 0;
        foreach (GameObject g in down){
            cube.D[index] = 0;
            index++;
        }
        index = 0;
        foreach (GameObject g in front){
            cube.F[index] = 0;
            index++;
        }
        index = 0;
        foreach (GameObject g in back){
            cube.B[index] = 0;
            index++;
        }
    }
    public void InterpretAlgorithm(string MoveSet){
        cube = new Cube();
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
                default:
                    break;
            }
        }
        Visualize();
    }

}

public class Cube
{
    #region Arrays
    public int[] U = new int[9] { 1,1,1,1,1,1,1,1,1 };
    public int[] F = new int[9] { 2,2,2,2,2,2,2,2,2 };
    public int[] L = new int[9] { 3,3,3,3,3,3,3,3,3 };
    public int[] B = new int[9] { 4,4,4,4,4,4,4,4,4 };
    public int[] R = new int[9] { 5,5,5,5,5,5,5,5,5 };
    public int[] D = new int[9] { 6,6,6,6,6,6,6,6,6 };
    #endregion

    #region Moves
    public void MOVE_U(){
        int[] temp = new int[] {0,0,0,0,0,0,0,0,0};

        temp[0] = F[0];
        temp[1] = F[1];
        temp[2] = F[2];

        F[0] = R[0];
        F[1] = R[1];
        F[2] = R[2];

        R[0] = B[0];
        R[1] = B[1];
        R[2] = B[2];

        B[0] = L[0];
        B[1] = L[1];
        B[2] = L[2];

        L[0] = temp[0];
        L[1] = temp[1];
        L[2] = temp[2];

        Array.Copy(U, temp, 9);
        U[0] = temp[6];
        U[1] = temp[3];
        U[2] = temp[0];
        U[3] = temp[7];
        U[4] = temp[4];
        U[5] = temp[1];
        U[6] = temp[8];
        U[7] = temp[5];
        U[8] = temp[2];
    }
    public void MOVE_U_PRIME(){
        int[] temp = new int[] {0,0,0,0,0,0,0,0,0};
        temp[0] = F[0];
        temp[1] = F[1];
        temp[2] = F[2];

        F[0] = L[0];
        F[1] = L[1];
        F[2] = L[2];

        L[0] = B[0];
        L[1] = B[1];
        L[2] = B[2];

        B[0] = R[0];
        B[1] = R[1];
        B[2] = R[2];

        R[0] = temp[0];
        R[1] = temp[1];
        R[2] = temp[2];

        Array.Copy(U, temp, 9);
        U[0] = temp[2];
        U[1] = temp[5];
        U[2] = temp[8];
        U[3] = temp[1];
        U[4] = temp[4];
        U[5] = temp[7];
        U[6] = temp[0];
        U[7] = temp[3];
        U[8] = temp[6];
    }
    public void MOVE_F(){
        int[] temp = new int[] {0,0,0,0,0,0,0,0,0};

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
        D[2] =  R[0];

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
    }
    public void MOVE_F_PRIME(){
        int[] temp = new int[] {0,0,0,0,0,0,0,0,0};
        temp[6] = U[6];
        temp[7] = U[7];
        temp[8] = U[8];

        U[6] = R[0];
        U[7] = R[3];
        U[8] = R[6];

        R[0] = D[2];
        R[3] = D[1];
        R[6] = D[0];

        D[0] = L[2];
        D[1] = L[5];
        D[2] = L[8];

        L[2] = temp[8];
        L[5] = temp[7];
        L[8] = temp[6];

        Array.Copy(F, temp, 9);
        F[0] = temp[2];
        F[1] = temp[5];
        F[2] = temp[8];
        F[3] = temp[1];
        F[4] = temp[4];
        F[5] = temp[7];
        F[6] = temp[0];
        F[7] = temp[3];
        F[8] = temp[6];
    }
    public void MOVE_L(){
        int[] temp = new int[]{0,0,0,0,0,0,0,0,0};
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
    }
    public void MOVE_L_PRIME(){
        int[] temp = new int[]{0,0,0,0,0,0,0,0,0};
        temp[0] = U[0];
        temp[3] = U[3];
        temp[6] = U[6];

        U[0] = F[0];
        U[3] = F[3];
        U[6] = F[6];

        F[0] = D[0];
        F[3] = D[3];
        F[6] = D[6];

        D[0] = B[8];
        D[3] = B[5];
        D[6] = B[2];

        B[2] = temp[6];
        B[5] = temp[3];
        B[8] = temp[0];

        Array.Copy(L, temp, 9);
        L[0] = temp[2];
        L[1] = temp[5];
        L[2] = temp[8];
        L[3] = temp[1];
        L[4] = temp[4];
        L[5] = temp[7];
        L[6] = temp[0];
        L[7] = temp[3];
        L[8] = temp[6];
    }
    public void MOVE_B(){
        int[] temp = new int[]{0,0,0,0,0,0,0,0,0};
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
    }
    public void MOVE_B_PRIME(){
        int[] temp = new int[]{0,0,0,0,0,0,0,0,0};
        temp[0] = U[0];
        temp[1] = U[1];
        temp[2] = U[2];

        U[0] = L[6];
        U[1] = L[3];
        U[2] = L[0];

        L[0] = D[6];
        L[3] = D[7];
        L[6] = D[8];

        D[6] = R[8];
        D[7] = R[5];
        D[8] = R[2];

        R[2] = temp[0];
        R[5] = temp[1];
        R[8] = temp[2];

        Array.Copy(B, temp, 9);
        B[0] = temp[2];
        B[1] = temp[5];
        B[2] = temp[8];
        B[3] = temp[1];
        B[4] = temp[4];
        B[5] = temp[7];
        B[6] = temp[0];
        B[7] = temp[3];
        B[8] = temp[6];
    }
    public void MOVE_D(){
        int[] temp = new int[]{0,0,0,0,0,0,0,0,0};
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
    }
    public void MOVE_D_PRIME(){
        int[] temp = new int[]{0,0,0,0,0,0,0,0,0};
        temp[6] = F[6];
        temp[7] = F[7];
        temp[8] = F[8];

        F[6] = R[6];
        F[7] = R[7];
        F[8] = R[8];

        R[6] = B[6];
        R[7] = B[7];
        R[8] = B[8];

        B[6] = L[6];
        B[7] = L[7];
        B[8] = L[8];

        L[6] = temp[6];
        L[7] = temp[7];
        L[8] = temp[8];
        
        Array.Copy(D, temp, 9);

        D[0] = temp[2];
        D[1] = temp[5];
        D[2] = temp[8];
        D[3] = temp[1];
        D[4] = temp[4];
        D[5] = temp[7];
        D[6] = temp[0];
        D[7] = temp[3];
        D[8] = temp[6];
    }
    public void MOVE_R(){
        int[] temp = new int[] {0,0,0,0,0,0,0,0,0};
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
    }
    public void MOVE_R_PRIME(){
        int[] temp = new int[] {0,0,0,0,0,0,0,0,0};
        temp[2] = U[2];
        temp[5] = U[5];
        temp[8] = U[8];

        U[2] = B[6];
        U[5] = B[3];
        U[8] = B[0];

        B[0] = D[8];
        B[3] = D[5];
        B[6] = D[2];

        D[2] = F[2];
        D[5] = F[5];
        D[8] = F[8];

        F[2] = temp[2];
        F[5] = temp[5];
        F[8] = temp[8];

        Array.Copy(R, temp, 9);
        R[0] = temp[2];
        R[1] = temp[5];
        R[2] = temp[8];
        R[3] = temp[1];
        R[4] = temp[4];
        R[5] = temp[7];
        R[6] = temp[0];
        R[7] = temp[3];
        R[8] = temp[6];
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