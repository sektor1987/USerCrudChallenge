using System;
using System.Collections.Generic;
using System.Text;

namespace UserCrudChallenge.CrossCutting.Common
{
    public interface IManagerEncryptDecrypt
    {
        string Encrypt(string EncryptPassword);
        string Decrypt(string DecryptPässword);
    }
}
