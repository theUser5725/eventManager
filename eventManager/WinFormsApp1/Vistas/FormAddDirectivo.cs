using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tp4_Prueba.Modelos;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;

namespace WinFormsApp1.Vistas
{
    public partial class FormAddDirectivo : Form
    {
        private Panel cabecera;
        private Panel contenido;
        private Label labelParticipantes;
        private Label labelDirectivos;
        private Label labelHead;
        private ListBox ListBoxIzqParticipantes;
        private ListBox ListBoxDerDirectivos;
        private Button btnCancelar;
        private Button btnConfirmar;
        public FormAddDirectivo()
        {
            InitializeComponent();

            //cabecera
            cabecera = new Panel();
            cabecera.Size = new Size(800, 100);
            cabecera.Location = new Point(0,0);
            cabecera.BackColor = Disenio.Colores.AzulOscuro;
            this.Controls.Add(cabecera);

            //panel de contenido
            contenido = new Panel();
            contenido.Size = new Size(800,400);
            contenido.Location = new Point(0,100);
            contenido.BackColor = Disenio.Colores.GrisClaro;
            this.Controls.Add(contenido);

            //labels
            //lb1
            labelParticipantes = new Label();
            labelParticipantes.Text = "Participantes";
            labelParticipantes.Font = Disenio.Fuentes.labelsLetras;
            labelParticipantes.ForeColor = Color.Black;
            labelParticipantes.Location = new Point(110, 20);
            labelParticipantes.AutoSize = true; 
            contenido.Controls.Add(labelParticipantes);

            //lb2
            labelDirectivos = new Label();
            labelDirectivos.Text = "Directiva";
            labelDirectivos.Font = Disenio.Fuentes.labelsLetras;
            labelDirectivos.ForeColor = Color.Black;
            labelDirectivos.Location = new Point(570, 20);
            labelDirectivos.AutoSize = true;
            contenido.Controls.Add(labelDirectivos);

            //labels de la cabecera
            //lb01
            labelHead = new Label();
            labelHead.Text = "Modificar Directiva";
            labelHead.Font = Disenio.Fuentes.labelsLetras;
            labelHead.ForeColor = Color.White;
            labelHead.Location = new Point(300, 10);
            labelHead.AutoSize = true;
            cabecera.Controls.Add(labelHead);

            //ListBoxs de los directivos y participantes

            //ListBox izq (participantes)
            ListBoxIzqParticipantes = new ListBox();
            ListBoxIzqParticipantes.Size = new Size(250, 200);
            ListBoxIzqParticipantes.Location = new Point(50,70);
            ListBoxIzqParticipantes.BackColor = Color.White;
            ListBoxIzqParticipantes.BorderStyle = BorderStyle.FixedSingle;
            contenido.Controls.Add(ListBoxIzqParticipantes);
            //para mostrar los participantes de la bd con getAll()
            List<Participante> participantes = pParticipante.getAll();
            foreach (var p in participantes)
            {
                ListBoxIzqParticipantes.Items.Add($"{p.Nombre} {p.Apellido}");
            }

            //ListBox der (directivos)
            ListBoxDerDirectivos = new ListBox();
            ListBoxDerDirectivos.Size = new Size(250, 200);
            ListBoxDerDirectivos.Location = new Point(500, 70);
            ListBoxDerDirectivos.BackColor = Color.White;
            ListBoxDerDirectivos.BorderStyle = BorderStyle.FixedSingle;
            contenido.Controls.Add(ListBoxDerDirectivos);
            
            

            //botones
            //btn de cancelar
            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Font = Disenio.Fuentes.Boton;
            btnCancelar.Size = new Size(100, 40);
            btnCancelar.Location = new Point(125, 290);
            btnCancelar.BackColor = Disenio.Colores.RojoOscuro;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.FlatAppearance.BorderColor = Color.Black;
            btnCancelar.Click += (sender, e) => this.Close(); 
            contenido.Controls.Add(btnCancelar);

            //btn de confirmar
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
            btnConfirmar.Click += (sender, e) => MessageBox.Show("Directivo agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            contenido.Controls.Add(btnConfirmar);

        }

    }
}