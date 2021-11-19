using System; // Math
using System.Collections;
using System.Collections.Generic; // List
using System.Linq; // Random
using UnityEngine; // using Random = System.Random;
using UnityEngine.UI; // Text
public class Timer : MonoBehaviour
{
    /* 
    bunlar [SerializeField] çünkü Debug.Log() yapmak yerine
    direkt Unity içinden değişiliyor.
    */
    [SerializeField] public bool _başlatmaİçinBasılıTutma; // basılı tutulma durumu (ilk)
    [SerializeField] public float _basılıTutmaSüresi; // basılı tutulma durumu (süre)
    [SerializeField] public float _zaman; // milisaniye cinsinden zaman
    [SerializeField] public bool _başladı; // başlatılma durumu
    [SerializeField] public bool _başlayabilir; // başlatılabilirlik durumu
    [SerializeField] public int _çözümSayısı; // çözüm sayısı (sağol copilot)
    public float ORT5;
    public float ORT10;
    [SerializeField] List<float> _zamanlar; // zamanları tutan liste

    /*
    bunların HEPSİ [SerializeField] çünkü
    TEKER TEKER GetComponent<> çekmek zor olur.
    */
    [SerializeField] public Text _zamanlayıcı;
    [SerializeField] public Text _eniyiZaman;
    [SerializeField] public Text _zamanListesi;
    [SerializeField] public Text _ort5Text;
    [SerializeField] public Text _ORT10Text;

    [SerializeField] public Text _ort5Label;
    [SerializeField] public Text _ort10Label;
    [SerializeField] public Text _enİyiZamanLabel;
    [SerializeField] public Text _karıştırmaLabel;

    [SerializeField] public Scrambler _scrambler;
    List<Text> kapatılacaklar;
    void Start()
    {
        kapatılacaklar = new List<Text>() { _eniyiZaman, _zamanListesi, _ort5Text, _ORT10Text,
                                            _ort5Label, _ort10Label, _enİyiZamanLabel, _karıştırmaLabel 
                                            };
        _başlatmaİçinBasılıTutma = false;
        _basılıTutmaSüresi = 0;
        _başladı = false;
        _zaman = 0;
        ORT5 = 0;
        ORT10 = 0;
    }

    void Update()
    {
        if (_başlatmaİçinBasılıTutma) // eğer basılı tutuluyosa
        {
            _basılıTutmaSüresi += Time.deltaTime; // basılı tutma süresi say
            if (_basılıTutmaSüresi >= 0.55f) // eğer 1 saniye basılı tutulduysa
            {
                _zamanlayıcı.color = Color.green;
                _başlayabilir = true; // bırakılıncaya kadar hazır dur
            }
        }
        if (_başladı) // başlandıysa zaman say
        {
            _zaman += Time.deltaTime;
            _zamanlayıcı.text = ZamanıSaateÇevir(_zaman);
        }

        if (Input.GetMouseButtonDown(0)) // eğer ekrana basıldıysa
        {
            Bas();
        }
        if (Input.GetMouseButtonUp(0)) // eğer ekrana basılma kesildiyse
        {
            Çek();
        }
        if (_zamanlar.Count >= 5) // eğer zaman listesi 5 ten fazla ise
        {
            ORT5 = Ortalama(_zamanlar.GetRange(_çözümSayısı-5, 5),5); // ortalama hesapla
            _ort5Text.text = ZamanıSaateÇevir(ORT5); // ortalamayı ekrana yazdır
            
        }
        if (_zamanlar.Count >= 10) // eğer liste 12 ten fazla ise
        {
            ORT10 = Ortalama(_zamanlar.GetRange(_çözümSayısı-10, 10), 10); // ortalama hesapla
            _ORT10Text.text = ZamanıSaateÇevir(ORT10); // ortalamayı ekrana yazdır
            _zamanlar.RemoveAt(0); // ilk elemanı sil
            _çözümSayısı = 9;
        }

        _zamanListesi.text = "";
        foreach (float zaman in _zamanlar){
            _zamanListesi.text += ZamanıSaateÇevir((float)Math.Round(zaman,2)) + "\n";
        }

    }
    void Bas() {
        if (!_başladı) { // eğer başlatılmamışsa basılı tutma süresini say
            _başlatmaİçinBasılıTutma = true;
            _zaman = 0;
            
            _zamanlayıcı.color = Color.red;
        }
        if (_başladı){ // eğer başlatılmışsa zamanlayıcıyı durdur
            ZamanlayıcıyıDurdur();
        }
    }
    void ZamanlayıcıyıDurdur() {
        foreach (Text t in kapatılacaklar)
        {
            t.enabled = true;
        }
        _başladı = false;
        _başlayabilir = false;
        _çözümSayısı++;
        _zamanlar.Add(_zaman);
        _eniyiZaman.text = ZamanıSaateÇevir(_zamanlar.Min());
        _zaman = 0;
        _scrambler.GenerateNewScramble();
        

    }
    void ZamanlayıcıyıBaşlat(){
        _zamanlayıcı.color = Color.black;
        _basılıTutmaSüresi = 0;
        _başladı = true;
        _başlatmaİçinBasılıTutma = false;
        
        
        foreach (Text t in kapatılacaklar)
        {
            t.enabled = false;
        }
    }
    void Çek() {
        if (!_başladı) { // eğer daha başlamadıysa başlama sürecini durdur
            _başlatmaİçinBasılıTutma = false;
            _basılıTutmaSüresi = 0;
            _zamanlayıcı.color = Color.black;
        }
        if (_başlayabilir) { // eğer başlatılabilirse başlat
            ZamanlayıcıyıBaşlat();
        }
    }

    float Ortalama(List<float> liste, int sayı){
        float toplam = 0;
        foreach (float i in liste)
        {
            toplam += i;
        }
        return (float)(toplam / sayı);
    }

    string ZamanıSaateÇevir(float time) // copilot sağolsun
    {

        int minutes = (int)time / 60;
        int seconds = (int)time % 60; 
        int miliseconds = (int)((time - (int)time) * 100); 

        if (minutes == 0){
            if (seconds < 10){
                return string.Format("{0:0}.{1:00}",seconds, miliseconds); // 05.00 ı engellemek için (doğrusu 5.00)
            }
            else{
                return string.Format("{0:00}.{1:00}",seconds, miliseconds);
            }
        }
        else {
            if (minutes < 10){
                return string.Format("{0:0}:{1:00}.{2:00}", minutes, seconds, miliseconds); // 01:10.00 ı engellemek için (doğrusu 1:10.00)
            }

            else {
                return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, miliseconds);
            }
        }
    }
}
