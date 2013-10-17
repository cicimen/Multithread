using System;
using System.Collections.Generic;
using System.Threading;


//Interrupt ve Abort metotlarını hiçbir zaman kullanma



#region 01_
//namespace Multithread
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Thread myThread = new Thread(Worker);

//            /*
//             * Aşağıdaki gibi IsBackground özelliği true yapıldığı zaman Main bloğu bittiği zaman
//             * pencere kapanıyor.Bunun sebebi arka planda çalışmak üzere ayarlanmış iş parçacıklarının 
//             * ana iş parçacığı sonlandığında CLR tarafından otomatik olarak durduruluyor olmasıdır. 
//             * Benim tavsiyem çok önemli olmayan iş parçacıklarınızı arka zeminde çalışacak şekilde  
//             * ayarlamanız ve bu sayede de programdan çıkışta onların durup durmadıklarını kontrol etmekten kurtulmanızdır. 
//             */
//            //myThread.IsBackground = true;
//            myThread.Start();
//            Thread.Sleep(1500);
//            Console.WriteLine("Benim işim bitti...");
//        }

//        static void Worker()
//        {
//            for (int i = 0; i < 10; i++)
//            {
//                Console.WriteLine("İşlem " + i);
//                Thread.Sleep(1000);
//            }
//        }
//    }

//}
#endregion

#region 02
//namespace Multithread
//{

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Program prg = new Program();
//            // Aşağıdaki iki satırı da parametre gönderilen bir metodu thread ile çalıştırmak
//            // amacı ile kullanabilirsiniz.
//            //Thread myThread = new Thread( new ParameterizedThreadStart( prg.Worker ) );
//            Thread myThread = new Thread(prg.Worker);
//            myThread.Start(5);
//            Thread.Sleep(1500);
//            Console.WriteLine("Benim işim bitti...");
//        }

//        void Worker(object startPos)
//        {
//            for (int i = (int)startPos; i < 10; i++)
//            {
//                Console.WriteLine("İşlem " + i);
//                Thread.Sleep(1000);
//            }
//        }

//    }

//}
#endregion


//parametreli thread başlatma
#region 03
//namespace Multithread
//{

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Program prg = new Program();

//            Thread myThread = new Thread(() => prg.Worker(5));

//            myThread.Start();
//            Thread.Sleep(1500);

//            Console.WriteLine("Benim işim bitti...");
//        }

//        void Worker(int startPos)
//        {
//            for (int i = startPos; i < 10; i++)
//            {
//                Console.WriteLine("İşlem " + i);
//                Thread.Sleep(1000);
//            }
//        }

//    }

//}
#endregion

//lock örneği
//ben pek anlamadım pek de beğenmedim.
#region 04
//namespace Multithread
//{

//    static class Ipucu
//    {
//        static readonly object kilit = new object();
//        Queue<String> kuyruk = new Queue<String>();
//        Thread doldurucu, isleyici;

//        private void Doldur()
//        {
//            while (true)
//            {
//                lock (kilit)
//                    kuyruk.Enqueue(new String(new char[] { 'a' }));
//            }
//        }

//        private Queue<String> oku()
//        {
//            lock (kilit)
//            {
//                Queue<String> tmp = kuyruk;
//                kuyruk = new Queue<String>();
//                return tmp;
//            }
//        }

//        private void isle()
//        {
//            Queue<String> kuyruk2 = new Queue<String>();
//            while (true)
//            {
//                if (kuyruk2.Count == 0)
//                    kuyruk2 = oku();

//                String veri = kuyruk2.Dequeue();

//                // Burada gerekli işlemleri yapabilirsiniz.
//            }
//        }

//        public void start()
//        {
//            doldurucu = new  Thread( () => Doldur() );
//            doldurucu.IsBackground = true;
//            doldurucu.Start();
//            isleyici = new  Thread( () => isle() );
//            isleyici.IsBackground = true;
//            isleyici.Start();
//        }
//    }

//}
#endregion


//join
#region 05
//namespace Multithread
//{

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Thread myThread = new Thread(Worker);
//            myThread.IsBackground = true;
//            myThread.Start();
//            myThread.Join();
//            Console.WriteLine("Benim işim bitti...");
//        }

//        static void Worker()
//        {
//            for (int i = 0; i < 10; i++)
//            {
//                Console.WriteLine("İşlem " + i);
//                Thread.Sleep(1000);
//            }
//        }
//    }

//}
#endregion


//Thread.Abort()
#region 07
//namespace Multithread
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Thread myThread = new Thread(Worker);
//            myThread.Start();
//            Thread.Sleep(2000);
//            Console.WriteLine(myThread.ThreadState);

//            myThread.Abort();
//            Console.WriteLine(myThread.ThreadState);

//            myThread.Join();
//            Console.WriteLine(myThread.ThreadState);
//        }

//        static void Worker()
//        {
//            try
//            {
//                while (true)
//                    try
//                    {

//                        // Kodlarınızı buraya yazabilirsiniz...

//                        Thread.Sleep(0);

//                    }
//                    catch (ThreadAbortException)
//                    {
//                        Console.WriteLine("Thread zorla kapatılmıştır...(2)");
//                        break;
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("Beklenmeyen Hata: " + ex.Message);
//                    }
//            }
//            catch (ThreadAbortException)
//            {
//                Console.WriteLine("Thread zorla kapatılmıştır...(1)");
//            }

//            // Burada son işlemlerinizi gerçekleştirebilirsiniz.
//        }
//    }

//}
#endregion

//Thread.Interrupt()
//Bu metod ile uyku modunda olan bir iş parçacığınızı zorla uyandırabilirsiniz. 
//Bu aslında doğru bir çözüm olmasa da iş parçacıkları arası iletişimde kullanabileceğiniz en basit yoldur.
#region 07
namespace Multithread
{
    class Program
    {
        static void Main()
        {
            Thread isim = new Thread(isciMetod);
            isim.Start();
            Thread.Sleep(5000);
            // ön hazırlıkların yapıldığınız varsayalım.
            isim.Interrupt();
        }

        private static void isciMetod()
        {
            try
            {
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
                Console.Write("Zorla Uyandırıldı");
            }
            // Burada işlerimizi yapabiliriz.
        }
    }

}
#endregion



//İş Parçacıkları ve UI Kontrolleri Arası Etkileşim

//Bu yazımda son olarak bahsetmek istediğim konu ise ana iş parçacığının haricindeki diğer iş parçacıklarından UI nesneleri ile etkileşime geçemiyor olmanızdır.
//Aslında bu söyleyiş tam doğru değil, doğrusu ise bir UI objesi ancak oluşturulduğu iş parçacığı ile etkileşebilmektedir.



//public partial class Form1 : Form
//{
//    public Form1()
//    {
//        InitializeComponent();
//    }

//    private void Form1_Load(object sender, EventArgs e)
//    {
//        new Thread(Worker).Start();
//    }

//    private void Worker()
//    {
//        Action action = () => textBox1.Text = "Deneme";
//        textBox1.Invoke(action);
//    }
//}


//public partial class Form1 : Form
//{
//    public Form1()
//    {
//        InitializeComponent();
//    }

//    private void Form1_Load(object sender, EventArgs e)
//    {
//        var synch = SynchronizationContext.Current;
//        new Thread(() =>
//        {
//            synch.Post(delegate
//            {
//                textBox1.Text = "Deneme";
//            }, null);
//        }).Start();
//    }
//}