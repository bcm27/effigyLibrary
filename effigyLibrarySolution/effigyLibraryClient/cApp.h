#pragma once
#include "cMain.h"
#include "cLogin.h"
#include "wx/wx.h"

class cApp : public wxApp {
public:
	cApp();
	~cApp();

	virtual bool OnInit();

private:

	cLogin* m_frame01 = nullptr;
	cMain* m_frame02 = nullptr;
};