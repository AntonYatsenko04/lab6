using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Security.AccessControl;
using Lab6;

namespace list
{
    public class LibraryRepository
    {
        private const string LibraryFileName = "library.txt";

        private readonly string _connectionString =
            $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={Directory.GetCurrentDirectory()};Extended Properties='text;HDR=yes;FMT=Delimited'";

        private const string PageNumber = "PageNumber";
        private const string FontSize = "FontSize";
        private const string BufferSize = "BufferSize";
        private const string FilePath = "FilePath";
        private  string _fileHeader = $"{PageNumber}, {FontSize}, {BufferSize}, {FilePath}";

        private readonly string _insertQuery =
            $"INSERT INTO [{LibraryFileName}] ({PageNumber}, {FontSize}, {BufferSize}, {FilePath}) VALUES (?,?,?,?)";

        private readonly string _selectAllQuery = $"SELECT * FROM [{LibraryFileName}]";

        private readonly CurrentUserSecurity _currentUserSecurity = new CurrentUserSecurity();

        public List<LibraryItemEntity> GetLibraryData()
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: new NoParams(), queryString: _selectAllQuery);
        }

        public void SetLibraryData(List<LibraryItemEntity> libraryItemEntities)
        {
            
            try
            {
                File.WriteAllText(LibraryFileName, $"{PageNumber}, {FontSize}, {BufferSize}, {FilePath}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new LibraryException();
            }
            
            _executeQuery(_setLibraryDataAction, input: libraryItemEntities,
                _insertQuery);
        }

        public void AddLibraryEntity(LibraryItemEntity libraryItemEntity)
        {
            bool isNewEntity = true;
            List<LibraryItemEntity> libraryItemEntities = GetLibraryData();
            foreach (LibraryItemEntity itemEntity in libraryItemEntities)
            {
                if (itemEntity.FilePath == libraryItemEntity.FilePath)
                {
                    itemEntity.BufferSize = libraryItemEntity.BufferSize;
                    itemEntity.PageNumber = libraryItemEntity.PageNumber;
                    itemEntity.FontSize = libraryItemEntity.FontSize;
                    isNewEntity = false;
                    break;
                }
            }

            if (isNewEntity)
            {
                libraryItemEntities.Add(libraryItemEntity);
            }

            SetLibraryData(libraryItemEntities);
        }

        public void DeleteLibraryEntity(string filePath)
        {
            var libraryItemEntities = GetLibraryData();

            for (var i = 0; i < libraryItemEntities.Count; i++)
            {
                var entity = libraryItemEntities[i];
                if (entity.FilePath == filePath)
                {
                    libraryItemEntities.RemoveAt(i);
                    break;
                }
            }

            SetLibraryData(libraryItemEntities);
        }

        private TOut _executeQuery<TIn, TOut>(Func<OleDbCommand, TIn, TOut> query, TIn input, string queryString)
        {
            try
            {
                if (_currentUserSecurity.HasAccess(new FileInfo(LibraryFileName), FileSystemRights.Modify))
                {
                    using (OleDbConnection connection = new OleDbConnection(_connectionString))
                    {
                        connection.Open();
                        using (OleDbCommand command = new OleDbCommand(queryString, connection))
                        {
                            return query(command, input);
                        }
                        
                    }
                }
                else
                {
                    Console.WriteLine("no access");
                    throw new NoAccessException();
                }
            }
            catch (NoAccessException e)
            {
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new LibraryException();
            }
        }

        private List<LibraryItemEntity> _getLibraryDataAction(OleDbCommand command, NoParams input)
        {
            using (OleDbDataReader reader = command.ExecuteReader())
            {
                List<LibraryItemEntity> libraryItemEntities = new List<LibraryItemEntity>();

                while (reader.Read())
                {
                    int pageNumber = _getIntFromTable(reader, PageNumber);
                    float fontSize = _getFloatFromTable(reader, FontSize);
                    int bufferSize = _getIntFromTable(reader, BufferSize);
                    string filePath = reader[FilePath].ToString().Trim();

                    LibraryItemEntity libraryItemEntity =
                        new LibraryItemEntity(pageNumber, fontSize, bufferSize, filePath);
                    libraryItemEntities.Add(libraryItemEntity);
                }

                return libraryItemEntities;
            }
        }

        private NoParams _setLibraryDataAction(OleDbCommand command,
            List<LibraryItemEntity> libraryItemEntities)
        {
            foreach (var libraryItemEntity in libraryItemEntities)
            {
                command.Parameters.AddWithValue("?", libraryItemEntity.PageNumber);
                command.Parameters.AddWithValue("?", libraryItemEntity.FontSize);
                command.Parameters.AddWithValue("?", libraryItemEntity.BufferSize);
                command.Parameters.AddWithValue("?", libraryItemEntity.FilePath);
                command.ExecuteNonQuery();
            }

            return new NoParams();
        }

        private void _createFileIfNotExists()
        {
            if (!File.Exists(LibraryFileName)||!File.ReadAllText(LibraryFileName).TrimStart().StartsWith(_fileHeader))
            {
                try
                {
                    File.WriteAllText(LibraryFileName, _fileHeader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new LibraryException();
                }
            }
        }

        private int _getIntFromTable(OleDbDataReader reader, string parameterName)
        {
            int result;
            if (int.TryParse(reader[parameterName].ToString().Trim(), out result))
            {
                return result;
            }
            else
            {
                throw new FileParseException();
            }
        }

        private float _getFloatFromTable(OleDbDataReader reader, string parameterName)
        {
            float result;
            if (float.TryParse(reader[parameterName].ToString().Trim(), out result))
            {
                return result;
            }
            else
            {
                throw new FileParseException();
            }
        }
    }
}