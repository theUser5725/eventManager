using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Vistas
{
    public partial class FormABM : Form
    {
        public FormABM(object mdl)
        {
            InitializeComponent();
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

                case Asistencia a:
                    if (nuevo) a = new Asistencia();

                    campos.Add(new CampoEditable { Nombre = "HorasAsistido", Valor = a.horasAsistido, Tipo = typeof(int), EsModificable = true });

                    var (camposEditadosAsistencia, accionAsistencia) = generarABM(campos, nuevo);
                    ShowDialog();
                    if (DialogResult == DialogResult.Cancel) return;

                    a.horasAsistido = Convert.ToInt32(camposEditadosAsistencia[0].Valor);

                    EjecutarPersistencia("pAsistencia", accionAsistencia, a);
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
        private (List<CampoEditable>, int) generarABM(List<CampoEditable> campos, bool nuevo)
        {
            // Simular retorno
            return (campos, 1);
        }

        private void EjecutarPersistencia(string nombreControlador, int accion, object objeto)
        {
            // Aquí deberías usar tus clases reales de persistencia
            Console.WriteLine($"[{nombreControlador}] Acción: {accion}, Objeto: {objeto}");
        }
    }

    public class CampoEditable
    {
        public string Nombre { get; set; }
        public object Valor { get; set; }
        public Type Tipo { get; set; }
        public bool EsModificable { get; set; }
    }
}
