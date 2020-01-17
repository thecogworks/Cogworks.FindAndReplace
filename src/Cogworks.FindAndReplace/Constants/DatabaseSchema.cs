namespace Cogworks.FindAndReplace.Constants
{
    public static class DatabaseSchema
    {
        public static class Tables
        {
            public const string TableUmbracoNamePrefix = "umbraco";

            public const string TableCmsNamePrefix = "cms";

            public const string Node = TableUmbracoNamePrefix + "Node";

            public const string Document = TableUmbracoNamePrefix + "Document";

            public const string DocumentVersion = TableUmbracoNamePrefix + "DocumentVersion";

            public const string Content = TableUmbracoNamePrefix + "Content";

            public const string ContentVersion = TableUmbracoNamePrefix + "ContentVersion";

            public const string ContentVersionCultureVariation = TableUmbracoNamePrefix + "ContentVersionCultureVariation";

            public const string Language = TableUmbracoNamePrefix + "Language";

            public const string PropertyData = TableUmbracoNamePrefix + "PropertyData";

            public const string PropertyType = TableCmsNamePrefix + "PropertyType";
        }
    }
}