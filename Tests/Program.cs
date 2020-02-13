using Core.Utilities.Security.FileOperations;
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSecurityOperations.EncryptDBConnectionFile();
            FileSecurityOperations.DecryptDBConnectionFile();
        }
    }
}
