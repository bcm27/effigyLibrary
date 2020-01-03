#include "cLogin.h"

cLogin::cLogin() : wxFrame(nullptr, wxID_HOME, "Effigy Library - Login", wxPoint(), wxSize(385, 225))
{
	cLogin_btn_login = new wxButton(this, wxID_ANY, "Login", wxPoint(110,135), wxSize(100,30));
	cLogin_txt_userName = new wxTextCtrl(this, wxID_ANY, "Username", wxPoint(110,100), wxSize(100,20));
	cLogin_btn_register = new wxButton(this, wxID_ANY, "Register", wxPoint(250, 135), wxSize(100, 30));
	cLogin_txt_password = new wxTextCtrl(this, wxID_ANY, "Password", wxPoint(250, 100), wxSize(100, 20));
	cLogin_txt_storedUsers = new wxListBox(this, wxID_ANY, wxPoint(10,10), wxSize(80,165));
	cLogin_sttxt_storedUsers = new wxStaticText(this, wxID_ANY, "", wxPoint(), wxSize());
}

cLogin::~cLogin()
{

}
