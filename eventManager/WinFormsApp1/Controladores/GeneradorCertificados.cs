using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

public class GeneradorCertificados
{
    private readonly string _connectionString = "Data Source=eventManager.db";

    public void GenerarCertificadosParaEvento(int idEvento)
    {
        try
        {
            // Obtener datos del evento
            var evento = ObtenerDatosEvento(idEvento);
            if (evento == null)
            {
                MessageBox.Show("Evento no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Pedir ubicación para guardar
            var folderDialog = new FolderBrowserDialog
            {
                Description = "Seleccione donde guardar los certificados",
                ShowNewFolderButton = true
            };

            if (folderDialog.ShowDialog() != DialogResult.OK) return;

            var basePath = folderDialog.SelectedPath;

            // Crear carpetas
            var pathParticipantes = Path.Combine(basePath, "certificadosParticipantes");
            var pathDirectivos = Path.Combine(basePath, "certificadosDirectiva");

            Directory.CreateDirectory(pathParticipantes);
            Directory.CreateDirectory(pathDirectivos);

            // Generar certificados
            GenerarCertificadosParticipantes(evento, pathParticipantes);
            GenerarCertificadosDirectivos(evento, pathDirectivos);

            MessageBox.Show("Certificados generados exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al generar certificados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private EventoInfo ObtenerDatosEvento(int idEvento)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var query = @"
                SELECT e.nombre, ce.nombre, e.fechaInicio, e.fechaFinalizacion 
                FROM Eventos e
                JOIN CategoriasEventos ce ON e.idCatEvento = ce.idCatEvento
                WHERE e.idEvento = @idEvento";

            using (var cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@idEvento", idEvento);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new EventoInfo
                        {
                            Nombre = reader.GetString(0),
                            Categoria = reader.GetString(1),
                            FechaInicio = reader.GetDateTime(2),
                            FechaFin = reader.GetDateTime(3)
                        };
                    }
                }
            }
        }
        return null;
    }

    private List<ParticipanteInfo> ObtenerParticipantes(int idEvento)
    {
        var participantes = new List<ParticipanteInfo>();
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var query = @"
                SELECT p.nombre, p.apellido, p.dni
                FROM Participantes p
                JOIN Inscripciones i ON p.idParticipante = i.idParticipante
                WHERE i.idEvento = @idEvento";

            using (var cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@idEvento", idEvento);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        participantes.Add(new ParticipanteInfo
                        {
                            Nombre = reader.GetString(0),
                            Apellido = reader.GetString(1),
                            Dni = reader.GetString(2),
                            EsDirectivo = false
                        });
                    }
                }
            }
        }
        return participantes;
    }

    private List<ParticipanteInfo> ObtenerDirectivos(int idEvento)
    {
        var directivos = new List<ParticipanteInfo>();
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var query = @"
                SELECT p.nombre, p.apellido, p.dni, cd.nombre
                FROM Participantes p
                JOIN Directivos d ON p.idParticipante = d.idParticipante
                JOIN CategoriasDirectivos cd ON d.idCatDirectiva = cd.idCatDirectiva
                JOIN Reuniones r ON d.idReunion = r.idReunion
                WHERE r.idEvento = @idEvento";

            using (var cmd = new SQLiteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@idEvento", idEvento);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        directivos.Add(new ParticipanteInfo
                        {
                            Nombre = reader.GetString(0),
                            Apellido = reader.GetString(1),
                            Dni = reader.GetString(2),
                            CategoriaDirectivo = reader.GetString(3),
                            EsDirectivo = true
                        });
                    }
                }
            }
        }
        return directivos;
    }

    private void GenerarCertificadosParticipantes(EventoInfo evento, string outputPath)
    {
        var participantes = ObtenerParticipantes(evento.IdEvento);
        foreach (var participante in participantes)
        {
            var doc = new Document(PageSize.A4.Rotate());
            var fileName = Path.Combine(outputPath, $"Certificado_{participante.Apellido}_{participante.Nombre}.pdf");

            using (var fs = new FileStream(fileName, FileMode.Create))
            using (var writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();
                AgregarContenidoCertificado(doc, evento, participante);
                doc.Close();
            }
        }
    }

    private void GenerarCertificadosDirectivos(EventoInfo evento, string outputPath)
    {
        var directivos = ObtenerDirectivos(evento.IdEvento);
        foreach (var directivo in directivos)
        {
            var doc = new Document(PageSize.A4.Rotate());
            var fileName = Path.Combine(outputPath, $"CertificadoDirectiva_{directivo.Apellido}_{directivo.Nombre}.pdf");

            using (var fs = new FileStream(fileName, FileMode.Create))
            using (var writer = PdfWriter.GetInstance(doc, fs))
            {
                doc.Open();
                AgregarContenidoCertificado(doc, evento, directivo);
                doc.Close();
            }
        }
    }

    private void AgregarContenidoCertificado(Document doc, EventoInfo evento, ParticipanteInfo participante)
    {
        // Configuración básica
        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, BaseColor.BLACK);
        var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 18, BaseColor.BLACK);
        var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);

        // Título
        var title = new Paragraph("CERTIFICADO DE PARTICIPACIÓN", titleFont);
        title.Alignment = Element.ALIGN_CENTER;
        title.SpacingAfter = 30f;
        doc.Add(title);

        // Contenido
        var content = new Paragraph();
        content.Add(new Phrase("Se certifica que ", normalFont));
        content.Add(new Phrase($"{participante.Nombre} {participante.Apellido}", boldFont));
        content.Add(new Phrase(" con DNI ", normalFont));
        content.Add(new Phrase(participante.Dni, boldFont));

        if (participante.EsDirectivo)
        {
            content.Add(new Phrase($" en su rol de {participante.CategoriaDirectivo} ", normalFont));
        }

        content.Add(new Phrase(" ha participado en el evento ", normalFont));
        content.Add(new Phrase(evento.Nombre, boldFont));
        content.Add(new Phrase($" ({evento.Categoria}) ", normalFont));
        content.Add(new Phrase($" realizado el {evento.FechaInicio:dd/MM/yyyy}", normalFont));

        content.Alignment = Element.ALIGN_JUSTIFIED;
        content.SpacingAfter = 20f;
        doc.Add(content);

        // Fecha de emisión
        var fechaEmision = new Paragraph($"Fecha de emisión: {DateTime.Today:dd/MM/yyyy}", normalFont);
        fechaEmision.Alignment = Element.ALIGN_RIGHT;
        doc.Add(fechaEmision);
    }
}

// Clases de apoyo
public class EventoInfo
{
    public int IdEvento { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

public class ParticipanteInfo
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Dni { get; set; }
    public bool EsDirectivo { get; set; }
    public string CategoriaDirectivo { get; set; }
}