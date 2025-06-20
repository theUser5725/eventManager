using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Vistas;

namespace WinFormsApp1
{
    public partial class FormPrincipal : Form
    {
        private Panel panelContenido;
        private Stack<int> historialVistas = new();
        private PanelHeader header;

        public FormPrincipal()
        {
            InitializeComponent();

            // Crear y añadir el header solo una vez
            header = new PanelHeader();
            header.Dock = DockStyle.Top;
            this.Controls.Add(header);

            header.btnBack.Click += BtnBack_Click;
            this.KeyPreview = true;
            this.KeyDown += FormPrincipal_KeyDown;


            panelContenido = new Panel();
            panelContenido.Dock = DockStyle.Fill;
            this.Controls.Add(panelContenido);

            // Cargar la vista inicial
            cambiarVista(0);
        }

        public void cambiarVista(int navSeleccionado, bool desdeAtras = false)
        {
            if (!desdeAtras && panelContenido.Controls.Count > 0)
            {
                // Guarda la vista actual antes de cambiar
                historialVistas.Push(navSeleccionado);
            }

            UserControl nuevaVista;
            switch (navSeleccionado)
            {
                case 0:
                    nuevaVista = new PanelHome();
                    header.MostrarBotonAtras(false); // Oculta el botón en Home
                    break;
                case 1:
                    nuevaVista = new PanelManager();
                    header.MostrarBotonAtras(false);
                    break;
                case 2:
                    nuevaVista = new PanelSettings();
                    header.MostrarBotonAtras(false);
                    break;
                default:
                    nuevaVista = new PanelEvento();
                    header.MostrarBotonAtras(true); // Muestra el botón en Evento
                    break;
            }
            CargarVista(nuevaVista);
        }

        private void CargarVista(UserControl nuevaVista)
        {
            panelContenido.Controls.Clear();
            nuevaVista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(nuevaVista);
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {

            VolverAtras();
        }

        private void FormPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                VolverAtras();
            }
        }

        private void VolverAtras()
        {
            if (historialVistas.Count > 0)
            {
                int vistaAnterior = historialVistas.Pop();

                cambiarVista(vistaAnterior, true);
            }
            else
            {
                cambiarVista(0, true); // Vuelve a Home si no hay historial
            }
        }



    }
}
