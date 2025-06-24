using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
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
        // Referencia al underline
        private PictureBox underline;
        // Lista de labels de navegación
        private List<Label> navLabels = new();
        private int margenLateralTabs = 50;
        private int margenLateralBody = 220;
        private int margenLateralTexto = 70;

        public PanelManager()
        {

            Inicializar();
        }

        private void Inicializar()
        {
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Dock = DockStyle.Fill;

            // Contenedor con scroll
            var panelScrollable = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
            };
            this.Controls.Add(panelScrollable);

            // Panel central que se centra automáticamente en el scroll
            var panelCentral = new Panel
            {
                BackColor = Disenio.Colores.GrisClaro,
                Padding = new Padding(32),
                Size = new Size(this.Width - (2 * margenLateralBody), 1200), // usa el margen
                Anchor = AnchorStyles.Top
            };

            panelScrollable.Resize += (s, e) =>
            {
                int anchoDisponible = panelScrollable.ClientSize.Width;

                // Si el espacio es muy chico (por ejemplo < 1200), eliminar márgenes
                int margenAplicado = (anchoDisponible < 1200) ? 0 : margenLateralBody;

                panelCentral.Width = anchoDisponible - (2 * margenAplicado);
                panelCentral.Left = margenAplicado;
                panelCentral.Top = 40; // constante vertical
            };

            panelScrollable.Controls.Add(panelCentral);

            // Centrado horizontal dinámico
            panelScrollable.Resize += (s, e) =>
            {
                panelCentral.Left = (panelScrollable.ClientSize.Width - panelCentral.Width) / 2;
            };


            panelCentral.Controls.Add(inicializarCategoriaEventos());




        }
        private TableLayoutPanel inicializarCategoriaEventos()
        {
            var categorias = pCategoriaEvento.GetAll();

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                Padding = new Padding(16),
                BackColor = Color.White,
            };

            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90)); // Nombre Cat
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10)); // Mail boton

            var btnDarDeBaja = new Button
            {
                Image = new Bitmap(Disenio.Imagenes.IconoEditar, new Size(Disenio.tamanoIcono, Disenio.tamanoIcono)), // <-- redimensionar ícono aquí
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 28,
                Height = 28,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Cursor = Cursors.Hand,
                Text = "",

            }
            ;
            btnDarDeBaja.Left = tabla.Left - Disenio.tamanoIcono - 10;

            btnDarDeBaja.FlatAppearance.BorderSize = 0;

            btnDarDeBaja.Click += (s, e) =>



           btnDarDeBaja.Click += (s, e) =>
            {
            };



            foreach (CategoriaEvento categoria in categorias)
            {
                Label lblTitulo = new Label
                {
                    Font = Disenio.Fuentes.General,

                };

                tabla.Controls.Add(lblTitulo, 0, tabla.RowCount);
                tabla.Controls.Add(btnDarDeBaja, 1, tabla.RowCount);

                tabla.RowCount++;


            }

            return tabla;
        }
    }
}

