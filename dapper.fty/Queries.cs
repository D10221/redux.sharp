namespace dapper.fty
{
    using System;
    using System.Collections.Generic;
    using static Operations;
    using static Transforms;

    public class Queries
    {
        public static ChangeFty<string> WithWhere = w => query => $"{query} WHERE {w}";

        public static Func<string, Select<IEnumerable<R>>> Where<R>(string sql)
        {
            return Change(Query<R>)(WithWhere)(sql);
        }
        public static Func<string, Select<P, IEnumerable<R>>> Where<P, R>(string sql)
        {
            return Change(Query<P, R>)(WithWhere)(sql);
        }
    }
}