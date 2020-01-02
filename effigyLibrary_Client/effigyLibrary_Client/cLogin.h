#pragma once
#include "wx/wx.h"
#include <wx/frame.h>

class cLogin : public wxFrame {
public:
	cLogin();
	~cLogin();

private:
	cLogin* m_frame1 = nullptr;
};