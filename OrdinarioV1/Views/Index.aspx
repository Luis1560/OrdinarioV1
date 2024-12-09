<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="OrdinarioV1.Views.Index" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gestión de Electrónicos</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom CSS (opcional) -->
    <style>
        /* Fondo de página */
        body {
            background-color: darkgrey;
            background-size: cover;
            color: #333;
        }

        .container {
            margin-top: 20px;
            background-color: white;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .form-group label {
            font-weight: bold;
        }

        .table th, .table td {
            vertical-align: middle;
        }

        h1, h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #343a40;
        }

        /* Estilos para los formularios */
        .form-group {
            margin-bottom: 20px;
        }

        .form-control {
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
            padding: 10px 20px;
            border-radius: 5px;
        }

            .btn-primary:hover {
                background-color: #0056b3;
                border-color: #004085;
            }

        /* Estilos para la tabla */
        .table {
            margin-top: 30px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

            .table th, .table td {
                text-align: center;
                vertical-align: middle;
            }

            .table thead {
                background-color: #007bff;
                color: white;
            }

        .table-bordered {
            border: 1px solid #ddd;
        }

        .table-hover tbody tr:hover {
            background-color: #f1f1f1;
        }

        .alert-info {
            margin-top: 20px;
            display: none;
        }


        .carrito-container {
            margin-top: 20px;
            margin-left: auto;
            margin-right: 0;
            width: 50%; 
        }
    </style>
</head>
<body>

    <form id="formElectronicos" runat="server">
        <div class="container">
            <h1>Gestión de Electrónicos</h1>
            <asp:Panel ID="pnlLogin" runat="server" Visible="true">
                <h3>Iniciar Sesión</h3>
                <div class="form-group">
                    <label for="txtUsuario">Usuario:</label>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtContrasena">Contraseña:</label>
                    <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
                </div>
            </asp:Panel>
            <hr />
            <div class="row">
                <div class="col-md-6">
                    <asp:Panel ID="pnlSesion" runat="server" Visible="false">
                        <div class="col-md-6 carrito-container">
                            <h3>Carrito de Compras</h3>
                            <div id="carrito">
                                <asp:Label ID="mensajeCarrito" runat="server" Text="Tu carrito tiene 0 artículos." CssClass="alert alert-info"></asp:Label>
                                <asp:Button ID="btnCarrito" runat="server" Text="Ver Carrito" CssClass="btn btn-secondary" OnClick="btnVerCarrito_Click" />
                            </div>
                        </div>
                        <br />
                        <div>
                            <asp:HiddenField ID="hfID_Electronico" runat="server" />
                            <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-info d-none"></asp:Label>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtNombre">Nombre:</label>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtMarca">Marca:</label>
                                    <asp:TextBox ID="txtMarca" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtModelo">Modelo:</label>
                                    <asp:TextBox ID="txtModelo" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPrecio">Precio:</label>
                                    <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtGarantia">Garantía:</label>
                                    <asp:TextBox ID="txtGarantia" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtCantidad">Cantidad Disponible:</label>
                                    <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group mt-3">
                            <label for="txtEspecificaciones">Especificaciones:</label>
                            <asp:TextBox ID="txtEspecificaciones" runat="server" TextMode="MultiLine" CssClass="form-control" />
                        </div>

                        <div class="text-center mt-4">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary px-4" />
                        </div>

                        <!-- Tabla para Mostrar Electrónicos -->
                        <h2>Lista de Electrónicos</h2>
                        <asp:GridView ID="gvElectronicos" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover"
                            DataKeyNames="ID_Electronico"
                            OnRowEditing="gvElectronicos_RowEditing"
                            OnRowDeleting="gvElectronicos_RowDeleting"
                            OnRowCancelingEdit="gvElectronicos_RowCancelingEdit"
                            OnRowCommand="gvElectronicos_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ID_Electronico" HeaderText="ID" ReadOnly="True" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Marca" HeaderText="Marca" />
                                <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="Garantia" HeaderText="Garantía" />
                                <asp:BoundField DataField="Especificaciones" HeaderText="Especificaciones" />
                                <asp:BoundField DataField="Cantidad_Disponible" HeaderText="Cantidad Disponible" />
                                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnAgregarCarrito" runat="server" Text="Agregar al carrito"
                                            CommandName="AgregarCarrito" CommandArgument='<%# Eval("ID_Electronico") %>' CssClass="btn btn-success" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>

            </div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
