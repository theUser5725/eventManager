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
        private TabControl tabControl;
        private TabPage tabReuniones;
        private TabPage tabParticipantes;
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
            btnEditarEvento = new Button
            {
                Text = "Editar Evento",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoEditar,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                ImageAlign = ContentAlignment.MiddleRight, // ícono a la derecha
                TextImageRelation = TextImageRelation.TextBeforeImage,
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(margenLateralTexto, lblSubtitulo.Bottom + 40)
            };
            btnEditarEvento.Click += (s, e) => OnEditarEvento();
            panelCentral.Controls.Add(btnEditarEvento);

            btnGenerarCertificados = new Button
            {
                Text = "Generar Certificados",
                Font = Disenio.Fuentes.Boton,
                Image = Disenio.Imagenes.IconoArchivos,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleRight,
                TextImageRelation = TextImageRelation.TextBeforeImage,
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                Location = new Point(btnEditarEvento.Right + 16, lblSubtitulo.Bottom + 16),
                Visible = (evento?.Estado == 2)
            };
            btnGenerarCertificados.Click += (s, e) => OnGenerarCertificados();
            panelCentral.Controls.Add(btnGenerarCertificados);



            tabControl = new TabControl
            {
                Font = Disenio.Fuentes.General,
                Location = new Point(margenLateralTabs, btnEditarEvento.Bottom + 40),
                Size = new Size(panelCentral.Width - margenLateralTabs * 2, 400),
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

            tabReuniones = new TabPage("Reuniones") { BackColor = Color.White };
            tabParticipantes = new TabPage("Participantes") { BackColor = Color.White };
            tabControl.TabPages.Add(tabReuniones);
            tabControl.TabPages.Add(tabParticipantes);

            // Márgenes internos y fondo blanco de cada tab
            foreach (TabPage tab in tabControl.TabPages)
            {
                tab.Padding = new Padding(24);
                tab.BackColor = Color.White;
            }

            panelCentral.Controls.Add(tabControl);


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
        private void OnEditarEvento() { /* lógica a implementar */ }
        private void OnGenerarCertificados() { /* lógica a implementar */ }
    }
}

