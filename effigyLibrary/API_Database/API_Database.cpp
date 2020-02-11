#include "API_Database.h"
// #include "C:\skds\plog-master\include\plog\log.h"

void API_Database::printStatus()
{
	//PLOG_DEBUG << "We have accesed the database file";
}

class API_Database::eSQL {

	void showSQLError(unsigned int handleType, const SQLHANDLE& handle)
	{
		SQLWCHAR SQLState[1024];
		SQLWCHAR message[1024];

		if (SQL_SUCCESS == SQLGetDiagRec(handleType, handle, 1, SQLState, NULL, message, 1024, NULL))
			// Returns the current values error, warning, and status flags
			std::cout << "SQL driver message: " << message << "\nSQL state: " << SQLState << std::endl;

		//TODO : https://www.youtube.com/watch?v=1g_Xng_uH2w
		//TODO : https://stackoverflow.com/questions/7860480/connecting-to-a-mysql-server-using-c

	}

	void sqlTestQuery()
	{
		SQLHANDLE SQLEnvHandle = NULL;
		SQLHANDLE SQLConnectionHandle = NULL;
		SQLHANDLE SQLStatementHandle = NULL;
		SQLRETURN SQLret;
		char SQLQuery[] = "Select * from users";

		do
		{
			if (SQL_SUCCESS != SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &SQLEnvHandle))
				break; // if the result is not a success break the do while
			if (SQL_SUCCESS != SQLSetEnvAttr(SQLEnvHandle, SQL_ATTR_ODBC_VERSION, (SQLPOINTER)SQL_OV_ODBC3, 0))
				break; // again break if we failed to set up the environment
			if (SQL_SUCCESS !=SQLAllocHandle(SQL_HANDLE_DBC,SQLEnvHandle, &SQLConnectionHandle))
				break; // brak if fail
			if (SQL_SUCCESS != SQLSetConnectAttr(SQLConnectionHandle, SQL_LOGIN_TIMEOUT, (SQLPOINTER)5, 0))
				break; // same as above

			SQLWCHAR retConString[1024];
			SQLDriverConnect(SQLConnectionHandle, NULL, (SQLWCHAR*)"" /*TODO add login info*/, SQL_NTS, retConString, 1024, NULL, SQL_DRIVER_NOPROMPT);
		} while (false);
	}
};


class API_Database::userInfo {

	userInfoStruct getUserInfo(userInfo user) {
		// normally we would access the database for this info and return
		// a new userInfo struct ; currently settng up mysql db on digital ocean

		userInfoStruct retUserInfo;
		retUserInfo.firstName = "Bjorn";
		retUserInfo.lastName = "Mathisen";
		retUserInfo.userName = "bcm27";
		retUserInfo.tempPassword = "pass";

		return retUserInfo;
	}
	userInfoStruct getUserPass(std::string P)
	{
		userInfoStruct ret; 
		ret.tempPassword = "temp Pass";
		return ret;
	}
	userInfoStruct getUserName(std::string N)
	{
		return userInfoStruct();
	}
};

