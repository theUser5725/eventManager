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
        private Button btnEditarEvento;
        private Button btnGenerarCertificados;
        private TabControl tabControl;
        private TabPage tabReuniones;
        private TabPage tabParticipantes;
        private int margenLateralTabs = 50;
        private int margenLateralBody=150;

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
                Size = new Size(this.Width - (2 * margenLateralBody), 600), // usa el margen
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
                Text = $": {evento?.Nombre}",
                Location = new Point(0, 0)
            };
            panelCentral.Controls.Add(lblTitulo);

            lblEstado = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.General,
                Text = pEvento.EstadoToString(evento?.Estado ?? 0),
                Location = new Point(panelCentral.Width - 150, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            panelCentral.Controls.Add(lblEstado);

            lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.General,
                Text = $"Desde {evento?.FechaInicio?.ToString("dd/MM/yyyy") ?? "-"}  Hasta {evento?.FechaFinalizacion?.ToString("dd/MM/yyyy") ?? "-"}    " +
                       $"Cantidad Inscritos {evento?.CantidadParticipantes?.ToString() ?? "-"}",
                Location = new Point(0, lblTitulo.Bottom + 16)
            };
            panelCentral.Controls.Add(lblSubtitulo);

            btnEditarEvento = new Button
            {
                Text = "Editar Evento",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoEditar,
                ImageAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Location = new Point(0, lblSubtitulo.Bottom + 16)
            };
            btnEditarEvento.Click += (s, e) => OnEditarEvento();
            panelCentral.Controls.Add(btnEditarEvento);

            btnGenerarCertificados = new Button
            {
                Text = "Generar Certificados",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoArchivos,
                ImageAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Location = new Point(btnEditarEvento.Right + 16, lblSubtitulo.Bottom + 16),
                Visible = (evento?.Estado == 2)
            };
            btnGenerarCertificados.Click += (s, e) => OnGenerarCertificados();
            panelCentral.Controls.Add(btnGenerarCertificados);

            
            tabControl = new TabControl
            {
                Font = Disenio.Fuentes.General,
                Location = new Point(50, btnEditarEvento.Bottom + 24),

                Anchor = AnchorStyles.Top
            };

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Alignment = TabAlignment.Top;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(100, 30); // ancho y alto de cada tab
            tabControl.DrawItem += TabControl_DrawItem;

            tabControl.Size = new Size(panelCentral.Width - margenLateralTabs*2, 400); // márgenes laterales de 50px


            panelCentral.Controls.Add(tabControl);  
            
            tabReuniones = new TabPage("Reuniones");
            tabParticipantes = new TabPage("Participantes");
            tabControl.TabPages.Add(tabReuniones);
            tabControl.TabPages.Add(tabParticipantes);
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage tab = tabControl.TabPages[e.Index];
            Rectangle rect = e.Bounds;
            using (SolidBrush backBrush = new SolidBrush(Color.White))
            using (SolidBrush textBrush = new SolidBrush(Color.Black))
            using (GraphicsPath path = new GraphicsPath())
            {
                // Bordes redondeados arriba
                int radius = 8;
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Color de fondo por selección
                Color fondo = (e.State & DrawItemState.Selected) != 0 ? Color.LightGray : Color.White;
                using (SolidBrush brush = new SolidBrush(fondo))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Texto
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(tab.Text, tab.Font, textBrush, rect, sf);
            }
        }

        // Métodos para eventos de botones (vacíos para que los completes)
        private void OnEditarEvento() { /* lógica a implementar */ }
        private void OnGenerarCertificados() { /* lógica a implementar */ }
    }
}

