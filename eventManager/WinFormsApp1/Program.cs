using WinFormsApp1.Controladores;

namespace WinFormsApp1
{
    internal static class Program
    {
 
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Conexion.OpenConnection();
            ApplicationConfiguration.Initialize();
            Application.Run(new FormPrincipal());
            Conexion.CloseConnection();
        }
    }
}