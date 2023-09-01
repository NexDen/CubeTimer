using System.Collections;
using UnityEngine;
using System;
public class BerilTest1 : MonoBehaviour
{
    void Start()
    {
        bolenleriBul(2, 98);
    }
    
    uint cort;

    void bolenleriBul(int sayi1, int sayi2){
        for (int bölen=2; bölen <= sayi2; bölen++){ // "bölen <= sayi2"'deki `sayi2` yi istediğin değerle değiştirebilirsin
            string sayılistesi = bölen.ToString() + ": ";
            for (int sayı = sayi1; sayı <= sayi2; sayı++){
                if (sayı % bölen == 0){
                    sayılistesi += sayı.ToString() + " ";
                }
            }
            print(sayılistesi);
        }
    }


    void bolenleriBul2(int sayi1, int sayi2)

    {
        ArrayList ikincibolme   = new ArrayList();
        ArrayList ucuncubolme   = new ArrayList();
        ArrayList dorduncubolme = new ArrayList();
        ArrayList besincibolme  = new ArrayList();

        string benimsayılistem = "Tum liste:";

        for (int i = sayi1; i <= sayi2; i++)
        {
            i += 0;
            if (i % 2 == 0)
                ikincibolme.Add(i);
            
            if (i % 3 == 0)
                ucuncubolme.Add(i);
            
            if (i % 4 == 0)
                dorduncubolme.Add(i);
            
            if (i % 5 == 0)
                besincibolme.Add(i);
            
        }

        for (int sayi=sayi1; sayi<=sayi2; sayi++)
        {
            benimsayılistem += " -*-*-*- ";
            benimsayılistem += sayi.ToString();
        }


        print(benimsayılistem);
        benimsayılistem = "İkiye Bölünenler:";
        foreach (int sayi in ikincibolme)
        {
            benimsayılistem += " -.-.- ";
            benimsayılistem += sayi.ToString();
        }
        print(benimsayılistem);
        benimsayılistem = "Üçe Bölünenler:";
        foreach (int sayi in ucuncubolme)
        {
            benimsayılistem += " -.-.- ";
            benimsayılistem += sayi.ToString();
        }
        print(benimsayılistem);
        benimsayılistem = "Dörde Bölünenler:";
        foreach (int sayi in dorduncubolme)
        {
            benimsayılistem += " -.-.- ";
            benimsayılistem += sayi.ToString();
        }
        print(benimsayılistem);
        benimsayılistem = "Beşe Bölünenler:";
        foreach (int sayi in besincibolme)
        {
            benimsayılistem += " -.-.- ";
            benimsayılistem += sayi.ToString();
        }
        print(benimsayılistem);
    }
  


    


} 

