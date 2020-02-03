#pragma once
#ifndef _EFFIGY_API_DATABASE_H
#define _EFFIGY_API_DATABASE_H

#include <iostream>
#include <string>
#include <string>

namespace API_Database {

	struct userInfo {
		std::string userName;
		std::string firstName;
		std::string lastName; 
		std::string tempPassword;
	};

	void printStatus();
	void getUserInfo(userInfo user);
}

#endif // !_EFFIGY_API_DATABASE_H