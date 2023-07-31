using System.Diagnostics.CodeAnalysis;

namespace Metin_Matematigi_CS
{
    public struct SayiMetni
    {

        // Değişkenler
        private bool _Isaret = true;
        private List<char> _TamKisim = new List<char>();
        private List<char> _OndalikliKisim = new List<char>();

        // Özellikler
        public bool Isaret
        {
            get
            {
                return _Isaret;
            }
            set
            {
                _Isaret = value;
            }
        }
        public string TamKisim
        {
            get
            {
                return new string(_TamKisim.ToArray());
            }
            set
            {
                _TamKisim.Clear();
                // Girilen Değer Sayı Değilse 0 Yaz
                if (TamSayiMi(value))
                {
                    int i;
                    // Varsa Baştaki Gereksiz Sıfırları Alma
                    for (i = 0; i < value.Length - 1; i++)
                        if (value[i] != '0')
                            break;

                    for (; i < value.Length; i++)
                        _TamKisim.Add(value[i]);
                }
                else
                    _TamKisim.Add('0');
            }
        }
        public string OndalikliKisim
        {
            get
            {
                return new string(_OndalikliKisim.ToArray());
            }
            set
            {
                _OndalikliKisim.Clear();
                // Girilen Değer Sayı Değilse 0 Yaz
                if (TamSayiMi(value))
                {
                    int i;
                    // Varsa Sondaki Sıfırları Alma
                    for (i = value.Length - 1; i >= 1; i--)
                        if (value[i] != '0')
                            break;

                    for (; i >= 0; i--)
                        _OndalikliKisim.Insert(0, value[i]);
                }
                else
                    _OndalikliKisim.Add('0');

            }
        }

        // Yapıcı Metotlar
        public SayiMetni()
        {
            TamKisim = "0";
            OndalikliKisim = "0";
        }
        public SayiMetni(bool Isaret = true, string TamKisim = "0", string OndalikliKisim = "0")
        {
            this.Isaret = Isaret;
            this.TamKisim = TamKisim;
            this.OndalikliKisim = OndalikliKisim;
        }
        public SayiMetni(string sayi)
        {
            int i = 0;

            if (sayi[0] == '+' || sayi[0] == '-')
            {
                i = 1;
                Isaret = (sayi[0] == '+');
            }

            string[] sonuc = sayi.Substring(i).Split('.');

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

        // Geçersiz Kılınan Metotlar
        public override string ToString()
        {
            bool ondalikGoster = false;
            if (OndalikliKisim.Length > 0)
                if (!(OndalikliKisim.Length == 1 && OndalikliKisim[0] == '0'))
                    ondalikGoster = true;

            if (!Isaret && ondalikGoster)
                return "-" + TamKisim + "." + OndalikliKisim;
            else if (!Isaret && !ondalikGoster)
                return "-" + TamKisim;
            else if (Isaret && ondalikGoster)
                return TamKisim + "." + OndalikliKisim;
            else // if (Isaret && !ondalikGoster)
                return TamKisim;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }

        // Aritmetiksel Metotlar
        private static SayiMetni Topla(SayiMetni s1, SayiMetni s2)
        {
            // Pozitif + Negatif -> Pozitif - Pozitif
            if (s1.Isaret && !s2.Isaret)
            {
                s2.Isaret = true;
                return s1 - s2;
            }

            // Negatif + Pozitif -> Pozitif - Pozitif
            if (!s1.Isaret && s2.Isaret)
            {
                s1.Isaret = true;
                return s2 - s1;
            }

            // Aynı İşarete Sahipler

            List<char> ondalikli = new List<char>();
            int ondalikUzunluk = Math.Max(s1.OndalikliKisim.Length, s2.OndalikliKisim.Length);

            // Ondalıklı Kısımları Topla
            int elde = 0;
            for (int i = ondalikUzunluk - 1; i >= 0; i--)
            {
                int rakam1 = 0;
                int rakam2 = 0;
                if (s1.OndalikliKisim.Length > i) rakam1 = s1.OndalikliKisim[i] - '0';
                if (s2.OndalikliKisim.Length > i) rakam2 = s2.OndalikliKisim[i] - '0';
                int rakamSonuc = rakam1 + rakam2 + elde;
                elde = rakamSonuc / 10;
                rakamSonuc = rakamSonuc % 10;
                ondalikli.Insert(0, (char)(rakamSonuc + '0'));
            }

            List<char> tam = new List<char>();
            int tamUzunluk = Math.Max(s1.TamKisim.Length, s2.TamKisim.Length);

            // Tam Kısımları Topla
            for (int i = 0; i < tamUzunluk; i++)
            {
                int rakam1 = 0;
                int rakam2 = 0;
                if (s1.TamKisim.Length - 1 - i >= 0) rakam1 = s1.TamKisim[s1.TamKisim.Length - 1 - i] - '0';
                if (s2.TamKisim.Length - 1 - i >= 0) rakam2 = s2.TamKisim[s2.TamKisim.Length - 1 - i] - '0';
                int rakamSonuc = rakam1 + rakam2 + elde;
                elde = rakamSonuc / 10;
                rakamSonuc = rakamSonuc % 10;
                tam.Insert(0, (char)(rakamSonuc + '0'));
            }
            while (elde > 0)
            {
                tam.Insert(0, (char)((elde % 10) + '0'));
                elde /= 10;
            }

            return new SayiMetni(s1.Isaret, new string(tam.ToArray()), new string(ondalikli.ToArray()));
        }
        private static SayiMetni Cikart(SayiMetni s1, SayiMetni s2)
        {
            // İkinci Sayı Negatifse Toplama İşlemidir
            if (!s2.Isaret)
            {
                s2.Isaret = true;
                return s1 + s2;
            }

            // Negatif - Pozitif -> Negatif + Negatif
            if (!s1.Isaret)
            {
                s2.Isaret = false;
                return s1 + s2;
            }

            // Pozitif - Pozitif
            if (s1 < s2)
            {
                SayiMetni sonuc = s2 - s1; // Büyükten Küçüğü Çıkart
                sonuc.Isaret = false; // Sonucu Negatif Yap
                return sonuc;
            }

            // s1 >= s2

            // Ondalıklı Kısmı Çıkart
            List<char> ondalikli = new List<char>();
            int ondalikUzunluk = Math.Max(s1.OndalikliKisim.Length, s2.OndalikliKisim.Length);
            int elde = 0;
            for (int i = ondalikUzunluk - 1; i >= 0; i--)
            {
                int rakam1 = 0;
                int rakam2 = 0;
                if (s1.OndalikliKisim.Length > i) rakam1 = s1.OndalikliKisim[i] - '0';
                if (s2.OndalikliKisim.Length > i) rakam2 = s2.OndalikliKisim[i] - '0';
                int rakamSonuc = (rakam1 - elde) - rakam2;
                elde = 0;
                if (rakamSonuc < 0)
                {
                    elde = (-rakamSonuc) / 10 + 1;
                    rakamSonuc = 10 - (-rakamSonuc) % 10;
                    if (rakamSonuc == 0) elde--;
                }
                ondalikli.Insert(0, (char)(rakamSonuc + '0'));
            }

            // Tam Kısmı Çıkart
            List<char> tam = new List<char>();
            int tamUzunluk = Math.Max(s1.TamKisim.Length, s2.TamKisim.Length);
            for (int i = 0; i < tamUzunluk; i++)
            {
                int rakam1 = 0;
                int rakam2 = 0;
                if (s1.TamKisim.Length - 1 - i >= 0) rakam1 = s1.TamKisim[s1.TamKisim.Length - 1 - i] - '0';
                if (s2.TamKisim.Length - 1 - i >= 0) rakam2 = s2.TamKisim[s2.TamKisim.Length - 1 - i] - '0';
                int rakamSonuc = (rakam1 - elde) - rakam2;
                elde = 0;
                if (rakamSonuc < 0)
                {
                    elde = (-rakamSonuc) / 10 + 1;
                    rakamSonuc = 10 - (-rakamSonuc) % 10;
                    if (rakamSonuc == 0) elde--;
                }
                tam.Insert(0, (char)(rakamSonuc + '0'));
            }

            return new SayiMetni(true, new string(tam.ToArray()), new string(ondalikli.ToArray()));
        }
        private static SayiMetni Carp(SayiMetni s1, SayiMetni s2)
        {
            if (s1 == "0") return new SayiMetni("0");
            if (s2 == "0") return new SayiMetni("0");
            if (s1 == "1") return s2;
            if (s2 == "1") return s1;
            if (s1 == "-1") { s2.Isaret = !s2.Isaret; return s2; }
            if (s2 == "-1") { s1.Isaret = !s1.Isaret; return s1; }

            SayiMetni sayi1 = new SayiMetni(s1.TamKisim + s1.OndalikliKisim); // 15.78 -> 1578
            SayiMetni sayi2 = new SayiMetni(s2.TamKisim + s2.OndalikliKisim); // 15.78 -> 1578

            SayiMetni sonuc = new SayiMetni("0");
            for (int i = 0; i < sayi2.TamKisim.Length; i++)
            {
                List<char> tam = new List<char>(); // Geçici Toplamlar
                int elde = 0;
                int rakam2 = sayi2.TamKisim[sayi2.TamKisim.Length - 1 - i] - '0';
                for (int j = 0; j < sayi1.TamKisim.Length; j++)
                {
                    int rakam1 = sayi1.TamKisim[sayi1.TamKisim.Length - 1 - j] - '0';
                    int rakamSonuc = rakam1 * rakam2 + elde;
                    elde = rakamSonuc / 10;
                    rakamSonuc %= 10;
                    tam.Insert(0, (char)(rakamSonuc + '0'));
                }
                while (elde > 0)
                {
                    tam.Insert(0, (char)(elde % 10 + '0'));
                    elde /= 10;
                }
                for (int j = 0; j < i; j++) tam.Add('0');

                sonuc += new SayiMetni(true, new string(tam.ToArray()));
            }

            // Tam Sayıya Dönüştürmek için 10^n ile Çarpmıştık, Şimdi 10^n e Bölüyoruz
            int uzunluk = s1.OndalikliKisim.Length + s2.OndalikliKisim.Length;

            string tamS = sonuc.TamKisim.Substring(0, sonuc.TamKisim.Length - uzunluk);
            string ondalikliS = sonuc.TamKisim.Substring(sonuc.TamKisim.Length - uzunluk, uzunluk);
            bool isaretS = (s1.Isaret == s2.Isaret);

            return new SayiMetni(isaretS, tamS, ondalikliS);
        }
        private static SayiMetni Bol(SayiMetni s1, SayiMetni s2, int ondalikBasamak = 20)
        {
            if (s1 == "0") return new SayiMetni("0");
            if (s2 == "0") throw new Exception("Sıfıra Bölünemez");
            if (s2 == "1") return s1;
            if (s2 == "-1") { s1.Isaret = !s1.Isaret; return s1; }

            // s1 = "1578.45", s2 = "21.496" --> sayi1 = "1578450", sayi2 = "21496"
            int uzunluk = Math.Max(s1.OndalikliKisim.Length, s2.OndalikliKisim.Length);
            List<char> sayi1 = new List<char>(s1.TamKisim);
            List<char> bolen = new List<char>(s2.TamKisim);
            for (int i = 0; i < uzunluk; i++)
            {
                if (i < s1.OndalikliKisim.Length) sayi1.Add(s1.OndalikliKisim[i]);
                else sayi1.Add('0');

                if (i < s2.OndalikliKisim.Length) bolen.Add(s2.OndalikliKisim[i]);
                else bolen.Add('0');
            }

            // Bölme İşlemi
            List<char> tam = new List<char>();
            List<char> ondalikli = new List<char>();
            bool isaret = s1.Isaret == s2.Isaret;
            // ...
            List<char> bolunen = new List<char>();
            bolunen.Add(sayi1[0]);

            int index = 1;
            bool virgul = false;
            while (true)
            {
                // Bölünen Bölümden Kuçükse
                if (new SayiMetni(new string(bolunen.ToArray())) < new SayiMetni(new string(bolen.ToArray())))
                {
                    if (sayi1.Count > index) // Yukarıdan Sayı Getir
                    {
                        bolunen.Add(sayi1[index]);
                    }
                    else // Yukarıda Sayı Yoksa Virgül Ekle ve 0 Getir
                    {
                        bolunen.Add('0');
                        virgul = true;
                    }
                    index++;

                    // Hala Küçükse Sonuca Sıfır Ekle
                    if (new SayiMetni(new string(bolunen.ToArray())) < new SayiMetni(new string(bolen.ToArray())))
                    {
                        if (virgul) ondalikli.Add('0');
                        else tam.Add('0');

                        if (ondalikli.Count == ondalikBasamak + 1)
                        {
                            // Yuvarla
                            if (ondalikli[ondalikBasamak] > '5') ondalikli[ondalikBasamak - 1]++;
                            ondalikli.RemoveAt(ondalikBasamak);
                            break;
                        }
                    }
                }
                else
                {
                    SayiMetni carpim = new SayiMetni(new string(bolen.ToArray()));
                    int sayi = 0;
                    while (carpim <= new SayiMetni(new string(bolunen.ToArray())))
                    {
                        sayi++;
                        carpim += new SayiMetni(new string(bolen.ToArray()));
                    }

                    SayiMetni yeniBolunen = new SayiMetni(new string(bolunen.ToArray())) + new SayiMetni(new string(bolen.ToArray())) - carpim;
                    bolunen.Clear();
                    for (int i = 0; i < yeniBolunen.TamKisim.Length; i++) bolunen.Add(yeniBolunen.TamKisim[i]);
                    if (virgul) ondalikli.Add((char)(sayi + '0'));
                    else tam.Add((char)(sayi + '0'));

                    if (ondalikli.Count == ondalikBasamak + 1)
                    {
                        // Yuvarla
                        if (ondalikli[ondalikBasamak] > '5') ondalikli[ondalikBasamak - 1]++;
                        ondalikli.RemoveAt(ondalikBasamak);
                        break;
                    }
                }
            }


            return new SayiMetni(isaret, new string(tam.ToArray()), new string(ondalikli.ToArray()));
        }
        public static SayiMetni UsAl(SayiMetni taban, SayiMetni us)
        {
            if (!us.Isaret) return new SayiMetni("-1");

            SayiMetni depoUs = us;
            TamSayiyaDonustur(depoUs);

            SayiMetni depoTaban = taban;
            SayiMetni sonuc = new SayiMetni("+1");

            while (depoUs > "0")
            {
                // Us Değeri Çiftse
                if ((depoUs.TamKisim[depoUs.TamKisim.Length - 1] - '0') % 2 == 0)
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
        public static SayiMetni UsAl(string taban, string us)
        {
            return UsAl(new SayiMetni(taban), new SayiMetni(us));
        }
        public static SayiMetni Faktoriyel(SayiMetni sayi)
        {
            SayiMetni depoSayi = sayi;
            TamSayiyaDonustur(depoSayi);

            if (!depoSayi.Isaret) return new SayiMetni("-1");
            if (depoSayi < "2") return new SayiMetni("+1");

            SayiMetni sonuc = depoSayi;

            while (depoSayi > "2")
            {
                depoSayi--;
                sonuc *= depoSayi;
            }

            return sonuc;
        }
        public static SayiMetni Faktoriyel(string sayi)
        {
            return Faktoriyel(new SayiMetni(sayi));
        }
        public static SayiMetni Karekok(SayiMetni sayi)
        {
            if (sayi < "0") return new SayiMetni("-1");

            SayiMetni kok = (sayi + "1") / "2";

            while (true)
            {
                SayiMetni depo = (kok + (sayi / kok)) / "2";
                if (depo == kok) return kok;
                kok = depo;
            }
        }
        public static SayiMetni Karekok(string sayi)
        {
            return Karekok(new SayiMetni(sayi));
        }

        // Aritmetiksel Operatörler
        public static SayiMetni operator +(SayiMetni s1, SayiMetni s2)
        {
            return Topla(s1, s2);
        }
        public static SayiMetni operator +(string s1, SayiMetni s2)
        {
            return Topla(new SayiMetni(s1), s2);
        }
        public static SayiMetni operator +(SayiMetni s1, string s2)
        {
            return Topla(s1, new SayiMetni(s2));
        }
        public static SayiMetni operator -(SayiMetni s1, SayiMetni s2)
        {
            return Cikart(s1, s2);
        }
        public static SayiMetni operator -(string s1, SayiMetni s2)
        {
            return Cikart(new SayiMetni(s1), s2);
        }
        public static SayiMetni operator -(SayiMetni s1, string s2)
        {
            return Cikart(s1, new SayiMetni(s2));
        }
        public static SayiMetni operator *(SayiMetni s1, SayiMetni s2)
        {
            return Carp(s1, s2);
        }
        public static SayiMetni operator *(string s1, SayiMetni s2)
        {
            return Carp(new SayiMetni(s1), s2);
        }
        public static SayiMetni operator *(SayiMetni s1, string s2)
        {
            return Carp(s1, new SayiMetni(s2));
        }
        public static SayiMetni operator /(SayiMetni s1, SayiMetni s2)
        {
            return Bol(s1, s2);
        }
        public static SayiMetni operator /(string s1, SayiMetni s2)
        {
            return Bol(new SayiMetni(s1), s2);
        }
        public static SayiMetni operator /(SayiMetni s1, string s2)
        {
            return Bol(s1, new SayiMetni(s2));
        }
        public static SayiMetni operator ++(SayiMetni s1)
        {
            return s1 + "1";
        }
        public static SayiMetni operator --(SayiMetni s1)
        {
            return s1 - "1";
        }

        // Karşılaştırma Metotları
        private static bool Karsilastir(SayiMetni s1, SayiMetni s2, bool EsitMi) //Büyükse "true" döndürür. Eşitse "EsitMi" döndürür
        {
            if (s1.Isaret && !s2.Isaret) return true; // Pozitif Negatiften Her Zaman Büyüktür
            if (!s1.Isaret && s2.Isaret) return false; // Negatif Pozitiften Her Zaman Küçüktür

            // Aynı İşarete Sahipler

            if (s1.TamKisim.Length > s2.TamKisim.Length) return true == s1.Isaret; // Basamak Sayısı Büyükse, Sayı Pozitifse Büyük, Sayı Negatifse Küçüktür
            if (s2.TamKisim.Length > s1.TamKisim.Length) return false == s1.Isaret; // Basamak Sayısı Küçükse, Sayı Pozitifse Küçük, Sayı Negatifse Büyüktür

            // Aynı Basamak Sayısına Sahipler

            // Basamaklarındaki Sayıları Karşılaştır
            for (int i = 0; i < s1.TamKisim.Length; i++)
                if (s1.TamKisim[i] > s2.TamKisim[i]) return true == s1.Isaret;
                else if (s2.TamKisim[i] > s1.TamKisim[i]) return false == s1.Isaret;

            // Tam Kısımları Aynı

            // Ondalıklı Kısımdaki Sayıları Karşılaştır
            int uzunluk = Math.Min(s1.OndalikliKisim.Length, s2.OndalikliKisim.Length);
            for (int i = 0; i < uzunluk; i++)
                if (s1.OndalikliKisim[i] > s2.OndalikliKisim[i]) return true == s1.Isaret;
                else if (s2.OndalikliKisim[i] > s1.OndalikliKisim[i]) return false == s1.Isaret;

            // Ondalıklı Kısımlarında Ortak Sayılar Aynı

            // Fazla Basamağı Kalan Var mı
            if (s1.OndalikliKisim.Length > s2.OndalikliKisim.Length) return true == s1.Isaret;
            if (s1.OndalikliKisim.Length < s2.OndalikliKisim.Length) return false == s1.Isaret;

            return EsitMi; // İki Sayı Birbirine Eşit
        }
        private static bool BuyukMu(SayiMetni s1, SayiMetni s2)
        {
            return Karsilastir(s1, s2, false);
        }
        private static bool BuyukEsitMi(SayiMetni s1, SayiMetni s2)
        {
            return Karsilastir(s1, s2, true);
        }
        private static bool KucukMu(SayiMetni s1, SayiMetni s2)
        {
            return !BuyukEsitMi(s1, s2);
        }
        private static bool KucukEsitMi(SayiMetni s1, SayiMetni s2)
        {
            return !BuyukMu(s1, s2);
        }
        private static bool EsitMi(SayiMetni s1, SayiMetni s2)
        {
            if (s1.Isaret != s2.Isaret) return false;
            if (s1.TamKisim.Length != s2.TamKisim.Length) return false;
            if (s1.OndalikliKisim.Length != s2.OndalikliKisim.Length) return false;

            for (int i = 0; i < s1.TamKisim.Length; i++)
                if (s1.TamKisim[i] != s2.TamKisim[i])
                    return false;

            for (int i = 0; i < s1.OndalikliKisim.Length; i++)
                if (s1.OndalikliKisim[i] != s2.OndalikliKisim[i])
                    return false;

            return true;
        }
        private static bool FarkliMi(SayiMetni s1, SayiMetni s2)
        {
            return !EsitMi(s1, s2);
        }

        // Karşılaştırma Operatörleri
        public static bool operator ==(SayiMetni s1, SayiMetni s2)
        {
            return EsitMi(s1, s2);
        }
        public static bool operator ==(SayiMetni s1, string s2)
        {
            return EsitMi(s1, new SayiMetni(s2));
        }
        public static bool operator ==(string s1, SayiMetni s2)
        {
            return EsitMi(new SayiMetni(s1), s2);
        }
        public static bool operator !=(SayiMetni s1, SayiMetni s2)
        {
            return FarkliMi(s1, s2);
        }
        public static bool operator !=(SayiMetni s1, string s2)
        {
            return FarkliMi(s1, new SayiMetni(s2));
        }
        public static bool operator !=(string s1, SayiMetni s2)
        {
            return FarkliMi(new SayiMetni(s1), s2);
        }
        public static bool operator >=(SayiMetni s1, SayiMetni s2)
        {
            return BuyukEsitMi(s1, s2);
        }
        public static bool operator >=(SayiMetni s1, string s2)
        {
            return BuyukEsitMi(s1, new SayiMetni(s2));
        }
        public static bool operator >=(string s1, SayiMetni s2)
        {
            return BuyukEsitMi(new SayiMetni(s1), s2);
        }
        public static bool operator <=(SayiMetni s1, SayiMetni s2)
        {
            return KucukEsitMi(s1, s2);
        }
        public static bool operator <=(SayiMetni s1, string s2)
        {
            return KucukEsitMi(s1, new SayiMetni(s2));
        }
        public static bool operator <=(string s1, SayiMetni s2)
        {
            return KucukEsitMi(new SayiMetni(s1), s2);
        }
        public static bool operator >(SayiMetni s1, SayiMetni s2)
        {
            return BuyukMu(s1, s2);
        }
        public static bool operator >(SayiMetni s1, string s2)
        {
            return BuyukMu(s1, new SayiMetni(s2));
        }
        public static bool operator >(string s1, SayiMetni s2)
        {
            return BuyukMu(new SayiMetni(s1), s2);
        }
        public static bool operator <(SayiMetni s1, SayiMetni s2)
        {
            return KucukMu(s1, s2);
        }
        public static bool operator <(SayiMetni s1, string s2)
        {
            return KucukMu(s1, new SayiMetni(s2));
        }
        public static bool operator <(string s1, SayiMetni s2)
        {
            return KucukMu(new SayiMetni(s1), s2);
        }

        // Diğer Metotlar
        public static bool TamSayiMi(string metin)
        {
            for (int i = 0; i < metin.Length; i++)
                if (metin[i] < '0' || metin[i] > '9')
                    return false;
            return metin.Length > 0; // "" -> false
        }
        public static SayiMetni TamSayiyaDonustur(SayiMetni s1)
        {
            s1.OndalikliKisim = "0";
            return s1;
        }
        public static SayiMetni RastgeleAralik(SayiMetni min, SayiMetni maks)
        {
            if (min > maks) return RastgeleAralik(maks, min);
            SayiMetni fark = maks - min; // min-maks arası değil, 0-fark arası sayı belirleyeceğiz
            SayiMetni sonuc = new SayiMetni("0");

            int kacBit = 0;
            // Farkın Kaç Bit Olduğunu Hesapla
            {
                SayiMetni depo = fark;
                while (depo > "0")
                {
                    if ((depo.TamKisim[depo.TamKisim.Length - 1] - '0') % 2 == 1) depo--;
                    depo /= "2";
                    kacBit++;
                }
            }

            Random random = new Random();
            while (true)
            {
                sonuc = new SayiMetni("0");
                for (int i = 0; i < kacBit; i++)
                {
                    SayiMetni bit = new SayiMetni(new string(new char[1] { (char)(random.Next(0, 2) + '0') }));
                    sonuc = sonuc * "2" + bit;
                }

                if (sonuc <= fark) return sonuc + min;
            }
        }
        public static SayiMetni RastgeleAralik(string min, string maks)
        {
            return RastgeleAralik(new SayiMetni(min), new SayiMetni(maks));
        }

    }
}
