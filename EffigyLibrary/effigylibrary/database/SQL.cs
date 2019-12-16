using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections;
using ClubBudgeting.Source;

namespace ClubBudgeting {
   class SQL {
      // Global variables for connecting to database
      string statement;
      MySqlConnection SQLCONN;
      MySqlCommand cmd;
      MySqlDataReader Reader;

      // String array to hold list of club names
      private ArrayList clubArray = new ArrayList();

      // Static instance of this class
      private static SQL SQLInstance;

      // To prevent access by more than one thread. This is the specific lock
      // belonging to the FileManager Class object.
      private static Object fmLock = typeof(User);

      /// Security for passwords
      private Security sec = Security.Instance;

      /// Open file locations to save
      private FolderBrowserDialog fbd = new FolderBrowserDialog();

      /// <summary>
      /// Private constructor so no one else can create one
      /// </summary>
      private SQL() { }

      /// <summary>
      /// Start connections
      /// </summary>
      private void Initilize() {
         SQLCONN = new MySqlConnection("server=localhost;"
            + "user=root;"
            + "database=ClubSchema2;"
            + "port=3306;"
            + "password=potato123");
         SQLCONN.Open();

         fillClubArray();
      }

      /// <summary>
      /// Close the database connection
      /// </summary>
      public void close() {
         SQLCONN.Close();
      }

      /// <summary>
      /// Management for static instance of this class
      /// </summaryUser
      public static SQL Instance {
         get {
            lock (fmLock) {
               // If no instance exists, create one
               if (SQLInstance == null) {
                  SQLInstance = new SQL();
                  SQLInstance.Initilize();
               }
               return SQLInstance; // Return the only instance of this class
            }
         }
      }

      /*
       * ---------
       * Functions
       * ---------
       */

      /// <summary>
      /// Add sterilized params to the command
      /// This takes the 'Listing', supplied by the function that calls 
      /// addParams and the actual information and formats it into
      /// the SQL call
      /// </summary>
      private MySqlCommand addParams
         (MySqlCommand cmd, string[] listing, ArrayList prams) {
         for (int i = 0; i < listing.Length; i++)
            cmd.Parameters.AddWithValue(listing[i], prams[i]);

         return cmd;
      }

      /// <summary>
      /// If user/password combo exists, it is a valid user
      /// </summary>
      public bool checkPass(string user, string pass) {
         Parameters pList = new Parameters(user, sec.hash(pass));
         string[] listing = { "@user", "@pass" };

         statement = "SELECT userName FROM Member"
          + " WHERE userName = @user AND pass = @pass;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            Reader = addParams(cmd, listing, pList.PARAM_LIST).ExecuteReader();
            Reader.Read();
         } catch {
            return false;
         } finally {
            Reader.Close();
         }
         return true;
      }

      /// <summary>
      /// Login function, if error exists, throw exception
      /// </summary>
      public string logIn(string user, string pass) {
         string ret = "";
         string[] listing = { "@user", "@pass" };

         if (checkPass(user, sec.hash(pass))) {
            Parameters pList = new Parameters(user, sec.hash(pass));

            statement = "SELECT adminRight, c.id FROM Member"
             + " JOIN Club c ON c.id = clubId"
             + " WHERE userName = @user AND pass = @pass;";
            cmd = new MySqlCommand(statement, SQLCONN);
            cmd.Prepare();

            try {
               Reader =
                addParams(cmd, listing, pList.PARAM_LIST).ExecuteReader();
               Reader.Read();
               if (Reader[0].ToString() == "False")
                  ret = Reader[1].ToString();
               else
                  ret = "0";
            } catch (MySql.Data.MySqlClient.MySqlException ex) {
               throw ex;
            } finally {
               Reader.Close();
            }
         }
         return ret;
      }

      /// <summary>
      /// Add a pdf receipt
      /// {@transId, @file, @ext}
      /// </summary>
      public bool addPDFReceipt(Parameters pLists) {
         string[] listing = { "@transId", "@file", "@ext" };

         statement = "INSERT INTO Receipt VALUES "
          + "( NULL, @transId, @file, @ext)";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Retrieves PDF from database
      /// {@transId}
      /// </summary>
      public bool getPDF(Parameters pLists) {
         string[] listing = { "@transId" };

         statement = "SELECT invoice, fileExtention FROM Receipt "
            + "WHERE transId = @transId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            Reader =
             addParams(cmd, listing, pLists.PARAM_LIST).ExecuteReader();
            Reader.Read();

            // Choose and set path
            if (fbd.ShowDialog() == DialogResult.OK) {
               System.IO.File.WriteAllBytes(fbd.SelectedPath + "\\invoice"
                + Reader[1].ToString(), (byte[])Reader[0]);
            }
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         } finally {
            Reader.Close();
         }
         return true;
      }

      /// <summary>
      /// Add transaction to the database
      /// {@date, @price, @DESC, @club}
      /// </summary>
      public bool addTransaction(Parameters pLists) {
         double balance = double.Parse(getCurrClubBalance(new Parameters
            (pLists.PARAM_LIST[3])));
         double price = double.Parse(pLists.PARAM_LIST[1].ToString());

         if (0 <= balance - price) {
            string[] listing = { "@Date", "@price", "@desc", "@club" };

            statement = "INSERT INTO Transactions VALUES "
             + "(null, @Date, @price, @desc, @club, false);";
            cmd = new MySqlCommand(statement, SQLCONN);
            cmd.Prepare();

            try {
               addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
               updateBudget(
                new Parameters(pLists.PARAM_LIST[3], balance - price));
               return true;
            } catch (MySql.Data.MySqlClient.MySqlException ex) {
               MessageBox.Show("Error " + ex.Number + " has occurred: " +
                ex.Message, "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
               return false;
            }
         }
         return false;
      }

      /// <summary>
      /// Return the balance for a club whose id id passed in
      /// </summary>
      public string getCurrClubBalance(Parameters pList) {
         string[] listings = { "@clubId" };
         string temp;

         statement = "SELECT balance, max(termId) FROM Budget "
          + "WHERE clubId = @clubId GROUP BY termId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            Reader =
             addParams(cmd, listings, pList.PARAM_LIST).ExecuteReader();
            Reader.Read();
            temp = Reader[0].ToString();
         } catch {
            return null;
         } finally {
            Reader.Close();
         }
         return temp;
      }

      /// <summary>
      /// Add user to the DB - hash password first
      /// {@user, @first, @last, @pass}
      /// </summary>
      public bool addUser(Parameters pLists) {
         string[] listing = { "@club","@admin", "@user", "@first", "@last",
          "@pass" };

         statement = "INSERT INTO Member VALUES "
          + "(null, @club, @admin, @user, @first, @last, @pass);";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Add club to the database
      /// </summary>
      /// <param name="pLists"></param>
      /// <returns></returns>
      public bool addClub(Parameters pLists) {
         string[] listing = { "@club", "@desc" };

         statement = "INSERT INTO Club VALUES "
            + "(null, @club, @desc);";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " + 
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Create new user to add to the database
      /// {@club, @admin, @user, @first, @last, @pass}
      /// </summary>
      public bool addMember(Parameters pLists) {
         string[] listing = { "@club", "@admin", "@user", "@first",
          "@last", "@pass" };

         statement = "INSERT INTO Club VALUES "
          + "(null, @club, @admin, @user, @first, @last, @pass);";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message + ex.StackTrace, "Error", MessageBoxButtons.OK, 
             MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Update budget with specific parameters
      /// </summary>
      public bool updateBudget(Parameters pList) {
         string[] listing = { "@clubId", "@budget" };

         statement = "UPDATE Budget SET balance = @budget WHERE id = @clubId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pList.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Add budget proposal
      /// {@clubId, @file, @ext}
      /// </summary>
      public bool AddBudgetProp(Parameters pLists) {
         string[] listing = { "@clubId", "@file", "@ext" };

         statement = "INSERT INTO BudgetProposal VALUES (null,"
          + getCurrSemesterId() + ",@clubId, @file, @ext);";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Retrieve budget proposal from database
      /// </summary>
      public bool getBudgetProp(Parameters pLists) {
         string[] listing = { "@clubId" };

         // the most recent proposal submitted
         statement = "SELECT proposal, fileExtention, MAX(termId) FROM "
          + "BudgetProposal WHERE clubId = @clubId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();
         Reader.Close();

         try {
            Reader =
             addParams(cmd, listing, pLists.PARAM_LIST).ExecuteReader();
            Reader.Read();

            // Choose and set path
            if (fbd.ShowDialog() == DialogResult.OK) {
               System.IO.File.WriteAllBytes(fbd.SelectedPath
                + "\\BudgetProposal"
                + Reader[1].ToString(), (byte[])Reader[0]);
            }
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         } finally {
            Reader.Close();
         }
         return true;
      }

      /// <summary>
      /// Return current semester Id
      /// </summary>
      public string getCurrSemesterId() {
         string tempId;

         statement = "SELECT max(id) FROM Term";
         cmd = new MySqlCommand(statement, SQLCONN);

         try {
            Reader = cmd.ExecuteReader();
            Reader.Read();
            tempId = Reader[0].ToString();
         } catch {
            return null;
         } finally {
            Reader.Close();
         }
         return tempId;
      }

      /// <summary>
      /// Returns the budget for the clubId passed as a parameter
      /// </summary>
      public string getCurrClubBudg(Parameters pList) {
         string[] listings = { "@clubId" };
         string tempBudget;

         statement = "SELECT balance, max(termId) FROM Budget "
          + "WHERE clubId = @clubId GROUP BY termId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            Reader =
             addParams(cmd, listings, pList.PARAM_LIST).ExecuteReader();
            Reader.Read();
            tempBudget = Reader[0].ToString();
         } catch {
            return null;
         } finally {
            Reader.Close();
         }
         return tempBudget;
      }

      /// <summary>
      /// Get an arraylist of the specified club's transactions
      /// </summary>
      public ArrayList getTransactions(Parameters pList) {
         ArrayList transactions = new ArrayList();
         string[] listings = { "@club" };

         statement = "SELECT * FROM Transactions WHERE clubId = @club;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            Reader = addParams(cmd, listings, pList.PARAM_LIST).ExecuteReader();

            // read in and store each transaction's information 
            while (Reader.Read()) {
               ArrayList partialTransaction = new ArrayList();

               int loop = 0;
               while (loop <= 5)
                  partialTransaction.Add(Reader[loop++].ToString());

               // create collection of information
               transactions.Add(partialTransaction);
            }
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message,  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         } finally {
            Reader.Close();
         }
         return transactions;
      }

      /// <summary>
      /// Fill the club arraylist with club names
      /// </summary>
      public ArrayList fillClubArray() {
         clubArray = new ArrayList();

         statement = "SELECT id, name FROM Club";
         cmd = new MySqlCommand(statement, SQLCONN);

         try {
            Reader = cmd.ExecuteReader();

            // Read in and store each club's name in the arraylist
            while (Reader.Read()) {
               ArrayList clubInfo = new ArrayList();
               clubInfo.Add(Reader[0].ToString());
               clubInfo.Add(Reader[1].ToString());
               clubArray.Add(clubInfo);
            }
         } catch {
            MessageBox.Show("Failed to fill the club array",
             "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         } finally {
            Reader.Close();
         }
         return clubArray;
      }

      /// <summary>
      /// Return a copy of the list of club names in the database
      /// </summary>
      public ArrayList CLUB_LIST {
         get { return clubArray; }
      }

      /// <summary>
      /// Add a budget to the specified club
      /// {@clubId, @termId, @Budget, @bal, @debt}
      /// </summary>
      public bool addBudget(Parameters pLists) {
         string[] listing = { "@clubId", "@termId", "@Budget", "@bal",
          "@debt" };

         statement = "INSERT INTO Budget VALUES "
          + "( NULL, @clubId, @termId, @Budget, @bal, @debt );";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            addParams(cmd, listing, pLists.PARAM_LIST).ExecuteNonQuery();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message + ex.StackTrace, 
             "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
         }
         return true;
      }

      /// <summary>
      /// Get the id of the most recent club added
      /// </summary>
      public string getRecClubId() {
         statement = "SELECT max(id) FROM Club";
         cmd = new MySqlCommand(statement, SQLCONN);

         try {
            Reader = cmd.ExecuteReader();
            Reader.Read();
            return Reader[0].ToString();
         } catch {
            throw new Exception("getRecClubId failed");
         } finally {
            Reader.Close();
         }
      }

      /// <summary>
      /// Get the total debt of the specified club
      /// </summary>
      public string getDebt(Parameters pList) {
         string[] listings = { "@clubId" };
         statement = "SELECT debt, max(termId) FROM Budget "
          + "WHERE clubId = @clubId;";

         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         string temp;

         try {
            Reader =
             addParams(cmd, listings, pList.PARAM_LIST).ExecuteReader();
            Reader.Read();
            temp = Reader[0].ToString();
         } catch {
            return null;
         } finally {
            Reader.Close();
         }
         return temp;
      }

      /// <summary>
      /// Return the clubName when searched for by the clubID
      /// </summary>
      /// <param name="pList">@clubId</param>
      /// <returns></returns>
      public string getClub(Parameters pList) {
         string[] listings = { "@clubId" };

         statement = "SELECT id FROM Member WHERE clubId = @clubId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         string temp;

         try {
            Reader =
             addParams(cmd, listings, pList.PARAM_LIST).ExecuteReader();
            Reader.Read();
            temp = Reader[0].ToString();
         } catch {
            return null;
         } finally {
            Reader.Close();
         }
         return temp;
      }

      /// <summary>
      /// Retrieve the club name from an id
      /// </summary>
      public string getClubName(Parameters pList) {
         if (pList.PARAM_LIST[0].Equals("0"))
            return "admin";

         string[] listings = { "@clubId" };
         string tempName;

         statement = "SELECT name FROM Club WHERE id = @clubId;";
         cmd = new MySqlCommand(statement, SQLCONN);
         cmd.Prepare();

         try {
            Reader =
             addParams(cmd, listings, pList.PARAM_LIST).ExecuteReader();
            Reader.Read();
            tempName = Reader[0].ToString();
         } catch (MySql.Data.MySqlClient.MySqlException ex) {
            MessageBox.Show("Error " + ex.Number + " has occurred: " +
             ex.Message + ex.StackTrace,
             "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
         } finally {
            Reader.Close();
         }
         return tempName;
      }

   }
}