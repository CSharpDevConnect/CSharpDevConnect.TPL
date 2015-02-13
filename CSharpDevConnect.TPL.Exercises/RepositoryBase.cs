using System.Data.SQLite;

namespace CSharpDevConnect.TPL.Exercises
{

    public abstract class RepositoryBase
    {
        private readonly SQLiteConnection _sqlConnection;

        protected RepositoryBase(SQLiteConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        protected SQLiteConnection SqlConnection
        {
            get
            {
                return _sqlConnection;
            }
        }

        protected SQLiteParameter CreateParameter(SQLiteCommand command, string parameterName, object value)
        {
            SQLiteParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }
    }
}