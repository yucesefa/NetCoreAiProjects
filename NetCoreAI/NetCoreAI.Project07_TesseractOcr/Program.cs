using Tesseract;
class Program
{


    static void Main(string[] args)
    {
        Console.Write("Karakter Okuması Yapılacak Rsim Yolu: ");
        string imagePath = Console.ReadLine();

        string tessDataPath = @"C:\tessdata";

        try
        {
            using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default)) //TesseractEngine sınıfı OCR işlemleri için kullanılan sınıftır.
            {
                using (var img = Pix.LoadFromFile(imagePath)) //Pix resimleri işlemek için kullanılan bir sınıftır.
                {
                    using (var page = engine.Process(img)) // Page  Tesseract OCR işlemi sonucunda elde edilen metinleri tutan sınıftır.
                    {
                        var text = page.GetText();
                        Console.WriteLine("Resimden Okunan Metin:");
                        Console.WriteLine(text);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir Hata Oluştu: {ex.Message}");
        }
    }
}