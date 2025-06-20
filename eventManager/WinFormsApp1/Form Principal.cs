using WinFormsApp1.Resources;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // Variable para guardar el último seleccionado
        private int navSeleccionado = 0;
        // Referencia al underline
        private PictureBox underline;
        // Lista de labels de navegación
        private List<Label> navLabels = new();

        public Form1()
        {
            InitializeComponent();
            inicializarHeader();
        }





        private void inicializarHeader()
        {
            // Panel principal
            Panel panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Disenio.Colores.AzulOscuro
            };
            this.Controls.Add(panelHeader);

            // Botón de retroceso
            Button btnBack = new Button
            {
                Image = Disenio.Imagenes.IconoAtras,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Disenio.Colores.GrisClaro,
                BackColor = Color.Transparent,
                Location = new Point(30, 28),
                Size = new Size(25, 25),
                TabStop = false
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.ImageAlign = ContentAlignment.MiddleCenter;
            panelHeader.Controls.Add(btnBack);

            // Botones de navegación
            string[] navItems = { "Home", "Manager", "Settings" };
            int navStartX = 80;
            for (int i = 0; i < navItems.Length; i++)
            {
                Label lblNav = new Label
                {
                    Text = navItems[i],
                    ForeColor = Disenio.Colores.GrisClaro,
                    Font = Disenio.Fuentes.Boton,
                    Location = new Point(navStartX, 25),
                    AutoSize = true,
                    Cursor = Cursors.Hand,
                    Tag = i // Guardamos el índice para referencia
                };
                lblNav.Click += NavLabel_Click;
                panelHeader.Controls.Add(lblNav);
                navLabels.Add(lblNav);

                navStartX += lblNav.PreferredWidth + 25;
            }

            // Underline único, debajo del primero ("Home")
            underline = new PictureBox
            {
                Location = new Point(navLabels[0].Left, 50),
                Size = new Size(navLabels[0].PreferredWidth, 4),
                BackColor = Color.Transparent
            };
            underline.Paint += (s, e) =>
            {
                int radius = 3;
                using (SolidBrush brush = new SolidBrush(Disenio.Colores.AmarilloClaro))
                using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(underline.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(underline.Width - radius, underline.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, underline.Height - radius, radius, radius, 90, 90);
                    path.CloseFigure();
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }
            };
            panelHeader.Controls.Add(underline);

            // Círculo de usuario
            int userCircleDiameter = 40;
            Panel userCircle = new Panel
            {
                BackColor = Disenio.Colores.AzulOscuro,
                Size = new Size(userCircleDiameter, userCircleDiameter),
                Location = new Point(panelHeader.Width - 120, 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            userCircle.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //e.Graphics.FillEllipse(new SolidBrush(Disenio.Colores.AzulOscuro), 0, 0, userCircleDiameter, userCircleDiameter);

                var icon = Disenio.Imagenes.IconoUsuario;
                int iconSize = (userCircleDiameter*2);
                int iconX = (userCircleDiameter - iconSize) / 2;
                int iconY = (userCircleDiameter - iconSize) / 2;
                e.Graphics.DrawImage(icon, iconX, iconY, iconSize, iconSize);
            };
            panelHeader.Controls.Add(userCircle);

            // Etiqueta de usuario
            Label lblUsuario = new Label
            {
                Text = "Usuario",
                ForeColor = Disenio.Colores.GrisClaro,
                Font = Disenio.Fuentes.Boton,
                Location = new Point(panelHeader.Width - 80, 27),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            panelHeader.Controls.Add(lblUsuario);

            // Ajustar posición al cambiar tamaño
            panelHeader.Resize += (s, e) =>
            {
                userCircle.Left = panelHeader.Width - 120;
                lblUsuario.Left = panelHeader.Width - 80;
            };
        }

        // Evento para mover el underline y guardar el seleccionado
        private void NavLabel_Click(object sender, EventArgs e)
        {
            if (sender is Label lbl)
            {
                underline.Location = new Point(lbl.Left, 50);
                underline.Width = lbl.PreferredWidth;
                underline.Invalidate(); // Redibuja el underline
                navSeleccionado = (int)lbl.Tag;
            }
        }
    
    }
}
