#include "cLogin.h"

wxBEGIN_EVENT_TABLE(cLogin, wxFrame)
	EVT_BUTTON(10001, onButtonClicked)
wxEND_EVENT_TABLE()

cLogin::cLogin() : wxFrame(nullptr, wxID_HOME, "Effigy Library - Login", wxPoint(), wxSize(385, 225))
{
	// create gui - buttons
	cLogin_btn_login = new wxButton(this, 10001, "Login", wxPoint(110,135), wxSize(100,30));
	cLogin_btn_register = new wxButton(this, 10002, "Register", wxPoint(250, 135), wxSize(100, 30));
	// create gui - txt boxes
	cLogin_txtbx_userName = new wxTextCtrl(this, wxID_ANY, "Username", wxPoint(110,100), wxSize(100,20));
	cLogin_txtbx_password = new wxTextCtrl(this, wxID_ANY, "Password", wxPoint(250, 100), wxSize(100, 20));
	cLogin_listbx_storedUsers = new wxListBox(this, wxID_ANY, wxPoint(10,10), wxSize(80,165));
	// create gui - static text
	cLogin_stattxt_storedUsers = new wxStaticText(this, wxID_ANY, "", wxPoint(), wxSize());

	Center();
}

cLogin::~cLogin()
{

}

void cLogin::onButtonClicked(wxCommandEvent& event)
{
	cLogin_listbx_storedUsers->AppendString(cLogin_txtbx_userName->GetValue());
}
