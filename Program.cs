namespace Metin_Matematigi_CS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SayiMetni s1 = new SayiMetni(false, "26", "84");
            SayiMetni s2 = new SayiMetni("-543583490.004543500");
            SayiMetni s3 = new SayiMetni();
            SayiMetni s4 = "+02546.437500";

            string deneme = (string) s2;
            Console.WriteLine("Sayi Metni Sinifindan String Sinifina Acik Donusturme Yapilabilir: " + deneme);

            Console.WriteLine("{0} Sayisinin 3. Basamagi -> {1}", s4, s4[3]);
            Console.WriteLine("{0} Sayisinin Virgulden Sonraki 4. Basamagi -> {1}", s4, s4[-4]);
            Console.WriteLine("{0} + {1} = {2}", "-465123", s2, "-465123" + s2);
            Console.WriteLine("{0} + {1} = {2}", s1, s2, s1 + s2);
            Console.WriteLine("{0} - {1} = {2}", s1, s2, s1 - s2);
            Console.WriteLine("{0} * {1} = {2}", s1, s2, s1 * s2);
            Console.WriteLine("{0} / {1} = {2}", s1, s2, s1 / s2);

            Console.WriteLine("{0} == {1} -> {2}", s1, s3, s1 == s3);
            Console.WriteLine("{0} != {1} -> {2}", s1, s3, s1 != s3);
            Console.WriteLine("{0} >= {1} -> {2}", s1, s3, s1 >= s3);
            Console.WriteLine("{0} <= {1} -> {2}", s1, s3, s1 <= s3);
            Console.WriteLine("{0} > {1} -> {2}", s1, s3, s1 > s3);
            Console.WriteLine("{0} < {1} -> {2}", s1, s3, s1 < s3);

            Console.WriteLine("{0} uzeri {1} = {2}", "2", "1000", SayiMetni.UsAl("2", "4000"));
            Console.WriteLine("300 Faktoriyel = {0}", SayiMetni.Faktoriyel("300"));
            Console.WriteLine("300 Karekok = {0}", SayiMetni.Karekok("300"));
            Console.WriteLine($"{s1} Tam Sayi -> {SayiMetni.TamSayiyaDonustur(s1)}");
            Console.WriteLine("{0} ile {1} Arasinda Rastgele Sayi -> {2}", "-48212002555435353248", "+95138494120284520548484", SayiMetni.RastgeleAralik("-48212002555435353248", "+95138494120284520548484"));
            
            Console.ReadKey();
        }
    }
}