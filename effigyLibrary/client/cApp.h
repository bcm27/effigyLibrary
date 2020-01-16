#pragma once
#ifndef _EFFIGY_CAPP_H
#define _EFFIGY_CAPP_H

#include <wx/wx.h>

#include "cMain.h"
#include "cLogin.h"

class cApp : public wxApp {
public:
	cApp();
	~cApp();

	virtual bool OnInit();

private:

	cLogin* m_frame01 = nullptr;
	cMain* m_frame02 = nullptr;
};

#endif // !_EFFIGY_CAPP_H