namespace Metin_Matematigi_CS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SayiMetni.OndalikHassasiyet = 100;
            SayiMetni s1 = "-26.84";
            SayiMetni s2 = "-543583490.004543500";
            SayiMetni s3 = "0";
            SayiMetni s4 = "02546.437500";
            string deneme = (string) s2;
            Console.WriteLine("Sayi Metni Sinifindan String Sinifina Acik Donusturme Yapilabilir: " + deneme);
            
            Console.WriteLine("{0} Sayisinin 3. Basamagi -> {1}", s4, s4[3]);
            Console.WriteLine("{0} Sayisinin Virgulden Sonraki 4. Basamagi -> {1}", s4, s4[-4]);

            Console.WriteLine("{0} + {1} = {2}", "-465123", s2, "-465123" + s2);
            Console.WriteLine("{0} + {1} = {2}", s1, s2, s1 + s2);
            Console.WriteLine("{0} - {1} = {2}", s1, s2, s1 - s2);
            Console.WriteLine("{0} * {1} = {2}", s1, s2, s1 * s2);
            Console.WriteLine("{0} / {1} = {2}", s1, s2, s2 / s1);
            Console.WriteLine("{0} % {1} = {2}", s1, s2, s2 % s1);

            Console.WriteLine("{0} == {1} -> {2}", s1, s3, s1 == s3);
            Console.WriteLine("{0} != {1} -> {2}", s1, s3, s1 != s3);
            Console.WriteLine("{0} >= {1} -> {2}", s1, s3, s1 >= s3);
            Console.WriteLine("{0} <= {1} -> {2}", s1, s3, s1 <= s3);
            Console.WriteLine("{0} > {1} -> {2}", s1, s3, s1 > s3);
            Console.WriteLine("{0} < {1} -> {2}", s1, s3, s1 < s3);

            Console.WriteLine("{0} uzeri {1} = {2}", "5748939", "30000", SayiMetni.UsAl("5748939", "30000"));
            Console.WriteLine("30000 Faktoriyel = {0}", SayiMetni.Faktoriyel("30000"));
            Console.WriteLine("30000 Karekok = {0}", SayiMetni.Karekok("30000"));
            Console.WriteLine($"{s1} Tam Sayi -> {SayiMetni.TamSayiyaDonustur(s1)}");
            Console.WriteLine("{0} ile {1} Arasinda Rastgele Sayi -> {2}", "-48212002555435353248", "+95138494120284520548484", SayiMetni.RastgeleAralik("-48212002555435353248", "+95138494120284520548484"));
            
            Console.ReadKey();
        }
    }
}