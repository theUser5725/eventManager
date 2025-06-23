using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Resources;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace WinFormsApp1.Vistas
{
    public partial class PanelEvento : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ReunionesBindingSource { get; private set; } = new BindingSource();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ParticipantesBindingSource { get; private set; } = new BindingSource();

        private Evento evento;

        // Controles principales
        private Label lblTitulo;
        private Label lblEstado;
        private Label lblSubtitulo;
        private Label lblInscriptos;
        private Button btnEditarEvento;
        private Button btnGenerarCertificados;
        private Button btnAgregarReunion;
        private TabControl tabControl;
        private TabPage tabReuniones;
        private TabPage tabParticipantes;
        private TableLayoutPanel contenedorReuniones;
        private int margenLateralTabs = 50;
        private int margenLateralBody = 220;
        private int margenLateralTexto = 70;

        public PanelEvento(Evento evento)
        {
            this.evento = evento;
            ReunionesBindingSource.DataSource = evento.Reuniones;
            ParticipantesBindingSource.DataSource = evento.Participantes;
      
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
                Size = new Size(this.Width - (2 * margenLateralBody), 900), // usa el margen
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

            // Título
            lblTitulo = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.Titulo,
                Text = $"{evento?.Categoria.Nombre}: {evento?.Nombre}",
                Location = new Point(0, 50) // temporal, lo centramos después
            };
            panelCentral.Controls.Add(lblTitulo);

            // Centrado manual horizontal (después de agregarlo para que AutoSize funcione)
            lblTitulo.Left = (panelCentral.ClientSize.Width - lblTitulo.Width) / 2;

            panelCentral.Resize += (s, e) =>
            {
                lblTitulo.Left = (panelCentral.ClientSize.Width - lblTitulo.Width) / 2;
            };


            lblEstado = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.General,
                Text = pEvento.EstadoToString(evento?.Estado ?? 0),
                Location = new Point(panelCentral.Width - 150, 100),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            panelCentral.Controls.Add(lblEstado);
            lblEstado.Left = panelCentral.ClientSize.Width - lblEstado.Width - margenLateralTexto;


            lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.General,
                Text = $"Desde {evento?.FechaInicio?.ToString("dd/MM/yyyy") ?? "-"}  Hasta {evento?.FechaFinalizacion?.ToString("dd/MM/yyyy") ?? "-"} ",
                Location = new Point(40, lblTitulo.Bottom + margenLateralTexto)
            };

            lblInscriptos = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.General,
                Text = $"Cantidad Inscritos: {evento?.CantidadParticipantes?.ToString() ?? "-"}",
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            lblInscriptos.Location = new Point(panelCentral.Width - lblInscriptos.Width , lblSubtitulo.Top); // Ajustar a la derecha
            panelCentral.Controls.Add(lblInscriptos);

            // Forzá el cálculo de AutoSize
            lblInscriptos.PerformLayout();
            lblInscriptos.Left = panelCentral.ClientSize.Width - lblInscriptos.Width - margenLateralTexto;

            panelCentral.Controls.Add(lblSubtitulo);

            // Botones  
            btnEditarEvento = new Button
            {
                Text = "Editar Evento",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoEditar,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                ImageAlign = ContentAlignment.MiddleRight, // ícono a la derecha
                TextImageRelation = TextImageRelation.TextBeforeImage,
                Size = new Size(210, 55),
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(margenLateralTexto, lblSubtitulo.Bottom + 40)
            };
            btnEditarEvento.Click += (s, e) => OnEditarEvento();
            panelCentral.Controls.Add(btnEditarEvento);

            btnAgregarReunion = new Button
            {
                Text = "Agregar Reunión",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoAgregar,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleRight,
                TextImageRelation = TextImageRelation.TextBeforeImage,
                Size = new Size(210,55),
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(24, 0, 0, 24),
                Location = new Point(btnEditarEvento.Right+ 16, lblSubtitulo.Bottom + 40)
            };

            btnAgregarReunion.Click += (s, e) => OnAgregarReunion();
            panelCentral.Controls.Add(btnAgregarReunion);

            btnGenerarCertificados = new Button
            {
                Text = "Generar Certificados",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoArchivos,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleRight,
                TextImageRelation = TextImageRelation.TextBeforeImage,
                Size = new Size(210, 55),
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(24, 0, 0, 24),
                Location = new Point(btnAgregarReunion.Right + 16, lblSubtitulo.Bottom + 40),
                Visible = (evento?.Estado == 2)
            };


            btnGenerarCertificados.Click += (s, e) => OnGenerarCertificados();
            panelCentral.Controls.Add(btnGenerarCertificados);


            // Tabss 

            tabControl = new TabControl
            {
                Font = Disenio.Fuentes.General,
                Location = new Point(margenLateralTabs, btnEditarEvento.Bottom + 40),
                Size = new Size(panelCentral.Width - margenLateralTabs * 2, 500),
                DrawMode = TabDrawMode.OwnerDrawFixed,
                Alignment = TabAlignment.Top,
                SizeMode = TabSizeMode.Fixed,
                ItemSize = new Size(120, 35),
                Appearance = TabAppearance.Normal,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Pintar fondo detrás de pestañas
            tabControl.Paint += (s, e) =>
            {
                Rectangle tabsArea = new Rectangle(0, 0, tabControl.Width, tabControl.ItemSize.Height + 6);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), tabsArea);
            };

            tabControl.DrawItem += TabControl_DrawItem;

            inicilaizarTabReuniones();
            
            tabParticipantes = new TabPage("Participantes") { BackColor = Color.White };
            
            tabControl.TabPages.Add(tabParticipantes);

            // Márgenes internos y fondo blanco de cada tab
            foreach (TabPage tab in tabControl.TabPages)
            {
                tab.Padding = new Padding(24);
                tab.BackColor = Color.White;
            }

            panelCentral.Controls.Add(tabControl);

            panelCentral.Resize += (s, e) =>
            {
                lblTitulo.Left = (panelCentral.ClientSize.Width - lblTitulo.Width) / 2;
                lblEstado.Left = panelCentral.ClientSize.Width - lblEstado.Width - margenLateralTexto;
                lblInscriptos.Left = panelCentral.ClientSize.Width - lblInscriptos.Width - margenLateralTexto;

                // Si hay reuniones cargadas, actualizá el ancho de cada panel
                foreach (Control c in tabReuniones.Controls)
                {
                    if (c is FlowLayoutPanel flp)
                    {
                        foreach (Control tarjeta in contenedorReuniones.Controls)
                        {
                            if (tarjeta is Panel p)
                            {
                                p.Width = contenedorReuniones.ClientSize.Width - p.Margin.Horizontal;
                            }
                        }
                    }
                }
            };

        }

        private void inicilaizarTabReuniones()
        {
            /*
            panel (la tarjeta)
            ├── panelCabecera (Dock: Top)
            │   ├── lblTitulo
            │   └── lblExpandir (esquina superior derecha)
            ├── contenidoExpandido (Dock: Top)
            │   └── layout (TableLayoutPanel de 2 columnas)
            │        ├── Row 0: lblHorario        | btnEditar
            │        ├── Row 1: lblDirectivos     | btnAdministrarDir
            │        ├── Row 2: panelDirectivos   |   (ocupa ambas columnas)


            */
            tabReuniones = new TabPage("Reuniones") { BackColor = Color.White };

            contenedorReuniones = new TableLayoutPanel
            {
                Name = "contenedorReuniones",
                Dock = DockStyle.Fill,
                AutoScroll = true,
                ColumnCount = 1,
                RowCount = 0,
            };
            contenedorReuniones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));


            tabReuniones.Controls.Add(contenedorReuniones);
            tabControl.TabPages.Add(tabReuniones);



            foreach (var reunion in evento.Reuniones)
            {
                Panel tarjetaReunion = CrearTarjetaReunion(reunion); // Crear la tarjeta
         
                if (tarjetaReunion != null)
                {
                    contenedorReuniones.Controls.Add(tarjetaReunion, 0, contenedorReuniones.Controls.Count); // Agregar la tarjeta al contenedor
                }
            }



        }
        private Panel CrearTarjetaReunion(Reunion reunion)
        {
            if (reunion == null)
            {
                return null; // Retornar nulo si la reunión no es válida
            }

            bool directivosCargados = false;

            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80, // Altura inicial colapsada
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10),
                Margin = new Padding(24, 6, 24, 6),
                BackColor = Color.White,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            panel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panel.MinimumSize = new Size(400, 80); // para evitar colapsos visuales
 


            var panelCabecera = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(10),
                BackColor = Color.Transparent
            };

            var lblTitulo = new Label
            {
                Text = $"{reunion.Nombre} - Lugar: {reunion.Lugar.nombre} - {reunion.HorarioInicio:dd/MM/yyyy}",
                Font = Disenio.Fuentes.General,
                AutoSize = true,
                Location = new Point(0, 10)
            };
            panelCabecera.Controls.Add(lblTitulo);

            var lblExpandir = new Label
            {
                Text = "▼",
                Font = Disenio.Fuentes.General,
                Size = new Size(30, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelCabecera.Controls.Add(lblExpandir);

            // Ajustar posición de lblExpandir dinámicamente
            panelCabecera.Resize += (s, e) =>
            {
                lblExpandir.Location = new Point(panelCabecera.Width - lblExpandir.Width - 10, (panelCabecera.Height - lblExpandir.Height) / 2);
            };


            var contenidoExpandido = new Panel
            {
                Visible = false,
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            //fila 1
            var lblHorario = new Label
            {
                Text = $"Desde {reunion.HorarioInicio:HH:mm} Hasta {reunion.HorarioFinalizacion:HH:mm}",
                Font = Disenio.Fuentes.labelsLetras,
                AutoSize = true
            };

            var btnEditar = new Button
            {
                Text = "Editar Reunión",
                Font = Disenio.Fuentes.Boton,
                AutoSize = true,
                Margin = new Padding(8)
            };
            var btnAdministrarDir = new Button
            {
                Text = "Administrar Directivos",
                Font = Disenio.Fuentes.Boton,
                AutoSize = true,
                Margin = new Padding(8)
            };
            
            layout.Controls.Add(lblHorario, 0, 0);
            layout.Controls.Add(btnEditar, 1, 0);
            layout.Controls.Add(btnAdministrarDir, 2, 0);
            //fila 2

            var lblDirectivos = new Label
            {
                Text = "Directivos:",
                Font = Disenio.Fuentes.labelsLetras,
                AutoSize = true
            };

            
            layout.Controls.Add(lblDirectivos, 0, 1);
            

            //fila 3
            var panelDirectivos = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            layout.Controls.Add(panelDirectivos, 0, 2);
            layout.SetColumnSpan(panelDirectivos, 2); //ambas columnas

            // separador 
            var lineaDivisoria = new Panel
            {
                Height = 2,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray
            };
            panel.Controls.Add(lineaDivisoria);



            contenidoExpandido.Controls.Add(layout);
            
            panel.Controls.Add(contenidoExpandido);
            panel.Controls.Add(panelCabecera);

            // Toggle expansión
            panelCabecera.Click += expandir;
            lblTitulo.Click += expandir;
            lblExpandir.Click += expandir;

            void expandir(object sender, EventArgs e)
            {
                contenidoExpandido.Visible = !contenidoExpandido.Visible;
                lblExpandir.Text = contenidoExpandido.Visible ? "▲" : "▼";

                if (contenidoExpandido.Visible && !directivosCargados)
                {
                    var directivos = pDirectivo.getAllByReunionId(reunion.IdReunion);

                    foreach (var dir in directivos)
                    {
                        var lblDir = new Label
                        {
                            Text = $"\t \t{dir.Participante.Nombre} {dir.Participante.Apellido} - {dir.Categoria.Nombre}",
                            Font = Disenio.Fuentes.labelsLetras,
                            AutoSize = true
                        };
                        panelDirectivos.Controls.Add(lblDir);
                    }

                    directivosCargados = true;
                }
            }



            return panel;
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tab = sender as TabControl;
            Graphics g = e.Graphics;
            Rectangle tabBounds = tab.GetTabRect(e.Index);

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Simular altura mayor para tab seleccionada
            if (isSelected)
            {
                tabBounds.Y -= 10; // sube
                tabBounds.Height += 10; // agranda
            }else
            {
                tabBounds.Y += 10; // sube
                tabBounds.Height -= 10; // agranda
            }


                // Estilos
                Color backColor = isSelected ? Color.White : Disenio.Colores.AzulOscuro;
            Color textColor = isSelected ? Color.Black : Color.White;
            Font font = isSelected
                ? new Font("Segoe UI", 12, FontStyle.Bold)
                : new Font("Segoe UI", 8, FontStyle.Bold);

            // Bordes redondeados
            int radius = 12;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(tabBounds.Left, tabBounds.Top, radius, radius, 180, 90);
                path.AddArc(tabBounds.Right - radius, tabBounds.Top, radius, radius, 270, 90);
                path.AddLine(tabBounds.Right, tabBounds.Top + radius, tabBounds.Right, tabBounds.Bottom);
                path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.Left, tabBounds.Bottom);
                path.AddLine(tabBounds.Left, tabBounds.Bottom, tabBounds.Left, tabBounds.Top + radius);

                using (SolidBrush backBrush = new SolidBrush(backColor))
                using (SolidBrush textBrush = new SolidBrush(textColor))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.FillPath(backBrush, path);

                    // Texto centrado
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(tab.TabPages[e.Index].Text, font, textBrush, tabBounds, sf);
                }
            }
        }




        // Métodos para eventos de botones (vacíos para que los completes)
        private void OnEditarEvento() { /* lógica a implementar */}
        private void OnAgregarReunion() { /* lógica a implementar */ }
        private void OnGenerarCertificados() { /* lógica a implementar */ }
    }
}

