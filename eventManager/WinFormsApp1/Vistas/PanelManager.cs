//Código Nuevo

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;


namespace WinFormsApp1.Vistas
{
    public partial class PanelManager : UserControl
    {
        //declaramos las variables privadas
        public PanelManager()
        {
            diseñoGeneralManager();
        }
        private void diseñoGeneralManager()
        {
            //panel general gris claro (1)
            Panel panelGris = new Panel();
            panelGris.Size = new Size(800, 600);
            panelGris.BackColor = Disenio.Colores.GrisClaro;

        }
    }
}