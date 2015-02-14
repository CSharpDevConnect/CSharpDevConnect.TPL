using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Threading;

namespace CSharpDevConnect.TPL.Exercises.Repository
{
    public class SQLiteDataStore : IDisposable
    {
        private readonly string _filePath;

        private readonly SQLiteConnection _sqlConnection;

        private readonly UserRepository _userRepository;

        private readonly CourseRepository _courseRepository;

        private readonly EnrollmentRepository _enrollmentRepository;

        public SQLiteDataStore(string filePath)
        {
            _filePath = filePath;

            string connectionString = GenerateConnectionString(_filePath);
            _sqlConnection = new SQLiteConnection(connectionString);

            _userRepository = new UserRepository(_sqlConnection);

            _courseRepository = new CourseRepository(_sqlConnection);

            _enrollmentRepository = new EnrollmentRepository(_sqlConnection, _userRepository, _courseRepository);
        }

        public UserRepository UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public CourseRepository CourseRepository
        {
            get
            {
                return _courseRepository;
            }
        }

        public EnrollmentRepository EnrollmentRepository
        {
            get
            {
                return _enrollmentRepository;
            }
        }

        public void InitializeDb()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            Console.WriteLine("{0}\tOpening db at: {1}", Thread.CurrentThread.ManagedThreadId, _filePath);
            _sqlConnection.Open();

            string createScript = ReadScriptFileIntoString();

            SQLiteCommand command = new SQLiteCommand(createScript, _sqlConnection);
            command.ExecuteNonQuery();

        }

        private static string GenerateConnectionString(string filename)
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = filename;
            sb.Version = 3;
            sb.PageSize = 16384;
            sb.CacheSize = 65536;
            sb.JournalMode = SQLiteJournalModeEnum.Wal;
            sb.Pooling = false;
            sb.LegacyFormat = false;
            sb.DefaultTimeout = 500;
            sb.ForeignKeys = true;
            sb.FailIfMissing = false;
            sb.SyncMode = SynchronizationModes.Off;

            return sb.ToString();
        }

        private static string ReadScriptFileIntoString()
        {
            const string SCRIPT_FILE = "CSharpDevConnect.TPL.Exercises.scripts.schema.sql";

            string result = null;
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(SCRIPT_FILE))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Unable to open load script stream for '{0}'.",
                        SCRIPT_FILE));
                }
            }

            if (String.IsNullOrWhiteSpace(result))
            {
                throw new InvalidOperationException(String.Format("Unable to retrieve '{0}'. Did you change the namespace?", SCRIPT_FILE));
            }

            return result;
        }

        public void Dispose()
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}
