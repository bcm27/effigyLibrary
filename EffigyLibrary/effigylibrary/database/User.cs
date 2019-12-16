using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubBudgeting {
   class User {
      // Username and club information
      private string username, clubId, clubName;

      // SQL class instance
      SQL sql = SQL.Instance;

      /// Static instance of this class
      private static User UsInstance;

      /// <summary>
      /// To prevent access by more than one thread
      /// </summary>
      private static Object fmLock = typeof(User);

      /// <summary>
      /// - Private constructor so no one else can create one.
      /// </summary>
      private User() { }

      /// <summary>
      /// Initizlises variables before login.
      /// </summary>
      private void Initilize() { }

      /*------------------
       * Get/Set Methods *
      -------------------*/

      /// <summary>
      /// Get/set methods for clubId
      /// </summary>
      public string CLUB_ID {
         get { return clubId; }
         set { clubId = value; }
      }

      /// <summary>
      /// Get/set methods for clubName
      /// </summary>
      public string CLUB_NAME {
         get { return clubName; }
         set { clubName = value; }
      }

      /// <summary>
      /// Management for static instance of this class
      /// </summaryUser
      public static User Instance {
         get {
            lock (fmLock) {
               if (UsInstance == null) // If no instance exists, create one
               {
                  UsInstance = new User();
                  UsInstance.Initilize();
               }
               return UsInstance; // Return the only instance of this class
            }
         }
      }


      /*
       * 
       * Functions
       * 
       */

      /// <summary>
      /// Check if the user/password exists in the database
      /// </summary>
      public bool LogIn(string name, string pass) {
         try {
            clubId = sql.logIn(name, pass);
            username = name;
            clubName = sql.getClubName(new Parameters(clubId));
            return true;
         } catch {
            return false;
         }
      }
   }
}
