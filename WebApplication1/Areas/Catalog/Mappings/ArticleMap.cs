﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using WebApplication1.Areas.Catalog.Entites;

namespace WebApplication1.Areas.Catalog.Mappings
{
    public class ArticleMap : ClassMap<Article>
    {
        public ArticleMap()
        {
            Table("[Article]");
            Id(x => x.Id, "ArticleId").GeneratedBy.Identity();
            Map(x => x.Author).Length(256)
                .Not.Nullable();
            Map(x => x.Title).Length(256)
                .Not.Nullable();
            Map(x => x.Content);
            References(x => x.Category);
        }
    }
}