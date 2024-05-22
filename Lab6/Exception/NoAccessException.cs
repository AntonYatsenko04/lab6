using System;


namespace list
{
    public class NoAccessException :Exception
    {
       
        public override string Message
        {
            get
            {
                return "Ошибка доступа к файлу";
            }
        } 
    }
}