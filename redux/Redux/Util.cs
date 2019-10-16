using System;
using System.Collections.Generic;
using System.Linq;

namespace Redux
{
    public class Utils
    {
        public static Func<T, T> Compose<T>(IEnumerable<Func<T, T>> funcs)
        {
            return funcs.Aggregate((a, b) => x => a(b(x)));
        }
        /// <summary>
        ///  Inline Factory Signature helper
        /// </summary>    
        public static Func<A, T> Fty<T, A>(Func<A, T> f) { return a => f(a); }
    }
}