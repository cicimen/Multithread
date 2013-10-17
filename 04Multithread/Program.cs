using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Timer Sınıfları
//.NET Framework 2 farklı tipte 4 timer sınıfına sahiptir. 
//Bunardan ikisi çok kanallı programlamada kullanılırken diğer ikisi WPF ve WindowsForm uygulamalarında çalışırlar 
//ve her ikisi de tek iş parçacıklı programlama için tasarlanmıştırlar.

//Aşağıdaki sınıflar çok kanallı programlamada kullanılabilirler.

//System.Threading.Timer
//System.Timers.Timer

//Alt listede gördüğünüz sınıflar ise sadece tek iş parçacıklı bir uygulamada kullanılmak üzere hazırlanmışlardır.

//System.Windows.Forms.Timer (Windows Form uygulamalarında kullanılabilir)
//System.Windows.Threading.DispatcherTimer (WPF uygulamalarında kullanılabilen timer sınıfı)


//System.Timers.Timer Sınıfı

//Yalnız bu sınıfta bilmeniz gereken ek nokta eğer nesnenizin AutoReset özniteliğini true yapmaz iseniz Elapsed olayı sadece bir kere çalıştırılacaktır. 
//O nedenle bu değerin true olduğuna emin olmanız gerekecektir.

//System.Timers.Timer zamanlayici = new System.Timers.Timer(1000);
//zamanlayici.Elapsed += new ElapsedEventHandler(zamanlayici_TimedEvent);
//Zamanlayici.Start();


//System.Threading.Timer

//Timer zamanlayici = new Timer(Tick, "tick...", 5000, 1000);


namespace _04Multithread
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
