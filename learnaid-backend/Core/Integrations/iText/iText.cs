using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel;
using iText.Layout;
using iText.Layout.Element;
using learnaid_backend.Core.Integrations.iText.Interfaces;
using learnaid_backend.Core.Models;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Properties;

namespace learnaid_backend.Core.Integrations.iText
{
    public class IText : IiText
    {
        public Stream GenerarPdF(EjercicioAdaptado ejercicio)
        {
            var ms = new MemoryStream();
            Console.WriteLine("Prueba");
            var pw = new PdfWriter(ms);
            pw.SetCloseStream(false);
            pw.SetSmartMode(true);
            var pdfDocument = new PdfDocument(pw);
            var doc = new Document(pdfDocument, PageSize.A4);

            PdfFont font = PdfFontFactory.CreateFont();
            // Agregar Titulo
            doc.Add(new Paragraph(ejercicio.Titulo)
                        .SetFontSize(20)
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER));
            doc.Add(new Paragraph(ejercicio.Consigna)
                        .SetFontSize(16));
            doc.Add(new Paragraph(ejercicio.Ejercicio)
                        .SetFontSize(16)
                        .SetMultipliedLeading((float)1.5));

            /*Paragraph titulo = new Paragraph();
            string[] words = ejercicio.Titulo.Split(' ');

            foreach(string word in words)
            {
                if (word.ToUpper() == word)
                {
                    titulo.Add(word);
                } else
                {
                    titulo.Add(word);
                }
                titulo.Add(" ");
            }
            doc.Add(titulo);*/

            doc.Close();

            //byte[] byteStream = ms.ToArray();
            //ms.Flush();
            //ms.Write(byteStream,0,byteStream.Length);
            ms.Position = 0;

            return ms;
        }
    }
}
