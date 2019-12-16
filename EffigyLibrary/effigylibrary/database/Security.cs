using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClubBudgeting.Source {
   class Security {
      /// Static instance of this class
      private static Security SecInstance;

      /// To prevent access by more than one thread
      private static Object sLock = typeof(User);

      // Have the ability to run SQL 
      private static SQL sql = SQL.Instance;

      /// <summary>
      /// Management for static instance of this class
      /// </summary>
      public static Security Instance {
         get {
            lock (sLock) {
               // If no instance exists, create one
               if (SecInstance == null) {
                  SecInstance = new Security();
               }
               return SecInstance; // Return the only instance of this class
            }
         }
      }

      /// <summary>
      /// Take the string as a parameter and return the hashed version of it
      /// </summary>
      public string hash(string tohash) {
         var bytes = new UTF8Encoding().GetBytes(tohash);
         var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash
          (bytes);

         return Convert.ToBase64String(hashBytes);
      }

      /// <summary>
      /// Return the result of the member search for the database (verify 
      /// password)
      /// </summary>
      public bool varifyPassword(string userName, string hashPass) {
         return sql.checkPass(userName, hashPass);
      }

   }
}
