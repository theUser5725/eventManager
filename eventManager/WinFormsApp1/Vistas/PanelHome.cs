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

namespace WinFormsApp1.Vistas
{
    public partial class PanelHome: UserControl
    {
        // Contenedor principal del panel

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ReunionesBindingSource { get; private set; } = new BindingSource();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ParticipantesBindingSource { get; private set; } = new BindingSource();

        private List<Evento> eventos; // lista eventos

        private List<Evento> eventosHoy;


        // Referencia al underline
        //private PictureBox underline;
        // Lista de labels de navegación
        //private List<Label> navLabels = new();

        // Controles principales
        private Label lblTitulo;
        private int margenLateralBody = 150;



        public PanelHome()
        {
            eventos = pEvento.GetAll();
            eventosHoy = eventos;
            
            
            Inicializar(); 

        }

        private void Inicializar()
        {
            if (eventos.Count == 0)
            {
                MessageBox.Show("No hay eventos programados para hoy.");
                return;
            }
            if (eventosHoy.Count == 0)
            {
                MessageBox.Show($"Eventos hoy: {eventosHoy.Count}");
            }
            // Configuración del panel principal
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Dock = DockStyle.Fill;
            // panel central 
            var panelCentral = new Panel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(25),
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = Disenio.Colores.AzulOscuro
            }; // 

            // Contenedor con scroll
            var panelScrolEventosHoy = new Panel
            {
                Dock = DockStyle.Right, // ubicacion de la lista de los eventos de hoy

                Width = 500,
                AutoScroll = true,
               

            };
            var panelScrolGeneral = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll= true,
                 BorderStyle = BorderStyle.FixedSingle,
            };
            // redimencion del scrol
            panelScrolEventosHoy.Resize += (s, e) =>
            {
                int anchoDisponible = panelScrolEventosHoy.ClientSize.Width;
                // cuando el espacio sea chico para mostrar el pantel (1200 en este caso), se elimina el margen
                int margenAplicado = (anchoDisponible < 1200) ? 0 : margenLateralBody;
                
                panelCentral.Width = anchoDisponible - (2* anchoDisponible);

                panelCentral.Left = margenAplicado;

                panelCentral.Top = 100; // constante vertical
            };
            //???
            panelScrolEventosHoy.Resize += (s, e) =>
            {
                panelCentral.Left = (panelScrolEventosHoy.ClientSize.Width - panelCentral.Width) / 2;
            };

            // Título
            lblTitulo = new Label
            {
                AutoSize = true,
                Font = Disenio.Fuentes.Titulo,

                Text = $" Reuniones del {DateTime.Now:dd - MM - yyyy}",
               
                ForeColor = Color.White,
                Location = new Point(25, 0),
    
            };
            
            //contenedor Eventos de hoy
            var contenedorEventosHoy = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,

                BackColor = Disenio.Colores.AzulOscuro,

                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(5),
                Width = panelCentral.Width - 40, // Margen
                
                //BorderStyle = BorderStyle.FixedSingle,
               
            };
            
            foreach (Evento evento in eventosHoy)
            {
                // Obtener todos los lugares para este evento 
                var lugaresEvento = pLugar.GetLugarByEventid(evento);

                foreach (Lugar lugar in lugaresEvento)
                {
                    
                    var lblevento = new Label() 
                    {
                        Text = $"{evento.Nombre} | {lugar.nombre} | De:{evento.FechaInicio:HH\\:mm} a {evento.FechaFinalizacion:HH\\:mm}",
                        TextAlign = ContentAlignment.TopCenter,
                        AutoSize = true,
                        Margin = new Padding(5, 0, 5, 10), 
                        Font = Disenio.Fuentes.Titulo,
                        BackColor = Disenio.Colores.GrisClaro,
                        

                        Tag = evento // Almacena lo que se va a enviar al hacer click en el label
                    };

                    lblevento.Click += (s, e) =>
                    {
                       // evento click en el label...
                       Cursor = Cursors.WaitCursor;
                        
                    };

                    contenedorEventosHoy.Controls.Add(lblevento); // agregar el label del evento al contenedor de eventos
                }
            }

            panelScrolEventosHoy.Controls.Add(panelCentral);

            panelCentral.Controls.Add(lblTitulo); // agregamos el titulo al panel central
            
            panelCentral.Controls.Add(contenedorEventosHoy); // agregamos el contenedor de eventos al panel central
           

            this.Controls.Add(panelScrolEventosHoy);




        }
    }        
}

