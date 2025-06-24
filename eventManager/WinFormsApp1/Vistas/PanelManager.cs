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
    public partial class PanelManager : Form
    {
        // Referencia al underline
        private PictureBox underline;

        // Lista de labels de navegación
        private List<Label> navLabels = new();
        private int margenLateralTabs = 50;
        private int margenLateralBody = 220;
        private int margenLateralTexto = 70;
        private Label labelTitulo;

        public PanelManager()
        {
            Inicializar(); //Inicializo el panel
        }

        private void Inicializar() //Método para inicializar en el panel
        {
            // Fondo y Dock
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Dock = DockStyle.Fill;

            // Panel con scroll
            var panelScrollable = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            this.Controls.Add(panelScrollable);

            // Panel central gris el del fondo
            var panelCentral = new Panel
            {
                BackColor = Disenio.Colores.RojoOscuro, //CAMBIAR A GRIS CLARO LUEGO
                Padding = new Padding(32),
                Anchor = AnchorStyles.Top
            };
            panelScrollable.Controls.Add(panelCentral);
            //titulo
            labelTitulo = new Label
            {
                Text = "Manager",
                AutoSize = true,
                Font = Disenio.Fuentes.Titulo,
                Location = new Point(650, 30)
            };
            panelCentral.Controls.Add(labelTitulo);

            // Redimensionado responsivo
            panelScrollable.Resize += (s, e) =>
            {
                int w = panelScrollable.ClientSize.Width;
                int m = (w < 1200) ? 0 : margenLateralBody;
                panelCentral.Width = w - 2 * m;
                panelCentral.Left = (w - panelCentral.Width) / 2;
                panelCentral.Top = 40;
                panelCentral.Height = panelScrollable.ClientSize.Height - 80;
            };


            // 2) Defino el Tabla 2x2 (Filas y Columnas)
            var grid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(16),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grid.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // 3) Creo los bloques  {Armar un foreach que ordene toda esta estructura}
            var bloque1 = CrearBloque(
                "Categorías Eventos",
                pCategoriaEvento.GetAll().Select(x => x.Nombre).ToList());
            var bloque2 = CrearBloque(
                "Categorías Directivos",
                pCategoriaDirectivo.GetAll().Select(x => x.Nombre).ToList());
            var bloque3 = CrearBloque(
                "Lugares",
                pLugar.GetAll().Select(x => x.nombre).ToList());

            // Anclaje centrar en cada celda
            bloque1.Anchor = AnchorStyles.None;
            bloque2.Anchor = AnchorStyles.None;
            bloque3.Anchor = AnchorStyles.None;

            // 4) Los dos superiores
            grid.Controls.Add(bloque1, 0, 0);
            grid.Controls.Add(bloque2, 1, 0);

            // 5) El tercero en la 2da fila, abarcando las 2 columnas
            grid.Controls.Add(bloque3, 0, 1);
            grid.SetColumnSpan(bloque3, 2);

            // 6) Finalmente el grid bajo el label
            panelCentral.Controls.Add(grid);
        }

        //Crea un bloque de panel con título y lista de items
        private Panel CrearBloque(string titulo, List<string> items)
        {
            var panel = new Panel //dISEÑO GRIS CLARO AHI situar el Label DockSytleTOP
            {
                BackColor = Color.White,
                Size = new Size(320, 240),
                Margin = new Padding(12)
            };


            // TableLayout para filas/columnas
            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = items.Count + 2
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            // Fila 0: título
            var lblT = new Label
            {
                Text = titulo,
                Font = Disenio.Fuentes.General,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Height = 28
            };
            tabla.Controls.Add(lblT, 0, 0);
            tabla.SetColumnSpan(lblT, 2);

            // Fila 1: Agregar
            var lblA = new Label
            {
                Text = "Agregar",
                Font = Disenio.Fuentes.General,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Height = 20
            };
            tabla.Controls.Add(lblA, 0, 1);
            tabla.Controls.Add(CrearBotonIcono(), 1, 1);

            // Filas siguientes: items + iconoEditar
            for (int i = 0; i < items.Count; i++)
            {
                var lbl = new Label
                {
                    Text = items[i],
                    Font = Disenio.Fuentes.General,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    Height = 30
                };
                tabla.Controls.Add(lbl, 0, i + 2);
                tabla.Controls.Add(CrearBotonEditar(), 1, i + 2);
            }

            panel.Controls.Add(tabla);
            return panel;


        }
        private Button CrearBotonIcono()
        {
            // Botón con icono de editar
            var btn = new Button
            {
                Image = new Bitmap(Disenio.Imagenes.IconoAgregar, new Size(Disenio.tamanoIcono, Disenio.tamanoIcono)),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Width = 30,
                Height = 30,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Cursor = Cursors.Hand,
                Text = ""
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }
        private Button CrearBotonEditar()
        {
            var btn = new Button
            {
                Image = new Bitmap(Disenio.Imagenes.IconoEditar, new Size(Disenio.tamanoIcono, Disenio.tamanoIcono)),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Width = 30,
                Height = 30,
                Margin = new Padding(0),
                Padding = new Padding(0),
                Cursor = Cursors.Hand,
                Text = ""
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

    }
}
