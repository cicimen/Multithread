using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;
using System.IO;
using System.Net;

//Caller Information

namespace _07Multithread
{
    class Program
    {
        static void Main(string[] args)
        {
            Deneme2 den2 = new Deneme2();
            Task<byte[]> task =  den2.GetURLContentsAsync("http://www.google.com");

            for (int i = 0; i < 10000000; i++)
			{
                if(i%100000  ==0)
			 Console.WriteLine(i);
			}
            Console.Write(System.Text.Encoding.UTF8.GetString(task.Result));
            Program pr = new Program();
            pr.CagiranMetod();
        }

        public void CagiranMetod()
        {
            MesajYazici("Something happened.");
        }

        public void MesajYazici(string gelenVeri,
                [CallerMemberName] string metodAdi = "",
                [CallerFilePath] string cagiranDosyaYolu = "",
                [CallerLineNumber] int kaynakSatirNo = 0)
        {
            Console.WriteLine("Gelen Mesaj: " + gelenVeri);
            Console.WriteLine("Çağıran Metod Adı: " + metodAdi);
            Console.WriteLine("Çağıran Dosya Yolu ve Adı: " + cagiranDosyaYolu);
            Console.WriteLine("Çağıran Komut Satır No: " + kaynakSatirNo);
        }
    }
}

//C# 5.0 Asenkron Programlama

public class Deneme
{
    private async Task<byte[]> GetURLContentsAsync(string url)
    {
        // The downloaded resource ends up in the variable named content.
        var content = new MemoryStream();

        // Initialize an HttpWebRequest for the current URL.
        var webReq = (HttpWebRequest)WebRequest.Create(url);

        // **Call GetResponseAsync instead of GetResponse, and await the result.
        // GetResponseAsync returns a Task<WebResponse>.
        using (WebResponse response = await webReq.GetResponseAsync())
        {
            // Get the data stream that is associated with the specified URL.
            using (Stream responseStream = response.GetResponseStream())
            {
                // ** Call CopyToAsync instead of CopyTo, and await the response.
                // CopyToAsync returns a Task, not a Task<T>.
                await responseStream.CopyToAsync(content);
            }
        }
        // Return the result as a byte array.
        return content.ToArray();
    }
}

//İstisna İşleme (Exception Handling)

public class Deneme2
{
    public async Task<byte[]> GetURLContentsAsync(string url)
    {
        // The downloaded resource ends up in the variable named content.
        var content = new MemoryStream();

        // Initialize an HttpWebRequest for the current URL.
        var webReq = (HttpWebRequest)WebRequest.Create(url);

        // **Call GetResponseAsync instead of GetResponse, and await the result.
        // GetResponseAsync returns a Task<WebResponse>.
        try
        {
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                // Get the data stream that is associated with the specified URL.
                using (Stream responseStream = response.GetResponseStream())
                {
                    // ** Call CopyToAsync instead of CopyTo, and await the response.
                    // CopyToAsync returns a Task, not a Task<T>.
                    await responseStream.CopyToAsync(content);
                }
            }
        }
        catch (Exception ee)
        {
            return null;
        }
        // Return the result as a byte array.
        Console.WriteLine(content.ToArray());
        return content.ToArray();
    }
}