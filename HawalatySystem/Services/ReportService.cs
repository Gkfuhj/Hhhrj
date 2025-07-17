using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using HawalatySystem.Models;

namespace HawalatySystem.Services
{
    public class ReportService
    {
        public async Task<string> GenerateTransferReceiptPdfAsync(Transfer transfer, string outputPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var document = new Document(PageSize.A4);
                    var writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
                    
                    document.Open();

                    // Header
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    // Title
                    var title = new Paragraph("Hawalaty System - Transfer Receipt", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    };
                    document.Add(title);

                    // Transfer Details Table
                    var table = new PdfPTable(2) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 30f, 70f });

                    // Add transfer details
                    AddTableRow(table, "Transfer ID:", transfer.TransferId, headerFont, normalFont);
                    AddTableRow(table, "Sender Name:", transfer.SenderName, headerFont, normalFont);
                    AddTableRow(table, "Receiver Name:", transfer.ReceiverName, headerFont, normalFont);
                    AddTableRow(table, "From Country:", transfer.FromCountry, headerFont, normalFont);
                    AddTableRow(table, "To Country:", transfer.ToCountry, headerFont, normalFont);
                    AddTableRow(table, "Currency:", transfer.Currency, headerFont, normalFont);
                    AddTableRow(table, "Amount:", $"{transfer.Amount:F2} {transfer.Currency}", headerFont, normalFont);
                    AddTableRow(table, "Commission:", $"{transfer.Commission:F2} {transfer.Currency}", headerFont, normalFont);
                    AddTableRow(table, "Final Amount:", $"{transfer.FinalAmount:F2} {transfer.Currency}", headerFont, normalFont);
                    AddTableRow(table, "Transfer Method:", transfer.TransferMethod, headerFont, normalFont);
                    AddTableRow(table, "Status:", transfer.Status.ToString(), headerFont, normalFont);
                    AddTableRow(table, "Created Date:", transfer.CreatedDate.ToString("dd/MM/yyyy HH:mm"), headerFont, normalFont);
                    
                    if (!string.IsNullOrEmpty(transfer.Notes))
                        AddTableRow(table, "Notes:", transfer.Notes, headerFont, normalFont);

                    document.Add(table);

                    // Footer
                    var footer = new Paragraph("\nGenerated on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), normalFont)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingBefore = 30
                    };
                    document.Add(footer);

                    document.Close();
                    return outputPath;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error generating PDF: {ex.Message}");
                }
            });
        }

        public async Task<string> GenerateTransfersReportExcelAsync(List<Transfer> transfers, string outputPath, 
            DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Transfers Report");

                    // Header
                    worksheet.Cell(1, 1).Value = "Hawalaty System - Transfers Report";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                    worksheet.Range(1, 1, 1, 15).Merge();

                    // Date range
                    if (fromDate.HasValue && toDate.HasValue)
                    {
                        worksheet.Cell(2, 1).Value = $"Period: {fromDate.Value:dd/MM/yyyy} - {toDate.Value:dd/MM/yyyy}";
                        worksheet.Range(2, 1, 2, 15).Merge();
                    }

                    // Column headers
                    var headers = new[]
                    {
                        "Transfer ID", "Sender Name", "Receiver Name", "From Country", "To Country",
                        "Currency", "Amount", "Commission", "Final Amount", "Transfer Method",
                        "Status", "Created Date", "Completed Date", "Created By", "Notes"
                    };

                    int headerRow = 4;
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(headerRow, i + 1).Value = headers[i];
                        worksheet.Cell(headerRow, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    // Data rows
                    int currentRow = headerRow + 1;
                    foreach (var transfer in transfers)
                    {
                        worksheet.Cell(currentRow, 1).Value = transfer.TransferId;
                        worksheet.Cell(currentRow, 2).Value = transfer.SenderName;
                        worksheet.Cell(currentRow, 3).Value = transfer.ReceiverName;
                        worksheet.Cell(currentRow, 4).Value = transfer.FromCountry;
                        worksheet.Cell(currentRow, 5).Value = transfer.ToCountry;
                        worksheet.Cell(currentRow, 6).Value = transfer.Currency;
                        worksheet.Cell(currentRow, 7).Value = transfer.Amount;
                        worksheet.Cell(currentRow, 8).Value = transfer.Commission;
                        worksheet.Cell(currentRow, 9).Value = transfer.FinalAmount;
                        worksheet.Cell(currentRow, 10).Value = transfer.TransferMethod;
                        worksheet.Cell(currentRow, 11).Value = transfer.Status.ToString();
                        worksheet.Cell(currentRow, 12).Value = transfer.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                        worksheet.Cell(currentRow, 13).Value = transfer.CompletedDate?.ToString("dd/MM/yyyy HH:mm") ?? "";
                        worksheet.Cell(currentRow, 14).Value = transfer.CreatedByUser?.FullName ?? "";
                        worksheet.Cell(currentRow, 15).Value = transfer.Notes;

                        currentRow++;
                    }

                    // Auto-fit columns
                    worksheet.Columns().AdjustToContents();

                    // Summary
                    currentRow += 2;
                    worksheet.Cell(currentRow, 1).Value = "Summary:";
                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Total Transfers:";
                    worksheet.Cell(currentRow, 2).Value = transfers.Count;

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Total Amount (TRY):";
                    worksheet.Cell(currentRow, 2).Value = transfers.Where(t => t.Currency == "TRY").Sum(t => t.Amount);

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Total Amount (LYD):";
                    worksheet.Cell(currentRow, 2).Value = transfers.Where(t => t.Currency == "LYD").Sum(t => t.Amount);

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Total Amount (USD):";
                    worksheet.Cell(currentRow, 2).Value = transfers.Where(t => t.Currency == "USD").Sum(t => t.Amount);

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "Total Commission:";
                    worksheet.Cell(currentRow, 2).Value = transfers.Sum(t => t.Commission);

                    workbook.SaveAs(outputPath);
                    return outputPath;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error generating Excel report: {ex.Message}");
                }
            });
        }

        private void AddTableRow(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
        {
            var labelCell = new PdfPCell(new Phrase(label, labelFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 5
            };
            
            var valueCell = new PdfPCell(new Phrase(value, valueFont))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                PaddingBottom = 5
            };

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }
    }
}