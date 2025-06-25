using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;

namespace WinFormsApp1.Vistas
{
    public partial class FormAddAsistencia : Form
    {
        private TabPage tabParticipantes;
        private List<Asistencia> asistenciasGeneral;
        public FormAddAsistencia(Reunion reunion)
        {
            InitializeComponent();

            asistenciasGeneral = pAsistencia.GetAllByReunionId(reunion.IdReunion);


            this.Controls.Add(inicializarTabParticipantes(reunion.IdReunion));

        }
        private TableLayoutPanel inicializarTabParticipantes(int idReunion)
        {
            var participantes = pParticipante.GetAllByReunionId(idReunion);

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 3,
                Padding = new Padding(16),
                BackColor = Color.White,
                AutoScroll = true
            };

            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40)); // Nombre + Apellido
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45)); // Mail
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15)); // Botón

            // Función auxiliar para agregar fila + línea
            void AgregarFila(int fila, string nombreCompleto, string mail, Button boton = null)
            {
                tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var lbl = new Label
                {
                    Text = nombreCompleto,
                    Font = Disenio.Fuentes.General,
                    AutoSize = true,
                    Padding = new Padding(4)
                };
                tabla.Controls.Add(lbl, 0, fila);
                tabla.Controls.Add(new Label { Text = mail, Font = Disenio.Fuentes.General, AutoSize = true, Padding = new Padding(4) }, 1, fila);

                if (boton != null)
                {
                    var panelBtn = new Panel
                    {
                        Anchor = AnchorStyles.None,
                        AutoSize = false,
                        Size = new Size(34, 34), // ligeramente más grande que el botón
                        Margin = new Padding(0),
                        Padding = new Padding(0)
                    };

                    panelBtn.Controls.Add(boton);
                    boton.Location = new Point(3, 3);
                    tabla.Controls.Add(panelBtn, 2, fila);
                }
                else
                {
                    tabla.Controls.Add(new Label { AutoSize = true }, 2, fila);
                }

                // Línea divisoria debajo de cada fila
                tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 2));
                var panelLinea = new Panel
                {
                    Height = 2,
                    Dock = DockStyle.Bottom,
                    BackColor = Color.Gray
                };
                tabla.Controls.Add(panelLinea, 0, fila + 1);
                tabla.SetColumnSpan(panelLinea, 3);
            }

            // Encabezado
            AgregarFila(0, "Nombre", "Mail");

            int fila = 2; // fila 1 es la línea divisoria
            foreach (var part in participantes)
            {

                var btnDarDeBaja = new Button
                {
                    Image = new Bitmap(Disenio.Imagenes.IconoAgregar, new Size(Disenio.tamanoIcono, Disenio.tamanoIcono)), // <-- redimensionar ícono aquí
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Width = 28,
                    Height = 28,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    Tag = part,
                    Cursor = Cursors.Hand,
                    Text = "",

                };

                btnDarDeBaja.Left = tabla.Left - Disenio.tamanoIcono - 10;

                btnDarDeBaja.FlatAppearance.BorderSize = 0;

                btnDarDeBaja.Click += (s, e) =>
                {
                    var btn = s as Button;
                    var participanteSeleccionado = btn?.Tag as Participante;
                    if (participanteSeleccionado != null)
                    {
                        var confirmar = MessageBox.Show(
                            $"¿Desea dar de baja a {participanteSeleccionado.Nombre} {participanteSeleccionado.Apellido}?",
                            "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmar == DialogResult.Yes)
                        {
                            pParticipante.Delete(participanteSeleccionado);
                            inicializarTabParticipantes(idReunion); // refrescar
                        }
                    }
                };

                if (asistenciasGeneral.Any(a => a.idParticipante == part.IdParticipante))
                {
                    AgregarFila(fila, part.toString(), part.Mail);
                }
                else
                {
                    AgregarFila(fila, part.toString(), part.Mail, btnDarDeBaja);
                }


                    fila += 2;
            }

            return tabla;
        }

    }
}
