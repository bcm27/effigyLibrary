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

	void printStatus();
	void showSQLError(unsigned int handleType, const SQLHANDLE& handle);

	class userInfo {
		void getUserInfo(userInfo user);

		userInfoStruct getUserPass(std::string P);
		userInfoStruct getUserName(std::string N);
	};
}

#endif // !_EFFIGY_API_DATABASE_H