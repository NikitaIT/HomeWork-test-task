using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WebApplication1.Areas.Catalog.Entites;
using WebApplication1.Areas.Catalog.Models;

namespace WebApplication1.Areas.Catalog.Mappings
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Table("[Category]");
            Id(x => x.Id, "CategoryId").GeneratedBy.Identity();

            Map(x => x.Name).Length(256)
                .Not.Nullable()
                .Unique();

            Map(x => x.IsActive).Not.Nullable().Default("1");

            References(x => x.ParentCategory)
                .Column("ParentCategoryId");

            HasMany(x => x.ChildCategories)
                .KeyColumn("ParentCategoryId").Inverse()
                .AsBag();

            HasMany(x => x.Articles).Cascade.All();
        }
    }
}