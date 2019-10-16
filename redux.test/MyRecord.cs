using System;
using ValueOf;

namespace redux.test
{
    ///<summary>
    /// Immutable record
    /// Tuple Convertible
    ///</summary>
    public partial class MyRecord : ValueOf<(string name, Exception error, bool busy, bool success), MyRecord>
    {
        public MyRecord()
        {
            this.Value = (name: "", error: null, busy: false, success: false);
        }
        
        [Ignore] // ValueOfJsonConverter: ignore this method
        public static MyRecord From(string name, Exception error, bool busy, bool success)
        {
            return MyRecord.From((name, error, busy, success));
        }
        
        public string Name => this.Value.name;
        public Exception Error => this.Value.error;
        public bool Busy => this.Value.busy;
        public bool Success => this.Value.success;

        public void Deconstruct(out string name, out Exception error, out bool busy, out bool success)
        {
            name = this.Value.name;
            error = this.Value.error;
            busy = this.Value.busy;
            success = this.Value.success;
        }

        public static implicit operator MyRecord((string name, Exception error, bool busy, bool success) x)
        {
            return MyRecord.From(x);
        }

        public static implicit operator (string name, Exception error, bool busy, bool success)(MyRecord x)
        {
            return x.Value;
        }
    }
}