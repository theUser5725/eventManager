using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;

namespace WinFormsApp1.Vistas
{
    public partial class FormAddDirectivo : Form
    {
        //variables priv
        private Panel cabecera;
        private Panel contenido;
        private Label labelParticipantes;
        private Label labelDirectivos;
        private Label labelHead;
        private FlowLayoutPanel FlowLayoutPanelIzqParticipantes;
        private FlowLayoutPanel FlowLayoutPanelDerDirectivos;
        private Button btnCancelar;
        private Button btnConfirmar;
        //listas priv para almacenar obj participantes y directivos
        private List<Participante> participantes;
        private List<Directivo> directivos;
        private Reunion reunion; //Varaible priv para almacenar el objeto reunion

        public FormAddDirectivo(Reunion reunion)
        {
            InitializeComponent();
            this.reunion = reunion;

            directivos = new List<Directivo>();

            DisenioFormulario();

            CargarParticipantes();
            CargarDirectivos();
        }

        private void DisenioFormulario()
        {
            // Panel superior (azul)
            cabecera = new Panel();
            cabecera.Size = new Size(800, 100);
            cabecera.Location = new Point(0, 0);
            cabecera.BackColor = Disenio.Colores.AzulOscuro;
            this.Controls.Add(cabecera);

            // Panel central (gris)
            contenido = new Panel();
            contenido.Size = new Size(800, 400);
            contenido.Location = new Point(0, 100);
            contenido.BackColor = Disenio.Colores.GrisClaro;
            this.Controls.Add(contenido);

            //label de la cabecera
            labelHead = new Label();
            string fechaHora = reunion.Horario.ToString("dd/MM/yyyy HH:mm");

            // Obtener el evento por ID
            Evento evento = pEvento.GetById(reunion.IdEvento);

            // Obtener el nombre de la categoría del evento
            var controladorCatEvento = new pCategoriaEvento.pCategoriaEventoControlador(Conexion.cadena);
            string nombreCategoria = controladorCatEvento.GetById(evento.IdCatEvento)?.Nombre ?? "Sin categoría";

            labelHead.Text = $"Modificar directiva\nReunión: {fechaHora}\n({nombreCategoria}) {evento.Nombre}"; labelHead.Font = Disenio.Fuentes.labelsLetras;
            labelHead.ForeColor = Color.White;
            labelHead.Location = new Point(240, 10);
            labelHead.AutoSize = true;
            cabecera.Controls.Add(labelHead);

            // Label Participantes
            labelParticipantes = new Label();
            labelParticipantes.Text = "Participantes";
            labelParticipantes.Font = Disenio.Fuentes.labelsLetras;
            labelParticipantes.ForeColor = Color.Black;
            labelParticipantes.Location = new Point(110, 20);
            labelParticipantes.AutoSize = true;
            contenido.Controls.Add(labelParticipantes);

            // Label Directiva
            labelDirectivos = new Label();
            labelDirectivos.Text = "Directiva";
            labelDirectivos.Font = Disenio.Fuentes.labelsLetras;
            labelDirectivos.ForeColor = Color.Black;
            labelDirectivos.Location = new Point(570, 20);
            labelDirectivos.AutoSize = true;
            contenido.Controls.Add(labelDirectivos);

            // Panel Participantes
            FlowLayoutPanelIzqParticipantes = new FlowLayoutPanel();
            FlowLayoutPanelIzqParticipantes.Size = new Size(250, 200);
            FlowLayoutPanelIzqParticipantes.Location = new Point(50, 70);
            FlowLayoutPanelIzqParticipantes.BackColor = Color.White;
            FlowLayoutPanelIzqParticipantes.BorderStyle = BorderStyle.FixedSingle;
            contenido.Controls.Add(FlowLayoutPanelIzqParticipantes);

            // Panel Directivos
            FlowLayoutPanelDerDirectivos = new FlowLayoutPanel();
            FlowLayoutPanelDerDirectivos.Size = new Size(250, 200);
            FlowLayoutPanelDerDirectivos.Location = new Point(500, 70);
            FlowLayoutPanelDerDirectivos.BackColor = Color.White;
            FlowLayoutPanelDerDirectivos.BorderStyle = BorderStyle.FixedSingle;
            contenido.Controls.Add(FlowLayoutPanelDerDirectivos);

            // Botón Cancelar
            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = Disenio.Fuentes.Boton;
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.Location = new Point(120, 290);
            btnCancelar.BackColor = Disenio.Colores.RojoOscuro;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.FlatAppearance.BorderColor = Color.Black;
            btnCancelar.Click += (sender, e) => this.Close();
            contenido.Controls.Add(btnCancelar);

            // Botón Confirmar
            btnConfirmar = new Button();
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.Font = Disenio.Fuentes.Boton;
            btnConfirmar.Size = new Size(120, 40);
            btnConfirmar.Location = new Point(575, 290);
            btnConfirmar.BackColor = Disenio.Colores.VerdeOscuro;
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.FlatAppearance.BorderSize = 1;
            btnConfirmar.FlatAppearance.BorderColor = Color.Black;
            btnConfirmar.Click += (sender, e) =>
            {
                GuardarDirectivos();
                MessageBox.Show("Se agregó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            };
            contenido.Controls.Add(btnConfirmar);

        }

        private void CargarParticipantes()
        {
            participantes = pParticipante.getAllByEventoId(reunion.IdEvento);
            FlowLayoutPanelIzqParticipantes.Controls.Clear();
            FlowLayoutPanelIzqParticipantes.Font = Disenio.Fuentes.General;

            foreach (var p in participantes.ToList())
            {
                Panel fila = new Panel();
                fila.Size = new Size(250, 30);
                fila.BackColor = Color.White;

                Label lbl = new Label();
                lbl.Text = $"{p.Nombre} {p.Apellido}";
                lbl.Font = Disenio.Fuentes.General;
                lbl.ForeColor = Color.Black;
                lbl.Size = new Size(200, 30);
                lbl.TextAlign = ContentAlignment.MiddleLeft;

                PictureBox iconoAgregar = new PictureBox();
                iconoAgregar.Image = Disenio.Imagenes.IconoAgregar;
                iconoAgregar.SizeMode = PictureBoxSizeMode.Zoom;
                iconoAgregar.Size = new Size(30, 30);
                iconoAgregar.Location = new Point(200, 0);
                iconoAgregar.Cursor = Cursors.Hand;

                iconoAgregar.Click += (sender, e) =>
                {
                    Directivo nuevoDirectivo = new Directivo(reunion.IdReunion, p.IdParticipante, 0, p);
                    directivos.Add(nuevoDirectivo);
                    participantes.Remove(p);
                    RefrescaPaneles();
                };

                fila.Controls.Add(lbl);
                fila.Controls.Add(iconoAgregar);
                FlowLayoutPanelIzqParticipantes.Controls.Add(fila);
            }
        }

        private void CargarDirectivos()
        {
            FlowLayoutPanelDerDirectivos.Controls.Clear();
            FlowLayoutPanelDerDirectivos.Font = Disenio.Fuentes.General;

            //para no modificar el controlador de categorias de directivos
            var categoriaControlador = new pCategoriaDirectivaControlador(Conexion.cadena);
            //para tener la lista de categorias de directivos con el GetAll()
            List<CategoriaDirectivas> categorias = categoriaControlador.GetAll();

            foreach (var d in directivos)
            {
                Participante participante = d.Participante;
                if (participante == null) continue;

                Panel fila = new Panel();
                fila.Size = new Size(250, 35);
                fila.BackColor = Color.White;

                Label lbl = new Label();
                lbl.Text = $"{participante.Nombre} {participante.Apellido}";
                lbl.Font = Disenio.Fuentes.General;
                lbl.ForeColor = Color.Black;
                lbl.Size = new Size(100, 30);
                lbl.TextAlign = ContentAlignment.MiddleLeft;
                lbl.Location = new Point(0, 0);

                ComboBox comboCargo = new ComboBox();
                comboCargo.Size = new Size(100, 30);
                comboCargo.Location = new Point(100, 2);
                comboCargo.Font = Disenio.Fuentes.General;
                comboCargo.DropDownStyle = ComboBoxStyle.DropDownList;

                //cargamos las categ de direct.
                comboCargo.DataSource = new BindingSource(categorias, null);
                comboCargo.DisplayMember = "Nombre";
                comboCargo.ValueMember = "IdCatDirectiva";
                comboCargo.SelectedIndex = -1; //arranca con nada seleccionado

                PictureBox IconoQuitar = new PictureBox();
                IconoQuitar.Image = Disenio.Imagenes.IconoQuitar;
                IconoQuitar.SizeMode = PictureBoxSizeMode.Zoom;
                IconoQuitar.Size = new Size(30, 30);
                IconoQuitar.Location = new Point(210, 0);
                IconoQuitar.Cursor = Cursors.Hand;

                IconoQuitar.Click += (sender, e) =>
                {
                    directivos.Remove(d);
                    participantes.Add(participante);
                    RefrescaPaneles();
                };

                fila.Controls.Add(lbl);
                fila.Controls.Add(comboCargo);
                fila.Controls.Add(IconoQuitar);
                FlowLayoutPanelDerDirectivos.Controls.Add(fila);
            }
        }

        private void RefrescaPaneles()
        {
            CargarParticipantes();
            CargarDirectivos();
        }

        private void GuardarDirectivos()
        {
            var directivosExistentes = nDirectivo.getAllByReunionId(reunion.IdReunion);
            foreach (var d in directivosExistentes)
            {
                nDirectivo.Delete(d);
            }

            foreach (var d in directivos)
            {
                nDirectivo.Save(d);
            }
        }

    }
}
