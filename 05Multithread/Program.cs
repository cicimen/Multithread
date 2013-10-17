using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

//Paralel Programlama
//Aslında iş parçacıkları arasında iki temel çeşit parçalama / bölümleme metodu bulunmaktadır: 
//veri paralelleştirme ve iş paralelleştirme. 
//Eğer bir grup işi büyük bir veri grubunda yapma ihtiyacı duyarsak bunu rahatlıklar 
//aynı işi yapan birden fazla iş parçacığı oluşturarak ve veriyi bu iş parçacıkları arasında paylaştırarak yapabiliriz.
//Bu işleme veri paralelleştirme (data parallelism) adı verilmektedir. 
//Bir de farklı işleri yapan iş parçacıklarını aynı anda çalıştırma durumu vardır ki buna da iş paralelleştirme (task parallelism) adı verilmektedir.

//PFX Bileşenleri

//Peki PFX'i Ne Zaman Kullanmalıyız

//Parallel Sınıfı

//Parallel.Invoke

namespace _05Multithread
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    Parallel.Invoke(
        //    () =>
        //    { 
        //        for (int i = 0; i < 100; i++)
        //        {
        //         Console.WriteLine("1.method" +i);
        //        }
        //    },
        //    () =>
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            Console.WriteLine("2.method" + i);
        //        }
        //    }
        //    );
        //}
    }
}

//Parallel.For ve Parallel.ForEach


//Görev Paralelleştirme (Task Parallelism)

//Task Oluşturma ve Çalıştırma

//class Program
//{
//    static void Main(string[] args)
//    {
//        ////Task.Factory.StartNew(() => Console.WriteLine("Yapacak bir işim yok."));

//        //Task<string> gorev = Task.Factory.StartNew<string>(() =>
//        //{
//        //    Thread.Sleep(5000);
//        //    return "İşim Bitti...";
//        //});
//        //// Burada varsa diğer işlerimizi yapabiliriz.
//        //Console.WriteLine("diğer işlemler");
//        //Console.WriteLine(gorev.Result); // Burada görev henüz tamamlanmamış ise görev tamamlanana kadar beklenir

//        Task gorev = new Task(() => Console.Write("Görev tarafından yazdırılmıştır."));
//        gorev.Start();
//    }
//}


//TaskCreationOption

//Yavru (Child) Görevler


class Program
{
    //static void Main(string[] args)
    //{
    //    Task ebeveyn = Task.Factory.StartNew(() =>
    //    {
    //        Console.WriteLine("Ebeveyn görev");

    //        Task.Factory.StartNew(() =>
    //        {
    //            Console.WriteLine("Yavru görev olarak bağlanmamış görev");
    //        });

    //        Task.Factory.StartNew(() =>
    //        {
    //            Console.WriteLine("Yavru görev");
    //        }, TaskCreationOptions.AttachedToParent);
    //    });

    //    Console.WriteLine("bitti");
    //}
}


//Görevin Bitmesini Beklemek

//Hata Kontrolü (Exception Handling)

//Görev İptalleri

class Program2
{
    static void Main(string[] args)
    {
        //var iptalKaynagi = new CancellationTokenSource();
        //CancellationToken token = iptalKaynagi.Token;

        //Task gorev = Task.Factory.StartNew(() =>
        //{
        //    while (true)
        //    {
        //        token.ThrowIfCancellationRequested();  // Burada iptal talebi gelip gelmediğine bakılır
        //        // Görevi yerine getiren kodlar
        //        Thread.Sleep(5);
        //    }
        //}, token);

        //// Diğer kodlarınız

        //try
        //{
        //    iptalKaynagi.Cancel();
        //    gorev.Wait();
        //}
        //catch (AggregateException ex)
        //{
        //    if (ex.InnerException is OperationCanceledException)
        //        Console.WriteLine("Görev İptal Edilmiştir!");
        //}
        //Console.WriteLine("bitti");


        Task<String> gorev = Task.Factory.StartNew<String>(() => "Mer");
        Task<String> gorev2 = gorev.ContinueWith(onceki => onceki.Result + "haba ");
        Task<String> gorev3 = gorev2.ContinueWith(onceki =>
        {
            Thread.Sleep(100);
            return onceki.Result + "Dünya";
        });

        Console.WriteLine(gorev3.Result);

    }
}


//Görev Devamı (Continuation)

//Devam Görevleri ve Yavru Görevler

//Şarta Bağlı Görev Devamları

//Görev Zamanlayıcıları ve UI (Kullanıcı Arabirimi)

//TaskFactory

//AgregateException

//Flatten

//Handle