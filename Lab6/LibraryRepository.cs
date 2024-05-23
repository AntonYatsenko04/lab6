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

        private const string Id = "Id";
        private const string DateTime = "DateTime";
        private const string PageNumber = "PageNumber";
        private const string FontSize = "FontSize";
        private const string FilePath = "FilePath";
        private  string _fileHeader = $"{Id}, {DateTime}, {PageNumber}, {FontSize}, {FilePath}";

        private readonly string _insertQuery =
            $"INSERT INTO [{LibraryFileName}] ([{Id}], [{DateTime}], [{PageNumber}], [{FontSize}], [{FilePath}]) VALUES (?,?,?,?,?)";
        
        private readonly string _selectAllQuery = $"SELECT * FROM [{LibraryFileName}]";
        private readonly string _selectByPageNumberQuery = $"SELECT * FROM [{LibraryFileName}] WHERE [{PageNumber}]>? AND [{PageNumber}]<?";
        private readonly string _selectByFontSizeQuery = $"SELECT * FROM [{LibraryFileName}] WHERE [{FontSize}]>? AND [{FontSize}]<?";
        private readonly string _selectByDateTimeQuery = $"SELECT * FROM [{LibraryFileName}] WHERE [{DateTime}]>? AND [{DateTime}]<?";
        private readonly string _selectByNameQuery = $"SELECT * FROM [{LibraryFileName}] WHERE [{FilePath}]=?";

        private readonly CurrentUserSecurity _currentUserSecurity = new CurrentUserSecurity();

        public List<LibraryEntity> GetLibraryData()
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: new List<Object>(), queryString: _selectAllQuery);
        }
        
        public List<LibraryEntity> GetLibraryEntityFilteredByDateTime(DateTime? min,DateTime? max)
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: (List<Object?>)[min,max], queryString: _selectByDateTimeQuery);
        }

        public List<LibraryEntity> GetLibraryEntityFilteredByPageNumber(int? min, int? max)
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: (List<Object?>)[min,max], queryString: _selectByPageNumberQuery);
        }
        
        public List<LibraryEntity> GetLibraryEntityFilteredByFontSize(float? min, float? max)
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: (List<Object?>)[(min ?? float.MinValue).ToString("#"),
                (max ?? float.MaxValue).ToString("#")], queryString: _selectByFontSizeQuery);
        }
        
        public List<LibraryEntity> GetLibraryEntityFilteredByName(string name)
        {
            _createFileIfNotExists();
            return _executeQuery(query: _getLibraryDataAction, input: (List<Object?>)[name], queryString: _selectByNameQuery);
        }
        
        public void SetLibraryData(List<LibraryEntity> libraryItemEntities)
        {
            try
            {
                File.WriteAllText(LibraryFileName, _fileHeader);
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
                Console.WriteLine(e);
                throw new LibraryException();
            }
        }

        private List<LibraryEntity> _getLibraryDataAction(OleDbCommand command, List<Object?> input)
        {
            foreach (Object o in input)
            {
                command.Parameters.AddWithValue("?", o);
            }

            using (OleDbDataReader reader = command.ExecuteReader())
            {
                List<LibraryEntity> libraryItemEntities = new List<LibraryEntity>();

                while (reader.Read())
                {
                    int id = _getIntFromTable(reader, Id);
                    if (!System.DateTime.TryParse(reader[DateTime].ToString(), out var dateTime))
                    {
                        throw new LibraryException();
                    }
                    
                    int pageNumber = _getIntFromTable(reader, PageNumber);
                    float fontSize = _getFloatFromTable(reader, FontSize);
                    string filePath = reader[FilePath].ToString().Trim();

                    LibraryEntity libraryEntity =
                        new LibraryEntity(id,dateTime,pageNumber, fontSize, filePath);
                    libraryItemEntities.Add(libraryEntity);
                }

                return libraryItemEntities;
            }
        }

        private int _getMaxId()
        {
            var lib = GetLibraryData();
            int id = 0;
            foreach (LibraryEntity libraryEntity in lib)
            {
                if (libraryEntity.Id > id)
                {
                    id = libraryEntity.Id??0;
                }
                
            }

            return id;
        }

        private NoParams _setLibraryDataAction(OleDbCommand command,
            List<LibraryEntity> libraryItemEntities)
        {
            
            foreach (var libraryItemEntity in libraryItemEntities)
            {
                System.DateTime currentDateTime = libraryItemEntity.DateTime?? System.DateTime.Now;
                int id =libraryItemEntity.Id?? _getMaxId()+1;
                
                command.Parameters.AddWithValue("?", id);
                command.Parameters.AddWithValue("?", currentDateTime.ToString());
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