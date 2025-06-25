using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;
using WinFormsApp1.Controladores;

namespace WinFormsApp1.Vistas
{
    public partial class FormABM : Form
    {
        private List<Control> _controlesEntrada;
        private List<CampoEditable> _camposActuales;
        private Button _btnConfirmar;
        private Button _btnCancelar;
        private Panel _panelPrincipal;
        private Panel _headerPanel;
        public FormABM(object mdl)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(400, 300);
            IniciarABM(mdl);
        }
        public void IniciarABM(object modelo)
        {
            bool nuevo = false;

            if (modelo == null)
            {
                nuevo = true;
            }

            List<CampoEditable> campos = new();

            switch (modelo)
            {
                case Evento e:
                    if (nuevo) e = new Evento();

                    campos.Add(new CampoEditable { Nombre = "Nombre", Valor = e.Nombre ?? "", Tipo = typeof(string), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "TotalHoras", Valor = e.TotalHoras ?? 0, Tipo = typeof(int), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "CantidadParticipantes", Valor = e.CantidadParticipantes ?? 0, Tipo = typeof(int), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "FechaInicio", Valor = e.FechaInicio ?? DateTime.Now, Tipo = typeof(DateTime), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "FechaFinalizacion", Valor = e.FechaFinalizacion ?? DateTime.Now, Tipo = typeof(DateTime), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "Estado", Valor = e.Estado, Tipo = typeof(int), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "Categoria", Valor = e.Categoria ?? new CategoriaEvento(), Tipo = typeof(CategoriaEvento), EsModificable = true });

                    var (camposEditadosEvento, accionEvento) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    e.Nombre = (string)camposEditadosEvento[0].Valor;
                    e.TotalHoras = Convert.ToInt32(camposEditadosEvento[1].Valor);
                    e.CantidadParticipantes = Convert.ToInt32(camposEditadosEvento[2].Valor);
                    e.FechaInicio = (DateTime)camposEditadosEvento[3].Valor;
                    e.FechaFinalizacion = (DateTime)camposEditadosEvento[4].Valor;
                    e.Estado = Convert.ToInt32(camposEditadosEvento[5].Valor);
                    e.Categoria = pCategoriaEvento.GetById((int)(camposEditadosEvento[6].Valor));

                    EjecutarPersistencia("pEvento", accionEvento, e);
                    break;

                case Reunion r:
                    if (nuevo) r = new Reunion();

                    campos.Add(new CampoEditable { Nombre = "Nombre", Valor = r.Nombre ?? "", Tipo = typeof(string), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "HorarioInicio", Valor = r.HorarioInicio == default ? DateTime.Now : r.HorarioInicio, Tipo = typeof(DateTime), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "HorarioFinalizacion", Valor = r.HorarioFinalizacion == default ? DateTime.Now : r.HorarioFinalizacion, Tipo = typeof(DateTime), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "Lugar", Valor = r.Lugar ?? new Lugar(), Tipo = typeof(Lugar), EsModificable = true });

                    var (camposEditadosReunion, accionReunion) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    r.Nombre = (string)camposEditadosReunion[0].Valor;
                    r.HorarioInicio = (DateTime)camposEditadosReunion[1].Valor;
                    r.HorarioFinalizacion = (DateTime)camposEditadosReunion[2].Valor;
                    r.Lugar = (Lugar)camposEditadosReunion[3].Valor;

                    EjecutarPersistencia("pReunion", accionReunion, r);
                    break;

                case Directivo d:
                    if (nuevo) d = new Directivo(0, 0, 0, new Participante(), new CategoriaDirectiva());

                    campos.Add(new CampoEditable { Nombre = "Participante", Valor = d.Participante ?? new Participante(), Tipo = typeof(Participante), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "Categoria", Valor = d.Categoria ?? new CategoriaDirectiva(), Tipo = typeof(CategoriaDirectiva), EsModificable = true });

                    var (camposEditadosDirectivo, accionDirectivo) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    d.Participante = (Participante)camposEditadosDirectivo[0].Valor;
                    d.Categoria = pCategoriaDirectivo.GetById((int)((ComboBox)camposEditadosDirectivo[1].Valor).SelectedValue);

                    EjecutarPersistencia("pDirectivo", accionDirectivo, d);
                    break;

                case Lugar l:
                    if (nuevo) l = new Lugar();

                    campos.Add(new CampoEditable { Nombre = "Nombre", Valor = l.nombre ?? "", Tipo = typeof(string), EsModificable = true });
                    campos.Add(new CampoEditable { Nombre = "Capacidad", Valor = l.capacidad, Tipo = typeof(int), EsModificable = true });

                    var (camposEditadosLugar, accionLugar) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    l.nombre = (string)camposEditadosLugar[0].Valor;
                    l.capacidad = Convert.ToInt32(camposEditadosLugar[1].Valor);

                    EjecutarPersistencia("pLugar", accionLugar, l);
                    break;



                case CategoriaEvento ce:
                    if (nuevo) ce = new CategoriaEvento();

                    campos.Add(new CampoEditable { Nombre = "Nombre", Valor = ce.Nombre ?? "", Tipo = typeof(string), EsModificable = true });

                    var (camposEditadosCatEvento, accionCatEvento) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    ce.Nombre = (string)camposEditadosCatEvento[0].Valor;

                    EjecutarPersistencia("pCategoriaEvento", accionCatEvento, ce);
                    break;

                case CategoriaDirectiva cd:
                    if (nuevo) cd = new CategoriaDirectiva();

                    campos.Add(new CampoEditable { Nombre = "Nombre", Valor = cd.Nombre ?? "", Tipo = typeof(string), EsModificable = true });

                    var (camposEditadosCatDir, accionCatDir) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    cd.Nombre = (string)camposEditadosCatDir[0].Valor;

                    EjecutarPersistencia("pCategoriaDirectivo", accionCatDir, cd);
                    break;

                default:
                    MessageBox.Show("Tipo de objeto no soportado.");
                    break;
            }
        }

        // Stub para compilar. Debe implementarse correctamente
        private void EjecutarPersistencia(string nombreControlador, int accion, object objeto)
        {
            object controlador = null;

            // Crear instancia del controlador por nombre
            switch (nombreControlador)
            {
                case "pCategoriaDirectivo":
                    controlador = new pCategoriaDirectivo();
                    break;
                case "pEvento":
                    controlador = new pEvento();
                    break;
                case "pReunion":
                    controlador = new pReunion();
                    break;
                case "pDirectivo":
                    controlador = new pDirectivo();
                    break;
                case "pLugar":
                    controlador = new pLugar();
                    break;
                case "pCategoriaEvento":
                    controlador = new pCategoriaEvento();
                    break;

                default:
                    MessageBox.Show($"Controlador '{nombreControlador}' no reconocido.");
                    return;
            }

            // Ejecutar la acción correspondiente
            switch (accion)
            {
                case 0: // Save
                    controlador.GetType().GetMethod("Save")?.Invoke(controlador, new[] { objeto });
                    break;

                case 1: // Update
                    controlador.GetType().GetMethod("Update")?.Invoke(controlador, new[] { objeto });
                    break;

                case 2: // Delete
                    controlador.GetType().GetMethod("Delete")?.Invoke(controlador, new[] { objeto });
                    break;

                default:
                    MessageBox.Show("Acción no soportada.");
                    break;
            }
        }

        private (List<CampoEditable>, int) generarABM(List<CampoEditable> campos, bool nuevo)
        {
            _camposActuales = campos;
            _controlesEntrada = new List<Control>();

            // Limpiar controles existentes
            this.Controls.Clear();

            // Configurar formulario
            this.Text = nuevo ? "Nuevo Registro" : "Editar Registro";
            this.BackColor = Disenio.Colores.GrisClaro;
            this.Size = new Size(800, 600);
            this.AutoScroll = true;

            // Header panel (azul oscuro)
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Disenio.Colores.AzulOscuro,
                Padding = new Padding(30, 0, 30, 0)
            };
            this.Controls.Add(_headerPanel);

            // Título en el header
            Label lblTitulo = new Label
            {
                Text = "ABM",
                Font = new Font(Disenio.Fuentes.Titulo.FontFamily, 24, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 80
            };
            _headerPanel.Controls.Add(lblTitulo);

            // Subtítulo en el header
            Label lblSubtitulo = new Label
            {
                Text = this.Text,
                Font = Disenio.Fuentes.Titulo,
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 80,
                Padding = new Padding(20, 0, 0, 0)
            };
            _headerPanel.Controls.Add(lblSubtitulo);

            // Panel principal con padding
            _panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Disenio.Colores.GrisClaro,
                AutoScroll = true
            };
            this.Controls.Add(_panelPrincipal);

            // Contenedor para campos
            TableLayoutPanel camposTable = new TableLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 35F),
                    new ColumnStyle(SizeType.Percent, 65F)
                },
                Dock = DockStyle.Top,
                BackColor = Disenio.Colores.GrisClaro
            };
            _panelPrincipal.Controls.Add(camposTable);

            // Crear controles para cada campo
            foreach (var campo in campos)
            {
                // Etiqueta
                Label lbl = new Label
                {
                    Text = campo.Nombre + ":",
                    Font = Disenio.Fuentes.labelsLetras,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    ForeColor = Disenio.Colores.AzulOscuro,
                    Margin = new Padding(0, 10, 20, 10)
                };

                // Panel para el control de entrada (borde inferior)
                Panel controlContainer = new Panel
                {
                    Dock = DockStyle.Fill,
                    Height = 50,
                    Padding = new Padding(0, 10, 0, 5),
                    BackColor = Color.White
                };

                // Línea inferior
                controlContainer.Paint += (sender, e) =>
                {
                    e.Graphics.DrawLine(
                        new Pen(Disenio.Colores.AzulOscuro, 1),
                        0, controlContainer.Height - 1,
                        controlContainer.Width, controlContainer.Height - 1
                    );
                };

                // Control de entrada
                Control control = CrearControlParaTipo(
                    campo.Tipo,
                    campo.Valor,
                    campo.EsModificable,
                    campo.Opciones
                );
                control.Dock = DockStyle.Fill;
                control.Font = Disenio.Fuentes.General;
                control.BackColor = Color.White;
                control.ForeColor = Disenio.Colores.AzulOscuro;
                _controlesEntrada.Add(control);

                controlContainer.Controls.Add(control);

                camposTable.RowCount++;
                camposTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                camposTable.Controls.Add(lbl, 0, camposTable.RowCount - 1);
                camposTable.Controls.Add(controlContainer, 1, camposTable.RowCount - 1);
            }

            // Espaciador
            Panel spacer = new Panel
            {
                Height = 30,
                Dock = DockStyle.Top
            };
            _panelPrincipal.Controls.Add(spacer);

            // Panel para botones
            Panel panelBotones = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Disenio.Colores.GrisClaro
            };
            _panelPrincipal.Controls.Add(panelBotones);

            // Botón Cancelar
            _btnCancelar = new Button
            {
                Text = "Cancelar",
                Font = Disenio.Fuentes.Boton,
                BackColor = Disenio.Colores.RojoOscuro,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(panelBotones.Width - 260, 20),
                Size = new Size(120, 40),
                Cursor = Cursors.Hand
            };
            _btnCancelar.FlatAppearance.BorderSize = 0;
            _btnCancelar.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Botón Confirmar
            _btnConfirmar = new Button
            {
                Text = "Confirmar",
                Font = Disenio.Fuentes.Boton,
                BackColor = Disenio.Colores.VerdeOscuro,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(panelBotones.Width - 130, 20),
                Size = new Size(120, 40),
                Cursor = Cursors.Hand
            };
            _btnConfirmar.FlatAppearance.BorderSize = 0;
            _btnConfirmar.Click += BtnConfirmar_Click;

            panelBotones.Controls.Add(_btnCancelar);
            panelBotones.Controls.Add(_btnConfirmar);

            return (campos, nuevo ? 1 : 2); // 1=Nuevo, 2=Editar
        }

        private Control CrearControlParaTipo(Type tipo, object valor, bool editable, List<object> opciones = null)
        {
            // Para campos con opciones (como categorías)
            if (opciones != null && opciones.Count > 0)
            {
                ComboBox combo = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Dock = DockStyle.Fill,
                    Enabled = editable,
                    FlatStyle = FlatStyle.Flat
                };

                // Configurar display y value según el tipo
                if (tipo == typeof(CategoriaEvento))
                {
                    combo.DisplayMember = "Nombre";
                    combo.ValueMember = "IdCategoriaEvento";
                }
                else if (tipo == typeof(CategoriaDirectiva))
                {
                    combo.DisplayMember = "Nombre";
                    combo.ValueMember = "IdCategoriaDirectiva";
                }
                else
                {
                    // Configuración por defecto
                    combo.DisplayMember = "Nombre";
                    combo.ValueMember = "Id";
                }

                // Cargar opciones
                combo.DataSource = opciones;

                // Seleccionar valor actual si existe
                if (valor != null)
                {
                    // Buscar por ID
                    PropertyInfo idProperty = valor.GetType().GetProperty(combo.ValueMember);
                    if (idProperty != null)
                    {
                        object currentId = idProperty.GetValue(valor);

                        foreach (var item in combo.Items)
                        {
                            object itemId = item.GetType().GetProperty(combo.ValueMember)?.GetValue(item);
                            if (itemId != null && itemId.Equals(currentId))
                            {
                                combo.SelectedItem = item;
                                break;
                            }
                        }
                    }
                }

                return combo;
            }

            // Tipos específicos
            if (tipo == typeof(string))
                return new TextBox
                {
                    Text = valor?.ToString() ?? "",
                    Enabled = editable,
                    BorderStyle = BorderStyle.None
                };

            if (tipo == typeof(int))
                return new NumericUpDown
                {
                    Value = valor != null ? Convert.ToDecimal(valor) : 0,
                    Enabled = editable,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White
                };

            if (tipo == typeof(DateTime))
                return new DateTimePicker
                {
                    Value = valor != null ? (DateTime)valor : DateTime.Now,
                    Enabled = editable,
                    Format = DateTimePickerFormat.Short,
                    ShowUpDown = false,
                    Dock = DockStyle.Fill
                };

            // Objetos complejos (solo lectura)
            return new TextBox
            {
                Text = valor?.ToString() ?? "",
                ReadOnly = true,
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.None
            };
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _camposActuales.Count; i++)
            {
                _camposActuales[i].Valor = ObtenerValorDeControl(
                    _controlesEntrada[i],
                    _camposActuales[i].Tipo
                );
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private object ObtenerValorDeControl(Control control, Type tipo)
        {
            if (control is ComboBox combo)
            {
                return combo.SelectedItem;
            }

            if (control is TextBox txt)
            {
                if (tipo == typeof(int))
                    return int.TryParse(txt.Text, out int val) ? val : 0;
                return txt.Text;
            }

            if (control is NumericUpDown nud)
                return (int)nud.Value;

            if (control is DateTimePicker dtp)
                return dtp.Value;

            return null;
        }

       
    }

    public class CampoEditable
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public Type Tipo { get; set; }
        public bool EsModificable { get; set; }
        public List<object> Opciones { get; set; } // Para opciones de ComboBox
    }
}