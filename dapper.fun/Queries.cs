namespace dapper.fun
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Selects;
    using static Transforms;

    public class Queries
    {
        public static ChangeFty<string> WithWhere = w => query => $"{query} WHERE {w}";

        public static Func<string, Select<IEnumerable<R>>> Where<R>(QueryString query)
        {
            return ChangeQuery(Query<R>)(WithWhere)(query);
        }
        public static Func<string, Select<P, IEnumerable<R>>> Where<P, R>(QueryString query)
        {
            return ChangeQuery(Query<P, R>)(WithWhere)(query);
        }
                
    }
}