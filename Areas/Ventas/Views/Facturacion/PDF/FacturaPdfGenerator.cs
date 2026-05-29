using PresupuestoMVC.Models.DTOs;
using PresupuestoMVC.Models.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace PresupuestoMVC.Areas.Ventas.Views.Facturacion.PDF
{
    public static class FacturaPdfGenerator
    {
        public static byte[] Generate(Sale sale, ClienteResponseDTO client, Company company)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // --- HEADER: información de la empresa y factura ---
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("NX").FontSize(28).Bold();
                            col.Item().Text(company.CompanyName).FontSize(14).Bold();
                            col.Item().Text($"{company.Street} {company.StreetNumber} - {company.Locality}, {company.Province} ({company.CP})").FontSize(9);
                            col.Item().Text($"Tel: {company.Phone}").FontSize(9);
                            col.Item().Text($"CUIT: {company.CUIT}").FontSize(9);
                            col.Item().Text("IVA Responsable Inscripto").FontSize(9);
                        });

                        row.ConstantItem(150).Column(col =>
                        {
                            col.Item().AlignRight().Text("FACTURA").FontSize(22).Bold().FontColor(Colors.Grey.Darken3);
                            col.Item().AlignRight().Text($"Número: 0003-00045871").FontSize(10);
                            col.Item().AlignRight().Text($"Fecha: {sale.DateInserted:dd/MM/yyyy}").FontSize(10);
                            col.Item().AlignRight().Text($"Punto Venta: 0003").FontSize(10);
                            col.Item().AlignRight().Text("Letra: A").FontSize(16).Bold();
                        });
                    });

                    // Espacio
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        // --- DATOS DEL CLIENTE ---
                        col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(5).Text("DATOS DEL CLIENTE").FontSize(12).Bold();
                        col.Item().PaddingTop(5).PaddingBottom(10).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span("Razón Social: ").Bold();
                                    t.Span("Comercial Delta SRL");
                                });
                                c.Item().Text(t =>
                                {
                                    t.Span("CUIT/DNI: ").Bold();
                                    t.Span(sale.DNI);
                                });
                                c.Item().Text(t =>
                                {
                                    t.Span("Domicilio: ").Bold();
                                    t.Span($"{client.Domicilio} - {client.Localidad}, {client.Provincia}, CP: {client.CodigoPostal}");
                                });
                            });
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t =>
                                {
                                    t.Span("Condición IVA: ").Bold();
                                    t.Span("Responsable Inscripto");
                                });
                                c.Item().Text(t =>
                                {
                                    t.Span("Condición de Pago: ").Bold();
                                    t.Span("Cuenta Corriente 30 días");
                                });
                                c.Item().Text(t =>
                                {
                                    t.Span("Vendedor: ").Bold();
                                    t.Span(company.CompanyName);
                                });
                            });
                        });

                        // --- TABLA DE PRODUCTOS ---
                        col.Item().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingBottom(5).Text("DETALLE DE PRODUCTOS").FontSize(12).Bold();

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Código
                                columns.RelativeColumn(3); // Descripción
                                columns.RelativeColumn(1); // Cant.
                                columns.RelativeColumn(1); // U.Med
                                columns.RelativeColumn(2); // Precio
                                columns.RelativeColumn(1); // Desc.
                                columns.RelativeColumn(2); // Total
                            });

                            // Cabecera
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Código").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Descripción").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Cant.").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("U.Med").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Precio").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Desc.").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).AlignRight().Text("Total").Bold();
                            });

                            // Filas de detalle
                            foreach (var item in sale.Detail)
                            {
                                table.Cell().Padding(5).Text(item.CodeItem ?? "-");
                                table.Cell().Padding(5).Text(item.NameItem);
                                table.Cell().Padding(5).AlignRight().Text(item.Quantity.ToString());
                                table.Cell().Padding(5).Text("UN");
                                table.Cell().Padding(5).AlignRight().Text($"${item.PrecioUnitario:N0}");
                                table.Cell().Padding(5).AlignRight().Text($"10%");
                                table.Cell().Padding(5).AlignRight().Text($"${item.Total:N0}");
                            }
                        });

                        // --- TOTALES ---
                        col.Item().PaddingTop(20).AlignRight().Column(colTotals =>
                        {
                            colTotals.Item().Text(t =>
                            {
                                t.Span("Subtotal: ").Bold();
                                t.Span($"${sale.Subtotal:N0}");
                            });
                            colTotals.Item().Text(t =>
                            {
                                t.Span("IVA 21%: ").Bold();
                                t.Span($"${sale.Subtotal * 0.21m:N0}");
                            });
                            colTotals.Item().Text(t =>
                            {
                                t.Span("Descuentos 10%: ").Bold();
                                t.Span($"${sale.Descuento:N0}");
                            });
                            colTotals.Item().PaddingTop(5).Text(t =>
                            {
                                t.Span("TOTAL: ").Bold().FontSize(14);
                                t.Span($"${sale.Total:N0}").FontSize(14).Bold();
                            });
                        });

                        // --- FOOTER (Observaciones y firma) ---
                        col.Item().PaddingTop(30).BorderTop(1).BorderColor(Colors.Grey.Lighten2).PaddingTop(10).Column(footerCol =>
                        {
                            footerCol.Item().Text("Observaciones").FontSize(10).Bold();
                            footerCol.Item().Text("Mercadería sujeta a disponibilidad de stock. Gracias por confiar en Nexus Distribuciones S.A. Conserve esta factura como comprobante válido.")
                                .FontSize(9).Italic();
                            footerCol.Item().PaddingTop(20).Row(r =>
                            {
                                r.ConstantItem(150).BorderBottom(1).BorderColor(Colors.Grey.Medium).Height(20);
                                r.RelativeItem().Text("Firma y aclaración").FontSize(9).AlignCenter();
                            });
                        });
                    });

                    // Pie de página (alternativo al que definiste arriba, pero lo dejo como ejemplo)
                    page.Footer().AlignCenter().Text("Gracias por su compra").FontSize(8).FontColor(Colors.Grey.Medium);
                });
            }).GeneratePdf();
        }
    }
}
