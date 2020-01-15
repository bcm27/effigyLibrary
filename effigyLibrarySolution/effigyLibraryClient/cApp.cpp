#include "cApp.h"

wxIMPLEMENT_APP(cApp);

cApp::cApp()
{
}

cApp::~cApp()
{
}

bool cApp::OnInit()
{

	m_frame01 = new cLogin();
	m_frame01->Show();

	return true;
}
