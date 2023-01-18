using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        /// <summary>
        /// Results
        /// </summary>
        /// <param name="success"></param>
        /// <param name="infoMessage">Kullanıcıya gösterilecek olan uyarı mesajı.</param>
        /// <param name="errorMessage">Log kayıtlarına eklenmesi gereken hata mesajı.</param>
        /// <param name="focus">focuslanacak nesnelerin Jquery selector formatta seçicileri Örn: focus="#UserEmail" </param>

        public Result(bool success, string errorMessage, string focus, string infoMessage) : this(success)
        {
            InfoMessage = infoMessage;
            ErrorMessage = errorMessage;
            Focus = focus;
        }
        public Result(bool success, string errorMessage, string infoMessage) : this(success)
        {
            InfoMessage = infoMessage;
            ErrorMessage = errorMessage;
        }

        // :this(success) Message dan sonra tekrar Success i yazmak yerine alttaki construction u çağırır.

        public Result(bool success)
        {
            Success = success;
        }



        public bool Success { get; }
        public string InfoMessage { get; }
        public string ErrorMessage { get; }
        public string Focus { get; }
    }
}
