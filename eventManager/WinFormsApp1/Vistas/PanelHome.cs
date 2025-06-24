using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsApp1.Vistas
{
    public partial class PanelHome : UserControl
    {
        // Contenedor principal del panel

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ReunionesBindingSource { get; private set; } = new BindingSource();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BindingSource ParticipantesBindingSource { get; private set; } = new BindingSource();

        //las propiedades FechaInicio y FechaFinalizacion son de tipo DateTime? (nullable), es necesario verificar .HasValue antes de acceder al .Value
        private List<Evento> eventos = pEvento.GetAll();



        // Controles principales
        private Label lblTitulocontenedorReunionesHoy;
        private int margenLateralBody = 150;



        public PanelHome()
        {
            ControlPaneles();

        }

        private void ControlPaneles()
        {

            // control de errores
            // Configuración del panel principal
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Dock = DockStyle.Right;

            if (eventos.Count == 0)
            {
                MessageBox.Show($"Eventos hoy: {eventos.Count}");
            }
            else
            {
                var btNuevoEvento = new System.Windows.Forms.Button
                {
                    Text = "Agregar Evento",
                    Font = Disenio.Fuentes.Boton,
                    Image = new Bitmap(Disenio.Imagenes.IconoAgregar, new Size(Disenio.tamanoIcono, Disenio.tamanoIcono)),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.White,
                    ImageAlign = ContentAlignment.MiddleRight, // ícono a la derecha
                    TextImageRelation = TextImageRelation.TextBeforeImage,
                    Size = new Size(180, 40),
                    Padding = new Padding(8, 4, 8, 4),
                    Left = 43,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left
                };
                
                btNuevoEvento.Click += (s, e) =>
                {
                    // Lógica para crear un nuevo evento
                    Cursor = Cursors.WaitCursor;
                };

                var cbxTipoBusqueda = new System.Windows.Forms.ComboBox
                {
                    Text = "Filtrar",
                    DropDownStyle = ComboBoxStyle.DropDownList, // Evita que el usuario escriba
                    AutoSize = false, // Mejor control del tamaño
                    Width = 280, // Ancho fijo recomendado
                    Font = Disenio.Fuentes.Boton, // Fuente estándar de Windows
                    FlatStyle = FlatStyle.Popup, // Apariencia moderna
                    Left = 400,
                    Anchor = AnchorStyles.Top | AnchorStyles.Left ,// Comportamiento al redimensionar
                    
                }; // combobox para tipo de busqueda

                var textoBusqueda = new System.Windows.Forms.TextBox
                {
                    PlaceholderText = "Buscar",
                    Width = 200, // Ancho fijo recomendado
                    Font = Disenio.Fuentes.Boton, // Fuente estándar de Windows
                    Anchor = AnchorStyles.Top | AnchorStyles.Left // Comportamiento al redimensionar
                }; // ingresar datos tipo string al buscar

                var btBuscar = new System.Windows.Forms.Button
                {
                    FlatStyle = FlatStyle.Flat,
                    Image = new Bitmap(Disenio.Imagenes.IconoLupa, new Size(Disenio.tamanoIcono, Disenio.tamanoIcono)),
                    Font = Disenio.Fuentes.Boton,
                    AutoSize = true,


                }; // bt busqueda 
                btBuscar.FlatAppearance.BorderSize = 0;

                btBuscar.Click += (s, e) =>
                {
                    string terminoBusqueda = textoBusqueda.Text.Trim();
                    int indiceBusqueda = cbxTipoBusqueda.SelectedIndex;
                    if (indiceBusqueda > 0 && !string.IsNullOrEmpty(terminoBusqueda))
                    {
                        eventos = FiltrarEventos(indiceBusqueda - 1, terminoBusqueda); // -1 porque el primer ítem es el placeholder
                        cbxTipoBusqueda.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Por favor, seleccione un tipo de búsqueda y escriba un término válido.");
                    }
                }; // evento click

                // panel general -> eventos  
                var PanelIntermedioEventos = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };

                //contenedor Eventos general
                var contenedorEventos = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    FlowDirection = FlowDirection.TopDown,
                    BorderStyle = BorderStyle.Fixed3D,
                    
                };

                MostrarEventos(pEvento.GetAll(), contenedorEventos,true);

                // Contenedor con scroll
                var scrolEventos = new Panel
                {
                    Dock = DockStyle.Fill, // ubicacion de la lista de los eventos de hoy
                    AutoScroll = true,
                    BackColor = Disenio.Colores.GrisClaro
                };

                // redimension del scroll
                scrolEventos.Resize += (s, e) =>
                {
                    int anchoDisponible = scrolEventos.ClientSize.Width;
                    int margen = (anchoDisponible < 1200) ? 10 : 50;

                    PanelIntermedioEventos.Width = Math.Max(300, anchoDisponible - (2 * margen));
                    PanelIntermedioEventos.Left = (anchoDisponible - PanelIntermedioEventos.Width) / 2;
                    PanelIntermedioEventos.Top = 100;
                };



                // agregamos los controles al panel correspondiente
                // scrol <- intermediario + extras <- contenedorEventos
                

                PanelIntermedioEventos.Controls.Add(contenedorEventos); // agregamos el contenedor de eventos al panel general

                scrolEventos.Controls.Add(btNuevoEvento);
                scrolEventos.Controls.Add(cbxTipoBusqueda);
                scrolEventos.Controls.Add(textoBusqueda); // agregamos el textbox al panel con scroll
                scrolEventos.Controls.Add(btBuscar); // agregamos el boton de busqueda al panel con scroll
                
                scrolEventos.Controls.Add(PanelIntermedioEventos); // agregamos el panel general al panel con scroll

                // control combobox
                cbxTipoBusqueda.Location = new Point(btNuevoEvento.Width + 50, 1);
                cbxTipoBusqueda.Items.Add("Filtrar por...");
                cbxTipoBusqueda.Items.AddRange(new object[] { "Categoria", "Nombre","Fecha" });
                cbxTipoBusqueda.SelectedIndex = 0;

                //control textbox
                textoBusqueda.Location = new Point(cbxTipoBusqueda.Location.X+cbxTipoBusqueda.Width + 5, 1);

                // control botonBusqueda
                btBuscar.Location = new Point(textoBusqueda.Location.X + textoBusqueda.Width + 5, 1);

                PanelIntermedioEventos.Padding = new Padding(btNuevoEvento.Height + 3);
                

                // panel  -> Reuniones  

                //contenedor reuniones de hoy
                var contenedorReunionesHoy = new FlowLayoutPanel
                {

                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    FlowDirection = FlowDirection.TopDown,
                    BorderStyle = BorderStyle.Fixed3D,
                };

                MostrarReuniones(pEvento.GetAll(), contenedorReunionesHoy, true); // mostrar reuniones de hoy

                // Título contenedor reuniones
                lblTitulocontenedorReunionesHoy = new Label
                {
                    AutoSize = true,
                    Font = Disenio.Fuentes.General,
                    Text = $" Reuniones de hoy: ",

                };

                // Contenedor con scroll - (defino las psiciones del resto de los panels)
                var ScrolReuniones = new Panel
                {
                    Dock = DockStyle.Fill, // ubicacion de la lista de los eventos de hoy
                    AutoScroll = true,
                    BackColor = Disenio.Colores.GrisClaro
                };
                // redimencion del scrol
                ScrolReuniones.Resize += (s, e) =>
                {
                    int anchoDisponible = ScrolReuniones.ClientSize.Width;
                    int margen = (anchoDisponible < 1200) ? 10 : 50;

                    contenedorReunionesHoy.Width = Math.Max(300, anchoDisponible - (2 * margen));
                    contenedorReunionesHoy.Left = (anchoDisponible - contenedorReunionesHoy.Width) / 2;
                    contenedorReunionesHoy.Top = 100;
                };

                var panelIntermedioReuniones = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                };
                // agregamos los controles al panel correspondiente

                // Scrol <- intermedio + Titulo <- contenedor
                ScrolReuniones.Controls.Add(lblTitulocontenedorReunionesHoy);
                panelIntermedioReuniones.Controls.Add(contenedorReunionesHoy);
                ScrolReuniones.Controls.Add(panelIntermedioReuniones);

                // control posiciones paneles(orientacino, direccion , colores)
                panelIntermedioReuniones.Padding = new Padding(lblTitulocontenedorReunionesHoy.Height);
                lblTitulocontenedorReunionesHoy.Location = new Point(ScrolReuniones.Width / 2);
                panelIntermedioReuniones.BackColor = ScrolReuniones.BackColor;


                // se utiliza este metodo para mostrar dos Paneles a la vez
                var MasterTableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
        
                    BackColor = Color.White,
                    ColumnStyles = {
                        new ColumnStyle(SizeType.Percent, 70F),
                        new ColumnStyle(SizeType.Percent, 30F)
                    },
                    
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None
                };

                Panel panelEspacio = new Panel
                {
                    Dock = DockStyle.Top,

                    ForeColor = Color.White,
                };
                panelEspacio.Height = 40;
                MasterTableLayout.Controls.Add(panelEspacio, 0, 0);
                MasterTableLayout.SetColumnSpan(panelEspacio, 1);

                MasterTableLayout.Controls.Add(scrolEventos, 0, 1);
                MasterTableLayout.Controls.Add(ScrolReuniones, 1, 1);

                this.Controls.Add(MasterTableLayout);
            }

        }
        public List<Evento> FiltrarEventos(int indiceBusqueda, string terminoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(terminoBusqueda))
                return new List<Evento>();

            return pEvento.BuscarEnBD(indiceBusqueda, terminoBusqueda);
        }
        public void MostrarEventos(List<Evento> eventos, FlowLayoutPanel panelContenedor, bool mostrar)
        {
            if (mostrar)
            {

                // foreach-> pertenece a contenedor de eventos (contenedorEventos) carga labels 
                foreach (Evento evento in eventos)
                {

                    var table = new TableLayoutPanel
                    {
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        ColumnCount = 4,
                        Margin = new Padding(5, 5, 5, 10),
                        Tag = evento,
                        
                    };
                    
                    Font fuente = Disenio.Fuentes.SecundarioBold;

                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                    table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

                    var lblCategoria = new Label()
                    {

                        Text = $"{evento.Categoria.Nombre} ",
                        AutoSize = true,
                        Font = fuente,
                        ForeColor = Color.Black,
                        Tag = evento,
                    };

                    var lblNombre = new Label()
                    {

                        Text = $"{evento.Nombre} ",
                        AutoSize = true,
                        Font = fuente,
                        ForeColor = Color.Black,
                        Tag = evento,
                    };



                    var lblFecha = new Label()
                    {

                        Text = $"{evento.FechaInicio:dd/MM/yyyy} - {evento.FechaFinalizacion:dd/MM/yyyy}",

                        AutoSize = true,
                        Font = fuente,
                        ForeColor = Color.Black,
                        Tag = evento,
                    };
                    var lblEstado = new Label()
                    {

                        Text = $"{FiltroPorEstado(evento)}",
                        AutoSize = true,
                        Font = fuente,
                        ForeColor = Color.Black,
                        Tag = evento,
                    };
                    lblCategoria.Click += expandirLabel;
                    lblNombre.Click += expandirLabel;

                    lblFecha.Click += expandirLabel;
                    lblEstado.Click += expandirLabel;

                    table.Controls.Add(lblCategoria, 0, 0);
                    table.Controls.Add(lblNombre, 1, 0);
                    table.Controls.Add(lblFecha, 2, 0);
                    table.Controls.Add(lblEstado, 3, 0);

                    table.Click += expandirTable;
                    panelContenedor.Controls.Add(table);

                }
            }
            else
            {
                return; // Si no se debe mostrar, salir del método

            }
        }
        private void expandirTable(object sender, EventArgs e)
        {
            if (sender is TableLayoutPanel table && table.Tag is Evento evento)
            {
                var parentForm = ((Control)sender).FindForm() as FormPrincipal;
                parentForm?.cambiarVista(3, true, evento);
                MessageBox.Show($"Evento seleccionado: {evento.IdEvento}"); // Mensaje de depuración
            }
        }

        private void expandirLabel(object sender, EventArgs e)
        {
            if (sender is Label label && label.Tag is Evento evento)
            {
                var parentForm = ((Control)sender).FindForm() as FormPrincipal;
                parentForm?.cambiarVista(3, true, evento);
                MessageBox.Show($"Evento seleccionado: {evento.IdEvento}"); // Mensaje de depuración
            }
        }

        public void MostrarReuniones(List<Evento> eventos, FlowLayoutPanel panelContenedor, bool mostrar)
        {
            if (mostrar)
            {
                foreach (Evento evento in eventos)
                {
                    List<Reunion> reunionesDia = pReunion.OrderByEventoAndDate(evento.IdEvento, DateTime.Now); // reuniones del día para ese evento

                    foreach (Reunion reunion in reunionesDia)
                    {
                        List<Lugar> lugares = pLugar.GetLugarByEventid(evento); // lugares asociados al evento

                        foreach (Lugar lugar in lugares)
                        {
                            var table = new TableLayoutPanel
                            {
                                Dock = DockStyle.Fill,
                                AutoSize = true,
                                ColumnCount = 4,
                                Margin = new Padding(5, 5, 5, 10),
                                Tag = evento,
                            };

                            Font fuente = Disenio.Fuentes.SecundarioBold;

                            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
                            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
                            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

                            var lblCategoria = new Label()
                            {
                                Text = $"{evento.Categoria.Nombre} ",
                                AutoSize = true,
                                Font = fuente,
                                ForeColor = Color.Black,
                            };

                            var lblNombre = new Label()
                            {
                                Text = $"{evento.Nombre} ",
                                AutoSize = true,
                                Font = fuente,
                                ForeColor = Color.Black,
                            };

                            var lblFecha = new Label()
                            {
                                Text = $"{evento.FechaInicio:dd/MM/yyyy} - {evento.FechaFinalizacion:dd/MM/yyyy}",
                                AutoSize = true,
                                Font = fuente,
                                ForeColor = Color.Black,
                            };

                            var lblEstado = new Label()
                            {
                                Text = $"{FiltroPorEstado(evento)}",
                                AutoSize = true,
                                Font = fuente,
                                ForeColor = Color.Black,
                            };

                            lblCategoria.Click += expandirLabel;
                            lblNombre.Click += expandirLabel;
                            lblFecha.Click += expandirLabel;
                            lblEstado.Click += expandirLabel;

                            table.Controls.Add(lblCategoria, 0, 0);
                            table.Controls.Add(lblNombre, 1, 0);
                            table.Controls.Add(lblFecha, 2, 0);
                            table.Controls.Add(lblEstado, 3, 0);

                            table.Click += expandirTable;
                            panelContenedor.Controls.Add(table);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        private Color FiltroColorPorEstado(Evento evento,bool fondoEspacio)
        {
            if (fondoEspacio)
            {
                return evento.Estado switch
                {
                    0 => Color.White, // Estado 0: Gris claro
                    1 => Color.LightGreen, // Estado 1: Verde claro
                    2 => Color.LightYellow, // Estado 2: Coral 
                    _ => Color.Red, // Por defecto: Blanco
                };
            }
            else
            {
                if (evento.Estado < 3)  
                {
                    return Color.White;
                } else
                {
                    return Color.Black;
                }
                 
            }
        }
        private String FiltroPorEstado(Evento evento)
        {
            // Devuelve un color basado en el estado del evento
            return evento.Estado switch
            {
                0 => "No iniciado",
                1 => "En curso",
                2 => "Finalizado",
                3 => "Cancelado",
                _ => "Desconocido",
            };

        }
   
    }
}
