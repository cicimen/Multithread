using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

//Monitor.Enter ve Monitor.Exit

//TryEnter

//deadlock örneği
#region 1
//namespace _02Multithread
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            object kilitnesnesi1 = new object();
//            object kilitnesnesi2 = new object();

//            new Thread (() => {
//                                  lock (kilitnesnesi1)
//                                  {
//                                      Thread.Sleep (1000);
//                                      lock (kilitnesnesi2);      // Deadlock
//                                  }
//                              }).Start();
//            lock (kilitnesnesi2)
//            {
//              Thread.Sleep (1000);
//              lock (kilitnesnesi1);                          // Deadlock
//            }
//        }
//    }
//}
#endregion

//Mutex

//Mutex C# lock'u gibi çalışsa da istenmesi durumunda bilgisayar çapında yani işlemler (process) arası kilitleme de yapabilmektedir. 
//Lock komutundan yaklaşık 50 kat daha yavaş olduğundan kullanımı gerçekten gerekmedikçe tavsiye edilmemektedir. 
//Mutex'in WaitOne ve ReleaseMutex metodları ile kilitleme ve kilidi kaldırma işlemi yapabilirsiniz.
#region 2
namespace _02Multithread
{
    class Program
    {
        static void Main()
        {
            using (var mutex = new Mutex(false, "xyz.com IslemlerArasıKilit"))
            {
                // 3 saniye bekledikten sonra kilit gerçekleşmemiş olursa programdan çıkılır

                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
                {
                    Console.WriteLine("Program zaten çalışıyor.");
                    return;
                }
                RunProgram();
            }
        }

        static void RunProgram()
        {
            Console.WriteLine("Çalışıyorum...");
            Console.ReadLine();
        }
    }
}
#endregion

//Semaphore

//Lazy Initialization (Tembel Başlatma)

//Lazy<T>

//LazyInitializer


//Engellenmeden (Kilitlemeden) Senkronizasyon (Non-blocking Syncronization):

//volatile Anahtar Kelimesi
//bir değişkenin içeriğinde bir iş parçacığın tarafından yapılan değişikliğin anında diğer iş parçacıkları tarafından da görülebilir olmasını sağlamaktadır. 
//Bu nedenle MSDN'de yazılana göre ortak değişkenlerinizi mutlaka volatile anahtar kelimesi ile oluşturmanız gerekmektedir. 
//Bu şekilde bir tanımlamanın kesin sonuç verdiği de iddia edilmektedir. 
//Ama aslında gerçek bu şekilde değildir biraz sonra bunu detaylı olarak işleyeceğiz.


//MemoryBarrier Metodu

//VolatileRead ve VolatileWrite metodları


//Interlocked Sınıfı
//Evet arkadaşlar, bence çoklu iş parçacığı olan programlar yazıyorsanız basit tiplerin değerlerini değiştirmek için kullanmanız gereken asıl sınıf budur. 
//Interlocked.Add( ref toplam1, 10 );
//Interlocked.Increment( ref toplam1 );
//Interlocked.Decrement( ref toplam1 );
//Interlocked.Exchange( ref toplam1, 5 );  
//Interlocked.CompareExchange( ref toplam1, 10, a ); ??


//Spinning ve Blocking Arasındaki Farklar
//Bloklama (Blocking)
//Döndürme/Döngüleme (Spinning)
//Bu nedenle SpinLock konusunda da üstüne basa basa belirteceğim üzere sadece çok kısa bir iş yapacağınız durumlarda (örneğil bir bağlantılı listeye (linked list) yazarken) kullanmak fayda sağlayabilecektir. 


//SpinLock Yapısı

//var spinLock = new SpinLock (true);   // Sahip takibini yapılsın
//bool lockTaken = false;
//try
//{
//  spinLock.Enter (ref lockTaken);
//  // İşlerinizi burada yapabilirsiniz.
//}
//finally
//{
//  if (lockTaken) spinLock.Exit();
//}

//SpinWait Yapısı
#region örnek thread safe integer class. spinwait kullanılmış
public class TS_Int32
{
  SpinWait sWait = new SpinWait();

  private Int32 deger;

  public Int32 Value
  {
    get { return this.deger; }
    set
    {
      Interlocked.Exchange( ref this.deger, value );
    }
  }

  public TS_Int32()
  {
    deger = 0;
  }

  public TS_Int32(int val)
  {
    deger = val;
  }

  #region Operator Overloading
  public static explicit operator Double(TS_Int32 a) { return (Double)a.Value; }
  public static explicit operator float(TS_Int32 a) { return (float)a.Value; }
  public static explicit operator Int64(TS_Int32 a) { return (Int64)a.Value; }
  public static explicit operator Int32(TS_Int32 a) { return a.Value; }

  public static implicit operator TS_Int32(Int32 a) { return new TS_Int32( a ); }
  public static implicit operator TS_Int32(Int16 a) { return new TS_Int32( a ); }

  public static TS_Int32 operator+(TS_Int32 a, TS_Int32 b)
  { return new TS_Int32( a.deger + b.deger ); }
  public static TS_Int32 operator+(Int32 a, TS_Int32 b) { return new TS_Int32( a + b.deger );  }
  public static TS_Int32 operator+(TS_Int32 a, Int32 b) { return new TS_Int32( a.deger + b ); }

  public static TS_Int32 operator-(TS_Int32 a, TS_Int32 b)
  { return new TS_Int32( a.deger - b.deger ); }
  public static TS_Int32 operator-(Int32 a, TS_Int32 b) { return new TS_Int32( a - b.deger ); }
  public static TS_Int32 operator-(TS_Int32 a, Int32 b) { return new TS_Int32( a.deger - b ); }

  public static TS_Int32 operator*(TS_Int32 a, TS_Int32 b)
  { return new TS_Int32( a.deger * b.deger ); }
  public static TS_Int32 operator*(Int32 a, TS_Int32 b) { return new TS_Int32( a * b.deger ); }
  public static TS_Int32 operator*(TS_Int32 a, Int32 b) { return new TS_Int32( a.deger * b ); }

  public static TS_Int32 operator/(TS_Int32 a, TS_Int32 b)
  { return new TS_Int32( a.deger / b.deger ); }
  public static TS_Int32 operator/(Int32 a, TS_Int32 b) { return new TS_Int32( a / b.deger ); }
  public static TS_Int32 operator/(TS_Int32 a, Int32 b) { return new TS_Int32( a.deger / b ); }

  public static TS_Int32 operator^(TS_Int32 a, TS_Int32 b)
  { return new TS_Int32( a.deger ^ b.deger ); }
  public static TS_Int32 operator^(Int32 a, TS_Int32 b) { return new TS_Int32( a ^ b.deger ); }
  public static TS_Int32 operator^(TS_Int32 a, Int32 b) { return new TS_Int32( a.deger ^ b ); }

  public static Boolean operator==(TS_Int32 a, TS_Int32 b) { return (a.deger == b.deger); }
  public static Boolean operator==(TS_Int32 a, Int32 b) { return (a.deger == b); }
  public static Boolean operator==(Int32 a, TS_Int32 b) { return (a == b.deger); }

  public static Boolean operator!=(TS_Int32 a, TS_Int32 b) { return (a.deger != b.deger); }
  public static Boolean operator!=(TS_Int32 a, Int32 b) { return (a.deger != b); }
  public static Boolean operator!=(Int32 a, TS_Int32 b) { return (a != b.deger); }

  public override bool Equals(object obj) { return deger.Equals( obj ); }
  public override int GetHashCode() { return deger.GetHashCode(); }

  public static Boolean operator>(TS_Int32 a, TS_Int32 b) { return (a.deger > b.deger); }
  public static Boolean operator>(TS_Int32 a, Int32 b) { return (a.deger > b); }
  public static Boolean operator>(Int32 a, TS_Int32 b) { return (a > b.deger); }

  public static Boolean operator<(TS_Int32 a, TS_Int32 b) { return (a.deger < b.deger); }
  public static Boolean operator<(TS_Int32 a, Int32 b) { return (a.deger < b); }
  public static Boolean operator <(Int32 a, TS_Int32 b) { return (a < b.deger); }

  public static Boolean operator>=(TS_Int32 a, TS_Int32 b) { return (a.deger >= b.deger); }
  public static Boolean operator>=(TS_Int32 a, Int32 b) { return (a.deger >= b); }
  public static Boolean operator>=(Int32 a, TS_Int32 b) { return (a >= b.deger); }

  public static Boolean operator<=(TS_Int32 a, TS_Int32 b) { return (a.deger <= b.deger); }
  public static Boolean operator<=(TS_Int32 a, Int32 b) { return (a.deger <= b); }
  public static Boolean operator<=(Int32 a, TS_Int32 b) { return (a <= b.deger); }
  #endregion

  public void Add(int a)
  {
    int aa = deger + a;
    while (true) {
      int sshot = deger;
      if (sshot == Interlocked.CompareExchange( ref deger, aa, sshot ))
        return;
      sWait.SpinOnce();
    }
  }
  public void Add(TS_Int32 a) { Add( a.Value ); }
  public void Substract(TS_Int32 a) { Add( -a.Value ); }
  public void Substract(int a) { Add( -a ); }

  private void Multiply(float a)
  {
    int aa = (int)(deger * a);
    while (true) {
      int sshot = deger;
      if (sshot == Interlocked.CompareExchange( ref deger, aa, sshot ))
        return;
        sWait.SpinOnce();
    }
  }

  public void Multiply(int a) { Multiply( (float)a ); }
  public void Multiply(TS_Int32 a) { Multiply( (float)a.Value ); }
  public void Divide(int a) { Multiply( 1/a ); }
  public void Divide(TS_Int32 a) { Multiply( 1/a.Value ); }

  public void Increment() { Interlocked.Increment( ref deger ); }
  public void Decrement() { Interlocked.Decrement( ref deger ); }

  public override string ToString() { return deger.ToString(); }
  public string ToString(String format) { return deger.ToString(format); }
}
#endregion


