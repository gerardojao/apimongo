using System;

namespace ApiBase.Utils
{
    public class UsersConfig
    {
        // Function to remove . from gmail
        public static String CheckGmail(string email)
        {
            var emailverify = "";
            var auxemail = email.Split("@");
            if (auxemail[1] == "gmail.com")
            {
                var newemail = auxemail[0].Split(".");
                if (newemail.Length != 0)
                {
                    auxemail[0] = string.Join("", newemail);
                }
                emailverify = string.Join("@", auxemail);
            }
            else
            {
                return email;
            }
            return emailverify;
        }

        internal static object VerifyGmail(object email)
        {
            throw new NotImplementedException();
        }
    }
}