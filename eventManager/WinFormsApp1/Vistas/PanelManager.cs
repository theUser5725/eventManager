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
            labelTitulo.Size = new Size(200, 70);
            labelTitulo.Location = new Point(595, 5);
            panelGris.Controls.Add(labelTitulo);

            //LABELS DE TITULOS (3)

            //LABEL TITULO CATEGORIAS EVENTOS (subido eje Y)
            Label tituloCatEv = new Label();
            tituloCatEv.Text = "Categorías de Eventos";
            tituloCatEv.Font = Disenio.Fuentes.labelsLetras;
            tituloCatEv.Size = new Size(500, 50);
            tituloCatEv.Location = new Point(275, 65); // antes 90
            panelGris.Controls.Add(tituloCatEv);

            //LABEL TITULO CATEGORIAS DIRECTIVOS (subido eje Y)
            Label TitulocatDirect = new Label();
            TitulocatDirect.Text = "Categorías Directas";
            TitulocatDirect.Font = Disenio.Fuentes.labelsLetras;
            TitulocatDirect.Size = new Size(500, 50);
            TitulocatDirect.Location = new Point(900, 65); // antes 90
            panelGris.Controls.Add(TitulocatDirect);

            //LABEL TITULO CATEGORIAS LUGARES (centrado en X, mismo Y)
            Label TituloLugar = new Label();
            TituloLugar.Text = "Lugares";
            TituloLugar.Font = Disenio.Fuentes.labelsLetras;
            TituloLugar.Size = new Size(100, 55);
            TituloLugar.Location = new Point(665, 445); // centrado respecto a flowLugares
            TituloLugar.BackColor = Color.Transparent;
            panelGris.Controls.Add(TituloLugar);

            //_________________________________________________________

            //FLOWLAYPOTPANEL PARA CATEGORÍAS DE EVENTOS (subido eje Y)
            flowCatEv = new FlowLayoutPanel();
            flowCatEv.Size = new Size(420, 300);
            flowCatEv.Location = new Point(190, 135); // antes 160
            flowCatEv.BackColor = Color.White;
            flowCatEv.BorderStyle = BorderStyle.FixedSingle;
            flowCatEv.AutoScroll = true;
            panelGris.Controls.Add(flowCatEv);

            // BOTÓN AGREGAR ALINEADO - EVENTOS
            Panel filaAgregarEv = new Panel();
            filaAgregarEv.Size = new Size(400, 50);
            filaAgregarEv.BackColor = Color.White;

            Label labelAgregarEv = new Label();
            labelAgregarEv.Text = "Agregar";
            labelAgregarEv.Font = Disenio.Fuentes.labelsLetras;
            labelAgregarEv.Size = new Size(300, 50);
            labelAgregarEv.Location = new Point(10, 5);

            PictureBox iconoAgregarCatEv = new PictureBox();
            iconoAgregarCatEv.Image = Disenio.Imagenes.IconoAgregar;
            iconoAgregarCatEv.Size = new Size(40, 40);
            iconoAgregarCatEv.Location = new Point(320, 5);
            iconoAgregarCatEv.Cursor = Cursors.Hand;

            filaAgregarEv.Controls.Add(labelAgregarEv);
            filaAgregarEv.Controls.Add(iconoAgregarCatEv);
            flowCatEv.Controls.Add(filaAgregarEv);

            //FLOWLAYPUTPANEL CATEGORIAS DIRECTIVOS (subido eje Y)
            flowCatDirect = new FlowLayoutPanel();
            flowCatDirect.Size = new Size(400, 300);
            flowCatDirect.Location = new Point(800, 135); // antes 160
            flowCatDirect.AutoScroll = true;
            flowCatDirect.BackColor = Color.White;
            flowCatDirect.BorderStyle = BorderStyle.FixedSingle;
            panelGris.Controls.Add(flowCatDirect);

            // BOTÓN AGREGAR ALINEADO - DIRECTIVOS
            Panel filaAgregarDir = new Panel();
            filaAgregarDir.Size = new Size(400, 50);
            filaAgregarDir.BackColor = Color.White;

            Label labelAgregarDir = new Label();
            labelAgregarDir.Text = "Agregar";
            labelAgregarDir.Font = Disenio.Fuentes.labelsLetras;
            labelAgregarDir.Size = new Size(300, 50);
            labelAgregarDir.Location = new Point(10, 5);

            PictureBox iconoAgregarCatDirect = new PictureBox();
            iconoAgregarCatDirect.Image = Disenio.Imagenes.IconoAgregar;
            iconoAgregarCatDirect.Size = new Size(40, 40);
            iconoAgregarCatDirect.Location = new Point(320, 5);
            iconoAgregarCatDirect.Cursor = Cursors.Hand;

            filaAgregarDir.Controls.Add(labelAgregarDir);
            filaAgregarDir.Controls.Add(iconoAgregarCatDirect);
            flowCatDirect.Controls.Add(filaAgregarDir);

            //FLOWLAYPUTPANEL PARA LUGARES (igual)
            flowLugares = new FlowLayoutPanel();
            flowLugares.Size = new Size(650, 290);
            flowLugares.Location = new Point(400, 500);
            flowLugares.BackColor = Color.White;
            flowLugares.BorderStyle = BorderStyle.FixedSingle;
            flowLugares.AutoScroll = true;
            panelGris.Controls.Add(flowLugares);

            // BOTÓN AGREGAR ALINEADO - LUGARES
            Panel filaAgregarLugar = new Panel();
            filaAgregarLugar.Size = new Size(600, 50);
            filaAgregarLugar.BackColor = Color.White;

            Label labelAgregarLugar = new Label();
            labelAgregarLugar.Text = "Agregar";
            labelAgregarLugar.Font = Disenio.Fuentes.labelsLetras;
            labelAgregarLugar.Size = new Size(500, 50);
            labelAgregarLugar.Location = new Point(10, 5);

            PictureBox iconoAgregarLugar = new PictureBox();
            iconoAgregarLugar.Image = Disenio.Imagenes.IconoAgregar;
            iconoAgregarLugar.Size = new Size(40, 40);
            iconoAgregarLugar.Location = new Point(520, 5);
            iconoAgregarLugar.Cursor = Cursors.Hand;

            filaAgregarLugar.Controls.Add(labelAgregarLugar);
            filaAgregarLugar.Controls.Add(iconoAgregarLugar);
            flowLugares.Controls.Add(filaAgregarLugar);
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
                iconoEditar.Location = new Point(320, 5);
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
                iconoEditar.Location = new Point(520, 5);
                iconoEditar.Cursor = Cursors.Hand;

                fila.Controls.Add(labelCategoria);
                fila.Controls.Add(iconoEditar);
                flowLugares.Controls.Add(fila);
            }
        }
    }
}
