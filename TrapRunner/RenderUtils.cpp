#include "pch.h"
#include "RenderUtils.h"


void RenderUtils::RenderText(HDC _hdc, Vec2 _pos, const wstring& _str)
{
	::TextOut(_hdc, _pos.x, _pos.y, _str.c_str(), _str.length());
}

void RenderUtils::RenderRect(HDC _hdc, Vec2 _pos, int _width, int _height)
{
	::Rectangle(_hdc
		, _pos.x - _width / 2
		, _pos.y - _height / 2
		, _pos.x + _width / 2
		, _pos.y + _height / 2
	);
}

void RenderUtils::RenderRectColor(HDC _hdc, Vec2 _pos, int _width, int _height, COLORREF _color)
{
	HBRUSH hBrush = ::CreateSolidBrush(_color);
	HBRUSH hOldBrush = (HBRUSH)::SelectObject(_hdc, hBrush);

	::Rectangle(_hdc
		, _pos.x - _width / 2
		, _pos.y - _height / 2
		, _pos.x + _width / 2
		, _pos.y + _height / 2
	);
	::SelectObject(_hdc, hOldBrush);
	::DeleteObject(hBrush);
}

void RenderUtils::RenderCircle(HDC _hdc, Vec2 _pos, int _radius)
{
	::Ellipse(_hdc
		, _pos.x - _radius / 2
		, _pos.y - _radius / 2
		, _pos.x + _radius / 2
		, _pos.y + _radius / 2
	);

}

void RenderUtils::RenderCircleColor(HDC _hdc, Vec2 _pos, int _radius, COLORREF _color)
{
	HBRUSH hBrush = ::CreateSolidBrush(_color);
	HBRUSH hOldBrush = (HBRUSH)::SelectObject(_hdc, hBrush);

	::Ellipse(_hdc
		, _pos.x - _radius / 2
		, _pos.y - _radius / 2
		, _pos.x + _radius / 2
		, _pos.y + _radius / 2
	);
	::SelectObject(_hdc, hOldBrush);
	::DeleteObject(hBrush);

}
void RenderUtils::RenderLine(HDC _hdc, Vec2 _from, Vec2 _to)
{
	::MoveToEx(_hdc, _from.x, _from.y, nullptr);
	::LineTo(_hdc, _to.x, _to.y);
}

void RenderUtils::RenderLineColor(HDC _hdc, Vec2 _from, Vec2 _to, COLORREF _color)
{
	HPEN hPen = ::CreatePen(PS_SOLID, 1, _color);
	HPEN hOldPen = (HPEN)::SelectObject(_hdc, hPen);
	::MoveToEx(_hdc, _from.x, _from.y, nullptr);
	::LineTo(_hdc, _to.x, _to.y);

	::SelectObject(_hdc, hOldPen);
	::DeleteObject(hPen);
}
