using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Resources;
using WinFormsApp1.Vistas;

namespace WinFormsApp1
{
    public partial class FormPrincipal : Form
    {
        private Panel panelContenido;
        private Stack<int> historialVistas = new();
        private PanelHeader header;
        private Button btnBack;

        public FormPrincipal()
        {
            InitializeComponent();

            inicializar();
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
                    btnBack.Visible = false;
                    break;
                case 1:
                    nuevaVista = new PanelManager();
                    btnBack.Visible = false;
                    break;
                case 2:
                    nuevaVista = new PanelSettings();
                     btnBack.Visible=false;
                    break;
                default:
                    nuevaVista = new PanelEvento();
                    btnBack.Visible = true; // Muestra atras el botón en Evento
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

        public void inicializar()
        {
            // Header
            header = new PanelHeader();
            header.Dock = DockStyle.Top;
            this.Controls.Add(header);

            // Botón atrás
            btnBack = new Button
            {
                FlatStyle = FlatStyle.Flat,
                ForeColor = Disenio.Colores.GrisClaro,
                BackColor = Color.Transparent,
                Location = new Point(25, 25),
                Size = new Size(25, 25),
                TabStop = false,
                Visible = false // Oculto por defecto
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.ImageAlign = ContentAlignment.MiddleCenter;
            btnBack.Image = new Bitmap(Disenio.Imagenes.IconoAtras, btnBack.Size);
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
            btnBack.BringToFront();

            // Panel de contenido
            panelContenido = new Panel();
            panelContenido.Dock = DockStyle.Fill;
            this.Controls.Add(panelContenido);

            // Tecla Escape
            this.KeyPreview = true;
            this.KeyDown += FormPrincipal_KeyDown;

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
