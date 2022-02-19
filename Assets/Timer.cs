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
- Optimizasyonlar (_zamanAmaString patladı)
*/
public class Timer : MonoBehaviour
{
    #region Zamanlayıcı Değişkenleri
    /* 
    bunlar [SerializeField] çünkü GetComponent<>() yapmak yerine
    direkt Unity içinden değişiliyor.
    */
    [SerializeField] public bool _başlatmaİçinBasılıTutma; // basılı tutulma durumu (ilk)
    [SerializeField] public float _basılıTutmaSüresi; // basılı tutulma durumu (süre)
    [SerializeField] public float _zaman; // milisaniye cinsinden zaman
    [SerializeField] public bool _başladı; // başlatılma durumu
    [SerializeField] public bool _başlayabilir; // başlatılabilirlik durumu
    [SerializeField] public bool _artı2Kilit;
    [SerializeField] public bool _dnfKilit;
    [SerializeField] public List<float> _zamanlar; // zamanları tutan liste
    [SerializeField] public bool _zamanlayıcıGörünüm;
    [SerializeField] public int _çözümSayısı;
    [SerializeField] public float _redlineLimit;
    #endregion
    
    #region UI Elemanları
    /*
    bunların HEPSİ [SerializeField] çünkü
    TEKER TEKER GetComponent<>() çekmek zor olur.
    */
    [SerializeField] public Text _zamanlayıcı;
    [SerializeField] public Text _eniyiZaman;
    [SerializeField] public Text _zamanListesi;
    [SerializeField] public Text _ort5Text;
    [SerializeField] public Text _ORT10Text;
    [SerializeField] public Text _ortText;

    [SerializeField] public Text _ort5Label;
    [SerializeField] public Text _ort10Label;
    [SerializeField] public Text _ortLabel;
    [SerializeField] public Text _enİyiZamanLabel;
    [SerializeField] public Text _karıştırmaLabel;


    [SerializeField] public Scrambler _scrambler;
    [SerializeField] List<GameObject> kapatılacaklar;

    [SerializeField] public Toggle _zamanlayıcıToggle;
    [SerializeField] public Text _toggleLabel1;
    [SerializeField] public bool _artı2;
    [SerializeField] public Text _güncelZaman;
    #endregion

    #region Ortalamalar
    public float ORT5;
    public float ORT10;
    public float ORT;
    public float eniyiZaman;


    #endregion
    
    #region Güncellemeler
    void zamanListesiGüncelle(){ 
        /* 
        _zamanListesi (ekranın en solundaki zaman listesi)
        nin üzerindeki zamanları dinamik olarak günceller  
        */
        _zamanListesi.text = "";
        int çözümIndex = Math.Max(_çözümSayısı-18, 1);
        foreach(float zaman in SonZamanlarıAl(_zamanlar, 18)){ // ekrana maksimum 18 tane sığıyor zaten
            if (zaman == _zamanlar.Min()){ // zaman = eniyizaman
                _zamanListesi.text += $"<color=#00ff00>({çözümIndex}) {ZamanıSaateÇevir(zaman)}</color>";
            }
            else if (eniyiZaman < zaman && zaman < _redlineLimit){ // eniyizaman<zaman<redlineLimit
                _zamanListesi.text += $"<color=#ff0000>({çözümIndex}) {ZamanıSaateÇevir(zaman)}</color>";
            }
            else{
                _zamanListesi.text += $"({çözümIndex}) {ZamanıSaateÇevir(zaman)}";
            }
            _zamanListesi.text += "\n";
            çözümIndex++;
        }
    }

    void EniyiZamanGüncelle(){
        /*
        en iyi zamanın ne olduğunu bulmak 
        ve onu ekranın sol altındaki yere 
        yazmak için basit fonksiyon
        */
        if (_zamanlar.Count() == 0){
            _eniyiZaman.text = "-";
        }
        else{
            eniyiZaman = _zamanlar.Min();
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
        _başlatmaİçinBasılıTutma = false;
        _basılıTutmaSüresi = 0;
        _başladı = false;
        _zaman = 0;
        ORT5 = 0;
        ORT10 = 0;
        _dnfKilit = true;
        _artı2Kilit = true;
    }

    void Update()
    {
        _zamanlayıcıGörünüm = _zamanlayıcıToggle.isOn; // eğer zamanlayıcı gösterilmek isteniyorsa
        if (_başlatmaİçinBasılıTutma) // eğer basılı tutuluyosa
        {
            _basılıTutmaSüresi += Time.deltaTime; // basılı tutma süresi say
            if (_basılıTutmaSüresi >= 0.55f) // eğer 0.55(resmi WCA sayaç bekleme süresi) saniye basılı tutulduysa
            {
                _zamanlayıcı.color = Color.green;
                _başlayabilir = true; // bırakılıncaya kadar hazır dur
            }
        }
        if (_başladı) // başlandıysa zaman say
        {
            _zaman += Time.deltaTime; // zaman say
           
            if (_zamanlayıcıGörünüm) _zamanlayıcı.text = ZamanıSaateÇevir(_zaman); // eğer zamanlayıcı gösterilmek isteniyorsa göster
            else _zamanlayıcı.text = "-"; // yoksa "-" ı bas geç
        }

        if (Input.GetMouseButtonDown(0)) // eğer ekrana basıldıysa
        {
            Bas();
        }
        if (Input.GetMouseButtonUp(0)) // eğer ekrana basılma kesildiyse
        {
            Çek();
        }
        if (_zamanlar.Count >= 5) // eğer zaman listesi 5 veya 5 ten fazla ise
        {
            ORT5 = Ortalama(SonZamanlarıAl(_zamanlar, 5)); // ortalama hesapla
            _ort5Text.text = ZamanıSaateÇevir(ORT5); // ortalamayı ekrana yazdır
        }
        if (_zamanlar.Count >= 11) // eğer liste 10 ise
        {
            ORT10 = Ortalama(SonZamanlarıAl(_zamanlar, 10)); // ortalama hesapla
            _ORT10Text.text = ZamanıSaateÇevir(ORT10); // ortalamayı ekrana yazdır
            // _zamanlar.RemoveAt(0); // ilk elemanı sil (gerek yok ama ne olur ne olmaz elde bulunsun)
        }
        if (_zamanlar.Count == 0){
            _ortText.text = "-";
        }
        else{
            ORT = Ortalama(_zamanlar); // ortalama hesapla
            _ortText.text = ZamanıSaateÇevir(ORT); // ortalamayı ekrana yazdır
        }
        zamanListesiGüncelle();

        _güncelZaman.text = System.DateTime.Now.ToString("HH:mm"); // ekranın en üstündeki saati ayarla
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
    }

    #endregion
    
    #region Zamanlayıcı Fonksiyonları
    void Bas() {
        if (!_başladı) { // eğer başlatılmamışsa basılı tutma süresini say
            _başlatmaİçinBasılıTutma = true;
            
            _zamanlayıcı.color = Color.red;
        }
        if (_başladı){ // eğer başlatılmışsa zamanlayıcıyı durdur
            ZamanlayıcıyıDurdur();
        }
    }
    void ZamanlayıcıyıDurdur() {
        EkranaEkle();
        _başladı = false;
        _başlayabilir = false;
        
        _zamanlar.Add(_zaman);
        EniyiZamanGüncelle();
        
        _eniyiZaman.text = ZamanıSaateÇevir(eniyiZaman);
        _zamanlayıcı.text = ZamanıSaateÇevir(_zaman);
        _scrambler.GenerateNewScramble();
        
        zamanListesiGüncelle();
        
    }
    void ZamanlayıcıyıBaşlat(){
        _artı2Kilit = false;
        _dnfKilit = false;
        _zamanlayıcı.color = Color.white;
        _zaman = 0;
        _basılıTutmaSüresi = 0;
        _başladı = true;
        _başlatmaİçinBasılıTutma = false;
        _çözümSayısı++;
        EkrandanKaldır();
    }
    void Çek() {
        if (!_başladı) { // eğer daha başlamadıysa başlama sürecini durdur
            _başlatmaİçinBasılıTutma = false;
            _basılıTutmaSüresi = 0;
            _zamanlayıcı.color = Color.white;
        }
        if (_başlayabilir) { // eğer başlatılabilirse başlat
            ZamanlayıcıyıBaşlat();
        }
    }
    public void DNF()
    {
        if (_dnfKilit) return;
        _zamanlar.RemoveAt(_zamanlar.Count - 1);
        _zamanlayıcı.text = "DNF";
        _artı2Kilit = true;
        zamanListesiGüncelle();
        EniyiZamanGüncelle();
        //_dnfKilit = true; // (gerek yok ama ne olur ne olmaz elde bulunsun)
    }
    public void artı2(){
        if (_artı2Kilit) return;
        float şuankizaman;
        şuankizaman = _zaman;
        şuankizaman += 2;

        _zamanlayıcı.text = ZamanıSaateÇevir(şuankizaman);
        _zamanlar.RemoveAt(_zamanlar.Count - 1);
        _zamanlar.Add(şuankizaman);

        zamanListesiGüncelle();
        EniyiZamanGüncelle();
    }
    public void _Reset(){
        _zamanlar.Clear();
        _zamanlayıcı.text = "0.00";
        _ort5Text.text = "-";
        _ORT10Text.text = "-";
        _eniyiZaman.text = "-";
        _çözümSayısı = 0;
        eniyiZaman = 0;
        ORT = 0;
        ORT5 = 0;
        ORT10 = 0;
        _scrambler.GenerateNewScramble();
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
    #endregion
}
