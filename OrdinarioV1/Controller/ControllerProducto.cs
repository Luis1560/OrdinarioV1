using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrdinarioV1.Models.DataSet1TableAdapters;
using System.IO;
using System.Data;

namespace OrdinarioV1.Controller
{
    public class ControllerProducto
    {
        private readonly electronicosTableAdapter electronicosAdapter;

        public ControllerProducto()
        {
            electronicosAdapter = new electronicosTableAdapter();
        }
        public void agregarElectronico(string nombre, string marca, string modelo, decimal precio,string garantia, string especificaciones, int cantidad_Disponible)
        {
            try
            {
                electronicosAdapter.InsertElectronico(nombre, marca, modelo, precio, garantia, especificaciones, cantidad_Disponible);
            }
            catch (Exception ex)
            {

                throw new Exception("Error al agregar el producto: " + ex.Message);
            }
        }

        public DataTable ObtenerProductos()
        {
            try
            {
                return electronicosAdapter.GetData();
            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener la lista de productos: " + ex.Message);
            }
        }
    }
}