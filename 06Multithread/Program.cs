using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Eşzamanlı Koleksiyonlar (Concurrent Collections)
//Eş zamanlı koleksiyonlar yazma işlemlerinde eski versiyonlarına göre kat kat daha yavaş çalışmaktadırlar.

//IProducerConsumerCollection<T> Arabirimi

//ConcurrentBag<T>
//Normalde listeden bir eleman almak istediğinizde yığın yapısında olduğu gibi en son eklenen elemanı alırsınız. 
//Ama bir fark vardır ki o da aldığınız bu eleman o iş parçacığında eklenen son elemandır. 
//Eğer ki o iş parçacığından eklenen eleman sayısı sıfır ise bu durumda rastgele olarak diğer iş parçacıklarından rastgele birinin 
//listesindeki son elemanı almaya çalışacaktır. 
//İşte sadece bu durumda iş parçacıkları arasında bir çekişme olmaktadır ki o da sadece (genel olarak) iki iş parçacığı arasında söz konusu olabilmektedir. 
//Yani eğer listeniz ile çalışan örneğin 5 iş parçacığınız var ise ve bunların hepsi bu listeye eleman ekleyip alıyorsa
//çok büyük ihtimalle bu iş parçacıkları ikili olarak bir çekişmeye gireceklerdir.
//Oysa bir kuyruk yada yığın listesi kullanıyor olursak tüm iş parçacıkları her zaman çekişme içinde olacaklardır.

namespace _06Multithread
{
    class EsZamanKiyaslama
    {
        static ConcurrentQueue<int> veriler = new ConcurrentQueue<int>();
        static ConcurrentBag<int> veriler2 = new ConcurrentBag<int>();

        static void Main(string[] args)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var gorev1 = Task.Factory.StartNew(YazOku);
            var gorev2 = Task.Factory.StartNew(YazOku);
            var gorev3 = Task.Factory.StartNew(YazOku);

            Task.WaitAll(new Task[] { gorev1, gorev2, gorev3 });

            sw.Stop();
            Console.WriteLine("Eşzamanlı Queue Toplam Sure : " + sw.ElapsedMilliseconds);

            Stopwatch sw2 = new Stopwatch();
            sw2.Start();

            var gorev11 = Task.Factory.StartNew(YazOku2);
            var gorev21 = Task.Factory.StartNew(YazOku2);
            var gorev31 = Task.Factory.StartNew(YazOku2);

            Task.WaitAll(new Task[] { gorev11, gorev21, gorev31 });
            sw2.Stop();

            Console.WriteLine("Eşzamanlı Bag Toplam Sure : " + sw2.ElapsedMilliseconds);
        }

        static void YazOku()
        {
            //for (int i = 0; i < 1000000; i++)
            //{
            //    veriler.Enqueue(i);
            //}

            for (int i = 0; i < 1000000; i++)
            {
                veriler.Enqueue(i);
                int a;
                veriler.TryDequeue(out a);
            }
        }

        static void YazOku2()
        {
            //for (int i = 0; i < 1000000; i++)
            //{
            //    veriler2.Add(i);
            //}

            for (int i = 0; i < 1000000; i++)
            {
                veriler2.Add(i);
                int a;
                veriler2.TryTake(out a);
            }
        }
    }
}


//BlockingCollection<T>