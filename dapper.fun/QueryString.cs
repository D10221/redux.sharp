using System.Data;

namespace dapper.fun
{
    public struct QueryString
    {
        public QueryString(string text, int? commandTimeout = null, CommandType? commandType = null)
        {
            Text = text;
            CommandTimeout = commandTimeout;
            CommandType = commandType;
        }
        public string Text { get; private set; }
        public int? CommandTimeout { get; private set; }
        public CommandType? CommandType { get; private set; }
        public void Deconstruct(out string text, out int? commandTimeout, out CommandType? commandType)
        {
            text = Text;
            commandTimeout = CommandTimeout;
            commandType = CommandType;
        }
        public static implicit operator QueryString((string text, int? commandTimeout, CommandType? commandType) x)
        {
            return new QueryString { Text = x.text, CommandTimeout = x.commandTimeout, CommandType = x.commandType };
        }
        public static implicit operator QueryString((string text, int? commandTimeout) x)
        {
            return new QueryString { Text = x.text, CommandTimeout = x.commandTimeout };
        }
        public static implicit operator QueryString((string text, CommandType? commandType) x)
        {
            return new QueryString { Text = x.text, CommandType = x.commandType };
        }
        public static implicit operator (string text, int? commandTimeout, CommandType? commandType)(QueryString x)
        {
            return (text: x.Text, commandTimeout: x.CommandTimeout, commandType: x.CommandType);
        }
        public static implicit operator QueryString(string cmd)
        {
            return new QueryString(cmd);
        }
        public static implicit operator string(QueryString query)
        {
            return query.Text;
        }
    }
}