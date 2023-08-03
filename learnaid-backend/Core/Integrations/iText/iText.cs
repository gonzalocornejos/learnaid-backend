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
using System.Text.RegularExpressions;

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
            // Agregar Texto
            doc.Add(CrearParrafo(ejercicio.Titulo,false).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            doc.Add(CrearParrafo(ejercicio.Consigna,true));
            doc.Add(CrearParrafo(ejercicio.Ejercicio,false));


            doc.Close();
            ms.Position = 0;

            return ms;
        }

        public Paragraph CrearParrafo(string texto,bool isUnderline)
        {
            var respuesta = new Paragraph()
                        .SetFontSize(16)
                        .SetMultipliedLeading((float)1.5);
            string[] words = texto.Split(" ");

            foreach(var word in words)
            {
                Text w = new Text(word);
                if (isUnderline)
                {
                    w.SetUnderline();
                }
                if(word.ToUpper() == word && Regex.IsMatch(word, @"^[a-zA-Z:]+$") && word != "I")
                {
                    respuesta.Add(w.SetBold());
                } else
                {
                    respuesta.Add(w);
                }
                w = new Text(" ");
                if (isUnderline)
                {
                    w.SetUnderline();
                }
                respuesta.Add(w);
            }
            return respuesta;
        }
    }
}
