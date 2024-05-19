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
            $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Directory.GetCurrentDirectory()};Extended Properties='text;HDR=yes;FMT=Delimited'";

        private const string PageNumber = "PageNumber";
        private const string FontSize = "FontSize";
        private const string FilePath = "FilePath";
        private  string _fileHeader = $"{PageNumber}, {FontSize}, {FilePath}";

        private readonly string _insertQuery =
            $"INSERT INTO [{LibraryFileName}] ({PageNumber}, {FontSize}, {FilePath}) VALUES (?,?,?)";

        private readonly string _selectAllQuery = $"SELECT * FROM [{LibraryFileName}]";

        private readonly CurrentUserSecurity _currentUserSecurity = new CurrentUserSecurity();

        public List<LibraryEntity> GetLibraryData()
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: new NoParams(), queryString: _selectAllQuery);
        }

        public void SetLibraryData(List<LibraryEntity> libraryItemEntities)
        {
            try
            {
                File.WriteAllText(LibraryFileName, $"{PageNumber}, {FontSize}, {FilePath}");
            }
            catch (Exception e)
            {
                throw new LibraryException();
            }
            
            _executeQuery(_setLibraryDataAction, input: libraryItemEntities,
                _insertQuery);
        }

        public void AddLibraryEntity(LibraryEntity libraryEntity)
        {
            List<LibraryEntity> libraryItemEntities = GetLibraryData();
            libraryItemEntities.Add(libraryEntity);
            SetLibraryData(libraryItemEntities);
        }

        public bool DeleteLibraryEntity(string filePath)
        {
            var libraryItemEntities = GetLibraryData();
            int initialSize = libraryItemEntities.Count; 
            for (var i = 0; i < libraryItemEntities.Count; i++)
            {
                var entity = libraryItemEntities[i];
                if (entity.FilePath == filePath)
                {
                    libraryItemEntities.RemoveAt(i);
                }
            }

            if (initialSize == libraryItemEntities.Count)
            {
                return false;
            }
            else
            {
                SetLibraryData(libraryItemEntities);
                return true;
            }
            
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
                    throw new NoAccessException();
                }
            }
            catch (NoAccessException e)
            {
                throw;
            }
            catch (Exception e)
            {
                File.WriteAllText(LibraryFileName, _fileHeader);
                throw new LibraryException();
            }
        }

        private List<LibraryEntity> _getLibraryDataAction(OleDbCommand command, NoParams input)
        {
            using (OleDbDataReader reader = command.ExecuteReader())
            {
                List<LibraryEntity> libraryItemEntities = new List<LibraryEntity>();

                while (reader.Read())
                {
                    int pageNumber = _getIntFromTable(reader, PageNumber);
                    float fontSize = _getFloatFromTable(reader, FontSize);
                    string filePath = reader[FilePath].ToString().Trim();

                    LibraryEntity libraryEntity =
                        new LibraryEntity(pageNumber, fontSize, filePath);
                    libraryItemEntities.Add(libraryEntity);
                }

                return libraryItemEntities;
            }
        }

        private NoParams _setLibraryDataAction(OleDbCommand command,
            List<LibraryEntity> libraryItemEntities)
        {
            foreach (var libraryItemEntity in libraryItemEntities)
            {
                command.Parameters.AddWithValue("?", libraryItemEntity.PageNumber);
                command.Parameters.AddWithValue("?", libraryItemEntity.FontSize);
                command.Parameters.AddWithValue("?", libraryItemEntity.FilePath);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            return new NoParams();
        }

        private void _createFileIfNotExists()
        {
            if (File.Exists(LibraryFileName))
            {
                if (_currentUserSecurity.HasAccess(new FileInfo(LibraryFileName), FileSystemRights.Read))
                {
                    if (!File.ReadAllText(LibraryFileName).TrimStart().StartsWith(_fileHeader))
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
                else
                {
                    throw new NoAccessException();
                }
            }
            else
            {
                try
                {
                    if (File.GetAttributes(LibraryFileName) == FileAttributes.Directory)
                    {
                        throw new NoAccessException();
                    }
                    else
                    {
                        File.WriteAllText(LibraryFileName, _fileHeader);
                    }

                }
                catch (FileNotFoundException)
                {
                    File.WriteAllText(LibraryFileName, _fileHeader);
                }
                catch (NoAccessException)
                {
                    throw;
                }
                catch (Exception e)
                {
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