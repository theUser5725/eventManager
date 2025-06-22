using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Resources;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;
namespace WinFormsApp1.Vistas
{
    public partial class PanelManager : UserControl
    {
        // Contenedor principal del panel


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ReunionesBindingSource { get; private set; } = new BindingSource();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ParticipantesBindingSource { get; private set; } = new BindingSource();

        private static List<Evento> eventos = pEvento.GetAll(); // lista eventos
        
        private static List<Evento> eventosHoy = eventos.FindAll(e => e.FechaInicio.HasValue && e.FechaInicio.Value.Date == DateTime.Today).ToList(); // eventos de hoy


        // Referencia al underline
        //private PictureBox underline;
        // Lista de labels de navegación
        //private List<Label> navLabels = new();

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
        private int margenLateralBody = 150;



        public PanelManager()
        {
            return;
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
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(20),
            };
            panelScrollable.Controls.Add(panelCentral);

            panelScrollable.Resize += (s, e) =>
            {
                int anchoDisponible = panelScrollable.ClientSize.Width;

                // cuando el espacio sea chico para mostrar el pantel (1200 en este caso), se elimina el margen
                int margenAplicado = (anchoDisponible < 1200) ? 0 : margenLateralBody;

                panelCentral.Width = anchoDisponible - (2 * margenAplicado);
                panelCentral.Left = margenAplicado;
                panelCentral.Top = 40; // constante vertical
            };

            panelScrollable.Controls.Add(panelCentral);

            panelScrollable.Resize += (s, e) =>
            {
                panelCentral.Left = (panelScrollable.ClientSize.Width - panelCentral.Width) / 2;
            };

            // Título
            lblTitulo = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.Titulo,
                Text = "Reuniones de Hoy",
                Location = new Point(0, 0)
            };
            panelCentral.Controls.Add(lblTitulo); // agregamos el titulo al panel central

            //contenedor eventos
            var contenedorEventos = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(20),
                Width = panelCentral.Width - 40, // Margen
                Location = new Point(0, lblTitulo.Bottom + 20)
            };
           

            foreach (Evento evento in eventosHoy)
            {
                // Obtener todos los lugares para este evento (fuera del bucle de creación de labels)
                var lugaresEvento = pLugar.GetLugarByEventid(evento);

                foreach (Lugar lugar in lugaresEvento)
                {
                    var lblevento = new Label() // Agregados los paréntesis que faltaban
                    {
                        Text = $"{evento.Nombre} | {lugar.nombre} | {evento.FechaInicio:HH\\:mm} - {evento.FechaFinalizacion:HH\\:mm}",
                        AutoSize = true,
                        Margin = new Padding(0, 0, 0, 10) // Espacio entre elementos
                    };

                    // Agregar el label a tu contenedor (FlowLayoutPanel u otro)
                    contenedorEventos.Controls.Add(lblevento);
                }
            }
            panelCentral.Controls.Add(contenedorEventos); // agregamos el contenedor de eventos al panel central

        }
    }        
}

