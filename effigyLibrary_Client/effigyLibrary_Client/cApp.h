#pragma once
#include "cMain.h"
#include "wx/wx.h"

class cApp : public wxApp {
public: 
	cApp();
	~cApp();

public:
	virtual bool OnInit();

private:
	cMain* m_frame1 = nullptr;
};