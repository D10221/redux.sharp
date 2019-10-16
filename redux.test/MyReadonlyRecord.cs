using Redux;
using System;

namespace redux.test
{
    public partial struct MyReadonlyRecord
    {
        public static MyReadonlyRecord From((string name, Exception error, bool busy, bool success) value)
        {
            var (name, error, busy, success) = value;
            return new MyReadonlyRecord(name, error, busy, success);
        }
        public MyReadonlyRecord(string name = "", Exception error = null, bool busy = false, bool success = false)
        {
            Name = name;
            Error = error;
            Busy = busy;
            Success = success;
        }
        public string Name { get; }
        public Exception Error { get; private set; }
        public bool Busy { get; private set; }
        public bool Success { get; private set; }

        public void Deconstruct(out string name, out Exception error, out bool busy, out bool success)
        {
            name = Name;
            error = Error;
            busy = Busy;
            success = Success;
        }
        public static implicit operator MyReadonlyRecord((string name, Exception error, bool busy, bool success) x) =>
            new MyReadonlyRecord(x.name, x.error, x.busy, x.success);
    }
}