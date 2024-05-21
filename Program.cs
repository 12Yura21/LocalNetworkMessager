namespace LocalNetworkMessager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Controller controller = new Controller();
            Form1 myForm = new Form1();
            myForm.Connect += controller.ConnectTo;
            myForm.Disconnect += controller.Disconnect;
            myForm.CreateServer += controller.CreateServer;
            myForm.CloseServer += controller.CloseServer;
            myForm.SendMessage += controller.Send;
            controller.MessageReceived += myForm.AddTextToLIst;
            ApplicationConfiguration.Initialize();
            Application.Run(myForm);
        }
    }
}