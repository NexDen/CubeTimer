using System; // Math
using System.Collections;
using System.Collections.Generic; // List
using System.Linq; // Random
using UnityEngine; // using Random = System.Random;
using UnityEngine.UI; // Text
/*
V0.31.10
DEĞİŞENLER:
- En iyi zaman yeşil, 20 saniyenin altındaki zamanlar kırmızı olarak gösteriliyor
- Ekranın en üstünde saati gösteren yazı eklendi
- Optimizasyonlar (zamanAmaString patladı)
*/
public class Timer : MonoBehaviour
{
    #region Zamanlayıcı Değişkenleri
    /* 
    bunlar [SerializeField] çünkü GetComponent<>() yapmak yerine
    direkt Unity içinden değişiliyor.
    */
    [SerializeField] public bool başlatmaİçinBasılıTutma; // basılı tutulma durumu (ilk)
    [SerializeField] public float basılıTutmaSüresi; // basılı tutulma durumu (süre)
    [SerializeField] public float zaman; // milisaniye cinsinden zaman
    [SerializeField] public bool başladı; // başlatılma durumu
    [SerializeField] public bool başlayabilir; // başlatılabilirlik durumu
    [SerializeField] public bool artı2Kilit;
    [SerializeField] public bool dnfKilit;
    [SerializeField] public List<float> zamanlar; // zamanları tutan liste
    [SerializeField] public bool zamanlayıcıGörünüm;
    [SerializeField] public int çözümSayısı;
    [SerializeField] public float redlineLimit;
    [SerializeField] public int cube_size;
    #endregion
    
    #region UI Elemanları
    /*
    bunların HEPSİ [SerializeField] çünkü
    TEKER TEKER GetComponent<>() çekmek zor olur.
    */
    [SerializeField] public Text zamanlayıcı;
    [SerializeField] public Text _eniyiZaman;
    [SerializeField] public Text zamanListesi;
    [SerializeField] public Text ort5Text;
    [SerializeField] public Text ORT10Text;
    [SerializeField] public Text ortText;

    [SerializeField] public Text ort5Label;
    [SerializeField] public Text ort10Label;
    [SerializeField] public Text ortLabel;
    [SerializeField] public Text enİyiZamanLabel;
    [SerializeField] public Text karıştırmaLabel;

    [SerializeField] public Text artı2uyarı;
    [SerializeField] public Scrambler scrambler;
    [SerializeField] List<GameObject> kapatılacaklar;

    [SerializeField] public Toggle zamanlayıcıToggle;
    [SerializeField] public Text toggleLabel1;
    [SerializeField] public Text güncelZaman;
    #endregion

    #region Ortalamalar
    public float ORT5;
    public float ORT10;
    public float ORT;
    public float eniyiZaman;


    #endregion
    

    [Range(0,40)] public float textsize2; // debug için :)

    #region Güncellemeler
    void zamanListesiGüncelle(){ 
        /* 
        zamanListesi (ekranın en solundaki zaman listesi)
        nin üzerindeki zamanları dinamik olarak günceller  
        */
        zamanListesi.text = "";
        int çözümIndex = Math.Max(çözümSayısı-17, 1);
        foreach(float zaman in SonZamanlarıAl(zamanlar, 18)){ // ekrana maksimum 18 tane sığıyor zaten
            if (zaman == zamanlar.Min()){ // zaman = eniyizaman
                if (çözümIndex == 1){
                    if (zaman < redlineLimit) zamanListesi.text += $"<color=#00ff00>({çözümIndex}) {ZamanıSaateÇevir(zaman)}</color> <size={textsize2}>({scrambler._prevScrambles[çözümIndex-1]})</size>";
                    else if (çözümSayısı != 1) zamanListesi.text += $"<color=#00ff00>({çözümIndex}) {ZamanıSaateÇevir(zaman)}</color>";
                    else zamanListesi.text += $"({çözümIndex}) {ZamanıSaateÇevir(zaman)}";
                }
                else zamanListesi.text += $"<color=#00ff00>({çözümIndex}) {ZamanıSaateÇevir(zaman)}</color> <size={textsize2}>({scrambler._prevScrambles[çözümIndex-1]})</size>";
            }
            else if (eniyiZaman < zaman && zaman < redlineLimit){ // eniyizaman<zaman<redlineLimit
                zamanListesi.text += $"<color=#ff0000>({çözümIndex}) {ZamanıSaateÇevir(zaman)}</color> <size={textsize2}>({scrambler._prevScrambles[çözümIndex-1]})</size>";
            }
            else{
                zamanListesi.text += $"({çözümIndex}) {ZamanıSaateÇevir(zaman)}";
            }
            zamanListesi.text += "\n";
            çözümIndex++;
        }
    }

    void EniyiZamanGüncelle(){
        /*
        en iyi zamanın ne olduğunu bulmak 
        ve onu ekranın sol altındaki yere 
        yazmak için basit fonksiyon
        */

        float öncekiEnİyiZaman = eniyiZaman;

        if (zamanlar.Count() == 0){
            _eniyiZaman.text = "-";
        }
        else{
            eniyiZaman = zamanlar.Min();
            _eniyiZaman.text = ZamanıSaateÇevir(eniyiZaman);
            
        }
        
    }

    #endregion

    #region Unity Fonksiyonları
    void Start()
    {
        /*
        genel başlangıç ve değişkenlerin ayarlanması
        */
        zamanListesiGüncelle();
        başlatmaİçinBasılıTutma = false;
        basılıTutmaSüresi = 0;
        başladı = false;
        zaman = 0;
        ORT5 = 0;
        ORT10 = 0;
        dnfKilit = true;
        artı2Kilit = true;
    }

    void Update()
    {
        zamanlayıcıGörünüm = zamanlayıcıToggle.isOn; // eğer zamanlayıcı gösterilmek isteniyorsa
        if (başlatmaİçinBasılıTutma) // eğer basılı tutuluyosa
        {
            basılıTutmaSüresi += Time.deltaTime; // basılı tutma süresi say
            if (basılıTutmaSüresi >= 0.55f) // eğer 0.55(resmi WCA sayaç bekleme süresi) saniye basılı tutulduysa
            {
                zamanlayıcı.color = Color.green;
                başlayabilir = true; // bırakılıncaya kadar hazır dur
            }
        }
        if (başladı) // başlandıysa zaman say
        {
            zaman += Time.deltaTime; // zaman say
           
            if (zamanlayıcıGörünüm) zamanlayıcı.text = ZamanıSaateÇevir(zaman); // eğer zamanlayıcı gösterilmek isteniyorsa göster
            else zamanlayıcı.text = "-"; // yoksa "-" ı bas geç
        }

        if (Input.GetMouseButtonDown(0)) // eğer ekrana basıldıysa
        {
            Bas();
        }
        if (Input.GetMouseButtonUp(0)) // eğer ekrana basılma kesildiyse
        {
            Çek();
        }
        
        zamanListesiGüncelle();
        güncelZaman.text = System.DateTime.Now.ToString("HH:mm"); // ekranın en üstündeki saati ayarla
    }
    
    #endregion
    
    #region Ekrana Ekleme-Kaldırma 
    /* 
    zamanlayıcı başlayınca ekrandan 
    kalkacak şeyleri kaldır/geri ekle 
    */
    void EkranaEkle(){ 
        foreach (GameObject g in kapatılacaklar)
        {
            g.gameObject.SetActive(true);
        }
    }

    void EkrandanKaldır(){
        foreach (GameObject g in kapatılacaklar)
        {
            g.gameObject.SetActive(false);
        }
        artı2uyarı.gameObject.SetActive(false);
    }

    #endregion
    
    #region Zamanlayıcı Fonksiyonları
    void Bas() {
        if (!başladı) { // eğer başlatılmamışsa basılı tutma süresini say
            başlatmaİçinBasılıTutma = true;
            
            zamanlayıcı.color = Color.red;
        }
        if (başladı){ // eğer başlatılmışsa zamanlayıcıyı durdur
            ZamanlayıcıyıDurdur();
        }
    }
    void ZamanlayıcıyıDurdur() {
        EkranaEkle();
        başladı = false;
        başlayabilir = false;
        
        zamanlar.Add(zaman);
        EniyiZamanGüncelle();
        
        _eniyiZaman.text = ZamanıSaateÇevir(eniyiZaman);
        zamanlayıcı.text = ZamanıSaateÇevir(zaman);
        scrambler.GenerateNewScramble(cube_size);
        
        zamanListesiGüncelle();
        OrtalamalarıHesapla();
        

        
    }
    void ZamanlayıcıyıBaşlat(){
        artı2Kilit = false;
        dnfKilit = false;
        zamanlayıcı.color = Color.white;
        zaman = 0;
        basılıTutmaSüresi = 0;
        başladı = true;
        başlatmaİçinBasılıTutma = false;
        çözümSayısı++;
        EkrandanKaldır();
    }
    void Çek() {
        if (!başladı) { // eğer daha başlamadıysa başlama sürecini durdur
            başlatmaİçinBasılıTutma = false;
            basılıTutmaSüresi = 0;
            zamanlayıcı.color = Color.white;
        }
        if (başlayabilir) { // eğer başlatılabilirse başlat
            ZamanlayıcıyıBaşlat();
        }
    }
    public void DNF()
    {
        if (dnfKilit) return;
        zamanlar.RemoveAt(zamanlar.Count - 1);
        zamanlayıcı.text = "DNF";
        artı2Kilit = true;
        zamanListesiGüncelle();
        EniyiZamanGüncelle();
        OrtalamalarıHesapla();
        artı2uyarı.gameObject.SetActive(false);
        dnfKilit = true; // (gerek yok ama ne olur ne olmaz elde bulunsun)
    }
    public void artı2(){
        if (artı2Kilit) return;
        float şuankizaman;
        şuankizaman = zaman;
        şuankizaman += 2;

        zamanlayıcı.text = ZamanıSaateÇevir(şuankizaman);
        zamanlar.RemoveAt(zamanlar.Count - 1);
        zamanlar.Add(şuankizaman);

        artı2uyarı.gameObject.SetActive(true);

        zamanListesiGüncelle();
        EniyiZamanGüncelle();
        OrtalamalarıHesapla();
    }
    public void Reset(){
        zaman = 0;
        zamanlar.Clear();
        zamanlayıcı.text = "0.00";
        ort5Text.text = "-";
        ORT10Text.text = "-";
        _eniyiZaman.text = "-";
        çözümSayısı = 0;
        eniyiZaman = 0;
        ORT = 0;
        ORT5 = 0;
        ORT10 = 0;
        zamanListesiGüncelle();
        OrtalamalarıHesapla();
        artı2Kilit = true;
        dnfKilit = true;
        artı2uyarı.gameObject.SetActive(false);
        scrambler.GenerateNewScramble(cube_size);
    }

    void OrtalamalarıHesapla(){
        if (zamanlar.Count >= 5) // eğer zaman listesi 5 veya 5 ten fazla ise
        {
            ORT5 = Ortalama(SonZamanlarıAl(zamanlar, 5)); // ortalama hesapla
            ort5Text.text = ZamanıSaateÇevir(ORT5); // ortalamayı ekrana yazdır
        }
        if (zamanlar.Count >= 11) // eğer liste 10 ise
        {
            ORT10 = Ortalama(SonZamanlarıAl(zamanlar, 10)); // ortalama hesapla
            ORT10Text.text = ZamanıSaateÇevir(ORT10); // ortalamayı ekrana yazdır
            // zamanlar.RemoveAt(0); // ilk elemanı sil (gerek yok ama ne olur ne olmaz elde bulunsun)
        }
        if (zamanlar.Count == 0){
            ortText.text = "-";
        }
        else{
            ORT = Ortalama(zamanlar); // ortalama hesapla
            ortText.text = ZamanıSaateÇevir(ORT); // ortalamayı ekrana yazdır
        }
    }


    #endregion

    #region Aritmetik Fonksiyonlar
    float Ortalama(List<float> liste){
            float toplam = 0;
            foreach (float i in liste)
            {
                toplam += i;
            }
            return (float)(toplam / liste.Count);
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

    List<float> SonZamanlarıAl(List<float> liste,int index){
        List<float> yeniListe = new List<float>();
        for (int i = Math.Max(0,liste.Count() - index); i < liste.Count(); i++){
            yeniListe.Add(liste[i]);
        }
        return yeniListe;
    }

    // Dictionary<float, string> ListeleriBirleştir(List<float> liste1, List<string> liste2){
    //     Dictionary<float, string> yeniListe = new Dictionary<float, string>();
    //     for (int i = 0; i < liste1.Count(); i++){
    //         yeniListe.Add(
    //             new object[] {liste1[i], liste2[i]}
    //         );
    //     }
    //     return yeniListe;
    // }

    #endregion
}
