#pragma checksum "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f3de673192ee71090a894bb326e49c1e6aab4db6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Promotions_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/Promotions/Index.cshtml")]
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
#line 1 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\_ViewImports.cshtml"
using TracyShop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\_ViewImports.cshtml"
using TracyShop.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f3de673192ee71090a894bb326e49c1e6aab4db6", @"/Areas/Admin/Views/Promotions/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"16f86741a3b48e9badd7d476ed76c310aac6acc8", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Promotions_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<TracyShop.Models.Promotion>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "Admin", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Promotions", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("deletePromotion"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Areas/Admin/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div class=\"d-sm-flex align-items-center justify-content-between mb-4\">\r\n    <h1 class=\"h3 mb-0 text-gray-800\">Danh sách ưu đãi</h1>\r\n\r\n");
#nullable restore
#line 13 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
     if (_context.UserRoles.Where(r => r.UserId.Contains(User.FindFirst("Id").Value)).First().RoleId == "1")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f3de673192ee71090a894bb326e49c1e6aab4db66548", async() => {
                WriteLiteral("\r\n            <i class=\"fas fa-plus-circle text-white-50\"></i> Thêm ưu đãi\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 18 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
</div>
<table class=""table"">
    <thead>
        <tr>
            <th>
                ID
            </th>
            <th>
                Mức giảm giá
            </th>
            <th>
                Ngày bắt đầu
            </th>
            <th>
                Ngày kết thúc
            </th>
            <th></th>
        </tr>
    </thead>
    <tbody>
");
#nullable restore
#line 40 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 44 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 47 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.percent));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 50 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.StartedDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 53 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.EndDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n");
#nullable restore
#line 55 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
                 if (_context.UserRoles.Where(r => r.UserId.Contains(User.FindFirst("Id").Value)).First().RoleId == "1")
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <td>\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f3de673192ee71090a894bb326e49c1e6aab4db610905", async() => {
                WriteLiteral("<i class=\"fas fa-edit\"></i>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 58 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
                                                                                            WriteLiteral(item.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |\r\n                        <button data-id=\"");
#nullable restore
#line 59 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
                                    Write(item.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" class=\"btn deletePromotion\" type=\"button\"> <i class=\"far fa-trash-alt\"></i></button> |\r\n                    </td>\r\n");
#nullable restore
#line 61 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <td></td>\r\n");
#nullable restore
#line 65 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </tr>\r\n");
#nullable restore
#line 68 "D:\ASP.NET  Projects\Graduation Essay\TracyShop\Areas\Admin\Views\Promotions\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f3de673192ee71090a894bb326e49c1e6aab4db614760", async() => {
                WriteLiteral("\r\n    <input type=\"hidden\" id=\"promotionId\" name=\"Id\" />\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>
        $(function(){
            $('.deletePromotion').click(function(e) {
                swal({
                    title: ""Bạn chắc chắn?"",
                    text: ""Bạn chắc chắn muốn xóa chương trình khuyến mãi này?"",
                    icon: ""warning"",
                    buttons: true,
                    dangerMode: true
                }).then((confirm) => {
                        if(confirm){
                            var btn = $(this);
                            var id = btn.data(""id"");
                            $('#promotionId').val(id);
                            $('#deletePromotion').submit();
                        }
                    });
            });
        });
    </script>
");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<TracyShop.Models.Promotion>> Html { get; private set; }
    }
}
#pragma warning restore 1591
