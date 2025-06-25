using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;

namespace WinFormsApp1.Vistas
{
    public partial class FormABM : Form
    {
        private List<Control> _controlesEntrada;
        private List<CampoEditable> _camposActuales;
        private Button _btnConfirmar;
        private Button _btnCancelar;
        private Panel _panelPrincipal;

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
                    e.Categoria = (CategoriaEvento)camposEditadosEvento[6].Valor;

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
                    d.Categoria = (CategoriaDirectiva)camposEditadosDirectivo[1].Valor;

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

                    EjecutarPersistencia("pCategoriaDirectiva", accionCatDir, cd);
                    break;

                default:
                    MessageBox.Show("Tipo de objeto no soportado.");
                    break;
            }
        }

        // Stub para compilar. Debe implementarse correctamente
        private void EjecutarPersistencia(string nombreControlador, int accion, object objeto)
        {

            // Aquí deberías usar tus clases reales de persistencia
            Console.WriteLine($"[{nombreControlador}] Acción: {accion}, Objeto: {objeto}");
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

            // Panel principal con padding
            _panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Disenio.Colores.GrisClaro
            };
            this.Controls.Add(_panelPrincipal);

            // Título del formulario
            Label lblTitulo = new Label
            {
                Text = this.Text,
                Font = Disenio.Fuentes.Titulo,
                ForeColor = Disenio.Colores.AzulOscuro,
                Dock = DockStyle.Top,
                Height = 60,
                Margin = new Padding(0, 0, 0, 20)
            };
            _panelPrincipal.Controls.Add(lblTitulo);

            // Contenedor para campos (FlowLayoutPanel para disposición automática)
            FlowLayoutPanel camposPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 0, 0, 30),
                BackColor = Disenio.Colores.GrisClaro
            };
            _panelPrincipal.Controls.Add(camposPanel);

            // Crear controles para cada campo
            foreach (var campo in campos)
            {
                // Panel contenedor para cada campo
                Panel campoContainer = new Panel
                {
                    Width = _panelPrincipal.Width - 60,
                    Height = 60,
                    Margin = new Padding(0, 0, 0, 15),
                    BackColor = Color.White
                };

                // Etiqueta
                Label lbl = new Label
                {
                    Text = campo.Nombre + ":",
                    Font = Disenio.Fuentes.labelsLetras,
                    Location = new Point(20, 20),
                    AutoSize = true,
                    ForeColor = Disenio.Colores.AzulOscuro
                };

                // Control de entrada
                Control control = CrearControlParaTipo(
                    campo.Tipo,
                    campo.Valor,
                    campo.EsModificable
                );
                control.Location = new Point(220, 15);
                control.Width = campoContainer.Width - 240;
                control.Font = Disenio.Fuentes.General;
                control.BackColor = Color.White;
                _controlesEntrada.Add(control);

                campoContainer.Controls.Add(lbl);
                campoContainer.Controls.Add(control);
                camposPanel.Controls.Add(campoContainer);
            }

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

        private Control CrearControlParaTipo(Type tipo, object valor, bool editable)
        {
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
                    ShowUpDown = true
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

        // Métodos existentes se mantienen...
    }

    public class CampoEditable
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public Type Tipo { get; set; }
        public bool EsModificable { get; set; }
    }
}


