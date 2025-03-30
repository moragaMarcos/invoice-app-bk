using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class MvpInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_customer",
                columns: table => new
                {
                    Run = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    customer_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    customer_email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_customer", x => x.Run);
                });

            migrationBuilder.CreateTable(
                name: "t_role",
                columns: table => new
                {
                    role_id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "t_invoice",
                columns: table => new
                {
                    InvoiceNumber = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerRun = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    invoice_status = table.Column<string>(type: "TEXT", nullable: false),
                    total_amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    payment_due_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    payment_status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_invoice", x => x.InvoiceNumber);
                    table.ForeignKey(
                        name: "FK_t_invoice_t_customer_CustomerRun",
                        column: x => x.CustomerRun,
                        principalTable: "t_customer",
                        principalColumn: "Run",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_t_user_t_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "t_role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "t_credit_note",
                columns: table => new
                {
                    CreditNoteNumber = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    credit_note_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    credit_note_amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_credit_note", x => x.CreditNoteNumber);
                    table.ForeignKey(
                        name: "FK_t_credit_note_t_invoice_InvoiceNumber",
                        column: x => x.InvoiceNumber,
                        principalTable: "t_invoice",
                        principalColumn: "InvoiceNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_payment",
                columns: table => new
                {
                    PaymentNumber = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    payment_method = table.Column<string>(type: "TEXT", nullable: true),
                    payment_date = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_payment", x => x.PaymentNumber);
                    table.ForeignKey(
                        name: "FK_t_payment_t_invoice_InvoiceNumber",
                        column: x => x.InvoiceNumber,
                        principalTable: "t_invoice",
                        principalColumn: "InvoiceNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_product",
                columns: table => new
                {
                    ProductNumber = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    product_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    unit_price = table.Column<decimal>(type: "TEXT", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    subtotal = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_product", x => x.ProductNumber);
                    table.ForeignKey(
                        name: "FK_t_product_t_invoice_InvoiceNumber",
                        column: x => x.InvoiceNumber,
                        principalTable: "t_invoice",
                        principalColumn: "InvoiceNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_invoice_number_tCreditNote",
                table: "t_credit_note",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "idx_customer_run_tInvoice",
                table: "t_invoice",
                column: "CustomerRun");

            migrationBuilder.CreateIndex(
                name: "idx_invoice_and_payment_status_tInvoice",
                table: "t_invoice",
                columns: new[] { "invoice_status", "payment_status" });

            migrationBuilder.CreateIndex(
                name: "idx_invoice_number_tInvoice",
                table: "t_invoice",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "idx_invoice_status_tInvoice",
                table: "t_invoice",
                column: "invoice_status");

            migrationBuilder.CreateIndex(
                name: "idx_payment_status_tInvoice",
                table: "t_invoice",
                column: "payment_status");

            migrationBuilder.CreateIndex(
                name: "idx_invoice_number_tPayment",
                table: "t_payment",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_invoice_number_tProduct",
                table: "t_product",
                column: "InvoiceNumber");

            migrationBuilder.CreateIndex(
                name: "idx_role_id_tUser",
                table: "t_user",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_credit_note");

            migrationBuilder.DropTable(
                name: "t_payment");

            migrationBuilder.DropTable(
                name: "t_product");

            migrationBuilder.DropTable(
                name: "t_user");

            migrationBuilder.DropTable(
                name: "t_invoice");

            migrationBuilder.DropTable(
                name: "t_role");

            migrationBuilder.DropTable(
                name: "t_customer");
        }
    }
}
