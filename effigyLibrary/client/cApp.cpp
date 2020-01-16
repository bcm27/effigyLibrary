#include "cApp.h"
#include "C:\skds\plog-master\include\plog\log.h"

wxIMPLEMENT_APP(cApp);

cApp::cApp()
{
}

cApp::~cApp()
{
}

bool cApp::OnInit()
{
	plog::init(plog::debug, "Debug.txt"); // Step2: initialize the logger
	PLOG_DEBUG << "Hello log!"; // long macro

	m_frame01 = new cLogin();
	m_frame01->Show();

	PLOG_DEBUG << "Frame initalized";

	return true;
}