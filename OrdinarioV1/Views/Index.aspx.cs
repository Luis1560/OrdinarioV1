using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrdinarioV1.Models.DataSet1TableAdapters;
using System.Security.Cryptography;
using OrdinarioV1.Controller;

namespace OrdinarioV1.Views
{
    public partial class Index : System.Web.UI.Page
    {
        private int contadorCarrito;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["NombreUsuario"] != null)
                {
                    lblMensaje.Text = "Bienvenido " + Session["Usuario"].ToString();
                }
                else
                {
                    lblMensaje.Text = "No hay sesión iniciada";
                }
                CargarProductos();
                ActualizarContadorCarrito();
            }
        }
        private void ActualizarContadorCarrito()
        {

            List<string> carrito = (List<string>)Session["Carrito"] ?? new List<string>();
            contadorCarrito = carrito.Count;

            mensajeCarrito.Text = $"Tu carrito tiene {contadorCarrito} artículo(s).";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text;
                string marca = txtMarca.Text;
                string modelo = txtModelo.Text;
                decimal precio = Convert.ToDecimal(txtPrecio.Text);
                string garantia = txtGarantia.Text;
                string especificaciones = txtEspecificaciones.Text;
                int cantidadDisponible = Convert.ToInt32(txtCantidad.Text);


                electronicosTableAdapter electronicosAdapter = new electronicosTableAdapter();
                if (string.IsNullOrEmpty(hfID_Electronico.Value))
                {
                    electronicosAdapter.InsertElectronico(nombre, marca, modelo, precio, garantia, especificaciones, cantidadDisponible);
                }
                else
                {
                    int idElectronico = Convert.ToInt32(hfID_Electronico.Value);
                    electronicosAdapter.UpdateElectronico(nombre, marca, modelo, precio, garantia, especificaciones, cantidadDisponible, idElectronico);
                    hfID_Electronico.Value = string.Empty;
                }
                LimpiarForm();
                CargarProductos();
            }
            catch (Exception ex)
            {

                MostrarMensaje($"Error al guardar el producto: {ex.Message}", true);
            }
        }

        private void CargarProductos()
        {
            electronicosTableAdapter electronicosAdapter = new electronicosTableAdapter();
            DataTable dtElectronicos = electronicosAdapter.GetData();
            gvElectronicos.DataSource = dtElectronicos;
            gvElectronicos.DataBind();
        }

        private void LimpiarForm()
        {
            txtNombre.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtModelo.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtGarantia.Text = string.Empty;
            txtEspecificaciones.Text = string.Empty;
            txtCantidad.Text = string.Empty;
        }
        protected void gvElectronicos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GridViewRow row = gvElectronicos.Rows[e.NewEditIndex];
                int idElectronico = Convert.ToInt32(gvElectronicos.DataKeys[e.NewEditIndex].Value);

                electronicosTableAdapter electronicosAdapter = new electronicosTableAdapter();
                DataTable dt = electronicosAdapter.GetDataByID(idElectronico);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    hfID_Electronico.Value = dr["ID_Electronico"].ToString();
                    txtNombre.Text = dr["Nombre"].ToString();
                    txtMarca.Text = dr["Marca"].ToString();
                    txtModelo.Text = dr["Modelo"].ToString();
                    txtPrecio.Text = dr["Precio"].ToString();
                    txtGarantia.Text = dr["Garantia"].ToString();
                    txtEspecificaciones.Text = dr["Especificaciones"].ToString();
                    txtCantidad.Text = dr["Cantidad_Disponible"].ToString();
                }

                e.Cancel = true; 
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al intentar editar el producto: {ex.Message}", true);
            }
        }

        protected void gvElectronicos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int idElectronico = Convert.ToInt32(gvElectronicos.DataKeys[e.RowIndex].Value);
                electronicosTableAdapter electronicosAdapter = new electronicosTableAdapter();
                electronicosAdapter.Delete(idElectronico);

                MostrarMensaje("Producto eliminado correctamente.", false);
                CargarProductos();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al eliminar el producto: {ex.Message}", true);
            }
        }

        protected void gvElectronicos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvElectronicos.EditIndex = -1; 
                CargarProductos();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cancelar la edición: {ex.Message}", true);
            }
        }
        protected void gvElectronicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AgregarCarrito")
            {

                int idElectronico = Convert.ToInt32(e.CommandArgument);
                var tableAdapter = new electronicosTableAdapter();
                var producto = tableAdapter.GetData().FirstOrDefault(p => p.id_Electronico == idElectronico);

                if (producto != null)
                {

                    List<string> carrito = (List<string>)Session["Carrito"] ?? new List<string>();
                    carrito.Add($"Producto: {producto.Nombre}, Marca: {producto.Marca}, Precio: {producto.Precio:C}");
                    Session["Carrito"] = carrito;

                    lblMensaje.Text = $"El producto {producto.Nombre} fue agregado al carrito.";
                    lblMensaje.CssClass = "alert alert-success";
                }
                else
                {
                    lblMensaje.Text = "El producto no pudo ser encontrado.";
                    lblMensaje.CssClass = "alert alert-danger";
                }
            }
        }
        private void MostrarMensaje(string mensaje, bool esError)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = esError ? "alert alert-danger" : "alert alert-success";
            lblMensaje.Visible = true;
            lblMensaje.Style["display"] = "block";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text;
                string contra = txtContrasena.Text;
                var tableAdapter = new usuariosTableAdapter();
                var usuarioEncontrado = tableAdapter.GetData().FirstOrDefault(u => u.Nombre == usuario);
                AESCryptography aESCryptography = new AESCryptography();

                if (usuarioEncontrado != null)
                {


                    string contrasenaDesencriptada = aESCryptography.Decrypt(usuarioEncontrado.Contraseña.ToString());

                    if (contra == contrasenaDesencriptada)
                    {

                        Session["Usuario"] = usuarioEncontrado.Nombre;
                        lblMensaje.Text = "Inicio de sesión exitoso.";
                        lblMensaje.CssClass = "alert alert-success";
                    }
                    else
                    {
                        lblMensaje.Text = "Usuario o contraseña incorrectos.";
                        lblMensaje.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMensaje.Text = "Usuario no encontrado.";
                    lblMensaje.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                throw;
            }

            gvElectronicos.Visible = true;
            pnlSesion.Visible = true;
            pnlLogin.Visible = false;

        }

        protected void btnVerCarrito_Click(object sender, EventArgs e)
        {
            Response.Redirect("Carrito.aspx");
        }
    }
}