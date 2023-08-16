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
        public Stream GenerarPdF(EjercitacionAdaptada ejercitacion)
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
            doc.Add(CrearParrafo(ejercitacion.Titulo,false).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            foreach(var ejercicio in ejercitacion.Ejercicios)
            {
                doc.Add(CrearParrafo(ejercicio.Consigna, true));
                doc.Add(CrearParrafo(ejercicio.Ejercicio, false));
            }

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

        public Stream GenerarPdFOriginal(EjercitacionNoAdaptada ejercitacion)
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
            doc.Add(CrearParrafoOriginal(ejercitacion.Titulo, false).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            foreach (var ejercicio in ejercitacion.Ejercicios)
            {
                doc.Add(CrearParrafoOriginal(ejercicio.Consigna, true));
                doc.Add(CrearParrafoOriginal(ejercicio.Texto, false));
                doc.Add(CrearParrafoOriginal(ejercicio.Ejercicio, false));
            }

            doc.Close();
            ms.Position = 0;

            return ms;
        }

        public Paragraph CrearParrafoOriginal(string texto, bool isUnderline)
        {
            var respuesta = new Paragraph()
                        .SetFontSize(14)
                        .SetMultipliedLeading((float)1);
            string[] words = texto.Split(" ");

            foreach (var word in words)
            {
                Text w = new Text(word);
                if (isUnderline)
                {
                    w.SetUnderline();
                }
                respuesta.Add(w);
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
