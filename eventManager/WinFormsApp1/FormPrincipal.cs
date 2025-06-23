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
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;

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
            this.WindowState = FormWindowState.Maximized;
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
                case 3:
                    nuevaVista = new PanelEvento(pEvento.GetById(1));
                    btnBack.Visible = true; // Muestra atras el botón en Evento
                    break;

                default:
                    nuevaVista = new PanelHome();
                    btnBack.Visible = false;
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

            // Reemplaza en inicializar()
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70)); // altura fija para header
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // resto para contenido
            this.Controls.Add(layout);

            // PanelHeader
            header = new PanelHeader();
            header.Dock = DockStyle.Fill;
            layout.Controls.Add(header, 0, 0);

            // PanelContenido
            panelContenido = new Panel();
            panelContenido.Dock = DockStyle.Fill;
            layout.Controls.Add(panelContenido, 0, 1);

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
