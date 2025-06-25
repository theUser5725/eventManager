using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;
using WinFormsApp1.Resources;

namespace WinFormsApp1.Vistas
{
    public partial class PanelManager : UserControl
    {
        //DECLARAMOS LAS VARIABLES QUE VAMOS A USAR PRIVADAS 
        private Label labelTitulo;
        private Label tituloCatEv;
        private Label TitulocatDirect;
        private Label TituloLugar;
        private FlowLayoutPanel flowCatDirect;
        private FlowLayoutPanel flowLugares;
        private FlowLayoutPanel flowLugaresEv;
        private FlowLayoutPanel flowCatEv;
        private PictureBox iconoAgregarCatEv;
        private PictureBox iconoAgregarCatDirect;
        private PictureBox iconoAgregarLugar;
        private PictureBox IconoEditar;
        private PictureBox iconoEditarCatEv;
        private pCategoriaEvento pCategoriaEvento = new pCategoriaEvento();
        private pCategoriaDirectivo pCategoriaDirectivo = new pCategoriaDirectivo();
        private pLugar pLugar = new pLugar();

        //DECLARAMOS LAS LISTAS QUE VAMOS A USAR
        private Evento evento;
        private List<CategoriaEvento> listaDeCategEvent;
        private List<CategoriaDirectiva> listaDeCatDirect;
        private List<Lugar> listaDeCatLugar;

        //CONSTRUCTOR DEL PANEL MANAGER
        public PanelManager()
        {
            diseñoGeneralManager();
            cargarCategoriasEventos();
            cargarCategoriasDirectivos();
            cargarLugares();
            this.evento = evento;
        }

        // MÉTODO QUE SE ENCARGA DE DISEÑAR EL PANEL MANAGER
        private void diseñoGeneralManager()
        {
            //PANEL GENERAL - GRIS CLARO (1)
            Panel panelGris = new Panel();
            panelGris.Size = new Size(1400, 800);
            panelGris.Location = new Point(250, 50);
            panelGris.BackColor = Disenio.Colores.GrisClaro;
            this.Controls.Add(panelGris);

            //LABEL DEL TITULO (2)
            Label labelTitulo = new Label();
            labelTitulo.Text = "Manager";
            labelTitulo.Font = Disenio.Fuentes.Titulo;
            labelTitulo.Size = new Size(335, 75);
            labelTitulo.Location = new Point(610, 5);
            panelGris.Controls.Add(labelTitulo);

            //LABELS DE TITULOS (3)

            //LABEL TITULO CATEGORIAS EVENTOS
            Label tituloCatEv = new Label();
            tituloCatEv.Text = "Categorías de Eventos";
            tituloCatEv.Font = Disenio.Fuentes.labelsLetras;
            tituloCatEv.Size = new Size(500, 50);
            tituloCatEv.Location = new Point(275, 70);
            panelGris.Controls.Add(tituloCatEv);

            //LAVEL TITULO CATEGORIAS DIRECTIVOS
            Label TitulocatDirect = new Label();
            TitulocatDirect.Text = "Categorías Directas";
            TitulocatDirect.Font = Disenio.Fuentes.labelsLetras;
            TitulocatDirect.Size = new Size(500, 50);
            TitulocatDirect.Location = new Point(900, 75);
            panelGris.Controls.Add(TitulocatDirect);

            //LABEL TITULO CATEGORIAS LUGARES
            Label TituloLugar = new Label();
            TituloLugar.Text = "Lugares";
            TituloLugar.Font = Disenio.Fuentes.labelsLetras;
            TituloLugar.Size = new Size(100, 55);
            TituloLugar.Location = new Point(655, 445);
            TituloLugar.BackColor = Color.Transparent;
            panelGris.Controls.Add(TituloLugar);

            //_________________________________________________________

            //FLOWLAYOUTPANEL PANELES (DE LOS LABELS ANTERIORES) (4)

            //FLOWLAYPOTPANEL PARA CATEGORÍAS DE EVENTOS (5)
            flowCatEv = new FlowLayoutPanel();
            flowCatEv.Size = new Size(420, 300);
            flowCatEv.Location = new Point(190, 130);  // SUBIDO eje Y
            flowCatEv.BackColor = Color.White;
            flowCatEv.BorderStyle = BorderStyle.FixedSingle;
            flowCatEv.AutoScroll = true;
            Label labelCatEv = new Label();
            labelCatEv.Text = "Agregar";
            labelCatEv.Font = Disenio.Fuentes.labelsLetras;
            labelCatEv.Size = new Size(100, 5);
            labelCatEv.AutoSize = true;
            flowCatEv.Controls.Add(labelCatEv);
            panelGris.Controls.Add(flowCatEv);

            //ICONO SOLO PARA AGREGAR (6)
            PictureBox iconoAgregarCatEv = new PictureBox();
            iconoAgregarCatEv.Image = Disenio.Imagenes.IconoAgregar;
            iconoAgregarCatEv.Size = new Size(50, 50);
            iconoAgregarCatEv.Margin = new Padding(255, 5, 0, 0);
            iconoAgregarCatEv.Cursor = Cursors.Hand;
            flowCatEv.Controls.Add(iconoAgregarCatEv);

            //FLOWLAYPUTPANEL CATEGORIAS DIRECTIVOS (7)
            flowCatDirect = new FlowLayoutPanel();
            flowCatDirect.Size = new Size(400, 300);
            flowCatDirect.Location = new Point(800, 130); // SUBIDO eje Y
            flowCatDirect.AutoScroll = true;
            flowCatDirect.BackColor = Color.White;
            flowCatDirect.BorderStyle = BorderStyle.FixedSingle;
            Label labelCatDir = new Label();
            labelCatDir.Text = "Agregar";
            labelCatDir.Font = Disenio.Fuentes.labelsLetras;
            labelCatDir.Size = new Size(100, 40);
            labelCatDir.AutoSize = true;
            flowCatDirect.Controls.Add(labelCatDir);
            panelGris.Controls.Add(flowCatDirect);

            //ICONO SOLO PARA AGREGAR (8)
            PictureBox iconoAgregarCatDirect = new PictureBox();
            iconoAgregarCatDirect.Image = Disenio.Imagenes.IconoAgregar;
            iconoAgregarCatDirect.Size = new Size(50, 50);
            iconoAgregarCatDirect.Margin = new Padding(3, 5, 0, 0);
            iconoAgregarCatDirect.Cursor = Cursors.Hand;
            flowCatDirect.Controls.Add(iconoAgregarCatDirect);

            //FLOWLAYPUTPANEL PARA LUGARES (9)
            flowLugares = new FlowLayoutPanel();
            flowLugares.Size = new Size(650, 290);
            flowLugares.Location = new Point(400, 500);
            flowLugares.BackColor = Color.White;
            flowLugares.BorderStyle = BorderStyle.FixedSingle;
            flowLugares.AutoScroll = true;
            Label labelLugar = new Label();
            labelLugar.Text = "Agregar";
            labelLugar.Font = Disenio.Fuentes.labelsLetras;
            labelLugar.Size = new Size(100, 20);
            labelLugar.AutoSize = true;
            flowLugares.Controls.Add(labelLugar);
            panelGris.Controls.Add(flowLugares);

            //ICONO SOLO PARA AGREGAR (10)
            PictureBox iconoAgregarLugar = new PictureBox();
            iconoAgregarLugar.Image = Disenio.Imagenes.IconoAgregar;
            iconoAgregarLugar.Size = new Size(50, 50);
            iconoAgregarLugar.Margin = new Padding(545, 0, 0, 0);
            iconoAgregarLugar.Cursor = Cursors.Hand;
            flowLugares.Controls.Add(iconoAgregarLugar);
        }

        // MÉTODOS PARA CARGAR LAS CATEGORÍAS DE EVENTOS DESDE LA BASE DE DATOS (11)
        private void cargarCategoriasEventos()
        {
            listaDeCategEvent = pCategoriaEvento.GetAll();
            flowCatEv.Font = Disenio.Fuentes.labelsLetras;

            foreach (var Ce in listaDeCategEvent.ToList())
            {
                Panel fila = new Panel();
                fila.Size = new Size(400, 50);
                fila.BackColor = Color.White;

                Label labelCategoria = new Label();
                labelCategoria.Text = $"{Ce.Nombre}";
                labelCategoria.Font = Disenio.Fuentes.labelsLetras;
                labelCategoria.Size = new Size(300, 50);
                labelCategoria.Location = new Point(10, 5);

                PictureBox iconoEditar = new PictureBox();
                iconoEditar.Image = Disenio.Imagenes.IconoEditar;
                iconoEditar.Size = new Size(40, 40);
                iconoEditar.Location = new Point(320, 5);
                iconoEditar.Cursor = Cursors.Hand;

                fila.Controls.Add(labelCategoria);
                fila.Controls.Add(iconoEditar);
                flowCatEv.Controls.Add(fila);
            }
        }

        // MÉTODOS PARA CARGAR LAS CATEGORÍAS DE DIRECTIVOS DESDE LA BASE DE DATOS (12)
        private void cargarCategoriasDirectivos()
        {
            listaDeCatDirect = pCategoriaDirectivo.GetAll();
            flowCatDirect.Font = Disenio.Fuentes.labelsLetras;

            foreach (var Cd in listaDeCatDirect.ToList())
            {
                Panel fila = new Panel();
                fila.Size = new Size(400, 50);
                fila.BackColor = Color.White;

                Label labelCategoria = new Label();
                labelCategoria.Text = $"{Cd.Nombre}";
                labelCategoria.Font = Disenio.Fuentes.labelsLetras;
                labelCategoria.Size = new Size(270, 50);
                labelCategoria.Location = new Point(10, 5);

                PictureBox iconoEditar = new PictureBox();
                iconoEditar.Image = Disenio.Imagenes.IconoEditar;
                iconoEditar.Size = new Size(40, 40);
                iconoEditar.Location = new Point(330, 5);
                iconoEditar.Cursor = Cursors.Hand;

                fila.Controls.Add(labelCategoria);
                fila.Controls.Add(iconoEditar);
                flowCatDirect.Controls.Add(fila);
            }
        }

        // MÉTODOS PARA CARGAR LOS LUGARES DESDE LA BASE DE DATOS (13)
        private void cargarLugares()
        {
            listaDeCatLugar = pLugar.GetAll();
            flowLugares.Font = Disenio.Fuentes.labelsLetras;

            foreach (var Cd in listaDeCatLugar.ToList())
            {
                Panel fila = new Panel();
                fila.Size = new Size(600, 50);
                fila.BackColor = Color.White;

                Label labelCategoria = new Label();
                labelCategoria.Text = $"{Cd.nombre} - Capacidad: {Cd.capacidad}";
                labelCategoria.Font = Disenio.Fuentes.labelsLetras;
                labelCategoria.Size = new Size(440, 50);
                labelCategoria.Location = new Point(10, 5);

                PictureBox iconoEditar = new PictureBox();
                iconoEditar.Image = Disenio.Imagenes.IconoEditar;
                iconoEditar.Size = new Size(40, 40);
                iconoEditar.Location = new Point(610 - 40 - 10, 5);
                iconoEditar.Cursor = Cursors.Hand;

                fila.Controls.Add(labelCategoria);
                fila.Controls.Add(iconoEditar);
                flowLugares.Controls.Add(fila);
            }
        }
    }
}
