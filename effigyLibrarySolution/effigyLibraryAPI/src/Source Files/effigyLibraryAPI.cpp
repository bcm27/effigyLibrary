#include "..\Headers\effigyLibraryAPI.h"
#include <iostream>

namespace effigyLibraryAPI {

	void printMessage()
	{
		std::cout << "Hello World this is the Effigy Library API" << std::endl;
	}

	
	effigyAPI_Database::effigyAPI_Database() {
			std::cout << "Database Succesfully Linked" << std::endl;
		}

	effigyAPI_Database::~effigyAPI_Database()
		{

		}
}