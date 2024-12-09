using System.Data;
using System;
using System.Web.UI.WebControls;
using OrdinarioV1.Models.DataSet1TableAdapters;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace OrdinarioV1.Views
{
    public partial class Carrito : System.Web.UI.Page
    {
        private DataTable carrito;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                List<string> carrito = (List<string>)Session["Carrito"] ?? new List<string>();

                GridViewCarrito.DataSource = carrito;
                GridViewCarrito.DataBind();

            }
        }

        protected void btnFin_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> carrito = (List<string>)Session["Carrito"];
                MemoryStream pdfStream = GeneratePDF(carrito);
                SendEmailWithAttachment(pdfStream);
                lblMensaje.Text = "Compra realizada con éxito. Te hemos enviado un correo con los detalles.";
                lblMensaje.CssClass = "alert alert-success";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al procesar la compra: " + ex.Message;
                lblMensaje.CssClass = "alert alert-danger";
            }
            
        }
        private MemoryStream GeneratePDF(List<string> carrito)
        {
            // Crear un documento de PDF
            Document doc = new Document(PageSize.A4);
            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(doc, ms);

            // Abrir el documento para agregar contenido
            doc.Open();

            // Título del documento
            doc.Add(new Paragraph("Resumen de tu Compra:"));
            doc.Add(new Paragraph("\n"));

            // Tabla de productos en el carrito
            PdfPTable table = new PdfPTable(2);
            table.AddCell("Producto");
            table.AddCell("Precio");

            foreach (var item in carrito)
            {
                string[] partes = item.Split(',');
                table.AddCell(partes[0]);  // Nombre del producto
                table.AddCell(partes[1]);  // Precio del producto
            }

            doc.Add(table);

            // Cerrar el documento
            doc.Close();

            // Devolver el MemoryStream con el PDF generado
            return ms;
        }

        private void SendEmailWithAttachment(MemoryStream pdfStream)
        {
            // Configurar el correo
            string smtpAddress = "smtp.office365.com";
            int smtpPort = 587;
            string emailFrom = "112469@alumnouninter.mx";
            string emailPassword = "Cut97765";
            string emailTo = "112469@alumnouninter.mx";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = "Detalles de tu compra";
                mail.Body = "Gracias por tu compra. Adjunto encontrarás el resumen de la compra en formato PDF.";

                // Crear el adjunto con el PDF generado
                mail.Attachments.Add(new Attachment(new MemoryStream(pdfStream.ToArray()), "CompraResumen.pdf", "application/pdf"));

                // Configuración del servidor SMTP
                using (SmtpClient smtp = new SmtpClient(smtpAddress, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, emailPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}