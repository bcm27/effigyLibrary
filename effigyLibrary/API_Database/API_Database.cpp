#include "API_Database.h"
// #include "C:\skds\plog-master\include\plog\log.h"

void API_Database::printStatus()
{
	//PLOG_DEBUG << "We have accesed the database file";
}

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
};
