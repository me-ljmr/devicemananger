using System;
using System.Windows.Forms;


namespace ZkemConnector.NET
{

    public class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DeviceManager());


        }


    }
}
