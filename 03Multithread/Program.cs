using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Threading;

//Sinyalleşme ve Olay Bekleme

//AutoResetEvent
//Bu olay bir iş parçacığını bir başka iş parçacığından onay gelene kadar beklemeye almak amacı ile hazırlanmıştır. 

//Tanımlama 1:
//var are = new AutoResetEvent(false);
//Tanımlama 2:
//var auto = new EventWaitHandle(false, EventResetMode.AutoReset);

//Burada parametre olarak gönderdiğimiz false değeri sınıfın sadece bir örneğinin oluşturulmasını sağlamaktadır. 
//Eğer true göndermiş olsaydık oluşturulduktan sonra AutoResetEvent örneğimiz otomatik olarak set edilecekti.

namespace _03Multithread
{
    class Program
    {
        static EventWaitHandle are = new AutoResetEvent(false);

        //static void Main()
        //{
        //    new Thread(BekleyenThread).Start();
        //    Thread.Sleep(1000);
        //    are.Set();                    // Bekleyen iş parçacığı tekrar harekete geçiriliyor
        //}

        static void BekleyenThread()
        {
            Console.WriteLine("Bekleme Modu Aktif...");
            are.WaitOne();                // Bekleme Başlatılıyor
            Console.WriteLine("Bekleme Modundan Çıkıldı.");
        }
    }
}


//Çift Yönlu Sinyalleşme

class CiftYonluSinyallesme
{
    static EventWaitHandle are4Thrd1 = new AutoResetEvent(false);
    static EventWaitHandle are4Thrd2 = new AutoResetEvent(false);

    //static void Main()
    //{
    //    new Thread(Thread1Worker).Start();
    //    new Thread(Thread2Worker).Start();
    //}

    static void Thread1Worker()
    {
        Console.WriteLine("THREAD1 1");
        Thread.Sleep(1000);
        // Öncelikle ilk hazırlıklarını tamamlayacaktır.
        Console.WriteLine("THREAD1 2");
        are4Thrd2.Set();           // Diğer iş parçacığını harekete geçirir
        Console.WriteLine("THREAD1 3");
        are4Thrd1.WaitOne();       // Kendisini beklemeye alır
        // Burada ise kalan işlerini tamamlar
        Console.WriteLine("THREAD1 4");
    }
    static void Thread2Worker()
    {
        Console.WriteLine("THREAD2 1");
        are4Thrd2.WaitOne();       // Diğer iş parçacığının hazır olmasını bekler
        // Burada kendi işini yapar
        Console.WriteLine("THREAD2 2");
        are4Thrd1.Set();           // Diğer iş parçacığını harekete geçirir
        Console.WriteLine("THREAD2 3");
    }
}

//ManualResetEvent

//CountdownEvent
//CountdownEvent'in oluşturulma amacı diğerlerinden biraz daha farklıdır. 
//Burada öncelikli amaç bir iş parçacığının birden fazla iş parçacığından sinyal beklemesini sağlamaktır.
//Ayrıca AddCount ve TryAddCount metodları ile beklenen sinyal sayısını değiştirebilirsiniz. 
//Burada dikkat etmeniz gereken nokta sinyal sayısının 0 olmasına izin vermemektir.
//Böyle bir durumda hata üretilecektir. Zaten TryAddCount metodunun oluşturulma nedeni de budur.

class CountdownEventDeneme
{
    static CountdownEvent geriSay = new CountdownEvent(4);
    //static CountdownEvent geriSay = new CountdownEvent(3);

    //static void Main()
    //{
    //    new Thread(bekleyici).Start();
    //}

    static void bekleyici()
    {
        Console.WriteLine("İlk işleri tamamladım şimdi diğer iş parçacıklarını çalıştıracağım");

        new Thread(() => sinyalci("Alt İş Parçacığı 1")).Start();
        new Thread(() => sinyalci("Alt İş Parçacığı 2")).Start();
        new Thread(() => sinyalci("Alt İş Parçacığı 3")).Start();
        new Thread(() => sinyalci("Alt İş Parçacığı 4")).Start();

        geriSay.Wait();

        Console.WriteLine("İşim Bitti...");
    }

    static void sinyalci(String yazi)
    {
        Thread.Sleep(1500);
        Console.WriteLine(yazi);
        geriSay.Signal();
    }
}

//ThreadPool
//ThreadPool.QueueUserWorkItem(new WaitCallback(IsciMetod));

#region RegisteredWaitHandle  RegisterWaitForSingleObject
//WaitHandles
//ThreadPool sınıfının bir diğer yeteneği de iş parçacığını başlatmak için bir WaitHandle bekleyebilmektir. 
//Bunun için ThreadPool.RegisterWaitForSingleObject metodunu kullanabilirsiniz. 
//Bu metoda göndereceğiniz bir ManualResefEvent örneğine daha sonradan Set metodu ile sinyal gönderene kadar iş parçacığı çalıştırılmayacaktır.

//Kullanımına gelirsek;

//Öncelikle bir ManualResetEvent örneği oluşturulur.

//static ManualResetEvent tetikleyici = new ManualResetEvent (false);

//Daha sonra kodunuzun içinde çalıştıracağınız RegisterWaitForSingleObject metodu ile iş parçacığı için hazırlıklar tamamlanır. 
//4. parametre olarak göreceğiniz -1 değeri bir zaman aşımı süresi tanımlamadığımızı ve sinyal gelene kadar bekleyeceği anlamına gelmektedir. 
//Eğer burada bir değer girmiş olsaydık zaman aşımı süresi tanımlamış olurduk ve o süre tamamlandığında iş parçacığımız otomatik olarak çalıştırılacaktı. 
//5. parametredeki true değeri ise iş parçacığımızın sadece bir kere çalıştırılacağı anlamına gelmektedir. 
//Eğer bu parametreyi false vermiş olsaydık her zaman aşımı gerçekleştiğinde tekrar tekrar çalıştırılacaktır.

//RegisteredWaitHandle reg = ThreadPool.RegisterWaitForSingleObject(tetikleyici, IsciMetod, "Parametre olarak gidecek", -1, true);


//İş parçacığınızı çalıştırma vakti geldiğinde yapmanız gereken tetikleyici nesnesinin Set metodunu çalıştırmaktır. 
//Siz bu metodu çalıştırana kadar iş parçacığı devreye alınmayacaktır.

//tetikleyici.Set();

//Son olarak ise bu nesne ile işiniz bittiğinde sistem kaynaklarını boşaltmak için Unregister metodunu kullanmayı unutmamalısınız.

//reg.Unregister (tetikleyici);

#endregion

//CancellationTokens


//Wait ve Pulse kullanarak sinyalleşme


//Barrier Sınıfı
//Barrier sınıfı birden fazla iş parçacığının belirlenmiş bir zamanda buluşmasını sağlar. 
//Framework 4.0 ile birlikte gelen bu sınıfı kullanarak belli bir zamanda iş parçacıklarınızı buluşturup sonra ikinci aşamaya geçmelerini sağlayabilirsiniz.

class BarrierEventDeneme
{
    //static Barrier _barrier = new Barrier(3);
    static Barrier _barrier = new Barrier(3, barrier => Console.WriteLine());

    //static void Main()
    //{
    //    new Thread(Speak).Start();
    //    new Thread(Speak).Start();
    //    new Thread(Speak).Start();
    //}

    static void Speak()
    {
        for (int i = 0; i < 5; i++)
        {
            Console.Write(i + " ");
            _barrier.SignalAndWait();
        }
    }
}


//Reader/Writer Kilitleri

class SlimDemo
{
    static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
    static List<int> _items = new List<int>();
    static Random _rand = new Random();

    static void Main()
    {
        new Thread(Read).Start();
        new Thread(Read).Start();
        new Thread(Read).Start();

        new Thread(Write).Start("A");
        new Thread(Write).Start("B");
    }

    static void Read()
    {
        while (true)
        {
            _rw.EnterReadLock();
            foreach (int i in _items) Thread.Sleep(10);
            _rw.ExitReadLock();
        }
    }

    static void Write(object threadID)
    {
        //while (true)
        //{
        //    int newNumber = GetRandNum(100);
        //    _rw.EnterWriteLock();
        //    _items.Add(newNumber);
        //    _rw.ExitWriteLock();
        //    Console.WriteLine("Thread " + threadID + " added " + newNumber);
        //    Thread.Sleep(100);
        //}

        while (true)
        {
            int newNumber = GetRandNum(100);
            _rw.EnterUpgradeableReadLock();
            if (!_items.Contains(newNumber))
            {
                _rw.EnterWriteLock();
                _items.Add(newNumber);
                _rw.ExitWriteLock();
                Console.WriteLine("Thread " + threadID + " added " + newNumber);
            }
            _rw.ExitUpgradeableReadLock();
            Thread.Sleep(100);
        }

    }

    static int GetRandNum(int max) { lock (_rand) return _rand.Next(max); }
}
