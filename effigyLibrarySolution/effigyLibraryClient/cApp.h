#pragma once

#include <wx/wx.h>

#include "cMain.h"
#include "cLogin.h"
#include "Database.h"

class cApp : public wxApp {
public:
	cApp();
	~cApp();

	virtual bool OnInit();

private:

	cLogin* m_frame01 = nullptr;
	cMain* m_frame02 = nullptr;
};