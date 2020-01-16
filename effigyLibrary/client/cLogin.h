#pragma once
#ifndef _EFFIGY_CLOGIN_H
#define _EFFIGY_CLOGIN_H

#include <wx/wx.h>
#include <wx/frame.h>

class cLogin : public wxFrame {
public:
	cLogin();
	~cLogin();

public:
	wxButton* cLogin_btn_login = nullptr;
	wxButton* cLogin_btn_register = nullptr;

	wxTextCtrl* cLogin_txtbx_userName = nullptr;
	wxTextCtrl* cLogin_txtbx_password = nullptr;

	wxStaticText* cLogin_stattxt_storedUsers = nullptr;
	wxListBox* cLogin_listbx_storedUsers = nullptr;

	void onButtonClicked(wxCommandEvent& event);

	wxDECLARE_EVENT_TABLE();

private:

	enum {
		ID_Login = 1
	};
};

#endif // !_EFFIGY_CLOGIN_H
