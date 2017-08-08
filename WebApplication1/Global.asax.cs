using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using FluentNHibernate.Conventions;
using WebApplication1.Areas.Catalog.Entites;
using WebApplication1.Areas.Catalog.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg => cfg.CreateMap<Category, CategoryTreeViewModel>()
                .ForMember(x => x.NestingLevel, o => o.Ignore())
                .ForMember(x => x.IsExpanded, o => o.Ignore())
                .ForMember(x => x.IsSelected, o => o.Ignore())
                 .ForMember(x => x.ParentCategory, o => o.Ignore())
                .ForMember(x => x.HasChildCategories, o => o.Ignore())
                .AfterMap((category, categoryModel) =>
                {
                    // Mapper doesn't know about categories relationship, so we need assign them explicitly.
                    // Without it setting IsExpanded = true will not have effect on childCategory.ParentCategory
                    // as this is another object
                    if (categoryModel == null || categoryModel.ChildCategories.IsEmpty())
                    {
                        return;
                    }
                    foreach (var childCategory in categoryModel.ChildCategories)
                    {
                        childCategory.ParentCategory = categoryModel;
                    }
                }));
        }
    }
}
