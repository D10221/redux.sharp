using System;

namespace redux.test
{
    ///<summary>
    /// Immutable record
    /// Tuple Convertible
    ///</summary>
    public partial struct MyRecord : IEquatable<MyRecord>
    {
        public string Name { get; }
        public Exception Error { get; }
        public bool Busy { get; }
        public bool Success { get; }        
        private readonly object[] _values;

        public MyRecord(string name = "", Exception error = null, bool busy = false, bool success = false)
        {
            Name = name;
            Error = error;
            Busy = busy;
            Success = success;
            _values = new object[] { Name, Error, Busy, Success };
        }
        
        public static MyRecord From((string name, Exception error, bool busy, bool success) value)
        {
            var (name, error, busy, success) = value;
            return new MyRecord(name, error, busy, success);
        }
        
        public void Deconstruct(out string name, out Exception error, out bool busy, out bool success)
        {
            name = Name;
            error = Error;
            busy = Busy;
            success = Success;
        }

        public bool Equals(MyRecord other)
        {
            // if (other == null) return false;
            // if (!(other is MyRecord)) return false;
            foreach (var value in _values)
            {
                foreach (var o in ((MyRecord)other)._values)
                {
                    if (!Equals(value, o)) return false;
                }
            }
            return true;
        }

        public static implicit operator MyRecord((string name, Exception error, bool busy, bool success) x)
        {
            return new MyRecord(x.name, x.error, x.busy, x.success);
        }

        public static implicit operator (string name, Exception error, bool busy, bool success)(MyRecord x)
        {
            return (name: x.Name, error: x.Error, busy: x.Busy, success: x.Success);
        }

    }
}