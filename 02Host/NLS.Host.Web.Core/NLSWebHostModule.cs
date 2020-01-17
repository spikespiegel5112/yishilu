using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NLS.Host.Web.Core.Register;
using System;
using System.IO;

namespace NLS.Host.Web.Core
{
    public static class NLSWebHostModule
    {
        /// <summary>
        /// 替换全局IoC
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceProvider AddGlobalIoC(this IServiceCollection services)
        {
            return WindsorRegistrationHelper.CreateServiceProvider(IoCRegister.Register(), services);
        }

        /// <summary>
        /// 启用静态文件支持
        /// 不支持 exe bat iso ppt pptx xlsx xls doc docx pdf dll
        /// </summary>
        /// <param name="directory">文件路径</param>
        /// <param name="request_name">路径映射名称，请求路径，默认为 /res </param>
        public static void NLSUseStaticFiles(this IApplicationBuilder app, string directory, string request_name = "/res")
        {
            if (!string.IsNullOrWhiteSpace(directory))
            {
                try
                {
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    var provider = new FileExtensionContentTypeProvider();
                    provider.Mappings.Remove(".exe");
                    provider.Mappings.Remove(".bat");
                    provider.Mappings.Remove(".iso");
                    provider.Mappings.Remove(".ppt");
                    provider.Mappings.Remove(".pptx");
                    provider.Mappings.Remove(".xlsx");
                    provider.Mappings.Remove(".xls");
                    provider.Mappings.Remove(".doc");
                    provider.Mappings.Remove(".docx");
                    provider.Mappings.Remove(".pdf");
                    provider.Mappings.Remove(".dll");
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), directory)),
                        RequestPath = new PathString(request_name),
                        OnPrepareResponse = p =>
                        {
                            p.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                        },
                        ContentTypeProvider = provider
                    });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}