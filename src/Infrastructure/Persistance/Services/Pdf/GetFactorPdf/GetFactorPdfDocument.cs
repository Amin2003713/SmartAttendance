using DNTPersianUtils.Core;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SmartAttendance.Persistence.Services.Pdf.GetFactorPdf;

public class GetFactorPdfDocument(
    SmartAttendanceTenantInfo company,
    Payments payments,
    Price price
)
    : IDocument
{
    private SmartAttendanceTenantInfo Seller { get; } = company;
    private Payments Payments { get; } = payments;
    private Price Price { get; } = price;

    public DocumentMetadata GetMetadata()
    {
        return DocumentMetadata.Default;
    }

    public void Compose(IDocumentContainer container)
    {
        // var fontStream = messageBroker.RequestAsync<GetFilesBrokerResponse, GetFilesQueryBroker>
        //         (new GetFilesQueryBroker(ApplicationConstant.Minio.DrpFont))
        //     .ConfigureAwait(false)
        //     .GetAwaiter()
        //     .GetResult()
        //     .Files.FirstOrDefault(a => a.Key == "Font")
        //     .Value; todo :// medatore

        Settings.CheckIfAllTextGlyphsAreAvailable = true;

        // FontManager.RegisterFontWithCustomName("test", fontStream.ToStream()); todo : register the font 


        Settings.License = LicenseType.Community;

        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.Letter.Landscape());
            page.PageColor(Colors.White);
            page.ContentFromRightToLeft();
            page.MarginHorizontal(70);
            page.MarginVertical(40);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily(@"test"));

            page.Content().Element(ComposeContent);
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(1);

            column.Item().Element(ComposeHeader);

            column.Item().Element(ComposeSeller);

            column.Item().Element(ComposeBuyer);

            column.Item().Element(ComposeProducts);


            column.Item()
                .Border(1)
                .BorderColor(Colors.Grey.Medium)
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Cell()
                        .Height(75)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مهر و امضاء فروشنده")
                        .AlignCenter()
                        .FontColor(Colors.Grey.Medium);

                    table.Cell()
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مهر و امضاء خریدار")
                        .AlignCenter()
                        .FontColor(Colors.Grey.Medium);
                });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        var date = Payments.CreatedAt.Date.ToShortPersianDateString();

        container.Table(table =>
        {
            table.ColumnsDefinition(col =>
            {
                col.RelativeColumn();
                col.RelativeColumn();
                col.RelativeColumn();
                col.RelativeColumn();
                col.RelativeColumn();
                col.RelativeColumn();
                col.RelativeColumn();
                col.RelativeColumn();
            });


            table.Cell()
                .ColumnSpan(7)
                .RowSpan(2)
                .PaddingRight(110)
                .AlignMiddle()
                .AlignCenter()
                .Text("صورت حساب فروش خدمات")
                .FontSize(14)
                .SemiBold();

            table.Cell().ColumnSpan(1).RowSpan(1).AlignMiddle().AlignCenter().Text("شماره : ۱۴۰۲-F-۳۵۲۱").Thin();
            table.Cell().ColumnSpan(1).RowSpan(1).AlignMiddle().AlignCenter().Text($"تاریخ : {date}").Thin();
        });
    }

    private void ComposeSeller(IContainer container)
    {
        container.PaddingTop(4)
            .Border(1)
            .BorderColor(Colors.Grey.Medium)
            .Table(table =>
            {
                table.Header(header =>
                {
                    header.Cell()
                        .ColumnSpan(12)
                        .Height(30)
                        .Background(Colors.Black)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مشخصات فروشنده")
                        .BackgroundColor(Colors.Black)
                        .FontColor(Colors.White)
                        .FontSize(12)
                        .SemiBold();
                });


                table.ColumnsDefinition(col =>
                {
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                });

                table.Cell()
                    .ColumnSpan(6)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text("نام شخص حقوقی :  شرکت همراه نرم افزار ایده پردازان آموزش");

                table.Cell()
                    .Column(7)
                    .Row(1)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .ScaleToFit()
                    .Text("شماره ثبت : ١٤٠٠٢٥٦٦٨٣٠");

                table.Cell()
                    .Column(10)
                    .ColumnSpan(3)
                    .Row(1)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .ScaleToFit()
                    .Text("شماره اقتصادی : ٤١١٤٣١٥٤٩٦٩٣");

                table.Cell()
                    .Row(2)
                    .Column(1)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text("استان : اصفهان");

                table.Cell()
                    .Row(2)
                    .Column(4)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text("شهر : اصفهان");

                table.Cell()
                    .Row(2)
                    .Column(7)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text("شهرستان : اصفهان");

                table.Cell()
                    .Row(2)
                    .Column(10)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text("کد پستی :٨٤١٥٦٨٣٢٨٠ ");


                table.Cell()
                    .Row(3)
                    .Column(1)
                    .ColumnSpan(7)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text(
                        "نشانی : بلوار دانشگاه صنعتی شهرک علمی تحقیقاتی اصفهان ، ساختمان فن آفرینی ۱، واحد ۱۰۵ ");

                table.Cell()
                    .Row(3)
                    .Column(8)
                    .ColumnSpan(5)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text("شماره تلفن / نمابر :۰۳۱-۳۳۹۳۲۲۹۲");
            });
    }

    private void ComposeBuyer(IContainer container)
    {
        container.Border(1)
            .BorderColor(Colors.Grey.Medium)
            .BorderColor(Colors.Grey.Medium)
            .Table(table =>
            {
                table.Header(header =>
                {
                    header.Cell()
                        .ColumnSpan(12)
                        .Height(30)
                        .Background(Colors.Black)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مشخصات خریدار")
                        .FontColor(Colors.White)
                        .FontSize(12)
                        .SemiBold();
                });


                table.ColumnsDefinition(col =>
                {
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                });

                table.Cell()
                    .ColumnSpan(8)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text(
                        $"{(Seller.IsLegal ? "نام شخص حقوقی : " : "نام شخص حقیقی : ")} : {Seller.LegalName ?? Seller.Name}");

                table.Cell()
                    .Column(9)
                    .Row(1)
                    .ColumnSpan(4)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .ScaleToFit()
                    .Text(
                        $"{(Seller.IsLegal ? "شماره ثبت : " : "شماره ملی : ")} {Seller.NationalCode.ToPersianNumbers()}");


                table.Cell()
                    .Row(2)
                    .Column(1)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text($"استان : {Seller.Province}");

                table.Cell()
                    .Row(2)
                    .Column(4)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text($"شهر : {Seller.City}");

                table.Cell()
                    .Row(2)
                    .Column(7)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text($"شهرستان : {Seller.Town}");

                table.Cell()
                    .Row(2)
                    .Column(10)
                    .ColumnSpan(3)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text($"کد پستی : {Seller.PostalCode.ToPersianNumbers()}");


                table.Cell()
                    .Row(3)
                    .Column(1)
                    .ColumnSpan(7)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text(
                        $"نشانی : {Seller.Address}");

                table.Cell()
                    .Row(3)
                    .Column(8)
                    .ColumnSpan(5)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .PaddingRight(4)
                    .Text($"شماره تلفن / نمابر : {Seller.PhoneNumber.ToPersianNumbers()}");
            });
    }

    private void ComposeProducts(IContainer container)
    {
        container.Border(1)
            .BorderColor(Colors.Grey.Medium)
            .Table(table =>
            {
                table.Header(header =>
                {
                    header.Cell()
                        .ColumnSpan(12)
                        .Background(Colors.Black)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مشخصات کالا یا خدمات مورد معامله")
                        .FontColor(Colors.White)
                        .FontSize(14)
                        .SemiBold();


                    header.Cell()
                        .Row(2)
                        .Column(1)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("ردیف");

                    header.Cell()
                        .Row(2)
                        .Column(2)
                        .ColumnSpan(4)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("شرح کالا یا خدمات");

                    header.Cell()
                        .Row(2)
                        .Column(6)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("تعداد کاربر");

                    header.Cell()
                        .Row(2)
                        .Column(7)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مبلغ واحد (ریال)");

                    header.Cell()
                        .Row(2)
                        .Column(8)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مبلغ کل (ریال)");

                    header.Cell()
                        .Row(2)
                        .Column(9)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مبلغ تخفیف");

                    header.Cell()
                        .Row(2)
                        .Column(10)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("مبلغ کل پس از تخفیف (ریال)");

                    header.Cell()
                        .Row(2)
                        .Column(11)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("جمع مالیات و عوارض (ریال)");

                    header.Cell()
                        .Row(2)
                        .Column(12)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten1)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text("جمع مبلغ کل بعلاوه جمع مالیات و عوارض (ریال)");
                });

                table.ColumnsDefinition(col =>
                {
                    col.RelativeColumn(0.9F);
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();

                    col.RelativeColumn();
                    col.RelativeColumn(0.75F);
                    col.RelativeColumn(1.5F);
                    col.RelativeColumn(1.5F);

                    col.RelativeColumn(2);
                    col.RelativeColumn(3);
                    col.RelativeColumn(3);
                    col.RelativeColumn(4);
                });

                var productTitle           = SetTitle(Payments.Status, payments.Duration ?? 0);
                var amount                 = (payments.Duration, payments.UsersCount);
                var price                  = Price;
                var totalPrice             = payments.BasePrice;
                var totalDiscount          = payments.DiscountAmount;
                var totalPriceWithDiscount = totalPrice - totalDiscount;
                var totalTax               = payments.TaxAmount;
                var finalPrice             = payments.Cost;
                var counter                = 1;

                table.Cell()
                    .Column(1)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{counter++}");

                table.Cell()
                    .Column(2)
                    .ColumnSpan(4)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{productTitle}");

                table.Cell()
                    .Column(6)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{amount.UsersCount}");

                table.Cell()
                    .Column(7)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{FormatPrice(Price?.Amount ?? 0).ToPersianNumbers()}");

                table.Cell()
                    .Column(8)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{FormatPrice(totalPrice).ToPersianNumbers()}");

                table.Cell()
                    .Column(9)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{FormatPrice(totalDiscount).ToPersianNumbers()}");

                table.Cell()
                    .Column(10)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{FormatPrice(totalPriceWithDiscount).ToPersianNumbers()}");

                table.Cell()
                    .Column(11)
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .ScaleToFit()
                    .Text($"{FormatPrice(totalTax).ToPersianNumbers()}");

                table.Cell()
                    .Column(12)
                    .ScaleToFit()
                    .Border(1)
                    .BorderColor(Colors.Grey.Medium)
                    .AlignMiddle()
                    .AlignCenter()
                    .Text($"{FormatPrice(finalPrice).ToPersianNumbers()}");


                table.Footer(footer =>
                {
                    footer.Cell()
                        .ColumnSpan(7)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten3)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignRight()
                        .PaddingRight(4)
                        .Text("جمع کل");

                    footer.Cell()
                        .Column(8)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten3)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .PaddingRight(4)
                        .AlignMiddle()
                        .AlignCenter()
                        .Text($"{FormatPrice(totalPrice).ToPersianNumbers()}");

                    footer.Cell()
                        .Column(9)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten3)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .PaddingRight(4)
                        .Text($"{FormatPrice(totalDiscount).ToPersianNumbers()}")
                        ;

                    footer.Cell()
                        .Column(10)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten3)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .PaddingRight(4)
                        .Text($"{FormatPrice(totalPriceWithDiscount).ToPersianNumbers()}");

                    footer.Cell()
                        .Column(11)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten3)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .PaddingRight(4)
                        .Text($"{FormatPrice(totalTax).ToPersianNumbers()}");

                    footer.Cell()
                        .Column(12)
                        .ScaleToFit()
                        .Background(Colors.Grey.Lighten3)
                        .Border(1)
                        .BorderColor(Colors.Grey.Medium)
                        .AlignMiddle()
                        .AlignCenter()
                        .PaddingRight(4)
                        .Text($"{FormatPrice(finalPrice).ToPersianNumbers()}");
                });
            });
    }

    private string FormatPrice(decimal cost)
    {
        return (cost * 10).ToString("N0");
    }

    private string SetTitle(int status, int duration)
    {
        return status switch
               {
                   1 or 3 or 2 => $"فروش آبونمان {duration} ماهه سامانه مديريت پروژه DRP", _ => ""
               };
    }
}