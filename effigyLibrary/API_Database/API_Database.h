#pragma once
#ifndef _EFFIGY_API_DATABASE_H
#define _EFFIGY_API_DATABASE_H

#include <iostream>
#include <string>
#include <string>

#include <sqlext.h>
#include <sqltypes.h>
#include <sql.h>

namespace API_Database {

	struct userInfoStruct {
		std::string userName;
		std::string firstName;
		std::string lastName; 
		std::string tempPassword;
	};

	// global API_DATABASE METHODS
	void printStatus();

	class eSQL {
	public:
		void showSQLError(unsigned int handleType, const SQLHANDLE& handle);

		void sqlTestQuery();
	};

	// class containing all user information 
	class userInfo {
	public:
		void getUserInfo(userInfo user);

		userInfoStruct getUserPass(std::string P);
		userInfoStruct getUserName(std::string N);
	private:
	};

	SQLHANDLE SQLEnvHandle = NULL;
}

#endif // !_EFFIGY_API_DATABASE_H