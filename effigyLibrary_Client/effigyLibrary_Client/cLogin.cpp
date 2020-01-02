#include "cLogin.h"

cLogin::cLogin() : wxFrame(nullptr, wxID_HOME, "Effigy Library - Login", wxPoint(), wxSize(400, 225))
{
	cLogin_btn_login = new wxButton(this, wxID_ANY, "Login", wxPoint(75,135), wxSize(100,30));
	cLogin_txt_userName = new wxTextCtrl(this, wxID_ANY, "Username", wxPoint(75,100), wxSize(100,20));
	cLogin_btn_register = new wxButton(this, wxID_ANY, "Register", wxPoint(200, 135), wxSize(100, 30));
	cLogin_txt_password = new wxTextCtrl(this, wxID_ANY, "Password", wxPoint(200, 100), wxSize(100, 20));

	cLogin_txt_storedUsers = new wxListBox(this, wxID_ANY, wxPoint(10,10), wxSize(50,165));
}

cLogin::~cLogin()
{

}
