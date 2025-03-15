#pragma once
#include "ResourceBase.h"

class Texture : public ResourceBase
{
public:
    Texture();
    Texture(HDC hdc, HBITMAP bitmap);
    virtual ~Texture();
public:
	void Load(const wstring& _path);
	Texture* MakeStretchTex(float ratioX, float ratioY);
	const LONG& GetWidth() const { return m_bitInfo.bmWidth; }
	const LONG& GetHeight()const { return m_bitInfo.bmHeight; }
	const HDC& GetTexDC()const { return m_hDC; }
private:
	HDC  m_hDC;  
	HBITMAP m_hBit;
	BITMAP m_bitInfo;

	HDC  m_StretchDC;
	HBITMAP m_StretchBit;
	BITMAP m_StretchBitInfo;
	Texture* makedTex;
};

