using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Stock = table.Column<bool>(type: "bit", nullable: true),
                    CantidadMinima = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria_Producto",
                columns: table => new
                {
                    Categoria_Id = table.Column<int>(type: "int", nullable: false),
                    Producto_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__00D810271367E606", x => new { x.Categoria_Id, x.Producto_Id });
                    table.ForeignKey(
                        name: "FK_Categoria_Producto_Categoria",
                        column: x => x.Categoria_Id,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categoria_Producto_Producto",
                        column: x => x.Producto_Id,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Whatsapp = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email_Verificado = table.Column<bool>(type: "bit", nullable: false),
                    Remember_Token = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carrito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carrito_Usuario",
                        column: x => x.Usuario_Id,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario_Id = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordenes_Usuarios",
                        column: x => x.Usuario_Id,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Item_Carrito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Carrito_Id = table.Column<int>(type: "int", nullable: false),
                    Producto_Id = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Carrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Carrito_Carrito",
                        column: x => x.Carrito_Id,
                        principalTable: "Carrito",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Carrito_Producto",
                        column: x => x.Producto_Id,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Item_Ordenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Orden_Id = table.Column<int>(type: "int", nullable: false),
                    Producto_Id = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Ordenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Ordenes_Ordenes",
                        column: x => x.Orden_Id,
                        principalTable: "Ordenes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Ordenes_Productos",
                        column: x => x.Producto_Id,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carrito_Usuario_Id",
                table: "Carrito",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Producto_Producto_Id",
                table: "Categoria_Producto",
                column: "Producto_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Carrito_Carrito_Id",
                table: "Item_Carrito",
                column: "Carrito_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Carrito_Producto_Id",
                table: "Item_Carrito",
                column: "Producto_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Ordenes_Orden_Id",
                table: "Item_Ordenes",
                column: "Orden_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Ordenes_Producto_Id",
                table: "Item_Ordenes",
                column: "Producto_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_Usuario_Id",
                table: "Ordenes",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RoleId",
                table: "Usuarios",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__A9D10534060DEAE8",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categoria_Producto");

            migrationBuilder.DropTable(
                name: "Item_Carrito");

            migrationBuilder.DropTable(
                name: "Item_Ordenes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Carrito");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
