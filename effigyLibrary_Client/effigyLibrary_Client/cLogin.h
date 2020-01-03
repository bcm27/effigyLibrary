#pragma once
#include "wx/wx.h"
#include <wx/frame.h>

class cLogin : public wxFrame {
public:
	cLogin();
	~cLogin();

public:
	wxButton* cLogin_btn_login = nullptr;
	wxButton* cLogin_btn_register = nullptr;

	wxTextCtrl* cLogin_txt_userName = nullptr;
	wxTextCtrl* cLogin_txt_password = nullptr;

	wxStaticText* cLogin_sttxt_storedUsers = nullptr;
	wxListBox* cLogin_txt_storedUsers = nullptr;

};