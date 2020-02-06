#pragma once
#ifndef _EFFIGY_API_DATABASE_H
#define _EFFIGY_API_DATABASE_H

#include <iostream>
#include <string>
#include <string>

namespace API_Database {

	struct userInfoStruct {
		std::string userName;
		std::string firstName;
		std::string lastName; 
		std::string tempPassword;
	};

	void printStatus();

	class userInfo {
		void getUserInfo(userInfo user);

		userInfoStruct getUserPass(std::string P);
		userInfoStruct getUserName(std::string N);
	};
}

#endif // !_EFFIGY_API_DATABASE_H