using System;


namespace list
{
    public class NoAccessException :Exception
    {
        public new string Message = "Ошибка доступа к файлу";
    }
}