#pragma checksum "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "35cccbec88da23052b4844e6b8657c2b3be974af"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Profile_PurchaseHistory), @"mvc.1.0.view", @"/Views/Profile/PurchaseHistory.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\ASP.NET  Projects\TracyShop\Views\_ViewImports.cshtml"
using TracyShop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\ASP.NET  Projects\TracyShop\Views\_ViewImports.cshtml"
using TracyShop.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\ASP.NET  Projects\TracyShop\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\ASP.NET  Projects\TracyShop\Views\_ViewImports.cshtml"
using Newtonsoft.Json;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"35cccbec88da23052b4844e6b8657c2b3be974af", @"/Views/Profile/PurchaseHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7da188806fb868db264f4692419326bcf4d842f1", @"/Views/_ViewImports.cshtml")]
    public class Views_Profile_PurchaseHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<TracyShop.ViewModels.PurchaseHistoryViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
  
    ViewData["Title"] = "Purchase History";
    Layout = "~/Views/Shared/_LayoutProfile.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"card\" style=\"padding: 3%\">\r\n    <h2 class=\"text text-center\" style=\"color: #FFBF00;\">Lịch sử đơn hàng</h2>\r\n</div>\r\n");
#nullable restore
#line 11 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
 foreach (var item in Model)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"container card\" style=\"margin-top: -14%\">\r\n        <div style=\"margin: 2%;\">Ngày mua: ");
#nullable restore
#line 14 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                                      Write(item.OrderDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n        <table>\r\n");
#nullable restore
#line 16 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
             foreach (var detail in item.OrderDetails)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <th>\r\n                        <div class=\"row\">\r\n                            <div class=\"col-md-4 col-xs-12\">\r\n                                <img");
            BeginWriteAttribute("src", " src=\"", 785, "\"", 863, 1);
#nullable restore
#line 22 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
WriteAttributeValue("", 791, _context.Image.Where(i => i.ProductId == detail.ProductId).First().Path, 791, 72, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("alt", " alt=\"", 864, "\"", 870, 0);
            EndWriteAttribute();
            WriteLiteral(" style=\"width: 50%\" />\r\n                            </div>\r\n                            <div class=\"col-md-8\" col-xs-12 style=\"margin-top: 2%;\">\r\n                                <h5>");
#nullable restore
#line 25 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                               Write(_context.Product.Where(p => p.Id == detail.ProductId).First().Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                                <p>Phân loại hàng: ");
#nullable restore
#line 26 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                                              Write(_context.Sizes.Where(s => s.Id == detail.SelectedSize).First().Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                                <p>x ");
#nullable restore
#line 27 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                                Write(detail.Quantity);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                            </div>\r\n                        </div>\r\n                    </th>\r\n                    <th>\r\n                       <div class=\"unit-price\">\r\n");
#nullable restore
#line 33 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                             if (detail.Promotion != 0)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <div class=\"unit-price-origin\" style=\"font-size: 12px; text-decoration: line-through;\">");
#nullable restore
#line 35 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                                                                                                                   Write(detail.Price/(1 - detail.Promotion));

#line default
#line hidden
#nullable disable
            WriteLiteral(" VNĐ</div>\r\n");
#nullable restore
#line 36 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <div class=\"unit-price-discount text text-danger\" style=\"font-size: 15px; font-weight: bold;\">");
#nullable restore
#line 37 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                                                                                                                      Write(detail.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral(" VNĐ</div>\r\n                        </div>    \r\n                    </th>\r\n                </tr>\r\n");
#nullable restore
#line 41 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        </table>
        <hr />
        <div class=""row"">
            <div class=""col-md-7 col-xs-12""></div>
            <div class=""col-md-5 col-xs-12"">
                <div class=""row"">
                    <div class=""col-md-6"">
                        <svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" fill=""green"" class=""bi bi-cash-coin"" viewBox=""0 0 16 16"">
                            <path fill-rule=""evenodd"" d=""M11 15a4 4 0 1 0 0-8 4 4 0 0 0 0 8zm5-4a5 5 0 1 1-10 0 5 5 0 0 1 10 0z"" />
                            <path d=""M9.438 11.944c.047.596.518 1.06 1.363 1.116v.44h.375v-.443c.875-.061 1.386-.529 1.386-1.207 0-.618-.39-.936-1.09-1.1l-.296-.07v-1.2c.376.043.614.248.671.532h.658c-.047-.575-.54-1.024-1.329-1.073V8.5h-.375v.45c-.747.073-1.255.522-1.255 1.158 0 .562.378.92 1.007 1.066l.248.061v1.272c-.384-.058-.639-.27-.696-.563h-.668zm1.36-1.354c-.369-.085-.569-.26-.569-.522 0-.294.216-.514.572-.578v1.1h-.003zm.432.746c.449.104.655.272.655.569 0 .339-.257.571-.709.614v-1.195l.054.012");
            WriteLiteral(@"z"" />
                            <path d=""M1 0a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h4.083c.058-.344.145-.678.258-1H3a2 2 0 0 0-2-2V3a2 2 0 0 0 2-2h10a2 2 0 0 0 2 2v3.528c.38.34.717.728 1 1.154V1a1 1 0 0 0-1-1H1z"" />
                            <path d=""M9.998 5.083 10 5a2 2 0 1 0-3.132 1.65 5.982 5.982 0 0 1 3.13-1.567z"" />
                        </svg>
                        Tổng số tiền:
                    </div>
                    <div class=""col-md-6"">
                        <h5 class=""text text-danger"" style=""font-weight: bold;"">");
#nullable restore
#line 58 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
                                                                           Write(item.TotalPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral(" VNĐ</h5>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 64 "D:\ASP.NET  Projects\TracyShop\Views\Profile\PurchaseHistory.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public TracyShop.Data.AppDbContext _context { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<TracyShop.ViewModels.PurchaseHistoryViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
