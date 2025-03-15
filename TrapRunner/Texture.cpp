#include "pch.h"
#include "Texture.h"
#include "ResourceManager.h"
#include <assert.h>
#include "Core.h"
static int StretchID = 0;
Texture::Texture()
	: m_hDC(nullptr)
	, m_hBit(nullptr)
	, m_bitInfo{}
	, m_StretchDC(nullptr)
	, m_StretchBit(nullptr)
	, m_StretchBitInfo{}
	, makedTex(nullptr)
{

}

Texture::Texture(HDC hdc, HBITMAP bitmap)
	: m_hDC(nullptr)
	, m_hBit(nullptr)
	, m_bitInfo{}
	, m_StretchDC(nullptr)
	, m_StretchBit(nullptr)
	, m_StretchBitInfo{}
	, makedTex(nullptr)
{
	m_hBit = bitmap;
	m_hDC = hdc;
	::SelectObject(m_hDC, m_hBit);
	::GetObject(m_hBit, sizeof(BITMAP), &m_bitInfo);
}

Texture::~Texture()
{
	if (m_hDC != nullptr)
	{
		::DeleteDC(m_hDC);
		::DeleteObject(m_hBit);
	}
	if (m_StretchDC != nullptr)
	{
		::DeleteDC(m_StretchDC);
		::DeleteObject(m_StretchBit);
	}
}

void Texture::Load(const wstring& _path)
{
	// 1: 인스턴스 핸들(nullptr: 독립형 리소스)
	// 2: 경로
	// 3. BITMAP / ICON / CURSOR / .. 
	// 4,5 : 이미지(리소스) 크기
	// 6: 추가 플래그
	m_hBit = (HBITMAP)::LoadImage(nullptr, _path.c_str(), IMAGE_BITMAP,
		0, 0, LR_CREATEDIBSECTION | LR_LOADFROMFILE);
	assert(m_hBit); // nullptr이면 assert가 걸립니다.
	m_hDC = ::CreateCompatibleDC(GET_SINGLE(Core)->GetMainDC());
	::SelectObject(m_hDC, m_hBit);
	::GetObject(m_hBit, sizeof(BITMAP), &m_bitInfo);
	int a = 0;
}

Texture* Texture::MakeStretchTex(float ratioX, float ratioY)
{
	assert(m_hBit); // nullptr이면 assert가 걸립니다.
	m_StretchBit = CreateCompatibleBitmap(m_hDC, m_bitInfo.bmWidth * ratioX, m_bitInfo.bmHeight * ratioY);
	m_StretchDC = ::CreateCompatibleDC(m_StretchDC);
	::SelectObject(m_StretchDC, m_StretchBit);
	::GetObject(m_StretchBit, sizeof(BITMAP), &m_StretchBitInfo);

	StretchBlt(m_StretchDC
		, 0, 0, m_StretchBitInfo.bmWidth, m_StretchBitInfo.bmHeight
		, m_hDC
		, 0, 0, m_bitInfo.bmWidth, m_bitInfo.bmHeight, SRCCOPY
	);

	makedTex = new Texture(m_StretchDC, m_StretchBit);
	GET_SINGLE(ResourceManager)->TestureAdd(std::to_wstring(StretchID++), makedTex);
	return makedTex;
}