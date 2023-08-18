using System.Diagnostics.CodeAnalysis;

namespace Metin_Matematigi_CS
{
    public class SayiMetni : IComparable
    {

        #region Değişkenler
        private readonly List<uint> _TamKisim = new ();
        private readonly List<uint> _OndalikliKisim = new ();
        #endregion

        #region Özellikler
        public bool Isaret { get; set; }
        public string TamKisim
        {
            get
            {
                string sonuc = _TamKisim[0].ToString();
                for (int i = 1; i < _TamKisim.Count; i++)
                {
                    string deger = _TamKisim[i].ToString();
                    deger = new string('0', 8 - deger.Length) + deger;
                    sonuc += deger;
                }
                return sonuc;
            }
            set
            {
                _TamKisim.Clear();
                // Girilen Değer Sayı Değilse 0 Yaz
                if (TamSayiMi(value))
                {
                    int bitisPos = value.Length;
                    while (bitisPos > 0)
                    {
                        int basPos = bitisPos - 8;
                        if (basPos < 0) basPos = 0;

                        _TamKisim.Insert(0, Convert.ToUInt32(value[basPos..bitisPos]));

                        bitisPos -= 8;
                    }
                }
                else
                    _TamKisim.Add(0);

                // Baştaki Gereksiz Sıfırları sil
                while (_TamKisim.Count > 1 && _TamKisim[0] == 0) _TamKisim.RemoveAt(0);
                
            }
        }
        public string OndalikliKisim
        {
            get
            {
                string sonuc = "";
                foreach (uint sayi in _OndalikliKisim)
                {
                    string deger = sayi.ToString();
                    deger = new string('0', 8 - deger.Length) + deger;
                    sonuc += deger;

                }
                int i;
                for (i = 1; i < sonuc.Length && sonuc[^i] == '0'; i++);

                return sonuc[..^(i-1)];
            }
            set
            {
                _OndalikliKisim.Clear();
                // Girilen Değer Sayı Değilse 0 Yaz
                if (TamSayiMi(value))
                {
                    int basPos = 0;
                    while (basPos < value.Length)
                    {
                        int bitisPos = basPos + 8;
                        if (bitisPos > value.Length) bitisPos = value.Length;

                        string deger = value[basPos..bitisPos];
                        deger += new string('0', 8 - deger.Length);

                        _OndalikliKisim.Add(Convert.ToUInt32(deger));

                        basPos += 8;
                    }
                }
                else
                    _OndalikliKisim.Add(0);

                // Sondaki Gereksiz Sıfırları sil
                while (_OndalikliKisim.Count > 1 && _OndalikliKisim[^1] == 0)
                    _OndalikliKisim.RemoveAt(_OndalikliKisim.Count-1);
            }
        }
        public int this[int i]
        {
            get
            {
                if (i == 0) throw new Exception("0 Indisi Bulunmamakta");

                // Ondalıklı Sayılar
                if (i < 0)
                {
                    i = -i;
                    int listIndex = (i - 1) / 8;
                    int stringIndex = (i - 1) % 8;
                    if (listIndex < _OndalikliKisim.Count)
                    {
                        string sayi = _OndalikliKisim[listIndex].ToString();
                        sayi = new string('0', 8-sayi.Length) + sayi;
                        if (stringIndex < sayi.Length)
                            return sayi[stringIndex] - '0';
                    }
                    return 0;
                }
                // Tam Kısım
                else
                {
                    int listIndex = (i - 1) / 8;
                    int stringIndex = (i - 1) % 8;
                    if (listIndex < _TamKisim.Count)
                    {
                        string sayi = _TamKisim[^(listIndex+1)].ToString();
                        if (stringIndex < sayi.Length)
                            return sayi[^(stringIndex+1)] - '0';
                    }
                    return 0;
                }
            }
        }
        #endregion

        #region Yapıcı Metotlar
        private SayiMetni(bool isaret = true, string tam = "0", string ondalikli = "0")
        {
            Isaret = isaret;
            TamKisim = tam;
            OndalikliKisim = ondalikli;
        }
        private SayiMetni(string sayi)
        {
            int i = 0;
            Isaret = true;

            if (sayi[0] == '+' || sayi[0] == '-')
            {
                i = 1;
                Isaret = (sayi[0] == '+');
            }

            string[] sonuc = sayi[i..].Split('.');

            if (sonuc.Length == 2)
            {
                TamKisim = sonuc[0];
                OndalikliKisim = sonuc[1];
            }
            else if (sonuc.Length == 1)
            {
                TamKisim = sonuc[0];
                OndalikliKisim = "0";
            }
            else
            {
                TamKisim = "0";
                OndalikliKisim = "0";
            }
        }
        #endregion

        #region Geçersiz Kılınan Metotlar
        public override string ToString()
        {
            bool ondalikGoster = OndalikliKisim != "0";

            if (!Isaret && ondalikGoster)
                return "-" + TamKisim + "." + OndalikliKisim;
            else if (!Isaret && !ondalikGoster)
                return "-" + TamKisim;
            else if (Isaret && ondalikGoster)
                return TamKisim + "." + OndalikliKisim;
            else //if (Isaret && !ondalikGoster)
                return TamKisim;
        }
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals([NotNullWhen(true)] object? obj) =>base.Equals(obj);
        #endregion

        #region Aritmetiksel Metotlar
        public static SayiMetni Topla(SayiMetni sayi1, SayiMetni sayi2)
        {
            // Pozitif + Negatif -> Pozitif - Pozitif
            if (sayi1.Isaret && !sayi2.Isaret)
                return sayi1 - (-sayi2);

            // Negatif + Pozitif -> Pozitif - Pozitif
            if (!sayi1.Isaret && sayi2.Isaret)
                return sayi2 - (-sayi1);

            // Aynı İşarete Sahiplerse

            int maxTam = Math.Max(sayi1._TamKisim.Count, sayi2._TamKisim.Count) + 1;
            int maxOnd = Math.Max(sayi1._OndalikliKisim.Count, sayi2._OndalikliKisim.Count);

            uint[] sayi = new uint[maxTam + maxOnd];

            for (int i = -maxOnd; i < maxTam; i++)
            {
                ulong basamak = 0;

                // İndis Numarasını Bul
                int indis = (i < 0) ? -i - 1 : i + 1;
                // Sayı1'in Basamağını Ekle
                if (i < 0 && indis < sayi1._OndalikliKisim.Count) basamak += sayi1._OndalikliKisim[indis];
                if (i >= 0 && indis <= sayi1._TamKisim.Count) basamak += sayi1._TamKisim[^indis];
                // Sayı2'nin Basamağını Ekle
                if (i < 0 && indis < sayi2._OndalikliKisim.Count) basamak += sayi2._OndalikliKisim[indis];
                if (i >= 0 && indis <= sayi2._TamKisim.Count) basamak += sayi2._TamKisim[^indis];
                // Sayıya Ekle
                for (int j = 0; basamak > 0; j++)
                {
                    basamak += sayi[^(i + j + maxOnd + 1)]; // İki Basamak Toplamı + O an Orada Yazan Değer
                    sayi[^(i + j + maxOnd + 1)] = (uint)(basamak % 100000000);
                    basamak /= 100000000;
                }
            }

            string sonuc = sayi1.Isaret ? "+" : "-";
            for (int i = 0; i < maxTam; i++)
                sonuc += new string('0', 8 - sayi[i].ToString().Length) + sayi[i].ToString();
            sonuc += ".";
            for (int i = maxTam; i < sayi.Length; i++)
                sonuc += new string('0', 8 - sayi[i].ToString().Length) + sayi[i].ToString();

            return sonuc;
        }
        private static SayiMetni Cikart(SayiMetni sayi1, SayiMetni sayi2)
        {
            // İkinci Sayı Negatifse Toplama İşlemidir
            if (!sayi2.Isaret)
                return sayi1 + (-sayi2);

            // Negatif - Pozitif -> Negatif + Negatif
            if (!sayi1.Isaret)
                return sayi1 + (-sayi2);

            // Pozitif - Pozitif: Büyükten Küçüğü Çıkart Sonucu Negatif Yap
            if (sayi1 < sayi2)
                return -(sayi2 - sayi1);

            // s1 >= s2 ve Pozitif - Pozitif

            int maxTam = Math.Max(sayi1._TamKisim.Count, sayi2._TamKisim.Count);
            int maxOnd = Math.Max(sayi1._OndalikliKisim.Count, sayi2._OndalikliKisim.Count);

            uint[] sayi = new uint[maxTam + maxOnd];

            long basamak = 0;
            for (int i = -maxOnd; i < maxTam; i++)
            {
                // İndis Numarasını Bul
                int indis = (i < 0) ? -i - 1 : i + 1;
                // Sayı1'in Basamağını Ekle
                if (i < 0 && indis < sayi1._OndalikliKisim.Count) basamak += sayi1._OndalikliKisim[indis];
                if (i >= 0 && indis <= sayi1._TamKisim.Count) basamak += sayi1._TamKisim[^indis];
                // Sayı2'nin Basamağını Ekle
                if (i < 0 && indis < sayi2._OndalikliKisim.Count) basamak -= sayi2._OndalikliKisim[indis];
                if (i >= 0 && indis <= sayi2._TamKisim.Count) basamak -= sayi2._TamKisim[^indis];
                // Sayıya Ekle
                basamak += sayi[^(i + maxOnd + 1)]; // İki Basamak Toplamı + O an Orada Yazan Değer
                int komsu;
                for (komsu = 0; basamak < 0; komsu++)
                    basamak += 100000000;
                sayi[^(i + maxOnd + 1)] = (uint)basamak;
                basamak = -komsu;
            }

            string sonuc = sayi1.Isaret ? "+" : "-";
            for (int i = 0; i < maxTam; i++)
                sonuc += new string('0', 8 - sayi[i].ToString().Length) + sayi[i].ToString();
            sonuc += ".";
            for (int i = maxTam; i < sayi.Length; i++)
                sonuc += new string('0', 8 - sayi[i].ToString().Length) + sayi[i].ToString();

            return sonuc;
        }
        private static SayiMetni Carp(SayiMetni sayi1, SayiMetni sayi2)
        {
            if (sayi1 == "0" || sayi2 == "0") return "0";

            if (sayi1 == "1") return sayi2;
            if (sayi2 == "1") return sayi1;

            if (sayi1 == "-1") return -sayi2;
            if (sayi2 == "-1") return -sayi1;


            int maxTam = sayi1._TamKisim.Count +  sayi2._TamKisim.Count;
            int maxOnd = sayi1._OndalikliKisim.Count + sayi2._OndalikliKisim.Count;

            uint[] TamKisim = new uint[maxTam];
            uint[] OndalikliKisim = new uint[maxOnd];

            // Sayıları Çarp
            for (int i = 0; i < sayi2._OndalikliKisim.Count + sayi2._TamKisim.Count; i++)
            {
                for (int j = 0; j < sayi1._OndalikliKisim.Count + sayi1._TamKisim.Count; j++)
                {
                    bool bool1 = false;
                    bool bool2 = false;
                    int indis1 = i + 1; if (indis1 > sayi2._OndalikliKisim.Count) { bool1 = true; indis1 -= sayi2._OndalikliKisim.Count; }
                    int indis2 = j + 1; if (indis2 > sayi1._OndalikliKisim.Count) { bool2 = true; indis2 -= sayi1._OndalikliKisim.Count; }

                    ulong sonuc = 1;
                    // Sayı2 ile Çarp
                    if (!bool1) sonuc *= sayi2._OndalikliKisim[^indis1];
                    else sonuc *= sayi2._TamKisim[^indis1];
                    // Sayı1 ile Çarp
                    if (!bool2) sonuc *= sayi1._OndalikliKisim[^indis2];
                    else sonuc *= sayi1._TamKisim[^indis2];

                    // Sonuça Yaz
                    bool bool3 = false;
                    int indis3 = i + j + 1; if (indis3 > maxOnd) { bool3 = true; indis3 -= maxOnd; }

                    if (!bool3)
                    {
                        sonuc += OndalikliKisim[^indis3];
                        OndalikliKisim[^indis3] = (uint)(sonuc % 100000000);
                    }
                    else
                    {
                        sonuc += TamKisim[^indis3];
                        TamKisim[^indis3] = (uint)(sonuc % 100000000);
                    }
                    bool bool4 = false;
                    int indis4 = i + j + 2; if (indis4 > maxOnd) { bool4 = true; indis4 -= maxOnd; }

                    if (!bool4)
                    {
                        OndalikliKisim[^indis4] += (uint)(sonuc / 100000000);
                    }
                    else
                    {
                        TamKisim[^indis4] += (uint)(sonuc / 100000000);
                    }
                }
            }

            string deger = sayi1.Isaret == sayi2.Isaret ? "+" : "-";
            foreach (var s in TamKisim)
                deger += new string('0', 8-s.ToString().Length) + s.ToString();
            deger += ".";
            foreach (var s in OndalikliKisim)
                deger += new string('0', 8 - s.ToString().Length) + s.ToString();

            return deger;
        }
        private static void Bol(SayiMetni sayi1, SayiMetni sayi2, int ondalikBasamak, out SayiMetni bolum, out SayiMetni kalan)
        {
            // sayi1 = 15.67 -> bolunen = 1567
            // sayi2 = 6.4   -> bolen   =  640
            // ondalikBasamak = 3 --> bolunen*=10^3
            int adet1 = sayi2.OndalikliKisim.Length - sayi1.OndalikliKisim.Length; if (adet1 < 0) adet1 = 0;
            int adet2 = sayi1.OndalikliKisim.Length - sayi2.OndalikliKisim.Length; if (adet2 < 0) adet2 = 0;

            string bolunen = sayi1.TamKisim + sayi1.OndalikliKisim + new string('0', adet1 + ondalikBasamak);
            string bolen = sayi2.TamKisim + sayi2.OndalikliKisim + new string('0', adet2);

            string sonuc = "0";
            SayiMetni sayi = "0";
            int index = 0;

            while (true)
            {
                if (sayi < bolen)
                {
                    if (index == bolunen.Length) break;
                    sayi = sayi*"10" + bolunen[index].ToString();
                    index++;
                    if (sayi < bolen)
                        sonuc += "0";
                } 
                else
                {
                    int adet = -1;
                    while (sayi >= "0") { sayi -= bolen; adet++; }
                    sayi += bolen;
                    sonuc += adet.ToString();
                }
            }

            // Bölümü Hesapla
            bolum = sonuc;
            bolum.Isaret = sayi1.Isaret == sayi2.Isaret;
            int uzunluk = ondalikBasamak;
            for (int i = 1; i <= uzunluk; i++)
            {
                var sonBasamak = '0';
                if (i <= bolum.TamKisim.Length)
                    sonBasamak = bolum.TamKisim[^i];
                bolum.OndalikliKisim = sonBasamak + bolum.OndalikliKisim;
            }
            if (uzunluk <= bolum.TamKisim.Length)
                bolum.TamKisim = bolum.TamKisim[..^(uzunluk)];
            else
                bolum.TamKisim = "0";

            // Kalanı Hesapla
            kalan = sayi;
            kalan.Isaret = sayi1.Isaret;
            uzunluk = ondalikBasamak + Math.Max(sayi1.OndalikliKisim.Length, sayi2.OndalikliKisim.Length);
            for (int i = 1; i <= uzunluk; i++)
            {
                var sonBasamak = '0';
                if (i <= kalan.TamKisim.Length)
                    sonBasamak = kalan.TamKisim[^i];
                kalan.OndalikliKisim = sonBasamak + kalan.OndalikliKisim;
            }
            if (uzunluk <= kalan.TamKisim.Length)
                kalan.TamKisim = kalan.TamKisim[..^(uzunluk)];
            else
                kalan.TamKisim = "0";

        }
        public static SayiMetni Bol(SayiMetni sayi1, SayiMetni sayi2, int ondalikBasamak = 20)
        {
            if (sayi1 == "0") return "0";
            if (sayi2 == "0") throw new Exception("Sıfıra Bölünemez");
            if (sayi2 == "1") return sayi1;
            if (sayi2 == "-1") return sayi2 * "-1";

            Bol(sayi1, sayi2, ondalikBasamak, out SayiMetni sonuc, out _);

            return sonuc;
        }
        public static SayiMetni UsAl(SayiMetni taban, SayiMetni us)
        {
            if (us.OndalikliKisim != "0") throw new Exception("Ondalıklı Üs Hesaplanamadı!");


            SayiMetni depoUs = us.ToString();
            if (!us.Isaret)
            {
                depoUs.Isaret = true;
                return "1" / UsAl(taban, depoUs);
            }

            SayiMetni depoTaban = taban;
            SayiMetni sonuc = "+1";

            while (depoUs > "0")
            {
                // Us Değeri Çiftse
                if (CiftMi(depoUs))
                {
                    depoUs /= "2";
                    depoTaban *= depoTaban;
                }
                else
                {
                    depoUs--;
                    sonuc *= depoTaban;
                }
            }

            return sonuc;
        }
        public static SayiMetni Faktoriyel(SayiMetni sayi)
        {
            if (!sayi.Isaret) throw new Exception("Negatif Sayının Faktöriyeli Hesaplanamadı!");
            if (sayi.OndalikliKisim != "0") throw new Exception("Ondalıklı Sayının Faktöriyeli Hesaplanamadı!");

            if (sayi < "2") return "+1";

            SayiMetni[] sayilar = new SayiMetni[1024];
            for (int i = 0; i < sayilar.Length; i++) sayilar[i] = "1";
            SayiMetni depoSayi = sayi;
            int index = 0;

            while (depoSayi >= "2")
            {
                sayilar[index] *= depoSayi;
                index++;
                if (index == sayilar.Length) index = 0;
                depoSayi--;
            }

            int kalan = sayilar.Length;
            while (kalan > 1)
            {
                for (int i = 0; i < kalan; i += 2)
                    sayilar[i / 2] = sayilar[i] * sayilar[i + 1];
                kalan /= 2;
            }

            return sayilar[0];
        }
        public static SayiMetni Karekok(SayiMetni sayi)
        {
            if (sayi < "0") throw new Exception("Negatif Sayının Karekökü Bulunamadı!");

            SayiMetni kok = (sayi + "1") / "2";

            while (true)
            {
                SayiMetni depo = (kok + (sayi / kok)) / "2";
                if (depo == kok) return kok;
                kok = depo.ToString();
            }
        }
        #endregion

        #region Aritmetiksel Operatörler
        public static SayiMetni operator +(SayiMetni s1, SayiMetni s2) => Topla(s1, s2);
        public static SayiMetni operator -(SayiMetni s1, SayiMetni s2) => Cikart(s1, s2);
        public static SayiMetni operator -(SayiMetni sayi)
        {
            SayiMetni sonuc = sayi.ToString();
            sonuc.Isaret = !sonuc.Isaret;
            return sonuc;
        }
        public static SayiMetni operator *(SayiMetni s1, SayiMetni s2) => Carp(s1, s2);
        public static SayiMetni operator /(SayiMetni s1, SayiMetni s2) => Bol(s1, s2);
        public static SayiMetni operator ++(SayiMetni s1) => s1 + "1";
        public static SayiMetni operator --(SayiMetni s1) => s1 - "1";
        public static SayiMetni operator %(SayiMetni s1, SayiMetni s2)
        {
            if (s2 == "2") return CiftMi(s1) ? "0" : "1";
            Bol(s1, s2, 0, out _, out SayiMetni kalan);
            return kalan;
        }
        #endregion

        #region Karşılaştırma Operatörleri
        public static bool operator ==(SayiMetni s1, SayiMetni s2) => s1.CompareTo(s2) == 0;
        public static bool operator !=(SayiMetni s1, SayiMetni s2) => s1.CompareTo(s2) != 0;
        public static bool operator >=(SayiMetni s1, SayiMetni s2) => s1.CompareTo(s2) >= 0;
        public static bool operator <=(SayiMetni s1, SayiMetni s2) => s1.CompareTo(s2) <= 0;
        public static bool operator >(SayiMetni s1, SayiMetni s2) => s1.CompareTo(s2) > 0;
        public static bool operator <(SayiMetni s1, SayiMetni s2) => s1.CompareTo(s2) < 0;
        #endregion

        #region Örtük Dönüştürme Operatörleri
        public static implicit operator SayiMetni(string sayi) => new SayiMetni(sayi);
        #endregion

        #region Açık Dönüştürme Operatörleri
        public static explicit operator string(SayiMetni sayiMetni) => sayiMetni.ToString();
        #endregion

        #region Diğer Metotlar
        public static bool TamSayiMi(string metin)
        {
            for (int i = 0; i < metin.Length; i++)
                if (metin[i] < '0' || metin[i] > '9')
                    return false;
            return metin.Length > 0; // "" -> false
        }
        public static SayiMetni TamSayiyaDonustur(SayiMetni sayi)
        {
            SayiMetni sonuc = sayi.ToString();
            sonuc.OndalikliKisim = "0";
            return sonuc;
        }
        public static bool CiftMi(SayiMetni sayi)
        {
            if ((sayi._OndalikliKisim[^1].ToString()[^1] - '0') % 2 != 0) return false;
            if ((sayi._TamKisim[^1].ToString()[^1] - '0') % 2 != 0) return false;
            return true;
        }
        public static SayiMetni RastgeleAralik(SayiMetni min, SayiMetni maks)
        {
            if (min.OndalikliKisim != "0" || maks.OndalikliKisim != "0") throw new Exception("Min ve Maks Parametreleri Tam Sayı Olmalıdır");
            if (min > maks) return RastgeleAralik(maks, min);
            SayiMetni fark = maks - min; // min-maks arası değil, 0-fark arası sayı belirleyeceğiz
            
            int kacBit = 0;
            // Farkın Kaç Bit Olduğunu Hesapla
            {
                SayiMetni depo = fark;
                while (depo > "0")
                {
                    depo = Bol(depo, "2", 0); // Tam Sayı Olarak Böl ( 3 / 2 == 0)
                    kacBit++;
                }
            }

            Random random = new Random();
            while (true)
            {
                SayiMetni sonuc = "0";
                for (int i = 0; i < kacBit; i++)
                {
                    SayiMetni bit = random.Next(0, 2).ToString();
                    sonuc *= "2";
                    sonuc += bit;
                }

                if (sonuc <= fark) return sonuc + min;
            }
        }
        #endregion

        #region Devralınan Metotlar
        public int CompareTo(object? obj)
        {
            if (obj == null) throw new ArgumentNullException("Parametre null Olamaz.");
            if (obj is not SayiMetni) throw new ArgumentException("Parametre, SayıMetni Türünde Olmalıdır.");

            SayiMetni s1 = this;
            SayiMetni s2 = (SayiMetni)obj;

            if (s1.Isaret != s2.Isaret) return s1.Isaret ? 1 : -1; // Pozitif Negatiften Her Zaman Büyüktür

            // Aynı İşarete Sahiplerse

            if (s1._TamKisim.Count > s2._TamKisim.Count) return s1.Isaret ? 1 : -1; // Basamak Sayısı Büyükse, Sayı Pozitifse Büyük, Sayı Negatifse Küçüktür
            if (s2._TamKisim.Count > s1._TamKisim.Count) return s1.Isaret ? -1 : 1; // Basamak Sayısı Küçükse, Sayı Pozitifse Küçük, Sayı Negatifse Büyüktür

            // Aynı Basamak Sayısına Sahiplerse

            // Basamaklarındaki Sayıları Karşılaştır
            for (int i = 0; i < s1._TamKisim.Count; i++)
                     if (s1._TamKisim[i] > s2._TamKisim[i]) return s1.Isaret ? 1 : -1;
                else if (s2._TamKisim[i] > s1._TamKisim[i]) return s1.Isaret ? -1 : 1;

            // Tam Kısımları Aynıysa

            // Ondalıklı Kısımdaki Sayıları Karşılaştır
            int uzunluk = Math.Min(s1._OndalikliKisim.Count, s2._OndalikliKisim.Count);
            for (int i = 0; i < uzunluk; i++)
                     if (s1._OndalikliKisim[i] > s2._OndalikliKisim[i]) return s1.Isaret ? 1 : -1;
                else if (s2._OndalikliKisim[i] > s1._OndalikliKisim[i]) return s1.Isaret ? -1 : 1;

            // Ondalıklı Kısımlarında Ortak Sayılar Aynıysa

            // Fazla Basamağı Kalan Var mı
            if (s1._OndalikliKisim.Count > s2._OndalikliKisim.Count) return s1.Isaret ? 1 : -1;
            if (s2._OndalikliKisim.Count > s1._OndalikliKisim.Count) return s1.Isaret ? -1 : 1;

            return 0; // Eşitler
        }
        #endregion

    }
}
