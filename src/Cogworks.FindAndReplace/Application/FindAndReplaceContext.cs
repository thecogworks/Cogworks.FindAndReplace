namespace Cogworks.FindAndReplace.Application
{
    class FindAndReplaceContext
    {

        private static readonly FindAndReplaceContext instance = new FindAndReplaceContext();

        static FindAndReplaceContext()
        {
        }

        private FindAndReplaceContext()
        {
        }

        public static FindAndReplaceContext Instance
        {
            get
            {
                return instance;
            }
        }

        public bool EnableFullTextSearch { get; set; }

    }
}
