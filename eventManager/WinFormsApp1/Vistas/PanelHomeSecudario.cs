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
    public partial class PanelHomeSecudario: UserControl
    {
        // Contenedor principal del panel

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ReunionesBindingSource { get; private set; } = new BindingSource();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ParticipantesBindingSource { get; private set; } = new BindingSource();
        
        //las propiedades FechaInicio y FechaFinalizacion son de tipo DateTime? (nullable), es necesario verificar .HasValue antes de acceder al .Value
        private List<Evento> eventosHoy = pEvento.GetAll()
            .Where(e => e.FechaInicio.HasValue && e.FechaFinalizacion.HasValue && e.FechaInicio.Value.Date <= DateTime.Today && e.FechaFinalizacion.Value.Date >= DateTime.Today).ToList();
        // Controles principales
        private Label lblTituloPanelGeneral;
        private int margenLateralBody = 150;



        public PanelHomeSecudario()
        {
            ControlPaneles();
        }

        private void ControlPaneles()
        {

            // control de errores
            // Configuración del panel principal
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Dock = DockStyle.Right;

            if (eventosHoy.Count == 0)
            {
                MessageBox.Show($"Eventos hoy: {eventosHoy.Count}");
            }
            else
            {

                // panel secundario -> eventos  
                var panelGeneral = new Panel
                {
                    Dock = DockStyle.Right,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Padding = new Padding(25),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Disenio.Colores.AzulOscuro
                };

                //contenedor Eventos de hoy
                var contenedorEventos = new FlowLayoutPanel
                {
                    Dock = DockStyle.Right,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Disenio.Colores.AzulOscuro,
                    FlowDirection = FlowDirection.TopDown,
                    Padding = new Padding(5),
                    Width = panelGeneral.Width - 40, // Margen

                    //BorderStyle = BorderStyle.FixedSingle,

                };

                // foreach-> pertenece a contenedor de eventos (contenedorEventos) carga labesl 
                foreach (Evento evento in eventosHoy)
                {
                    // Obtener todos los lugares para este evento 
                    var lugaresEvento = pLugar.GetLugarByEventid(evento);

                    foreach (Lugar lugar in lugaresEvento) // agrega lablels en el contenedor 
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
                        contenedorEventos.Controls.Add(lblevento); // agregar el label del evento al contenedor de eventos
                    }
                }

                // Título
                lblTituloPanelGeneral = new Label
                {
                    AutoSize = true,
                    Font = Disenio.Fuentes.Titulo,

                    Text = $" Reuniones del {DateTime.Now:dd - MM - yyyy}",

                    ForeColor = Color.White,
                    Location = new Point(25, 0),

                };

                // Contenedor con scroll - (defino las psiciones del resto de los panels)
                var MasterScrolEventos = new Panel
                {
                    Dock = DockStyle.Fill, // ubicacion de la lista de los eventos de hoy
                    Width = 500,
                    AutoScroll = true,
                };
                // redimencion del scrol
                MasterScrolEventos.Resize += (s, e) =>
                {
                    int anchoDisponible = MasterScrolEventos.ClientSize.Width;
                    int margen = (anchoDisponible < 1200) ? 10 : 50;

                    panelGeneral.Width = Math.Max(300, anchoDisponible - (2 * margen));
                    panelGeneral.Left = (anchoDisponible - panelGeneral.Width) / 2;
                    panelGeneral.Top = 100;
                };


                // agregamos los controles al panel correspondiente
                panelGeneral.Controls.Add(contenedorEventos); // agregamos el contenedor de eventos al panel secundario

                panelGeneral.Controls.Add(lblTituloPanelGeneral); // agregamos el titulo al panel secudario

                MasterScrolEventos.Controls.Add(panelGeneral); // agregamos el panel secundario al panel con scroll

                this.Controls.Add(MasterScrolEventos);
            }
        }   
    }        
}

