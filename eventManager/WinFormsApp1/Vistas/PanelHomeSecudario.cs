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
        private List<Evento> eventos = pEvento.GetAll();
        
        // Controles principales
        private Label lblTitulopanelSecundario;
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
                // panel general -> eventos  
                var panelGeneral = new Panel
                {
                    AutoScroll = true,
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Padding = new Padding(25),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Disenio.Colores.AzulOscuro
                };

                //contenedor Eventos general
                var contenedorEventosGeneral = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Disenio.Colores.AzulOscuro,
                    FlowDirection = FlowDirection.TopDown,
                    Padding = new Padding(5),
                    Width = panelGeneral.Width - 40, // Margen
                    BorderStyle = BorderStyle.Fixed3D,
                };

                // foreach-> pertenece a contenedor de eventos (contenedorEventosGeneral) carga labels 
                foreach (Evento evento in eventos)
                {
                    // Obtener todos los lugares para este evento 
                    var lugaresEvento = pLugar.GetLugarByEventid(evento);

                    foreach (Lugar lugar in lugaresEvento) // agrega labels en el contenedor 
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
                        contenedorEventosGeneral.Controls.Add(lblevento);
                    }
                }

                // Contenedor con scroll
                var ScrolEventosGeneral = new Panel
                {
                    Dock = DockStyle.Fill, // ubicacion de la lista de los eventos de hoy
                    Width = 500,
                    AutoScroll = true,
                };

                // redimension del scroll
                ScrolEventosGeneral.Resize += (s, e) =>
                {
                    int anchoDisponible = ScrolEventosGeneral.ClientSize.Width;
                    int margen = (anchoDisponible < 1200) ? 10 : 50;

                    panelGeneral.Width = Math.Max(300, anchoDisponible - (2 * margen));
                    panelGeneral.Left = (anchoDisponible - panelGeneral.Width) / 2;
                    panelGeneral.Top = 100;
                };

                // agregamos los controles al panel correspondiente
                panelGeneral.Controls.Add(contenedorEventosGeneral); // agregamos el contenedor de eventos al panel general

                ScrolEventosGeneral.Controls.Add(panelGeneral); // agregamos el panel general al panel con scroll

               //__________________________________________________________

                // panel secundario -> eventos  
                var panelSecundario = new Panel
                {
                    AutoScroll = true,
                    Dock = DockStyle.Right,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Padding = new Padding(25),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Disenio.Colores.AzulOscuro
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
                    Width = panelSecundario.Width - 40, // Margen
                    BorderStyle = BorderStyle.Fixed3D,
                };

                // foreach-> pertenece a contenedor de eventos (contenedorEventosHoy) carga labesl 
                foreach (Evento eventohoy in eventosHoy)
                {
                    // Obtener todos los lugares para este evento 
                    var lugaresEvento = pLugar.GetLugarByEventid(eventohoy);

                    foreach (Lugar lugar in lugaresEvento) // agrega lablels en el contenedor 
                    {

                        var lblevento = new Label()
                        {
                            Text = $"{eventohoy.Nombre} | {lugar.nombre} | De:{eventohoy.FechaInicio:HH\\:mm} a {eventohoy.FechaFinalizacion:HH\\:mm}",
                            TextAlign = ContentAlignment.TopCenter,
                            AutoSize = true,
                            Margin = new Padding(5, 0, 5, 10),
                            Font = Disenio.Fuentes.Titulo,
                            BackColor = Disenio.Colores.GrisClaro,
                            Tag = eventohoy // Almacena lo que se va a enviar al hacer click en el label
                        };

                        lblevento.Click += (s, e) =>
                        {
                            // evento click en el label...
                            Cursor = Cursors.WaitCursor;
                        };
                        contenedorEventosHoy.Controls.Add(lblevento); // agregar el label del evento al contenedor de eventos
                    }
                }

                // Título
                lblTitulopanelSecundario = new Label
                {
                    AutoSize = true,
                    Font = Disenio.Fuentes.Titulo,

                    Text = $" Reuniones del {DateTime.Now:dd - MM - yyyy}",

                    ForeColor = Color.White,
                    Location = new Point(25, 0),

                };

                // Contenedor con scroll - (defino las psiciones del resto de los panels)
                var ScrolEventos = new Panel
                {
                    Dock = DockStyle.Fill, // ubicacion de la lista de los eventos de hoy
                    Width = 500,
                    AutoScroll = true,
                };
                // redimencion del scrol
                ScrolEventos.Resize += (s, e) =>
                {
                    int anchoDisponible = ScrolEventos.ClientSize.Width;
                    int margen = (anchoDisponible < 1200) ? 10 : 50;

                    panelSecundario.Width = Math.Max(300, anchoDisponible - (2 * margen));
                    panelSecundario.Left = (anchoDisponible - panelSecundario.Width) / 2;
                    panelSecundario.Top = 100;
                };

                
                // agregamos los controles al panel correspondiente
                panelSecundario.Controls.Add(contenedorEventosHoy); // agregamos el contenedor de eventos al panel secundario

                panelSecundario.Controls.Add(lblTitulopanelSecundario); // agregamos el titulo al panel secudario

                ScrolEventos.Controls.Add(panelSecundario); // agregamos el panel secundario al panel con scroll

                // se utiliza este metodo para mostrar dos Paneles a la vez
                var MasterTableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Percent, 70F),
                        new ColumnStyle(SizeType.Percent, 30F)
                    },
                        
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                };

                MasterTableLayout.Controls.Add(ScrolEventosGeneral, 0, 0);
                MasterTableLayout.Controls.Add(ScrolEventos, 1, 0);

                this.Controls.Add(MasterTableLayout);

            }
        }
    }
}


