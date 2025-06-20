﻿using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accessibility;


namespace WinFormsApp1.Resources
{
    public static class Disenio
    {
        private static float modificadorFuente = 1.0f; // Modificador de fuente para escalabilidad  
        public static class Colores
        {
            public static Color AzulOscuro => ColorTranslator.FromHtml("#123142");
            public static Color GrisAzulado => ColorTranslator.FromHtml("#142026");
            public static Color AmarilloClaro => ColorTranslator.FromHtml("#FFECC3");
            public static Color GrisClaro => ColorTranslator.FromHtml("#E4E4E4");
            public static Color RojoOscuro => ColorTranslator.FromHtml("#661213");
            public static Color VerdeOscuro => ColorTranslator.FromHtml("#165023");
            public static Color MarronOscuro => ColorTranslator.FromHtml("#191408");
        }

        
        public static class Fuentes
        {

            public static Font General => new Font("Segoe UI", 10 * modificadorFuente, FontStyle.Regular);
            public static Font Boton => new Font("Segoe UI Semibold", 12 * modificadorFuente, FontStyle.Regular);
            public static Font Titulo => new Font("Segoe UI Semilight", 9 * modificadorFuente, FontStyle.Regular);
        }

        public static class Imagenes
        {
            public static Image IconoEditar => ByteArrayToImage(Properties.Resources.iconoEditar);
            public static Image IconoAgregar => ByteArrayToImage(Properties.Resources.iconoAniadir);
            public static Image IconoQuitar => ByteArrayToImage(Properties.Resources.iconoQuitar);
            public static Image IconoUsuario => ByteArrayToImage(Properties.Resources.iconoUsuario);
            public static Image IconoAtras => ByteArrayToImage(Properties.Resources.iconoAtras);


        }





        private static Image ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}


